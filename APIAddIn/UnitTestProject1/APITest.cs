using System.Collections.Generic;
using System.IO;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using UnitTestProject1.EAFacade;
using UnitTestProject1.APIModels;

using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace UnitTestProject1
{
    [TestClass]
    public class APITest
    {
        [TestMethod]
        public void TestConvert()
        {
            DateTime dt =Convert.ToDateTime("2016-01-01");


            DateTime dt2 = Convert.ToDateTime("2016-01-01T00:00:00Z");

            DateTime dt3 =  dt2.ToLocalTime();


            //Assert.AreEqual(dt,dt2);
            Assert.AreEqual(dt3, dt2);

    
        }

        [TestMethod]
        public void TestYaml()
        {
            YamlMappingNode seq = new YamlMappingNode();

            YamlScalarNode n = new YamlScalarNode("#0.8Raml");
           n.Style = ScalarStyle.Raw;

           seq.Add("#0.8Raml","");

            YamlStream stream = new YamlStream();
            YamlDocument doc = new YamlDocument(seq);
            stream.Documents.Add(doc);

            StringWriter writer = new StringWriter();
            stream.Save(writer);

            string yaml = writer.ToString();
            Assert.IsNotNull(yaml);
        }


        [TestMethod]
        public void TestSerializeYaml()
        {

            //string fr = File.ReadAllText(fullpath);

            //StreamReader reader = new StreamReader("f:/generated/UnitTest/APITitle.raml");
            //YamlStream stream = new YamlStream();
            //stream.Load(reader);

            YamlScalarNode node = new YamlScalarNode("!include foo.raml");

            node.Style = ScalarStyle.Raw;
            //node.Tag = "tag:yaml.org,2002:null";

            //node.Value = "!includ2 foo2.raml";
            //node.Anchor = "!include";

            YamlMappingNode map = new YamlMappingNode();
            ////map.Style = YamlDotNet.Core.Events.MappingStyle.Flow;

            YamlSequenceNode seq = new YamlSequenceNode();
            //seq.Add(node);
                       
            map.Add("api", node);
            
            YamlStream stream = new YamlStream();            
            YamlDocument doc = new YamlDocument(map);
            stream.Documents.Add(doc);

            StringWriter writer = new StringWriter();
            stream.Save(writer);

            string yaml = writer.ToString();            
        }

        [TestMethod]
        public void TestAPI1()
        {
            EAMetaModel meta = new EAMetaModel();
            meta.setupAPIPackage();
            EAFactory api = APIModel.createAPI1(meta);
                                    
            YamlMappingNode map = new YamlMappingNode();

            //Test             
            RAMLManager.reifyAPI(EARepository.Repository, api.clientElement, map);

            YamlDocument d = new YamlDocument(map);

            YamlStream stream = new YamlStream();
            stream.Documents.Add(d);

            StringWriter writer = new StringWriter();
            stream.Save(writer, false);

            string yaml = writer.ToString();

            Assert.IsTrue(yaml.Contains("is: [notcacheable]"));

            Assert.IsTrue(yaml.Contains("dev-environment"));
            Assert.IsTrue(yaml.Contains("prod-environment"));

            Assert.IsTrue(yaml.Contains("someuriparameter"));

            Assert.IsTrue(yaml.Contains("data_item_description"));

            FileManager fileManager = new FileManager(null);
            fileManager.setBasePath(".");
            fileManager.initializeAPI(EARepository.currentPackage.Name);
            fileManager.setup(APIAddinClass.RAML_0_8);
            fileManager.exportAPI(EARepository.currentPackage.Name, APIAddinClass.RAML_0_8,  yaml.ToString());
        }

        [TestMethod]
        public void TestHomeQuote()
        {
            EAMetaModel meta = new EAMetaModel().setupAPIPackage();
            EAFactory api = APIModels.APIModel.createHomeQuote(meta);            

            YamlMappingNode map = new YamlMappingNode();

            //Test             
            RAMLManager.reifyAPI(EARepository.Repository, api.clientElement, map);

            YamlDocument d = new YamlDocument(map);

            YamlStream stream = new YamlStream();
            stream.Documents.Add(d);

            StringWriter writer = new StringWriter();
            stream.Save(writer, false);

            string yaml = writer.ToString();

            FileManager fileManager = new FileManager(null);
            fileManager.setBasePath(".");
            fileManager.initializeAPI(EARepository.currentPackage.Name);
            fileManager.setup(APIAddinClass.RAML_0_8);
            fileManager.exportAPI(EARepository.currentPackage.Name, APIAddinClass.RAML_0_8,yaml);
        }

        [TestMethod]
        public void TestEAMock()
        {
            EAMetaModel meta = new EAMetaModel();

            EARepository rep = EARepository.Repository;

            EAElement metaAPI = new EAElement();
            metaAPI.Name = "API";

            EAElement api = new EAElement();
            api.Name = "api";
            api.Type = "Class";
            api.Stereotype = "stereotype";
            api.ClassifierID = metaAPI.ElementID;

            api.RunState = "runstate";

            EAElement resource = new EAElement();
            resource.Name = "/resource";

            EA.Collection connectors = api.Connectors;
            object con = connectors.AddNew("", APIAddIn.APIAddinClass.EA_TYPE_ASSOCIATION);

            EAConnector c = (EAConnector)con;

            c.ClientID = api.ElementID;
            c.SupplierID = resource.ElementID;

            c.SupplierEnd.Role = "SupplierRole";

            Assert.AreEqual(1, api.Connectors.Count);


            EAElement metaAPI2 = (EAElement)rep.GetElementByID(metaAPI.ElementID);
            Assert.AreEqual(metaAPI.ElementID, metaAPI2.ElementID);

            EAElement api2 = (EAElement)rep.GetElementByID(c.ClientID);
            EAElement resource2 = (EAElement)rep.GetElementByID(c.SupplierID);

            Assert.AreEqual(api.ElementID, api2.ElementID);
            Assert.AreEqual(resource.ElementID, resource2.ElementID);

            Assert.IsNotNull(c.SupplierEnd);
            Assert.IsNotNull(c.SupplierEnd.Role);
        }


        [TestMethod]
        public void MultipleLevelsOfResources()
        {
            EAMetaModel meta = new EAMetaModel();
            meta.setupAPIPackage();
            EAFactory api = APIModel.createMOM(meta);

            YamlMappingNode map = new YamlMappingNode();

            //Test             
            RAMLManager.reifyAPI(EARepository.Repository, api.clientElement, map);

            YamlScalarNode mom = new YamlScalarNode();
            YamlNode momValue;
            mom.Value = "/mom";
            Assert.IsTrue(map.Children.TryGetValue(mom,out momValue));


            YamlMappingNode resourceProps = (YamlMappingNode)momValue;
            YamlScalarNode ev = new YamlScalarNode();
            YamlNode eventValue;
            ev.Value = "/event";
            Assert.IsTrue(resourceProps.Children.TryGetValue(ev, out eventValue));



        }

        [TestMethod]
        public void TestRAML1()
        {
            EAMetaModel meta = new EAMetaModel();
            meta.setupAPIPackage();
            EAFactory api = APIModel.createAPI1(meta);

            YamlMappingNode map = new YamlMappingNode();

            RAMLManager.REIFY_VERSION = APIAddinClass.RAML_1_0;

            //Test             
            RAMLManager.reifyAPI(EARepository.Repository, api.clientElement, map);

            YamlDocument d = new YamlDocument(map);

            YamlStream stream = new YamlStream();
            stream.Documents.Add(d);

            StringWriter writer = new StringWriter();
            stream.Save(writer, false);

            string yaml = writer.ToString();

            Assert.IsTrue(yaml.Contains("is: [notcacheable]"));

            Assert.IsTrue(yaml.Contains("dev-environment"));
            Assert.IsTrue(yaml.Contains("prod-environment"));

            Assert.IsTrue(yaml.Contains("someuriparameter"));

            Assert.IsTrue(yaml.Contains("data_item_description"));

            FileManager fileManager = new FileManager(null);
            fileManager.setBasePath(".");
            fileManager.initializeAPI(EARepository.currentPackage.Name);
            fileManager.setup(APIAddinClass.RAML_0_8);
            fileManager.exportAPI(EARepository.currentPackage.Name, APIAddinClass.RAML_0_8, yaml.ToString());
        }
    }
}