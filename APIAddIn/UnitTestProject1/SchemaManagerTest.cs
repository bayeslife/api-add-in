using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using UnitTestProject1.EAFacade;
using UnitTestProject1.APIModels;


namespace UnitTestProject1
{
    [TestClass]
    public class SchemaManagerTest
    {
        [TestMethod]
        public void TestJSchemaValidation()
        {
            {
                JSchema rootSchema = new JSchema();
                JSchema subSchema = new JSchema();
                JSchema stringSchema = new JSchema { Type = JSchemaType.String };

                rootSchema.Properties.Add("rootProperty", subSchema);
                rootSchema.Properties.Add("rootProperty2", subSchema);
                rootSchema.Required.Add("rootProperty");

                subSchema.Properties.Add("subSchemaProperty", stringSchema);

                JObject o = new JObject();
                o.Add("modelType", subSchema);
                rootSchema.ExtensionData.Add("definitions", o);

                string seralized = rootSchema.ToString();

                JObject sample = JObject.Parse(@"{
                    'rootProperty': {
                        'subSchemaProperty': 'fff'
                    }
                 }");

                JObject invalidSample = JObject.Parse(@"{
                    'foo2': {
                        'foo2': 'fff'
                    }
                 }");

                bool valid = sample.IsValid(rootSchema);
                Assert.IsTrue(valid);

                bool invalid = invalidSample.IsValid(rootSchema);
                Assert.IsFalse(invalid);
            }
        }


        [TestMethod]
        public void TestSchemaEnum()
        {
            JSchema en1 = new JSchema();
            en1.Enum.Add("v1");
            en1.Enum.Add("v2");
            JSchema s1 = new JSchema();
            s1.Title = "title";
            s1.Properties.Add("someEnum", en1);
            s1.Properties.Add("someEnum2", en1);

            string s = s1.ToString();
            Assert.IsNotNull(s1);
        }

        [TestMethod]
        public void TestManageSchemaDependencies()
        {
            List<string> resources = new List<string>();

            DependencyManager c = new DependencyManager();

            List<string> ordered = c.getDependencies();

            c.setDependency("class1", null);

            Assert.AreEqual("class1", ordered[0]);

            c.setDependency("class2", "class1");

            Assert.AreEqual("class2", ordered[0]);
            Assert.AreEqual("class1", ordered[1]);

            c.setDependency("class3", "class2");

            Assert.AreEqual("class3", ordered[0]);
            Assert.AreEqual("class2", ordered[1]);

            c.setDependency("class4", "class5");

            Assert.AreEqual("class4", ordered[3]);
            Assert.AreEqual("class5", ordered[4]);

            c.setDependency("class6", "class3");

            Assert.AreEqual("class6", ordered[0]);
            Assert.AreEqual("class3", ordered[1]);

            c.setDependency("class3", "class6");

            Assert.AreEqual("class3", ordered[0]);
            Assert.AreEqual("class6", ordered[1]);
        }

        [TestMethod]
        public void TestSchemaExport()
        {
            EAMetaModel meta = new EAMetaModel();
            APIModel.createAPI1(meta);
            meta.setupSchemaPackage();
            
            FileManager fileManager = new FileManager(null);
            SchemaManager.setFileManager(fileManager);

            //Test
            JSchema jschema = SchemaManager.schemaToJsonSchema(EARepository.Repository, EARepository.currentDiagram).Value;


            Assert.IsTrue(jschema.Properties.ContainsKey("booleanAttr"));
            Assert.AreEqual(JSchemaType.Boolean,jschema.Properties["booleanAttr"].Type);

        }

        [TestMethod]
        public void TestSchemaJavaType()
        {
            JSchema schema = JSchema.Parse(@"{  'type': 'object', 'javaType': 'foo.bar.Foo'}");

            string schemaString = schema.ToString();

            Assert.IsNotNull(schema);

        }

