using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;

namespace APIAddIn
{
    /* This class deals with serialization of API */
    public class APIManager
    {        

        public static string TITLE = "title";
        public static string BASEURI = "baseuri";
        public static string VERSION = "version";
        public static string MEDIATYPE = "media-type";
        public static string RESOURCETYPEs = "resourceTypes";
        public static string SCHEMAS = "schemas";
        public static string DOCUMENATION = "documentation";
        public static string TRAITS = "traits";

        static Dictionary<string, string> schemaMapping = new Dictionary<string, string>();

        static Logger logger = new Logger();

        static FileManager fileManager = new FileManager(logger);

        static public void setLogger(Logger l)
        {
            logger = l;
        }

        static public void setFileManager(FileManager fm)
        {
            fileManager = fm;
        }

        static string nameOfTargetElement(EA.Connector con, EA.Element e, EA.Element client)
        {
            return e.Name;
        }

        

        static public YamlMappingNode exportAPI(EA.Repository Repository, EA.Diagram diagram)
        {                        
            logger.log("Exporting an API ");
            EA.Element apiEl = MetaDataManager.diagramAPI(Repository,diagram);

            logger.log("2");

            DiagramManager.captureDiagramLinks(diagram);
                        
            YamlMappingNode map = new YamlMappingNode();
            
            reifyAPI(Repository, apiEl, map);

            YamlStream stream = new YamlStream();
            YamlDocument yamlDoc = new YamlDocument(map);            
            stream.Documents.Add(yamlDoc);

            StringWriter writer = new StringWriter();
            stream.Save(writer,false/*no alias*/);

            string yaml = "#%RAML 0.8\n" + writer.ToString();

            EA.Package apiPackage = Repository.GetPackageByID(diagram.PackageID);

            fileManager.initializeAPI(apiPackage.Name);
            fileManager.setup();
            fileManager.exportAPI(apiPackage.Name, yaml);
            return map;
        }

        public static YamlMappingNode reifyAPI(EA.Repository Repository, EA.Element apiEl, YamlMappingNode map)
        {            
            schemaMapping.Clear();

            logger.log("Reify API:" + apiEl.Name);

            logger.log("API Runstate:" + apiEl.RunState);
            Dictionary<string, RunState> rs = ObjectManager.parseRunState(apiEl.RunState);
            foreach (string key in rs.Keys)
            {
                logger.log("Adding:" + key + "=>" + rs[key].value);
                map.Add(key, rs[key].value);
            }

            try
            {
                map.Add("baseUri", "https://{environment}");
            }catch(Exception e){
                //do nothing is fine
            }            

            YamlMappingNode bps = new YamlMappingNode();
            map.Add("baseUriParameters", bps);
            {
                YamlMappingNode environment = new YamlMappingNode();
                bps.Add("environment", environment);

                YamlSequenceNode environments = new YamlSequenceNode();
                visitOutboundConnectedElements(Repository, apiEl, environments, MetaDataManager.filterReleasePipeline, nameMethod, reifyReleasePipelne);

                string envs = "[";
                if(environments.Children.Count==0)
                {
                    envs = "[ ]";                    
                }
                else
                {
                    foreach (YamlScalarNode env in environments)
                    {
                        envs += env.Value+",";                        
                    }
                    envs = envs.Substring(0, envs.Length - 1);
                    envs += "]";
                }
                YamlScalarNode d = new YamlScalarNode(envs);
                d.Style = ScalarStyle.Raw;
                environment.Add("enum", d);
            }
                                
            YamlSequenceNode sn = new YamlSequenceNode();
            {
                YamlMappingNode dn = new YamlMappingNode();
                sn.Add(dn);
                dn.Add("title", "Description");
                if(apiEl.Notes!=null)
                    dn.Add("content", apiEl.Notes);
            }
            {
                YamlMappingNode dn = new YamlMappingNode();
                sn.Add(dn);
                dn.Add("title", "History");
                YamlScalarNode d = new YamlScalarNode("!include documentation/history.md");
                d.Style = ScalarStyle.Raw;
                dn.Add("content", d);
            }
            {
                YamlMappingNode dn = new YamlMappingNode();
                sn.Add(dn);
                dn.Add("title", "Effort");
                YamlScalarNode d = new YamlScalarNode("!include documentation/effort.md");
                d.Style = ScalarStyle.Raw;
                dn.Add("content", d);
            }         
            map.Add("documentation", sn);

            
            visitOutboundConnectedElements(Repository, apiEl, map, MetaDataManager.filterSecurity, nameSecurity, reifySecurity);
            

            YamlScalarNode t = new YamlScalarNode("!include traits.raml");
            t.Style = ScalarStyle.Raw;
            map.Add("traits", t);

            logger.log("Reify Resource Type");

            visitOutboundConnectedElements(Repository, apiEl, map, MetaDataManager.filterCommunity, nameResourceTypes, reifyResourceTypes);
                        

            //Dictionary<string, object> schemas = new Dictionary<string, object>();
            YamlSequenceNode schemas = new YamlSequenceNode();

            reifyInfoSchema(schemas);

            visitOutboundConnectedElements(Repository, apiEl, schemas, MetaDataManager.filterSchema, nameSupplierRole, reifySchema);

            map.Add("schemas", schemas);


            visitOutboundConnectedElements(Repository, apiEl, map, MetaDataManager.filterResource, nameMethod, reifyResource);

            return map;
        }

