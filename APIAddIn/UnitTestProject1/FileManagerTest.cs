using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using UnitTestProject1.EAFacade;

namespace UnitTestProject1
{
    [TestClass]
    public class FileManagerTest
    {
        [TestMethod]
        public void TestFileManager()
        {
            FileManager fm = new FileManager((Logger)null);

            fm.setBasePath(@"c:\tmp");
            fm.initializeAPI("api");

            string p = fm.apiPath("api1", APIAddinClass.RAML_0_8, fm.getNamespace(EARepository.Repository, EARepository.currentPackage));
            Assert.AreEqual(@"c:\tmp\api1\src\main\api\api.raml", fm.apiPath("api1", APIAddinClass.RAML_0_8, ""));
            Assert.AreNotEqual(@"c:\tmp2\api1\src\main\api\api.raml", fm.apiPath("api1", APIAddinClass.RAML_0_8, ""));

            Assert.AreEqual(@"c:\tmp\schema1\src\main\api\schemas\schema1.json", fm.schemaPath("schema1", ""));
            Assert.AreNotEqual(@"c:\tmp2\schema1\src\main\api\schemas\schema1.json", fm.schemaPath("schema1", ""));
           
        }
    }
}
