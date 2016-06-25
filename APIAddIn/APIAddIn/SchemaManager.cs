using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.IO;

using EA;

namespace APIAddIn
{
    /* This class deals with serialization of EA class diagrams to JSON Schema files. */
    public class SchemaManager
    {
        static Logger logger = new Logger();

        static FileManager fileManager = new FileManager(null);
        

        static public void setLogger(Logger l)
        {
            logger = l;
        }
        static public void setFileManager(FileManager fm)
        {
            fileManager = fm;
        }

        static private JSchema convertEATypeToJSchemaType(string t)
        {
            if (t.Equals(APIAddinClass.EA_TYPE_CURRENCY))
            {
                return new JSchema { 
                    Type = JSchemaType.String,
                    Pattern =@"^\$?(?=\(.*\)|[^()]*$)\(?\d{1,3}(,?\d{3})?(\.\d\d)?\)?$",
                    Description="A currency",
                    Title="Currency"
                };
            }
            if (t.Equals(APIAddinClass.EA_TYPE_DECIMAL))
            {
                return new JSchema { Type = JSchemaType.Number };
            }
            if (t.Equals(APIAddinClass.EA_TYPE_FLOAT))
            {
                return new JSchema { Type = JSchemaType.Number };
            }
            if (t.Equals(APIAddinClass.EA_TYPE_INT))
            {
                return new JSchema { Type = JSchemaType.Integer };
            }
            else if (t.Equals(APIAddinClass.EA_TYPE_DATE))
            {
                return new JSchema { Type = JSchemaType.String };
            }
            else if (t.Equals(APIAddinClass.EA_TYPE_BOOLEAN))
            {
                return new JSchema { Type = JSchemaType.Boolean };
            }
            else
                return new JSchema { Type = JSchemaType.String };
        }
       
        static private void setRequirement(JSchema s, string name, string lowerBound, string upperBound)
        {
            if (lowerBound.Equals("1"))
                s.Required.Add(name);
        }

        static private void setRequirement(JSchema s, string name, string cardinality)
        {
            if (cardinality.StartsWith("1"))
                s.Required.Add(name);
        }

        static private bool cardinalityOfMany(string cardinality)
        {
            return (cardinality.Equals(APIAddinClass.CARDINALITY_0_TO_MANY) || cardinality.Equals(APIAddinClass.CARDINALITY_1_TO_MANY));           
        }