        static YamlNode reifySecurity(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            logger.log("Reify Security:"+ e.Name+"-"+e.Version);            
            YamlScalarNode security = new YamlScalarNode("!include " + e.Version);
            security.Style = ScalarStyle.Raw;
            return security;
        }

        static void reifyInfoSchema(YamlSequenceNode schemas)
        {
            schemaMapping.Add("ErrorResponse", "infoSchema");
            {
                YamlScalarNode inc = new YamlScalarNode("!include " + fileManager.schemaIncludePath("ErrorResponse"));
                inc.Style = ScalarStyle.Raw;
                YamlMappingNode nd = new YamlMappingNode();
                nd.Add("infoSchema", inc);
                schemas.Add(nd);                
            }          
        }

        static YamlMappingNode reifyMethod(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            logger.log("Reify Method:" + e.Name);
            YamlMappingNode methodProps = new YamlMappingNode();

            if (e.Notes!=null && e.Notes.Length>0)
                methodProps.Add("description", e.Notes);

            Dictionary<string, RunState> rs = ObjectManager.parseRunState(e.RunState);
            foreach (string key in rs.Keys)
            {
                try
                {
                    methodProps.Add(key, rs[key].value);
                }
                catch (Exception ex)
                {
                    //ignore on purpose
                }                
            }
            

            YamlMappingNode responses = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, responses, MetaDataManager.filterResponse, nameOfTargetElement, reifyResponse);
            if (responses.Children.Count > 0)
            {
                methodProps.Add("responses", responses);
            }

            YamlMappingNode queryParameters = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, queryParameters, MetaDataManager.filterQueryParameter, nameOfTargetElement, reifyQueryParameter);
            if (queryParameters.Children.Count > 0)
            {
                methodProps.Add("queryParameters", queryParameters);
            }

