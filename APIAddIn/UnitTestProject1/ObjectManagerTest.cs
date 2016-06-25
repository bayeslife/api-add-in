using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

using UnitTestProject1.EAFacade;

namespace UnitTestProject1
{
     [TestClass]
    public class ObjectManagerTest
    {
        [TestMethod]
        public void TestRunState()
        {
            {
                string rs = "";

                rs = ObjectManager.addRunState(rs, "foo", "bar", 100);                   

                Dictionary<string, RunState> r = ObjectManager.parseRunState(rs);

                Assert.AreEqual(1, r.Count);
                Assert.AreEqual("bar", r["foo"].value);
            }
            {
                string rs = "@Var;Variable=foo;Value=bar;Note=123;Op==;@EndVar;@Var;Variable=foo2;Value=bar2;Note=456;Op=>;@EndVar";

                Dictionary<string, RunState> r = ObjectManager.parseRunState(rs);

                Assert.AreEqual(2, r.Count);
                Assert.AreEqual("bar", r["foo"].value);
                Assert.AreEqual("123", r["foo"].reference);
                Assert.AreEqual("bar2", r["foo2"].value);
                Assert.AreEqual("456", r["foo2"].reference);
            }
            {                
                string rs = "@VAR;Variable=baseUri;Value=https://api.live.iag.co.nz;Op==;@ENDVAR;@VAR;Variable=version;Value=v1510;Op==;@ENDVAR;@VAR;Variable=mediaType;Value=application/json;Op==;@ENDVAR;@VAR;Variable=title;Value=IAG Quotes Web API;Op==;@ENDVAR;";
                Dictionary<string, RunState> r = ObjectManager.parseRunState(rs);
                Assert.AreEqual(4, r.Count);

            }
            
        }
        [TestMethod]
        public void TestObjectManager()
        {
            var api = ObjectManager.parseRunState(null);
            Assert.IsNotNull(api);
        }

         [TestMethod]
        public void TestJObject()
        {

            JObject jo = new JObject();
             jo.Add("foo", "bar");

             jo.AddAnnotation("foobar2");
            string serial = JsonConvert.SerializeObject(jo, Newtonsoft.Json.Formatting.Indented);
            JObject jo2 = JObject.Parse(serial);            
        }


        [TestMethod]
        public void TestExportObjectWithListAttribute()
        {
            EAMetaModel meta = new EAMetaModel();            
            EAFactory rootClass = APIModels.APIModel.createAPI1(meta);
            meta.setupSamplePackage();
                                                
            //Test
            JObject jobject = (JObject)SampleManager.sampleToJObject(EARepository.Repository,EARepository.currentDiagram)["json"];

            Assert.AreEqual(1,jobject.Count);

            JToken t = null;
            t = jobject.Value<JToken>("0OrMoreAttribute");            
            Assert.IsNotNull(t);           
            Assert.AreEqual(t.Type, JTokenType.Array);           
        }

        //[TestMethod]
        //public void TestExportClassWithObjecctAttribute()
        //{
        //    EAMetaModel meta = new EAMetaModel();
        //    APIModels.APIModel.createAPI2(meta);
        //    meta.setupSamplePackage();

        //    SchemaManager.generateSample();


        //    //Test
        //    JObject jobject = SampleManager.sampleToJObject(EARepository.Repository, EARepository.currentDiagram).Value;

        //    JToken t = null;
        //    t = jobject.Value<JToken>("objectAttr");
        //    Assert.IsNotNull(t); ;
        //}


        [TestMethod]
        public void TestExportSample()
       {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();

            EAFactory rootClass = APIModels.APIModel.createAPI1(meta);
            meta.setupSchemaPackage();
                        
            EA.Package package = SchemaManager.generateSample(EARepository.Repository);

            Assert.AreEqual(1, package.Diagrams.Count);
            object o = package.Diagrams.GetAt(0);
            EA.Diagram diagram = (EA.Diagram)o;

            Assert.AreEqual(3, package.Elements.Count);

            o = package.Elements.GetAt(0);
            EA.Element sample = (EA.Element)o;
            sample.RunState = ObjectManager.addRunState(sample.RunState, "intAttribute", "123", rootClass.clientElement.ElementID);

            meta.setupSamplePackage();
            //Test
            JObject jobject = (JObject)SampleManager.sampleToJObject(EARepository.Repository, diagram)["json"];

            Assert.AreEqual(8,jobject.Count);
        }

        [TestMethod]
        public void TestValidateDiagram_Invalid()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();

            EAFactory rootClass = APIModels.APIModel.createInvalidAPI(meta);
            meta.setupSamplePackage();
          
            //Test
            IList<string> errors = SampleManager.diagramValidation(EARepository.Repository, meta.sampleDiagram);

            Assert.IsTrue(errors.Count>0);            
        }

          [TestMethod]
        public void TestValidateDiagram_Valid()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();

            EAFactory rootClass = APIModels.APIModel.createAPI1(meta);
            meta.setupSchemaPackage();

            EA.Package package = SchemaManager.generateSample(EARepository.Repository);

            Assert.AreEqual(1, package.Diagrams.Count);
            object o = package.Diagrams.GetAt(0);
            EA.Diagram diagram = (EA.Diagram)o;

            //Test
            IList<string> errors = SampleManager.diagramValidation(EARepository.Repository, diagram);

            Assert.IsNull(errors);
        }


    }
}
