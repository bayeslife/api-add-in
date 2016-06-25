using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;
using UnitTestProject1.EAFacade;
using UnitTestProject1.APIModels;
using System.Web.Services.Description;

namespace UnitTestProject1
{
    [TestClass]
    public class WSDLTest
    {
        [TestMethod]
        public void importWSDL()
        {
            EAMetaModel meta = new EAMetaModel().setupSOAPackage();


            String filename = @"D:\SOA\Modelling\Schema\CommunicationManagement_implementation_v1_datarich.wsdl";

            WSDLManager mgr = new WSDLManager();

            mgr.importWSDL(filename,EARepository.Repository,meta.soaDiagram);

        }

        [TestMethod]
        public void exportWSDL()
        {
            EAMetaModel meta = new EAMetaModel();
            EA.Element component = new EAElement();
            component.Name = "CommunicationManagement";
            component.Version = "http://usermodel.namespace";

            {
                object obj = component.Methods.AddNew("operation1", "");
                EA.Method op1 = (EA.Method)obj;
                op1.Name = "DoSomething1";
                object objParam = op1.Parameters.AddNew("paramString", "");
                EA.Parameter param = (EA.Parameter)objParam;
                param.Name = "paramStringName";
                param.Type = "String";
                param.Kind = "Input";
            }
            {
                object obj = component.Methods.AddNew("operation2", "");
                EA.Method op1 = (EA.Method)obj;
                op1.Name = "DoSomething2";
                object objParam = op1.Parameters.AddNew("paramString", "");
                EA.Parameter param = (EA.Parameter)objParam;
                param.Name = "paramStringName";
                param.Type = "String";
                param.Kind = "Input";
            }

            WSDLManager mgr = new WSDLManager();

            ServiceDescription sd = mgr.exportWSDL(EARepository.Repository, component);


            sd.Write(@"D:\tmp\service.wsdl");

            Assert.IsNotNull(sd);
            Assert.AreEqual(1, sd.PortTypes.Count);

        }

    }
}
