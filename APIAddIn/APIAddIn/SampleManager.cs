using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace APIAddIn
{
    public class SampleManager
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
                
        public static void syncSample(EA.Repository Repository,EA.Diagram diagram)
        {
            logger.log("Sync Sample");
            IList<EA.Element> samples = MetaDataManager.diagramSamples(Repository,diagram);

            EA.Element container = container = findContainer(Repository, diagram);
            EA.Element containerClassifierEl = Repository.GetElementByID(container.ClassfierID);
            string containerName = container.Name;
            string containerClassifier = containerClassifierEl.Name;

           
            //logger.log("Sample Root:" + container.Name);

            EA.Package samplePkg = Repository.GetPackageByID(diagram.PackageID);
            EA.Package samplesPackage = Repository.GetPackageByID(samplePkg.ParentID);
            EA.Package apiPackage = Repository.GetPackageByID(samplesPackage.ParentID);
            if(fileManager!=null){
                fileManager.initializeAPI(apiPackage.Name);
                fileManager.setup(APIAddinClass.RAML_0_8);
                if (!fileManager.sampleExists(container.Name, containerClassifier))
                {
                    MessageBox.Show("No file exists at:" + fileManager.samplePath(container.Name,containerClassifier));
                    return;
                }
                else
                {
                    string fullpath = fileManager.samplePath(containerName,containerClassifier);
                    JObject jo = JObject.Parse(File.ReadAllText(fullpath));
                    sync_sample(Repository, container, jo);
                }
            }            
        }

        private static void sync_sample(EA.Repository Repository, EA.Element sample, JObject jo)
        {
            logger.log("Syncing:" + sample.Name);
            Dictionary<string, RunState> rs = ObjectManager.parseRunState(sample.RunState);
            Dictionary<string, RunState> nrs = new Dictionary<string, RunState>();

            foreach (JProperty p in jo.Properties())
            {                
                //string rsv=null;
                if (p.Value.Type != JTokenType.Object && p.Value.Type != JTokenType.Array)
                {
                    //logger.log("Adding Property:" + sample.Name);
                    RunState r;
                    if(rs.ContainsKey(p.Name)){
                        r = rs[p.Name];
                    }else {
                        r = new RunState();
                        r.key = p.Name;
                    }                    
                    r.value = p.Value.ToString();

                    nrs.Add(r.key, r);                    
                }                                  
            }
            
            sample.RunState = ObjectManager.renderRunState(nrs);
            logger.log(sample.RunState);
            sample.Update();

            foreach (EA.Connector con in sample.Connectors)
            {
                logger.log("Connector:" + con.SupplierEnd.Role);
                EA.Element related = null;
                
                if (sample.ElementID == con.ClientID)
                {                    
                    related = Repository.GetElementByID(con.SupplierID);

                    JProperty p = jo.Property(con.SupplierEnd.Role);
                    
                    if (p != null)
                    {
                        //logger.log("Found Json Property:" + con.SupplierEnd.Role);
                        if (p.Value.Type == JTokenType.Object)
                        {
                            JObject pjo = (JObject)p.Value;
                            sync_sample(Repository, related, pjo);
                        }
                        else if (p.Value.Type == JTokenType.Array)
                        {
                            JArray ja = (JArray)p.Value;
                            if (ja.Count > 0)
                            {
                                JToken t = ja.ElementAt(0);
                                ja.RemoveAt(0);
                                if (t.Type == JTokenType.Object)
                                {
                                    sync_sample(Repository, related, (JObject)t);
                                }
                                else
                                {
                                    MessageBox.Show("Arrays of types other than object not supported");
                                }
                            }                                                                                                                        
                        }
                    }                    
                }
            }
        }


        static public object convertEATypeToValue(string t,string value)
        {
            if (t.Equals(APIAddinClass.EA_TYPE_NUMBER) || t.Equals(APIAddinClass.EA_TYPE_FLOAT))
            {
                try
                {
                    return float.Parse(value);
                }
                catch (FormatException)
                {
                    return 0;// "Not a number:"+ value;
                }
            }            
            if (t.Equals(APIAddinClass.EA_TYPE_INT))
            {
                try
                {
                    return int.Parse(value);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            else if (t.Equals(APIAddinClass.EA_TYPE_DATE))
            {
                
                    return value;                
                
            }
            else if (t.Equals(APIAddinClass.EA_TYPE_BOOLEAN))
            {
                try
                {
                    return bool.Parse(value);
                }
                catch (FormatException)
                {
                    return false;
                }
                
            }
            else if (t.Equals(APIAddinClass.EA_TYPE_DECIMAL))
            {
                try
                {
                    return float.Parse(value);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            else
                return value;
        }

        static EA.Element findContainer(EA.Repository Repository, EA.Diagram diagram)
        {
            IList<EA.Element> samples = MetaDataManager.diagramSamples(Repository, diagram);
            foreach (EA.Element sample in samples)
            {                
                if (sample.Stereotype!=null && sample.Stereotype == APIAddinClass.EA_STEREOTYPE_SAMPLE)
                {
                    logger.log("Sample is identified by SAMPLE classified root");

                    if (sample.Connectors.Count != 1)
                    {
                        MessageBox.Show("root Sample should refer to a single class");
                        break;
                    }
                    else
                    {
                        int id = sample.Connectors.GetAt(0).SupplierID;
                        EA.Element root = Repository.GetElementByID(id);
                        return root;
                    }
                }
            }

            //Deprecated mechanism
            foreach (EA.Element sample in samples){
                 if (sample.Stereotype==APIAddinClass.EA_STEREOTYPE_REQUEST) {
                    return sample;
                 }
            }

            
            //MessageBox.Show("object stereotyped as Sample needs to be linked to schema root");
            throw new ModelValidationException("object stereotyped as Sample needs to be linked to schema root");                        
        }

        //static public KeyValuePair<string,JObject>  sampleToJObject(EA.Repository Repository,EA.Diagram diagram)
        static public Hashtable sampleToJObject(EA.Repository Repository, EA.Diagram diagram)
        {
            Hashtable result = new Hashtable();

            IList<EA.Element> clazzes = MetaDataManager.diagramClasses(Repository, diagram);

            IList<EA.Element> samples = MetaDataManager.diagramSamples(Repository,diagram);

            EA.Element root = findContainer(Repository, diagram);
            
            
            EA.Element rootClassifier = Repository.GetElementByID(root.ClassifierID);

            Dictionary<int, JObject> instances = new Dictionary<int, JObject>();
            JObject container =  new JObject();
            string containerName = root.Name;
            string containerClassifier = rootClassifier.Name;
             
             instances.Add(root.ElementID, container);
            
            foreach (EA.Element sample in samples)
            {
                //logger.log("Sample Name:" + sample.Name+"\t"+sample.ElementID);

                if (sample.Stereotype == APIAddinClass.EA_STEREOTYPE_SAMPLE)
                    continue;

                EA.Element clazz = null;
                if (sample.ClassifierID != 0)
                {
                    clazz = Repository.GetElementByID(sample.ClassifierID);
                } else {
                    logger.log("Classifier is null");
                }

                JObject jsonClass=null;

                        
                if (!instances.TryGetValue(sample.ElementID, out jsonClass))
                {
                    jsonClass = new JObject();
                    instances.Add(sample.ElementID, jsonClass);
                }
                                                                           
                string rs = sample.RunState;
                                
                // Loop through all attributes in run state and add to json
                Dictionary<string, RunState> runstate = ObjectManager.parseRunState(rs);
                foreach (string key in runstate.Keys)
                {
                    logger.log("Adding property:" + key + " =>" + runstate[key].value);
                    object o = runstate[key].value;

                    // Find classifier attribute specified in run state
                    string attrType = null;
                    string attrUpperBound = null;
                    if (clazz != null) {
                        foreach (EA.Attribute a in clazz.Attributes)
                        {
                            if (a.Name.Equals(key))
                            {
                                attrType = a.Type;
                                attrUpperBound = a.UpperBound;
                                break;
                            }
                        }

                        // Check if attribuite is defined as related enumeration. When cardinaltity is 0..* then set the attribute cardinality so we serialize as an array
                        foreach (EA.Connector con in clazz.Connectors)
                        {
                            // Check relation is named the same as the run state attribute name and is an enumeration
                            EA.Element related = Repository.GetElementByID(con.SupplierID);
                            if (con.SupplierEnd.Role == key && related.Type == APIAddinClass.EA_TYPE_ENUMERATION)
                            {
                                //if (con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_0_TO_MANY))
                                //{
                                    //logger.log("  matching enum with 0..*:" + con.SupplierEnd.Cardinality);
                                //}
                                attrType = related.Type;
                                attrUpperBound = con.SupplierEnd.Cardinality;
                                break;
                            }
                        }

                        // Check if attribute is defined as related DataItem
                        foreach (EA.Connector con in clazz.Connectors)
                        {
                            // Check relation is named the same as the run state attribute name and is an enumeration
                            EA.Element related = Repository.GetElementByID(con.SupplierID);
                            if (con.SupplierEnd.Role == key && related.Stereotype == APIAddinClass.EA_STEREOTYPE_DATAITEM)
                            {
                                attrType = SchemaManager.getDataItemType(related);                                
                                attrUpperBound = con.SupplierEnd.Cardinality;
                                break;
                            }
                        }

                    }

                    // Add attribute to json as either value or array
                    if (attrType != null)
                    {
                        //logger.log("  upper bound:" + key + " =>" + attrUpperBound);
                        if (attrUpperBound.Equals("*") || attrUpperBound.Equals(APIAddinClass.CARDINALITY_0_TO_MANY))
                        {
                            // Create array and split values separated by commas
                            JArray ja = new JArray();
                            foreach (string value in runstate[key].value.Split(','))
                            {
                                o = convertEATypeToValue(attrType, value);
                                ja.Add(o);
                            }
                            jsonClass.Add(new JProperty(key, ja));
                        }
                        else
                        {
                            // Not array so convert and add attribute and formatted value
                            o = convertEATypeToValue(attrType, runstate[key].value);
                            logger.log("Attr:" + attrType + " " + o.ToString());
                            jsonClass.Add(new JProperty(key, o));
                        }
                    }
                    else
                    {
                        // No classifier found so add as object serialized as string
                        logger.log("Attr:" + key + "-" + o.ToString());
                        jsonClass.Add(new JProperty(key, o));
                    }                                            
                }
            }

            logger.log("Export container:" + containerName);

            foreach (EA.Element clazz in samples)
            {
                   
                JObject jsonClass =null;
                if (!instances.TryGetValue(clazz.ElementID, out jsonClass))
                    continue;
                if (jsonClass != null)
                {
                    logger.log("Found jsonClass:" + clazz.Name);
                    foreach (EA.Connector con in clazz.Connectors)
                    {
                        //logger.log("Found connector:");
                        EA.Element related = null;
                        if (clazz.ElementID == con.ClientID)
                        {
                            related = Repository.GetElementByID(con.SupplierID);

                            try
                            {
                                object o = instances[related.ElementID];
                            }
                            catch (KeyNotFoundException)
                            {
                                //Object is in package but not on the diagram
                                continue;
                            }
                                                       
                            if (related != null && instances[related.ElementID] != null)
                            {

                                if (con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_0_TO_MANY) ||
                                    con.SupplierEnd.Cardinality.Equals(APIAddinClass.CARDINALITY_1_TO_MANY)
                                )
                                {
                                    //logger.log("Found array");
                                    
                                    string propertyName = related.Name;
                                    //Override with the connection supplier end
                                    try{
                                        if(con.SupplierEnd.Role.Length>0)
                                            propertyName = con.SupplierEnd.Role;
                                    }catch (Exception) {  }

                                    JProperty p = jsonClass.Property(propertyName);
                                    if (p == null){
                                        JArray ja = new JArray();
                                        ja.Add(instances[related.ElementID]);
                                        //logger.log("Adding array property:"+ related.Name);   
                                        jsonClass.Add(new JProperty(propertyName, ja));                                       
                                    }else {
                                        JArray ja = (JArray)p.Value;
                                        //logger.log("Adding to array property");   
                                        ja.Add(instances[related.ElementID]);
                                    }                                     
                                }
                                else
                                {
                                    string propertyName = related.Name;
                                    //Override with the connection supplier end
                                        try {
                                            if(con.SupplierEnd.Role.Length>0)
                                                propertyName = con.SupplierEnd.Role;
                                        }catch(Exception){ }                                    
                                    //logger.log("Adding property:" + related.Name);
                                    jsonClass.Add(new JProperty(propertyName, instances[related.ElementID]));
                                }
                                
                            }
                        }
                    }
                }
            }

            //KeyValuePair<string,JObject> kv = new KeyValuePair<string,JObject>(containerName,container);            
            //return kv;

            //logger.log("REturning result");
            result.Add("sample", containerName);
            result.Add("class", containerClassifier);
            result.Add("json", container);
            return result;
        }

        static public void exportSample(EA.Repository Repository, EA.Diagram diagram)
        {
            Hashtable ht = sampleToJObject(Repository, diagram);
            string sample = (string)ht["sample"];
            string clazz = (string)ht["class"];
            JObject container = (JObject)ht["json"];


            if (!diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM))
            {
                logger.log("exportSample: Ignore diagam that isnt a sample diagram");
                return;
            }

            //KeyValuePair<string,JObject> kv = sampleToJObject(Repository, diagram);
            //JObject container = kv.Value;

            if(container==null){
                MessageBox.Show("No object linked to root with classification sample declared nor  (older style) object of classification Request declared");
                return;
            }
                
                string msg = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented) + "\n";
                EA.Package samplePkg = Repository.GetPackageByID(diagram.PackageID);            
                EA.Package samplesPackage = Repository.GetPackageByID(samplePkg.ParentID);
                EA.Package apiPackage = Repository.GetPackageByID(samplesPackage.ParentID);

                string sourcecontrolPackage = apiPackage.Name;
                if (MetaDataManager.isCDMPackage(Repository,apiPackage))
                {
                    sourcecontrolPackage = "cdm";
                }
            
                if (fileManager != null)
                {
                    fileManager.initializeAPI(sourcecontrolPackage);
                    fileManager.setup(APIAddinClass.RAML_0_8);
                    fileManager.exportSample(sample,clazz, msg);
                }            
        }        

        ///
        /// Validate all object run state keys correspond to classifier attributes
        ///
        static public void validateDiagram(EA.Repository Repository,EA.Diagram diagram)
        {                        
            IList<string> messages = diagramValidation(Repository,diagram);

            logger.log("**ValidationResults**");
            if(messages!=null)
            {                
                foreach (string m in messages)
                {
                    logger.log(m);                    
                }                                
            }                        
        }

        static public IList<string> diagramValidation(EA.Repository Repository, EA.Diagram diagram)
        {
            JSchema jschema = null;
            JObject json = null;
            try
            {
                //logger.log("Validate Sample");
                json = (JObject)sampleToJObject(Repository, diagram)["json"];

                //logger.log("JObject formed");
            
                EA.Package samplePkg = Repository.GetPackageByID(diagram.PackageID);            
                EA.Package samplesPackage = Repository.GetPackageByID(samplePkg.ParentID);            
                EA.Package apiPackage = Repository.GetPackageByID(samplesPackage.ParentID);
            
                EA.Package schemaPackage = null;
            
                foreach (EA.Package p in apiPackage.Packages)
                {                
                    if (p!=null && p.Name.Equals(APIAddinClass.API_PACKAGE_SCHEMAS))
                    {
                        schemaPackage = p;
                    }
                }
                if (schemaPackage == null)
                {
                    throw new Exception("No Schema package found");                
                }
                            
                EA.Diagram schemaDiagram = null;            
                foreach (EA.Diagram d in schemaPackage.Diagrams)
                {
                    if (d.Stereotype != null && d.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM))
                    {
                        schemaDiagram = d;
                    }
                }

                
            
                jschema = SchemaManager.schemaToJsonSchema(Repository, schemaDiagram).Value;
            }
            catch (ModelValidationException ex)
            {
                return ex.errors.messages;
            }
                        
            IList<string> messages;

            if (!json.IsValid(jschema, out messages))
            {
                logger.log("Sample is not valid:");
                return messages;
            }
            else{
                logger.log("Sample is Valid!");
                return null;
            }
                
        }

    }
}