        static public KeyValuePair<string, JSchema> schemaToJsonSchema(EA.Repository Repository, EA.Diagram diagram)
        {
            DiagramManager.captureDiagramLinks(diagram);

            //logger.log("Export Schemas");
            KeyValuePair<string, JSchema> kv = new KeyValuePair<string,JSchema>("",null);            

            DependencyManager dm = new DependencyManager();

            EA.Package schemaPackage = Repository.GetPackageByID(diagram.PackageID);
            EA.Package apiPackage = Repository.GetPackageByID(schemaPackage.ParentID);

            string msg = "";
            IList<EA.Element> classes = MetaDataManager.diagramClasses(Repository, diagram);

            Dictionary<string, JSchema> schemas = new Dictionary<string, JSchema>();

            EA.Element root = null;
            JSchema container = null;

            List<int> possibleRoots = new List<int>();
            IList<EA.Element> objects = MetaDataManager.diagramSamples(Repository, diagram);
            foreach (EA.Element obj in objects)
            {
                foreach (EA.Connector conn in obj.Connectors)
                {
                    possibleRoots.Add(conn.SupplierID);
                }
            }
            foreach (EA.Element clazz in classes)
            {
                //logger.log("Create Schema for :" + clazz.Name);
                JSchema schema = new JSchema();
                if (clazz.Type.Equals("Enumeration") || (clazz.GetStereotypeList() != null && clazz.GetStereotypeList().Contains("enumeration")))
                    schema.Type = JSchemaType.String;
                else
                    schema.Type = JSchemaType.Object;
                schema.Title = clazz.Name;
                if(clazz.Notes!=null)
                    schema.Description = clazz.Notes.Trim().Replace("\r\n", "");                
                schema.AllowAdditionalProperties = false;

                schema.ExtensionData.Add("javaType", "nz.co.iag.model.consumer.json." + clazz.Name);
                schema.ExtensionData.Add("$schema", "http://json-schema.org/draft-04/schema#");
               
                schemas.Add(clazz.Name, schema);

                if (possibleRoots.Contains(clazz.ElementID))
                {
                    //logger.log("Found root as related from object:");
                    container = schema;
                    root = clazz;
                }
                else
                {
                    //logger.log("Stereotype:" + clazz.GetStereotypeList());
                    if (clazz.GetStereotypeList() != null && clazz.GetStereotypeList().Contains(APIAddinClass.EA_STEREOTYPE_REQUEST))
                    {
                        container = schema;
                        root = clazz;
                    }
                }
        
                //New way to represent properties
                visitOutboundConnectedElements(Repository,clazz, schema, MetaDataManager.filterDataItem, handleDataItem);
                        
                //Old way
                foreach (EA.Attribute attr in clazz.Attributes){ handleAttribute(schema,clazz,attr); }
                
            }

            if (container == null)
            {
                MessageBox.Show("No container identified for the schema diagram [" + diagram.Name + "] in package[" + apiPackage.Name + "], link an object to a class:");
                return kv;
            }

            logger.log("Validating Sample");

            validateDiagram(Repository, classes, root);


            foreach (Element clazz in classes)
            {
                logger.log("Adding Attributes for Schema:" + clazz.Name);
                JSchema schema = schemas[clazz.Name];

                if (clazz.Type.Equals("Enumeration") || (clazz.GetStereotypeList() != null && clazz.GetStereotypeList().Contains("enumeration")))
                {                    
                   foreach (EA.Attribute attr in clazz.Attributes)
                    {
                        schema.Description += " " + attr.Name;
                        if(attr.Notes!=null && attr.Notes.Length>0)
                            schema.Description += "[" + attr.Notes + "]";
                        schema.Description += ",";
                        
                    }
                }
                
                foreach (EA.Connector con in clazz.Connectors)
                {                    
                    EA.Element related = null;
                    if (clazz.ElementID == con.ClientID)
                    {
                        related = Repository.GetElementByID(con.SupplierID);
                        if (related != null)
                        {
                            if (!DiagramManager.isVisible(con))
                                continue; //ignore entities not on the diagram

                            if (related.Stereotype.Contains(APIAddinClass.EA_STEREOTYPE_DATAITEM))
                                continue;//Skip data items

                                                        
                            {
                                logger.log("FoundSupplier:" + related.Name + " to " + clazz.Name);
                                if (schemas.ContainsKey(related.Name))
                                {
                                    JSchema schemaRelated = schemas[related.Name];
                                    if (con.SupplierEnd.Role != null && con.SupplierEnd.Role.Length > 0)
                                        dm.setDependency(related.Name, clazz.Name);

                                    try
                                    {
                                        if (con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_0_TO_MANY) || con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_1_TO_MANY))
                                        {
                                            List<JSchema> l = new List<JSchema>();
                                            l.Add(schemaRelated);
                                            JSchema arraySchema = new JSchema()
                                            {
                                                Type = JSchemaType.Array,
                                                Description = con.SupplierEnd.RoleNote,
                                                Items = { schemaRelated }

                                            };
                                            logger.log("Adding:" + con.SupplierEnd.Role + " as array ");
                                            schema.Properties.Add(con.SupplierEnd.Role, arraySchema);
                                        }
                                        else if (con.Stereotype != null && con.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_EMBEDDED))
                                        {
                                            foreach (KeyValuePair<string, JSchema> p in schemaRelated.Properties)
                                            {
                                                logger.log("Adding:" + p.Key + " to " + p.Value);
                                                schema.Properties.Add(p.Key, p.Value);
                                            }
                                        }
                                        else
                                        {
                                            logger.log("Adding:" + con.SupplierEnd.Role + " to " + schemaRelated.Title);
                                            schema.Properties.Add(con.SupplierEnd.Role, schemaRelated);
                                            logger.log("Adding2:" + con.SupplierEnd.Role + " to " + schemaRelated.Title);
                                        }
                                        if (con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_1_TO_MANY) || con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_ONE))
                                        {
                                            logger.log("Adding Required:" + con.SupplierEnd.Role);
                                            schema.Required.Add(con.SupplierEnd.Role);
                                        }

                                    }
                                    catch (System.ArgumentException ex)
                                    {
                                        logger.log("Duplicate Property Exception of:" + con.SupplierEnd.Role + " in " + clazz.Name);
                                        throw ex;
                                    }
                                }
                            }                            
                        }
                    }
                }
            }
            //foreach (Element clazz in classes)
            //{
            //    logger.log("Adding Attributes for Schema:" + clazz.Name);
            //    JSchema schema = schemas[clazz.Name];
            //    JSchema links = new JSchema();
            //    links.Title = "Links from " + clazz.Name;
            //    visitOutboundConnectedElements(Repository, clazz, links, MetaDataManager.filterResource, handleLink);
            //    schema.Properties.Add("links", links);
            //}

            
            JObject definitions = new JObject();
            container.ExtensionData.Add("definitions", definitions);
            foreach (string s in dm.getDependencies())
            {
                logger.log("Adding definitions:" + s);
                definitions.Add(s, schemas[s]);
            }
            
            kv = new KeyValuePair<string, JSchema>(root.Name, container);

            return kv;
        }

        static void handleAttribute(JSchema schema,EA.Element clazz, EA.Attribute attr)
        {
            JSchema attributeSchema = SchemaManager.convertEATypeToJSchemaType(attr.Type);
            {
                foreach (EA.AttributeTag da in attr.TaggedValues)
                {                            
                    if (da.Name.Equals(APIAddinClass.EA_TAGGEDVALUE_PATTERN))
                    {
                        if (da.Value != null && da.Value.Length > 0)
                            attributeSchema.Pattern = da.Value;
                    }
                }
            }                                                                

            //logger.log("Adding Attribute:" + attr.Name + " of EAType:" + attr.Type);
            if (attr.Notes != null && attr.Notes.Length > 0)
                attributeSchema.Description = attr.Notes.Trim().Replace("\r\n","");

            //logger.log(clazz.Name + " has stereotypes:" + clazz.GetStereotypeList());

            if (clazz.Type.Equals("Enumeration") || (clazz.GetStereotypeList() != null && clazz.GetStereotypeList().Contains("enumeration")))
            {
                schema.Enum.Add(attr.Name);
                schema.AllowAdditionalItems = false;
            }
            else
            {
                setRequirement(schema, attr.Name, attr.LowerBound, attr.UpperBound);
                if (attr.UpperBound.Equals(APIAddinClass.CARDINALITY_ONE))
                    schema.Properties.Add(attr.Name, attributeSchema);
                else
                {
                    JSchema arraySchema = new JSchema()
                    {
                        Type = JSchemaType.Array,
                        Items = { attributeSchema }
                    };
                    schema.Properties.Add(attr.Name, arraySchema);
                }
            }
        }

        static public string getDataItemType(EA.Element dataitem)
        {
            String s = dataitem.GetStereotypeList().Replace(APIAddinClass.EA_STEREOTYPE_DATAITEM, "");
            s = s.Replace(",", "");
            return s;
            
        }

        static Object handleLink(Object context, EA.Element clazz, EA.Connector connector, EA.Element attr)
        {
            JSchema links = (JSchema)context;

            JSchema typeString = SchemaManager.convertEATypeToJSchemaType("string");

            links.Properties.Add(connector.SupplierEnd.Role, typeString);
            return links;
        }

        static Object handleDataItem(Object context,EA.Element clazz, EA.Connector connector, EA.Element attr)
        {
            //logger.log("Handling Data Item:"+clazz.Name);
            JSchema schema = (JSchema)context;

            String s = getDataItemType(attr);
            //logger.log("Got data item type" + s);
            JSchema attributeSchema = SchemaManager.convertEATypeToJSchemaType(s);
            //logger.log("Got attribute schema");
            {
                
                foreach (EA.TaggedValue da in attr.TaggedValues)
                {                    
                    if (da.Name.Equals(APIAddinClass.EA_TAGGEDVALUE_PATTERN))
                    {
                        if (da.Value != null && da.Value.Length > 0)
                            attributeSchema.Pattern = da.Value;
                    }
                }
            }

            //logger.log("Adding Attribute:" + attr.Name + " of EAType:" + attr.Type);
            if (attr.Notes != null && attr.Notes.Length > 0)
                attributeSchema.Description = attr.Notes.Trim().Replace("\r\n","");

            //logger.log(attr.Name + " has stereotypes:" + clazz.GetStereotypeList());
            
            String nm = connector.SupplierEnd.Role;
            if (nm == null || nm.Length == 0)
            {
                nm = attr.Name;
            }

            if (attr.Type.Equals("Enumeration") || (attr.GetStereotypeList() != null && attr.GetStereotypeList().Contains("enumeration")))
            {
                schema.Enum.Add(attr.Name);
                schema.AllowAdditionalItems = false;
            }
            else
            {
                setRequirement(schema, nm, connector.SupplierEnd.Cardinality);
                if (!cardinalityOfMany(connector.SupplierEnd.Cardinality))
                    schema.Properties.Add(nm, attributeSchema);
                else
                {
                    JSchema arraySchema = new JSchema()
                    {
                        Type = JSchemaType.Array,
                        Items = { attributeSchema }
                    };                    
                    schema.Properties.Add(nm, arraySchema);
                }
            }
            return null;            
        }

     

        static public void exportSchema(EA.Repository Repository, EA.Diagram diagram)
        {
            //logger.log("Export Schemas");

            if (!diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM))
            {
                logger.log("exportSchema: Ignore diagam that isnt a schema diagram");
                return;
            }

            KeyValuePair<string, JSchema> container = schemaToJsonSchema(Repository, diagram);

            if (container.Value != null)
            {
                string msg = container.Value.ToString() + "\n";

                EA.Package schemaPkg = Repository.GetPackageByID(diagram.PackageID);                
                EA.Package apiPackage = Repository.GetPackageByID(schemaPkg.ParentID);

                string sourcecontrolPackage = apiPackage.Name;
                if (MetaDataManager.isCDMPackage(Repository, apiPackage))
                {
                    sourcecontrolPackage = "cdm";
                }

                fileManager.initializeAPI(sourcecontrolPackage);
                fileManager.setup();
                fileManager.exportSchema(container.Value.Title, msg);                
            }
            else
            {                
                return;
            }
        }

     

        //static public void exportCanonical(EA.Repository Repository, EA.Diagram diagram)
        //{            
        //    if (!diagram.Stereotype.Equals(APIAddInClass.EA_STEREOTYPE_CANONICALDIAGRAM))
        //    {
        //        logger.log("exportCanonical: Ignore diagam that isnt a canonical diagram");
        //        return;
        //    }

        //    KeyValuePair<string, JSchema> container = schemaToJsonSchema(Repository, diagram);

        //    if (container.Value != null)
        //    {
        //        string msg = container.Value.ToString();

        //        EA.Package schemaPkg = Repository.GetPackageByID(diagram.PackageID);
        //        EA.Package cdmPackage = Repository.GetPackageByID(schemaPkg.ParentID);

        //        fileManager.initializeCanonical(cdmPackage.Name);
        //        fileManager.setupCanonical();
        //        fileManager.exportCanonical(container.Value.Title, msg);
        //    }
        //    else
        //    {                
        //        return;
        //    }
        //}


        static public EA.Package generateSample(EA.Repository Repository)
        {
            string nameOfGenerated = "Generated";

            EA.Diagram diagram = Repository.GetCurrentDiagram();

            EA.Package schemaPackage = Repository.GetPackageByID(diagram.PackageID);
            EA.Package apiPackage = Repository.GetPackageByID(schemaPackage.ParentID);
            EA.Package samplePackage = null;

            Dictionary<int, string> connectionsVisited = new Dictionary<int, string>();

            foreach (EA.Package p in apiPackage.Packages)
            {
                //logger.log("Package:" + p.Name);
                if (p.Name.Equals(APIAddinClass.API_PACKAGE_SAMPLES))
                {
                    samplePackage = p;
                    break;
                }
            }

            if (samplePackage == null)
            {
                MessageBox.Show("Create the '" + APIAddinClass.API_PACKAGE_SAMPLES + "' package under the API");
                return null;
            }

            //logger.log("Package:" + schemaPackage.Name);

            DiagramManager.captureDiagramLinks(diagram);

            Dictionary<int,string> possibleRoots = new Dictionary<int,string>();
            IList<EA.Element> objects = MetaDataManager.diagramSamples(Repository, diagram);
            foreach (EA.Element obj in objects)
            {
                //logger.log("Found Object:" + obj.ElementID);
                foreach (EA.Connector conn in obj.Connectors)                
                {
                    if (DiagramManager.isVisible(conn))
                    {
                        //logger.log("Possible Root:" + conn.SupplierID);
                        if (!possibleRoots.ContainsKey(conn.SupplierID))
                            possibleRoots.Add(conn.SupplierID, conn.SupplierEnd.Role);
                    }                    
                }
            }

            EA.Element clazz = null;
            IList<EA.Element> clazzes = MetaDataManager.diagramClasses(Repository, diagram);
            foreach (EA.Element diaclazz in clazzes)
            {                
                if (possibleRoots.ContainsKey(diaclazz.ElementID))
                {
                    //logger.log("Found root as relation from object:"+diaclazz.Name);
                    clazz = diaclazz;                    
                    nameOfGenerated = possibleRoots[diaclazz.ElementID];
                    break;
                }
                else if (diaclazz.GetStereotypeList().Contains(APIAddinClass.EA_STEREOTYPE_REQUEST))
                {
                    clazz = diaclazz;                    
                    break;
                }
            }
            if (clazz == null)
            {
                MessageBox.Show("Ensure there an object linked to a class or a Request stereotyped clazz on the diagram");
                return null;
            }

            
            //logger.log("Selected El:" + clazz.Name);

            //logger.log("Type" + clazz.Type);

            if (!clazz.Type.Equals("Class"))
            {
                MessageBox.Show("Select a class");
                return null;
            }

            EA.Collection pkgs = samplePackage.Packages;
            samplePackage = pkgs.AddNew(clazz.Name + "-Sample", "Package");
            samplePackage.Update();
            pkgs.Refresh();

            EA.Diagram newdia = samplePackage.Diagrams.AddNew(clazz.Name + "-Sample", "Object");
            if (!newdia.Update())
            {
                MessageBox.Show(diagram.GetLastError());
                return null;
            }
            samplePackage.Diagrams.Refresh();
            newdia.Stereotype = APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM;
            newdia.Update();
            samplePackage.Update();

            //logger.log("Added diagram:" + newdia.DiagramID);

            IList<EA.Element> diagramClasses = MetaDataManager.diagramClasses(Repository, diagram);
            List<int> diagramElementIds = new List<int>();
            foreach (EA.Element de in diagramClasses) diagramElementIds.Add(de.ElementID);

            EA.Element o = addSample(Repository, nameOfGenerated, clazz, samplePackage, newdia, connectionsVisited, diagramElementIds);            
            
            o.Update();

            {
                EA.Element sample = samplePackage.Elements.AddNew("sample", "Object");                                
                sample.Stereotype = APIAddinClass.EA_STEREOTYPE_SAMPLE;                
                sample.Update();
                EA.DiagramObject diaObj = newdia.DiagramObjects.AddNew("sample", "");                
                diaObj.ElementID = sample.ElementID;                
                diaObj.Update();
                diagram.Update();               
                EA.Connector link = sample.Connectors.AddNew("", "Association");                                
                link.SupplierID = o.ElementID;
                link.Update();
                EA.DiagramLink dl = newdia.DiagramLinks.AddNew("", "");
                diagram.Update();
                dl.ConnectorID = link.ConnectorID;
                dl.Update();                
            }                       

            samplePackage.Update();
            
            samplePackage.Elements.Refresh();
            pkgs.Refresh();

            return samplePackage;
        }


        static Object addDataItemSample(Object o, EA.Element clazz, EA.Connector connector, EA.Element attr)
        {
            //logger.log("Adding Attribute:" + attr.Name);
            EA.Element obj = (EA.Element)o;
            string nv = null;

            foreach (EA.TaggedValue da in attr.TaggedValues)
            {
                if (da.Name.Equals(APIAddinClass.EA_TAGGEDVALUE_DEFAULT))
                {
                    if (da.Value != null && da.Value.Length > 0)
                        nv= da.Value;
                }
            }
            if (nv == null || nv.Length == 0)
                nv = "Value";
            else
                nv = DomainValueManager.selectDefault(nv);

            string nm = connector.SupplierEnd.Role;
            if (nm == null || nm.Length == 0)
            {
                nm = attr.Name;
            }

            obj.RunState = ObjectManager.addRunState(obj.RunState, nm, nv, attr.ElementID);
                         
            return obj;
        }
        static Object addAttributeSample(EA.Element obj, EA.Attribute attr)
        {
            //logger.log("Adding Attribute:" + attr.Name);

            string nv = attr.Default;
            if (nv == null || nv.Length == 0)
                nv = "Value";
            else
                nv = DomainValueManager.selectDefault(nv);

            obj.RunState = ObjectManager.addRunState(obj.RunState, attr.Name, nv, attr.AttributeID);
            return obj;
        }

        static private EA.Element addSample(EA.Repository Repository, string prefix, EA.Element clazz, EA.Package package, EA.Diagram diagram, Dictionary<int, string> connectionsVisited,List<int> diagramElementIds)
        {           
            EA.Element o = package.Elements.AddNew(prefix, "Object");
            
            package.Update();

            o.ClassifierID = clazz.ElementID;
            o.Update();

                        
            visitOutboundConnectedElements(Repository,clazz, o, MetaDataManager.filterDataItem, addDataItemSample);
            foreach (EA.Attribute attr in clazz.Attributes){addAttributeSample(o,attr);}

            //logger.log("new run state:" + o.RunState);            
            if (!o.Update())
            {
                MessageBox.Show(o.GetLastError());
            }

            EA.DiagramObject diaObj = diagram.DiagramObjects.AddNew(prefix, "");
            diagram.Update();

            diaObj.ElementID = o.ElementID;
            //logger.log("Added object to diagram:");
            diaObj.Update();

            foreach (EA.Connector con in clazz.Connectors)
            {
                //logger.log("Processing Connector:" + con.Name);

                try
                {
                    string s = connectionsVisited[con.ConnectorID];
                    if (s != null)
                        continue;
                }
                catch (KeyNotFoundException ex)
                {
                }
  
                connectionsVisited[con.ConnectorID]=con.Name;
                
                EA.Element related = null;
                if (clazz.ElementID == con.ClientID)
                {
                    related = Repository.GetElementByID(con.SupplierID);
                    if (related != null && diagramElementIds.Contains(related.ElementID))
                    {
                        //logger.log("Get related Clazz");
                        EA.Element relatedClazz = Repository.GetElementByID(related.ElementID);
                        //logger.log("Related Class:" + relatedClazz.Name);

                        if (relatedClazz.Stereotype.Contains(APIAddinClass.EA_STEREOTYPE_DATAITEM))
                            continue;//skip data items which are already handled;

                        if (relatedClazz.Type.Equals("Enumeration") || (relatedClazz.GetStereotypeList() != null && relatedClazz.GetStereotypeList().Contains("enumeration")))
                        {
                            //logger.log("Found Enumeration:" + clazz.Name);
                            EA.Attribute val = relatedClazz.Attributes.GetAt(0);
                            o.RunState = ObjectManager.addRunState(o.RunState, con.SupplierEnd.Role, val.Name, val.AttributeID);
                            o.Update();
                        }
                        else if (con.Stereotype!=null && con.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_EMBEDDED))
                        {
                            //logger.log("Found Embedded:" + clazz.Name);
                            foreach (EA.Attribute attr in relatedClazz.Attributes)
                            {
                                string nv = attr.Default;
                                if (nv == null || nv.Length == 0)
                                    nv = "Value";
                                else
                                    nv = DomainValueManager.selectDefault(nv);
                                o.RunState = ObjectManager.addRunState(o.RunState, attr.Name, nv, attr.AttributeID);
                                o.Update();
                            }                            
                        }
                        else if (relatedClazz.Type.Equals("Class"))
                        {
                            //logger.log("Found Class:" + clazz.Name);
                            EA.Element relatedObject = addSample(Repository, con.SupplierEnd.Role, relatedClazz, package, diagram, connectionsVisited,diagramElementIds);                            
                            EA.Connector link = o.Connectors.AddNew(con.SupplierEnd.Role, "Association");
                            //link.Name = con.SupplierEnd.Role;
                            link.SupplierEnd.Role = con.SupplierEnd.Role;
                            link.Name = "";
                            link.SupplierEnd.Cardinality = con.SupplierEnd.Cardinality;
                            link.Direction = APIAddinClass.DIRECTION_SOURCE_TARGET;
                            link.SupplierID = relatedObject.ElementID;
                            link.Notes = "" + con.ConnectorID;
                            if (!link.Update())
                            {
                                MessageBox.Show(link.GetLastError());
                                return o;
                            }

                            o.Connectors.Refresh();
                            EA.DiagramLink dl = diagram.DiagramLinks.AddNew(prefix, "");
                            diagram.Update();
                            dl.ConnectorID = link.ConnectorID;
                        }
                    }
                }
            }
            return o;
        }

        static public void operateOnSample(EA.Repository Repository,System.Func<EA.Repository,EA.Element,bool> f)
        {
            EA.Diagram diagram = Repository.GetCurrentDiagram();

            if (!diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM) &&
                !diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_APIDIAGRAM))
            {
                MessageBox.Show("The diagram must be stereotyped as:" + APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM +" or "+APIAddinClass.EA_STEREOTYPE_APIDIAGRAM);
                return;
            }

            EA.Package samplePkg = Repository.GetPackageByID(Repository.GetCurrentDiagram().PackageID);
            EA.Package samplesPackage = Repository.GetPackageByID(samplePkg.ParentID);
            EA.Package apiPackage = Repository.GetPackageByID(samplesPackage.ParentID);

            EA.Collection diagramSelected = diagram.SelectedObjects;
            if (diagramSelected.Count < 1)
            {
                MessageBox.Show("Select the objects/samples to update from.");
                return;
            }
            foreach (object dia in diagramSelected)
            {
                EA.DiagramObject diagramObj = (EA.DiagramObject)dia;
                EA.Element sampleObject = Repository.GetElementByID(diagramObj.ElementID);

                f(Repository, sampleObject);
            }
        }


        /* In this method we are iterating over all selected elements in the diagram
         * and invoking the method to update the class from the sample.
         * 
         */
        static public void updateClassFromInstance(EA.Repository Repository)
        {
            EA.Diagram diagram = Repository.GetCurrentDiagram();

            if(!diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM))
            {
                MessageBox.Show("The diagram must be stereotyped as:" + APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM);
                return;
            }

            EA.Package samplePkg = Repository.GetPackageByID(Repository.GetCurrentDiagram().PackageID);
            EA.Package samplesPackage = Repository.GetPackageByID(samplePkg.ParentID);
            EA.Package apiPackage = Repository.GetPackageByID(samplesPackage.ParentID);

            EA.Collection diagramSelected = diagram.SelectedObjects;
            if (diagramSelected.Count < 1)
            {
                MessageBox.Show("Select the objects/samples to update from.");
                return;
            }
            foreach (object dia in diagramSelected)
            {
                EA.DiagramObject diagramObj = (EA.DiagramObject)dia;
                EA.Element sampleObject = Repository.GetElementByID(diagramObj.ElementID);

                updateClassFromSample(Repository, sampleObject);
            }
        }

        /*
         * In this method we are updating the default values on the class from the sample values stored in the sample instance 
         */
        static public void updateClassFromSample(EA.Repository Repository, EA.Element sampleObject)
        {
            //logger.log("Update Class From Sample");
            int clazzId=sampleObject.ClassifierID;            
            if(clazzId==0)
            {
                logger.log("Skipping class which doesnt have classifier");
            }
 
            EA.Element clazz = Repository.GetElementByID(clazzId);

            Dictionary<string, RunState> sampleState = ObjectManager.parseRunState(sampleObject.RunState);
            
            //new way to represent data items
            visitOutboundConnectedElements(Repository, clazz, sampleState, MetaDataManager.filterDataItem, syncDataItem);

            foreach (object attrO in clazz.Attributes)
            {                
                EA.Attribute attr = (EA.Attribute)attrO;
                
                if (sampleState.ContainsKey(attr.Name))
                {
                    string st = sampleState[attr.Name].value;
                    attr.Default = DomainValueManager.updateDefault(attr.Default, st);
                    //logger.log("Update Attribute[" + attr.Name+"] with new defaultvalue:"+attr.Default);
                    attr.Update();
                }
                
            }
        }

        static Object syncDataItem(Object context, EA.Element clazz, EA.Connector connector, EA.Element attr)
        {
            //logger.log("Syncing sample to data item");
            Dictionary<string, RunState> sampleState = (Dictionary<string, RunState>)context;            

            if (sampleState.ContainsKey(attr.Name))
            {
                string st = sampleState[attr.Name].value;

                EA.TaggedValue defVal = getAttributeDefault(attr);

                if (defVal != null) {
                    string newdefault = DomainValueManager.updateDefault(defVal.Value, st);
                    defVal.Value = newdefault;
                    defVal.Update();
                } else {

                    EA.TaggedValue attrTag = attr.TaggedValues.AddNew("", "");
                    attrTag.Name = APIAddinClass.EA_TAGGEDVALUE_DEFAULT;
                    attrTag.Value = st;
                    attrTag.Update();
                }                                                
            }
            return null;
        }

        static public EA.TaggedValue getAttributeDefault(EA.Element attr)
        {
            foreach (EA.TaggedValue da in attr.TaggedValues)
            {
                if (da.Name.Equals(APIAddinClass.EA_TAGGEDVALUE_DEFAULT))
                {
                    if (da.Value != null && da.Value.Length > 0)
                    {
                        return da;
                    }
                }
            }
            return null;
        }

        /* This method will update the sample instance from the class definition.
         * Renamed attributes are updated.
         * Sample values on the class replace 'Value' on the instance.
         */
        static public bool updateSampleFromClass(EA.Repository Repository,EA.Element sampleObject)
        {
            //logger.log("Update Sample From Class");
            int clazzId = sampleObject.ClassifierID;
            if (clazzId == 0)
            {
                logger.log("Skipping object which doesnt have classifier");
            }

            EA.Element clazz = Repository.GetElementByID(clazzId);

            Dictionary<string, RunState> sampleState = ObjectManager.parseRunState(sampleObject.RunState);
            Dictionary<string, RunState> updatedSampleState = new Dictionary<string, RunState>();

            foreach (RunState rs in sampleState.Values)
            {
                //logger.log("Processing Sample Property:" + rs.key);
                EA.Element attrnew = null;
                try{
                    attrnew = Repository.GetElementByID(int.Parse(rs.reference));
                }catch (Exception e){}
                EA.Attribute attr = null;
                try{
                    attr = Repository.GetAttributeByID(int.Parse(rs.reference));
                }catch (Exception e){}
                if(attr!=null || attrnew!=null)
                {
                    String nm = null;
                    if (attr != null) nm = attr.Name;
                    if (attrnew != null) nm = attrnew.Name;

                    String defVal = "Value";
                    if(attr!=null) defVal = attr.Default;
                    
                    if(attrnew!=null) {
                        EA.TaggedValue tv = getAttributeDefault(attrnew);
                        if(tv!=null)
                            defVal = tv.Value;
                     }

                     if (rs.value.Equals("Value"))
                     {
                            rs.value = DomainValueManager.selectDefault(defVal);
                     }

                     logger.log("Updating to property name:" + nm);
                     rs.key = nm;
                     updatedSampleState.Add(rs.key, rs);
                    // Element  container = Repository.GetElementByID(attr.ParentID);
                    //if(!container.Type.Equals(APIAddInClass.EA_TYPE_ENUMERATION))
                    //{
                    //    logger.log("Updating to property name:" + nm);
                    //    rs.key = nm;
                    //    updatedSampleState.Add(rs.key, rs);
                    //}
                    //else
                    //{
                    //    logger.log("Retaining:" + rs.key);
                    //    updatedSampleState.Add(rs.key, rs);
                    //}
                }
                else
                {
                    logger.log("Retaining:" + rs.key);
                    updatedSampleState.Add(rs.key, rs);
                }
            }

            sampleObject.RunState = ObjectManager.renderRunState(updatedSampleState);
            sampleObject.Update();
            return true;
        }

        static public void validateDiagram(EA.Repository Repository,EA.Diagram diagram)
        {
            try
            {
                //logger.log("Validate Schemas");

                IList<EA.Element> classes = MetaDataManager.diagramClasses(Repository, diagram);
                List<int> possibleRoots = new List<int>();

                IList<EA.Element> objects = MetaDataManager.diagramSamples(Repository, diagram);

                EA.Element root = null;
                foreach (EA.Element obj in objects)
                {
                    foreach (EA.Connector conn in obj.Connectors)
                    {
                        possibleRoots.Add(conn.SupplierID);
                    }
                }
                foreach (EA.Element clazz in classes)
                {
                    if (possibleRoots.Contains(clazz.ElementID))
                    {
                        //logger.log("Found root as related from object:");
                        root = clazz;
                    }
                }
                validateDiagram(Repository, classes, root);
            }
            catch (ModelValidationException e)
            {
                logger.log("**ValidationErrors**");
                foreach (string msg in e.errors.messages)
                {
                    logger.log(msg);
                }                
            }
            catch (System.Exception e)
            {
                throw e;
            }            
        }     

        static public void validateDiagram(EA.Repository Repository, IList<EA.Element> diagramClasses,EA.Element root)
        {            
            SchemaValidationManager v = new SchemaValidationManager();
            v.logger = logger;
            v.theRepository = Repository;
            v.theDiagramClasses = diagramClasses;
            v.validateSchema(root);

            if (v.validationErrors.hasAny())
                throw new ModelValidationException(v.validationErrors);
        }

        static public void visitOutboundConnectedElements(EA.Repository theRepository,EA.Element clientElement, Object context, System.Func<EA.Connector, EA.Element,EA.Element, bool> filter, System.Func<Object,EA.Element, EA.Connector, EA.Element,Object> processConnection)
        {
            //logger.log("Processing Connections from:" + clientElement.Name);
            foreach (EA.Connector con in clientElement.Connectors)
            {
                //logger.log("Processing Connector:" + con.Name);
                EA.Element supplierElement = null;
                if (clientElement.ElementID == con.ClientID)
                {
                    supplierElement = theRepository.GetElementByID(con.SupplierID);
                    //logger.log("Found supplier:" + supplierElement.Name);

                    EA.Element supplierClassifier = null;
                    if (supplierElement.ClassifierID != 0)
                        supplierClassifier = theRepository.GetElementByID(supplierElement.ClassifierID);

                    if (!filter(con,supplierElement,supplierClassifier))
                        continue;

                    //logger.log("Filtered");

                    processConnection(context,clientElement, con, supplierElement);                    
                }
            }
            //logger.log("Processed Connections from:" + clientElement.Name);
        }
    }

    public class SchemaValidationManager
        {

        public Logger logger;
        public IList<EA.Element> theDiagramClasses;
        public EA.Repository theRepository;
        public ValidationErrors validationErrors = new ValidationErrors();
        public Dictionary<string,string> classesValidated = new Dictionary<string,string>();

        public void validateSchema(EA.Element root)
        {                        
            validateClass(root);
        }

        public bool validateClass(EA.Element root)
        {
            if(classesValidated.ContainsKey(root.Name))
                return true;

            classesValidated.Add(root.Name,root.Name);

            //logger.log("Validating Class :" + root.Name);

            if (root.Stereotype.Contains(APIAddinClass.EA_STEREOTYPE_DATAITEM))
            {
                string firstLetter = root.Name.Substring(0, 1);
                if (!firstLetter.Equals(firstLetter.ToLower()))
                {
                    string msg = "Data Item Name needs to start with an lowercase character:" + root.Name;
                    logger.log(msg);
                    validationErrors.add(msg);
                }
                //data items can start with a lower case
                return true;
            }
            else
            {
                string firstLetter = root.Name.Substring(0, 1);
                if (!firstLetter.Equals(firstLetter.ToUpper()))
                {
                    string msg = "Class Name needs to start with an uppercase character:" + root.Name;
                    logger.log(msg);
                    validationErrors.add(msg);
                }
            }

            validateClassAttributes(root);
            visitOutboundConnectedElements(root, validateRelationship);

            return true;
        }
        public void validateClassAttributes(EA.Element root)
        {
            foreach(EA.Attribute attr in root.Attributes){
                if (attr.Name.Length == 0)
                {
                    validationErrors.add("Attribute Name is null in class:" + root.Name);
                }                
            }
        }

        public bool validateRelationship(EA.Element client, EA.Connector connector, EA.Element supplierElement)
        {
            //logger.log("Validating relationship ["+connector.Name+"] between:" + client.Name + " and " + supplierElement.Name);

            EA.Element supplierClassifier = null;
            if (supplierElement.ClassifierID != 0)
                supplierClassifier = theRepository.GetElementByID(supplierElement.ClassifierID);


            bool ondiagram=false;
            foreach (EA.Element cl in theDiagramClasses)
            {
                if (supplierElement.ElementID == cl.ElementID)
                    ondiagram = true;
            }
            if (!ondiagram)
            {
                //skip
                return true;
            }


            if (connector.SupplierEnd.Role.Length == 0 && (!supplierElement.Stereotype.Contains(APIAddinClass.EA_STEREOTYPE_DATAITEM)))
            {
                string msg = "Connector with empty SupplierEnd.Role is not allowed. Relationship between:" + client.Name + " and " + supplierElement.Name;
                logger.log(msg);
                validationErrors.add(msg);
            }

            if (connector.SupplierEnd.Cardinality == null || connector.SupplierEnd.Cardinality.Length == 0)
            {
                string msg = "Connector with no SupplierEnd.Role cardinality is not allowed. Relationship between:" + client.Name + " and " + supplierElement.Name;
                logger.log(msg);
                validationErrors.add(msg);
            }

            validateClass(supplierElement);

            return true;
        }

        public void visitOutboundConnectedElements(EA.Element clientElement, System.Func<EA.Element, EA.Connector, EA.Element, bool> processConnection)
        {
            //logger.log("Processing Connections from:" + clientElement.Name);
            foreach (EA.Connector con in clientElement.Connectors)
            {
                //logger.log("Processing Connector:" + con.Name);
                EA.Element supplierElement = null;
                if (clientElement.ElementID == con.ClientID)
                {
                    supplierElement = theRepository.GetElementByID(con.SupplierID);
                    //logger.log("Found supplier:" + supplierElement.Name);
            
                    processConnection(clientElement, con, supplierElement);
                }
            }
            //logger.log("Processed Connections from:" + clientElement.Name);
        }
    }

    public class ModelValidationException : System.Exception
    {
        public ValidationErrors errors;

        public ModelValidationException(ValidationErrors e)
        {
            errors = e;
        }
        public ModelValidationException(string error)
        {
            errors = new ValidationErrors();
            errors.messages.Add(error);
        }
    }




    /**
     * This class manages how attribute domain values are managed in the EA model.
     * Currently they are managed as a comma separate set of values stored in the Default/Initial value of the EA attribute.
     **/
    public class DomainValueManager
    {
        static System.Random rnd = new System.Random();

        static public string[] parseDefault(string initial)
        {
            char[] sep = { ',' };
            if (initial == null || initial.Length == 0)
                return new string[0];
            return initial.Split(sep);
        }
        static public string updateDefault(string initial, string newvalue)
        {
            string[] values = parseDefault(initial);
            string answer = initial;
            if (values.Length == 0)
                return newvalue;

            foreach (string s in values)
            {
                if (s.Equals(newvalue))
                    return initial;
            }

            if (values.Length > 10)
            {
                answer = "";
                for (int i = 1; i < 10; i++)
                {
                    answer += values[i] + ",";
                }

            }
            return initial + "," + newvalue;
        }

        static public string selectDefault(string initial)
        {
            string[] dfs = parseDefault(initial);

            int r = rnd.Next(0, dfs.Length);
            return dfs[r];
        }

        
        

    }
}