        [TestMethod]
        public void TestExportClassWithListAttribute()
        {
            EAMetaModel meta = new EAMetaModel();
            APIModel.createAPI1(meta);
            meta.setupSchemaPackage();

                                   
            FileManager fileManager = new FileManager(null);
            SchemaManager.setFileManager(fileManager);

            //Test
            JSchema jschema = SchemaManager.schemaToJsonSchema(EARepository.Repository,EARepository.currentDiagram).Value;

            JSchema child = jschema.Properties["0OrMoreAttribute"];
            Assert.AreEqual(JSchemaType.Array, child.Type);

            JSchema listStringProp = jschema.Properties["listStringAttr"];
            Assert.AreEqual(JSchemaType.Array, listStringProp.Type);
        }

        [TestMethod]
        public void TestExportClassWithObjecctAttribute()
        {
            EAMetaModel meta = new EAMetaModel();
            APIModel.createAPI2(meta);
            meta.setupSchemaPackage();
          
            //Test
            JSchema jschema = SchemaManager.schemaToJsonSchema(EARepository.Repository, EARepository.currentDiagram).Value;

            JSchema child = jschema.Properties["objectAttr"];
            Assert.AreEqual(JSchemaType.String, child.Type);

            EA.Package package = SchemaManager.generateSample(EARepository.Repository);
            object o = package.Diagrams.GetAt(0);
            EA.Diagram diaObj = (EA.Diagram)o;

            IList<EA.Element> objects = MetaDataManager.diagramSamples(EARepository.Repository,diaObj);

            Assert.AreEqual(1+1, objects.Count);
        }


        [TestMethod]
        public void TestGenerateSample()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();
            EAFactory rootClass = new EAFactory();

            rootClass.setupClient("RootClass", APIAddinClass.EA_TYPE_CLASS, APIAddinClass.EA_STEREOTYPE_REQUEST, 0, null);
            EAFactory childClass = rootClass.addSupplier("ChildClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "optionalAttribute"/*supplierEndRole*/, "1"/*cardinality*/, null);


            //Test
            EA.Package package = SchemaManager.generateSample(EARepository.Repository);
            Assert.AreEqual(1, package.Diagrams.Count);

            Assert.AreEqual(3, package.Elements.Count);

            object o  = package.Diagrams.GetAt(0);
            EA.Diagram diaObj = (EA.Diagram)o;
            Assert.AreEqual(2+1,diaObj.DiagramObjects.Count);
            
        }

        [TestMethod]
        public void TestDomainValueManager()
        {
            {
                string initial = "";
                string newvalue = "abc";
                string newdefault = DomainValueManager.updateDefault(initial, newvalue);
                Assert.AreEqual(newvalue,newdefault);
            }
            {
                string initial = "abc";
                string newvalue = "def";
                string newdefault = DomainValueManager.updateDefault(initial, newvalue);
                Assert.AreEqual("abc,def", newdefault);
            }
            {//Here we are checking we dont update defaults with an existing default
                string initial = "def,abc,tef";
                string newvalue = "abc";
                string newdefault = DomainValueManager.updateDefault(initial, newvalue);
                Assert.AreEqual(initial, newdefault);
            }
            {//Here we are checking we get one of the three default values
                string initial = "def,abc,tef";
                string newdefault = DomainValueManager.selectDefault(initial);
                Assert.AreEqual(3,newdefault.Length);
            }

        }


        [TestMethod]
        public void TestUpdateClassFromInstance()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();
            EAFactory rootClass = new EAFactory();

            rootClass.setupClient("SomeClass", APIAddinClass.EA_TYPE_CLASS, APIAddinClass.EA_STEREOTYPE_REQUEST, 0, null);
            EA.Element el = rootClass.clientElement;

            object o = el.Attributes.AddNew("SomeAttribute", "Attribute");
            EA.Attribute attr = (EA.Attribute)o;
            
            EA.Package pkg = SchemaManager.generateSample(EARepository.Repository);

            object os = pkg.Elements.GetAt(0);
            EA.Element sample = (EA.Element)os;

            string nrs = ObjectManager.addRunState(sample.RunState, "SomeAttribute", "foobar",0);
            sample.RunState = nrs;