            YamlMappingNode contentTypes = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, contentTypes, MetaDataManager.filterContentType, nameOfTargetElement, reifyContentType);
            if (contentTypes.Children.Count > 0)
            {
                methodProps.Add("body", contentTypes);
            }

            String traits = "[";
            YamlSequenceNode traitsMap = new YamlSequenceNode();            
            visitOutboundConnectedElements(Repository, e, traitsMap, MetaDataManager.filterTrait, nameTrait, reifyTrait);
            if (traitsMap.Children.Count > 0)
            {
                foreach (YamlNode node in traitsMap.Children)
                {
                    YamlScalarNode sn = (YamlScalarNode)node;
                    String trait = sn.ToString();
                    traits += trait + ",";
                }
                traits = traits.Substring(0, traits.Length - 1);
                traits += "]";
                YamlScalarNode traitNode = new YamlScalarNode(traits);
                traitNode.Style = ScalarStyle.Raw;
                methodProps.Add("is", traitNode);
            }            
            return methodProps;
        }

        static YamlSequenceNode reifyReleasePipelne(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            YamlSequenceNode map = new YamlSequenceNode();
            visitOutboundConnectedElements(false,Repository, e, map, MetaDataManager.filterEnvironment, nameMethod, reifyEnvironment);
            return map;
        }

        static string nameTrait(EA.Connector con, EA.Element e, EA.Element client)
        {
            return e.Name;
        }

        static YamlScalarNode reifyTrait(EA.Repository Repository, EA.Element e, EA.Connector con,EA.Element client)
        {
            return e.Name;            
        }

        static YamlScalarNode reifyEnvironment(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            return e.Name;
        }

        static YamlMappingNode reifyQueryParameter(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            YamlMappingNode props = new YamlMappingNode();

            props.Add("description", e.Notes);

            string r = e.RunState;
            Dictionary<string, RunState> rs = ObjectManager.parseRunState(r);
            foreach (string key in rs.Keys)
            {                
                try
                {
                    props.Add(key, rs[key].value);
                }
                catch (Exception ex)
                {
                    logger.log("Problem with adding QueryParameter RunState. The [" + key + "] already exists");
                }                
            }
            return props;
        }

        static YamlMappingNode reifyResponse(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            YamlMappingNode body = new YamlMappingNode();
            YamlMappingNode props = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, props, MetaDataManager.filterContentType, nameOfTargetElement, reifyContentType);
            if (props.Children.Count > 0)
            {
                body.Add("body", props);
            }
            return body;
        }

        static YamlMappingNode reifyContentType(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {            
            {//new way
                YamlMappingNode props = new YamlMappingNode();
                YamlMappingNode props2 = new YamlMappingNode();                
                logger.log("reifyExamplesForContentType");
                visitOutboundConnectedElements(Repository, e, props2, MetaDataManager.filterSample, nameSupplierRole, reifyExample);
                if(props2.Children.Count>0){
                    props.Add("examples", props2);
                    return props;
                }                
            }
            {//Old Way
                YamlMappingNode props = new YamlMappingNode();
                YamlMappingNode props2 = new YamlMappingNode();
                props.Add(e.Name, props2);
                logger.log("reifyExampleForContentType");
                visitOutboundConnectedElements(Repository, e, props2, MetaDataManager.filterObject, nameSupplierRole, reifyExample);
                return props;
            }            
        }

        static YamlNode reifyExample(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            EA.Element classifier = Repository.GetElementByID(e.ClassifierID);
            YamlScalarNode node = new YamlScalarNode("!include " + fileManager.sampleIncludePath(e.Name,classifier.Name));
            node.Style = ScalarStyle.Raw;
            return node;
        }

        static YamlNode reifyExamples(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            EA.Element classifier = Repository.GetElementByID(e.ClassifierID);
            YamlScalarNode node = new YamlScalarNode("!include " + fileManager.sampleIncludePath(e.Name,classifier.Name));
            node.Style = ScalarStyle.Raw;
            return node;
        }

        static string nameResource(EA.Element e)
        {
            return e.Name;
        }

        static string nameUriParameter(EA.Connector con, EA.Element supplier, EA.Element client)
        {
            if (!client.Name.Contains("{"))
                return null;

            int start = client.Name.IndexOf("{") + 1;
            int end = client.Name.IndexOf("}");
            string param = client.Name.Substring(start, end - start);
            return param;
        }

        static YamlNode reifyUriParameter(EA.Repository Repository, EA.Element e, EA.Connector con,EA.Element client)
        {
            if (!client.Name.Contains("{"))
                return null;
                  
            YamlMappingNode props = new YamlMappingNode();

            if(e.Notes!=null)
                props.Add("description", e.Notes);

            if (e.Stereotype != null)
            {                
                string type = e.GetStereotypeList();
                logger.log("URIPARAM:" + type);
                type = type.Replace(APIAddinClass.EA_STEREOTYPE_DATAITEM, "");
                type = type.Replace(",", "");

                props.Add("type", type);
            }

            foreach (EA.TaggedValue da in e.TaggedValues)
            {
                if (da.Name.Equals(APIAddinClass.EA_TAGGEDVALUE_DEFAULT))
                {
                    if (da.Value != null && da.Value.Length > 0)
                        logger.log("URIPARAM:Example" + da.Value);
                        props.Add("example",da.Value);
                }
            }
         
            return props;
        }

        static YamlMappingNode reifyResource(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {            
            logger.log("Reify Resources:" + e.Name);
            YamlMappingNode resourceProps = new YamlMappingNode();
            string r = e.RunState;
            Dictionary<string, RunState> rs = ObjectManager.parseRunState(r);
            foreach (string key in rs.Keys)
            {
                resourceProps.Add(key, rs[key].value);
            }
            
            if(e.Notes!=null && e.Notes.Length>0)
                resourceProps.Add("description", e.Notes);

            YamlMappingNode uriParamMap = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, uriParamMap, MetaDataManager.filterDataItem, nameUriParameter, reifyUriParameter);            
            resourceProps.Add("uriParameters", uriParamMap);


            visitOutboundConnectedElements(Repository, e, resourceProps, MetaDataManager.filterResource, nameMethod, reifyResource);

            YamlMappingNode types = new YamlMappingNode();


            visitOutboundConnectedElements(Repository, e, types, MetaDataManager.filterTypeForResource, nameMethod, reifyTypeForResource);

            if (types.Children.Count > 0) {
                resourceProps.Add("type", types);
            }

            YamlMappingNode method = new YamlMappingNode();
            visitOutboundConnectedElements(Repository, e, resourceProps, MetaDataManager.filterMethod, nameMethod, reifyMethod);                                
                        
            logger.log("Reified Resources:" + e.Name);
            return resourceProps;
        }        

        static YamlNode reifyTypeForResource(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {           
            YamlMappingNode specificType = new YamlMappingNode();


            { 
            YamlScalarNode node = new YamlScalarNode("infoSchema");
            node.Style = ScalarStyle.Raw;
            specificType.Add("infoSchema", node);
            }

            if (e.Name.Equals(APIAddinClass.RESOURCETYPE_ITEMPOST) || e.Name.Equals(APIAddinClass.RESOURCETYPE_ITEMPOST_ONEWAY) || e.Name.Equals(APIAddinClass.RESOURCETYPE_COLLECTIONGETPOST))
            {
                YamlScalarNode node = new YamlScalarNode("infoSchema");
                node.Style = ScalarStyle.Raw;
                specificType.Add("postInfoSchema", node);
            }

            logger.log("Reify Type for Resource:" + e.Name);
            visitOutboundConnectedElements(Repository, e, specificType, MetaDataManager.filterClass, nameSupplierRole, reifyTypeReference);
           
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample400BadRequest-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample400Resp", node);
            
            }
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample401-Unauthorized-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample401Resp", node);
            }  
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample403-Forbidden-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample403Resp", node);
            }  


            if(!e.Name.Equals(APIAddinClass.RESOURCETYPE_ITEMPOST_SYNC) && !e.Name.Equals(APIAddinClass.RESOURCETYPE_ITEMPOST_ONEWAY))
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample404Resp-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample404Resp", node);
            }            
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample405-MethodNotAllowed-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample405Resp", node);
            }
            {
                YamlScalarNode node = new YamlScalarNode("!include samples/sample406-NotAcceptable-sample.json");
                node.Style = ScalarStyle.Raw;
                specificType.Add("sample406Resp", node);
            }


            visitOutboundConnectedElements(Repository, e, specificType, MetaDataManager.filterObject, nameSupplierRole, reifyTypeSample);            

            logger.log("End Reify Type for Resource:" + e.Name);
            return specificType;
        }

        static YamlScalarNode reifyTypeReference(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            logger.log("Reify Type Reference:" + e.Name);    
        
            if(!schemaMapping.ContainsKey(e.Name))
            {
                System.Windows.Forms.MessageBox.Show("Schema [" + e.Name + "] is not referenced by the API");
                return null;
            }

            string reference = schemaMapping[e.Name];
            YamlScalarNode node = new YamlScalarNode(reference);
            node.Style = ScalarStyle.Raw;
            return node;            
        }

        static YamlScalarNode reifyTypeSample(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {                
            EA.Element classifier = Repository.GetElementByID(e.ClassifierID);

            if (classifier.Name == APIAddinClass.METAMODEL_PLACEHOLDER)
            {
                YamlScalarNode node = new YamlScalarNode("!include " + fileManager.samplePlaceholderPath(e.Name));
                node.Style = ScalarStyle.Raw;
                return node;
            }
            else
            {
                YamlScalarNode node = new YamlScalarNode("!include " + fileManager.sampleIncludePath(e.Name, classifier.Name));
                node.Style = ScalarStyle.Raw;
                return node;
            }                        
        }


        static YamlScalarNode reifyResourceTypes(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {
            logger.log("Reify ResourceTypes:" + e.Name + "-" + e.Version);
            YamlScalarNode security = new YamlScalarNode("!include " + e.Version);
            security.Style = ScalarStyle.Raw;
            return security;

           // if (e.ClassifierID == 0)
           // {
           //     return null;
           // }
           // string className = Repository.GetElementByID(e.ClassifierID).Name;
           // if (!className.Equals(APIAddinClass.METAMODEL_RESOURCETYPE))
           // {
           //     return null;
           // }
           //logger.log("Reify ResourceType:" + e.Name);

           // YamlScalarNode node = new YamlScalarNode("!include " + fileManager.resourceTypeIncludePath(e.Name));
           // node.Style = ScalarStyle.Raw;
           // return node;
        }


        static YamlNode reifySchema(EA.Repository Repository, EA.Element e, EA.Connector con, EA.Element client)
        {            
            logger.log("Reify Schema:" + e.Name);

            YamlMappingNode map = new YamlMappingNode();            

            YamlScalarNode inc = new YamlScalarNode("!include " + fileManager.schemaIncludePath(e.Name));
            inc.Style = ScalarStyle.Raw;
            
            map.Add(con.SupplierEnd.Role,inc);

            schemaMapping.Add(e.Name, con.SupplierEnd.Role);

            return map;
        }


       

        static string nameMethod(EA.Connector con, EA.Element e,EA.Element client)
        {
            return e.Name;
        }

        static string nameSecurity(EA.Connector con, EA.Element e, EA.Element client)
        {
            return "securitySchemes";            
        }

        static string nameResourceTypes(EA.Connector con, EA.Element e, EA.Element client)
        {
            return "resourceTypes";
        }

        static string nameSupplierRole(EA.Connector con, EA.Element e, EA.Element client)
        {
            EA.ConnectorEnd end = con.SupplierEnd;
            return end.Role;
        }

        static void visitOutboundConnectedElements( EA.Repository Repository, EA.Element clientElement, YamlNode parent, Func<EA.Connector, EA.Element, EA.Element, bool> filter, Func<EA.Connector, EA.Element,EA.Element, string> name, Func<EA.Repository, EA.Element, EA.Connector, EA.Element, YamlNode> properties)
        {
            visitOutboundConnectedElements(true,Repository, clientElement, parent, filter, name, properties);
        }

        static void visitOutboundConnectedElements(bool ensureVisible, EA.Repository Repository, EA.Element clientElement, YamlNode parent, Func<EA.Connector, EA.Element, EA.Element, bool> filter, Func<EA.Connector, EA.Element, EA.Element,string> name, Func<EA.Repository, EA.Element, EA.Connector, EA.Element, YamlNode> properties)
        {
            //logger.log("Processing Connections from:" + clientElement.Name);           
            foreach (EA.Connector con in clientElement.Connectors)
            {
                //logger.log("Processing Connector:" + con.Name);

                if(ensureVisible)
                    if (!DiagramManager.isVisible(con))
                        continue;

                EA.Element supplierElement = null;
                if (clientElement.ElementID == con.ClientID)
                {
                    supplierElement = Repository.GetElementByID(con.SupplierID);
                    //logger.log("Found resource:" + supplierElement.Name);

                    EA.Element supplierClassifier = null;
                    if (supplierElement.ClassifierID != 0)
                        supplierClassifier = Repository.GetElementByID(supplierElement.ClassifierID);

                    //logger.log("Classifier");
                    if (!filter(con, supplierElement, supplierClassifier))
                        continue;

                    //logger.log("Filtered");

                    String nm = name(con, supplierElement, clientElement);

                    YamlNode o = properties(Repository, supplierElement, con,clientElement);
                    if (o == null)
                    {
                        continue;
                    }
                    else if (parent.GetType().Name.StartsWith("YamlSequenceNode"))
                    {
                        YamlSequenceNode seq = (YamlSequenceNode)parent;
                        if (o.GetType().Name.StartsWith("YamlSequenceNode"))
                        {
                            YamlSequenceNode oseq = (YamlSequenceNode)o;
                            foreach(YamlNode n in oseq.Children){
                                seq.Add(n);
                            }
                        }
                        else
                        {
                            seq.Add(o);                                                
                        }                        
                    }
                    else if (parent.GetType().Name.StartsWith("YamlMappingNode"))
                    {
                        if (nm == null)
                            continue;
                        YamlMappingNode map = (YamlMappingNode)parent;
                        try {
                            map.Add(nm, o);        
                        }catch (Exception ex){
                            throw new Exception("Duplicate entry:"+ nm,ex);                            
                        }                                                                                                    
                    }
                }
            }
            //logger.log("Processed Connections from:" + clientElement.Name);
        }
        

        static public void validateDiagram(EA.Repository Repository, EA.Diagram diagram)
        {



        }
    }
}