            SchemaManager.updateClassFromSample(EARepository.Repository,sample);
            
            Assert.AreEqual("foobar", attr.Default );

        }

        
        [TestMethod]
        public void TestUpdateClassFromInstance_NewDataItem()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();
            EAFactory rootClass = new EAFactory();

            rootClass.setupClient("SomeClass", APIAddinClass.EA_TYPE_CLASS, APIAddinClass.EA_STEREOTYPE_REQUEST, 0, null);
            EA.Element el = rootClass.clientElement;

            EAFactory attrFactory = rootClass.addSupplier("SomeAttribute", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_STEREOTYPE_DATAITEM, null, "someAttribute", APIAddinClass.CARDINALITY_0_TO_ONE, null);            
            EA.Element attr = attrFactory.clientElement;
            
            EA.Package pkg = SchemaManager.generateSample(EARepository.Repository);

            object os = pkg.Elements.GetAt(0);
            EA.Element sample = (EA.Element)os;

            string nrs = ObjectManager.addRunState(sample.RunState, "SomeAttribute", "foobar", 0);
            sample.RunState = nrs;

            SchemaManager.updateClassFromSample(EARepository.Repository, sample);

            EA.TaggedValue def = SchemaManager.getAttributeDefault(attr);
            Assert.AreEqual("foobar", def.Value);            

        }
        /// <summary>
        /// ///////////////////
        /// </summary>
        [TestMethod]
        public void TestUpdateInstanceFromClass()
        {
            EAMetaModel meta = new EAMetaModel().setupSchemaPackage();
            EAFactory rootClass = new EAFactory();

            rootClass.setupClient("SomeClass", APIAddinClass.EA_TYPE_CLASS, APIAddinClass.EA_STEREOTYPE_REQUEST, 0, null);
            EA.Element el = rootClass.clientElement;

            object o = el.Attributes.AddNew("SomeAttribute", "Attribute");
            EA.Attribute attr = (EA.Attribute)o;

            EAFactory anotherAttribute = rootClass.addSupplier("anotherAttribute", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_STEREOTYPE_DATAITEM, null, "", APIAddinClass.CARDINALITY_0_TO_ONE, "");
            EA.Element attrClass = anotherAttribute.clientElement;
            
            EA.Package pkg = SchemaManager.generateSample(EARepository.Repository);
            
            object os = pkg.Elements.GetAt(0);
            EA.Element sample = (EA.Element)os;

            string nrs = ObjectManager.addRunState(sample.RunState, "SomeAttribute", "foobar", 0);
            sample.RunState = nrs;

            nrs = ObjectManager.addRunState(sample.RunState, "anotherAttribute", "foobar2", 0);
            sample.RunState = nrs;

            attr.Name = "SomeAttribute2";//We update the attribute name to test that this update gets reflected onto the object
            attrClass.Name = "anotherAttribute2";

            SchemaManager.updateSampleFromClass(EARepository.Repository, sample);

            Assert.IsTrue(ObjectManager.parseRunState(sample.RunState).ContainsKey("SomeAttribute2"));
            Assert.IsTrue(ObjectManager.parseRunState(sample.RunState).ContainsKey("anotherAttribute2"));            
        }


        [TestMethod]
        public void TestInvalidSchema()
        {
            EAMetaModel meta = new EAMetaModel();
            APIModel.createInvalidAPI(meta);
            meta.setupSchemaPackage();
            
            FileManager fileManager = new FileManager(null);
            SchemaManager.setFileManager(fileManager);

            string theexception = "An exception should be thrown";
            try
            {
                JSchema jschema = SchemaManager.schemaToJsonSchema(EARepository.Repository, EARepository.currentDiagram).Value;
                Assert.Fail(theexception);
            }
            catch (ModelValidationException e)
            {
                string r = "";
                foreach (string s in e.errors.messages)
                {
                    r += s;
                }
                Assert.AreNotEqual("Assert.Fail failed. " +theexception, r);

                Assert.IsTrue(r.Contains("Class Name needs to start with an uppercase character"));

                Assert.IsTrue(r.Contains("Connector with empty SupplierEnd.Role is not allowed"));

                Assert.IsTrue(r.Contains("Connector with no SupplierEnd.Role cardinality is not allowed"));
            }                                   
        }

        [TestMethod]
        public void TestGetDataItemType()
        {
            EAMetaModel meta = new EAMetaModel();
            {
                EA.Element attr = new EAElement();
                attr.Stereotype = "DataItem,string";

                string s = SchemaManager.getDataItemType(attr);
                Assert.AreEqual("string", s);                
            }
            {
                EA.Element attr = new EAElement();
                attr.Stereotype = "string,DataItem";

                string s = SchemaManager.getDataItemType(attr);
                Assert.AreEqual("string", s);
            }
            {
                EA.Element attr = new EAElement();
                attr.Stereotype = null;

                string s = SchemaManager.getDataItemType(attr);
                Assert.IsNull(s);
            }
        }

        [TestMethod]
        public void TestGetDataItemExample()
        {
            EAMetaModel meta = new EAMetaModel();
            {
                EA.Element attr = new EAElement();
                object o = attr.TaggedValues.AddNew(APIAddinClass.EA_TAGGEDVALUE_DEFAULT, "");
                EA.TaggedValue tv = (EA.TaggedValue)o;
                tv.Name = APIAddinClass.EA_TAGGEDVALUE_DEFAULT;
                tv.Value = "foobar";
                string s = SchemaManager.getDataItemExample(attr);
                Assert.AreEqual("foobar", s);
            }
            {
                EA.Element attr = new EAElement();
                object o = attr.TaggedValues.AddNew(APIAddinClass.EA_TAGGEDVALUE_DEFAULT, "");
                EA.TaggedValue tv = (EA.TaggedValue)o;
                tv.Name = APIAddinClass.EA_TAGGEDVALUE_DEFAULT;
                tv.Value = null;
                string s = SchemaManager.getDataItemExample(attr);
                Assert.IsNull(s);
            }
        }

        [TestMethod]
        public void TestExportClassWithFirstClassAttribute()
        {
            EAMetaModel meta = new EAMetaModel();
            APIModel.createAPI_AttributesFirstClass(meta);
            meta.setupSchemaPackage();

            //Test
            JSchema jschema = SchemaManager.schemaToJsonSchema(EARepository.Repository, EARepository.currentDiagram).Value;

            {
                JSchema child = jschema.Properties["propClass"];
                Assert.AreEqual(JSchemaType.String, child.Type);
            }            
            {
                JSchema child = jschema.Properties["propertyClass2"];
                Assert.AreEqual(JSchemaType.String, child.Type);
            }
            

            EA.Package package = SchemaManager.generateSample(EARepository.Repository);
            object o = package.Diagrams.GetAt(0);
            EA.Diagram diaObj = (EA.Diagram)o;

            IList<EA.Element> objects = MetaDataManager.diagramSamples(EARepository.Repository, diaObj);

            Assert.AreEqual(1+1, objects.Count);
        }

        [TestMethod]
        public void TestRecursiveSchema()
        {
            JSchema rootSchema = new JSchema();
            rootSchema.Description = "root";

            JSchema child1Schema = new JSchema();
            child1Schema.Description = "Child1";

            JSchema child2Schema = new JSchema();
            child2Schema.Description = "Child2";

            JSchema arraySchema = new JSchema()
            {
                Type = JSchemaType.Array,
                Items = { child2Schema }
            };
            child1Schema.Properties.Add("children2", arraySchema);


            JSchema arraySchema2 = new JSchema()
            {
                Type = JSchemaType.Array,
                Items = { child1Schema }
            };
            child2Schema.Properties.Add("children1", arraySchema2);

            rootSchema.Properties.Add("child1", child1Schema);

            string msg = rootSchema.ToString() ;
            msg = msg + "\n";
            
        }
    }


}