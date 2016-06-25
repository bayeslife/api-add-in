using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Web.Services.Description;
using System.IO;

namespace APIAddIn
{
    public class WSDLManager
    {

        static Logger logger = new Logger();

        string path = null;

        static public void setLogger(Logger l)
        {
            logger = l;
        }

        static public string queryIAAType(EA.Repository Repository,String type)
        {
            string resultDoc = Repository.SQLQuery("select Object_ID from t_object where package_id=(SELECT package_id FROM t_package where Name='Iagiaa_v2') and Name='" + type + "'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultDoc);
            List<string> result = new List<string>();
            foreach (XmlNode node in doc.GetElementsByTagName("Object_ID"))
            {
                logger.log("Found ClassifierId for:" + type + " as " + node.InnerText);
                return node.InnerText;                
            }
            return null;
        }

         static public void importWSDLs(EA.Repository repository,EA.Diagram diagram)
        {
            string [] wsdls = Directory.GetFiles(@"D:\SOA\Modelling\Schema\", "*datarich.wsdl");
             foreach(string filename in wsdls){
                  WSDLManager mgr = new WSDLManager();
                  mgr.importWSDL(filename, repository, diagram);
             }            
        }

         XmlSchemaObject findType(XmlSchema schema,XmlQualifiedName nm){
             {                 
                 XmlSchemaObjectTable types = schema.SchemaTypes;
                 foreach (XmlQualifiedName n in types.Names)
                 {                   
                     if (nm.Name.Equals(n.Name))
                     {
                         return types[nm];
                     }
                 }
             }
             return null;
         }

         XmlSchemaObject findElement(XmlSchema schema, XmlQualifiedName nm)
         {
             XmlSchemaObjectTable t = schema.Elements;
             foreach (XmlQualifiedName n in t.Names)
             {                 
                 if (nm.Name.Equals(n.Name))
                 {
                     return t[n];
                 }
             }
             return null;
         }
         
         void addParameter(EA.Repository r,EA.Method method, XmlQualifiedName elName,ServiceDescription wsdl)
         {                     
             foreach (XmlSchema sch in wsdl.Types.Schemas)
             {
                 XmlSchemaObject so = findElement(sch, elName);
                 if (so != null)
                 {                     
                     XmlSchemaElement sel = (XmlSchemaElement)so;                                                   
                     XmlSchemaComplexType complexType = (XmlSchemaComplexType)sel.SchemaType;                                          
                     XmlSchemaSequence sequence = (XmlSchemaSequence)complexType.Particle;

                     foreach (XmlSchemaObject sp in sequence.Items)
                     {
                         XmlSchemaElement spel = (XmlSchemaElement)sp;
                         
                         EA.Parameter param = method.Parameters.AddNew(spel.Name, "");
                         param.Name = spel.Name;

                         string classifiedId = queryIAAType(r,spel.SchemaTypeName.Name);
                            
                         param.Type = spel.SchemaTypeName.Name;   
                         param.ClassifierID = classifiedId;
                                                  
                         param.Update();
                         method.Parameters.Refresh();
                     }                                          
                 }                    
             }
         }

         void addReturn(EA.Repository rep,EA.Method method, XmlQualifiedName elName, ServiceDescription wsdl)
         {             
             foreach (XmlSchema sch in wsdl.Types.Schemas)
             {
                 XmlSchemaObject so = findElement(sch, elName);
                 if (so != null)
                 {
                     XmlSchemaElement sel = (XmlSchemaElement)so;
                     XmlSchemaComplexType complexType = (XmlSchemaComplexType)sel.SchemaType;
                     XmlSchemaSequence sequence = (XmlSchemaSequence)complexType.Particle;

                     foreach (XmlSchemaObject sp in sequence.Items)
                     {
                         XmlSchemaElement spel = (XmlSchemaElement)sp;

                         method.ReturnType = spel.SchemaTypeName.Name;
                         method.Update();                         
                     }
                 }
             }
         }

        public void importWSDL(String filename,EA.Repository repository,EA.Diagram diagram)
        {
            logger.log("Importing: " + filename);

            EA.Package package = repository.GetPackageByID(diagram.PackageID);

            XmlTextReader reader = new XmlTextReader(filename);

            ServiceDescription wsdl =  ServiceDescription.Read(reader);

            foreach (PortType pt in wsdl.PortTypes)
            {
                //logger.log("PortType: "+ pt.Name);

                EA.Element clazz = package.Elements.AddNew(pt.Name, APIAddinClass.EA_TYPE_CLASS);
                clazz.Notes = wsdl.Documentation;
                clazz.Version = wsdl.TargetNamespace;                
                package.Update();

                EA.DiagramObject diaObj = diagram.DiagramObjects.AddNew(pt.Name, "");
                diagram.Update();

                diaObj.ElementID = clazz.ElementID;
                
                diaObj.Update();

              
                foreach (Operation op in pt.Operations)
                {
                    //logger.log("\tOperation: "+ op.Name);
                    
                    EA.Method method = clazz.Methods.AddNew(op.Name, "");

                    method.Name = op.Name;

                    method.Update();

                    clazz.Update();

                    clazz.Methods.Refresh();

                    OperationInput oi=null;
                    OperationOutput oo=null;
                    foreach (OperationMessage msg in op.Messages)
                    {
                        if (msg is OperationInput)
                        {                            
                            oi = msg as OperationInput;                            
                        }

                        else if (msg is OperationOutput){                            
                            oo = msg as OperationOutput;
                        }
                        else
                        {                            
                        }                            
                    }

                    if (oi == null || oo == null)
                        return;

                    foreach (Message m in wsdl.Messages)
                    {
                        //logger.log("\n\tMessage: " + m.Name);
                        //logger.log("\tOperationInput: " + oi.Message.Name);

                        if (m.Name.Equals(oi.Message.Name))
                        {
                            foreach (MessagePart p in m.Parts)
                            {
                                if (!p.Name.Equals("Header"))
                                {                                    
                                    addParameter(repository, method, p.Element, wsdl);
                                }                                                                
                            }    
                        }
                        else if (m.Name.Equals(oo.Message.Name))
                        {
                            foreach (MessagePart p in m.Parts)
                            {
                                if (!p.Name.Equals("Header"))
                                {                                    
                                    addReturn(repository, method, p.Element, wsdl);
                                }
                            }    
                        }                        
                    }
                }

            }
            diagram.DiagramObjects.Refresh();            
        }

        static public void exportWSDLs(EA.Repository repository, EA.Diagram diagram)
        {
            WSDLManager mgr = new WSDLManager();            
            //mgr.path = @"D:\SOA\Modelling\Schema\";
            mgr.path = @"D:\tmp\";
            foreach (EA.DiagramObject obj in diagram.DiagramObjects)
            {
                EA.Element component = repository.GetElementByID(obj.ElementID);
                ServiceDescription sd = mgr.exportWSDL(repository, component);
                sd.Write(mgr.path + "/" + sd.Name + ".wsdl");
            }            
        }

        public ServiceDescription exportWSDL(EA.Repository repository, EA.Element component)
        {
            string faultnamespace = "http://www.iag.co.nz/soa/iagiaa/fault";
            string iaaNamespace = "http://www.iag.co.nz/soa/iagiaa/v2";

            string wsdlNamespace = component.Version;

            ServiceDescription service = new ServiceDescription();
            service.TargetNamespace = wsdlNamespace;
            service.Name = component.Name;

            service.Namespaces.Add("iaa", iaaNamespace);
            service.Namespaces.Add("fault", faultnamespace);
            service.Namespaces.Add("soa", wsdlNamespace);

            XmlSchema schema = new XmlSchema();
            service.Documentation = component.Notes;
            {                
                schema.ElementFormDefault = XmlSchemaForm.Qualified;
                schema.TargetNamespace = component.Version;

                {
                    XmlSchemaImport si = new XmlSchemaImport();
                    si.SchemaLocation = "iagiaa_.xsd";
                    si.Namespace = iaaNamespace;
                    schema.Includes.Add(si);
                }
                {
                    XmlSchemaImport si = new XmlSchemaImport();
                    si.SchemaLocation = "iagiaa_fault.xsd";
                    si.Namespace = faultnamespace;                    
                    schema.Includes.Add(si);
                }
                service.Types.Schemas.Add(schema);
            }

            Message faultMessage = new Message();
            {                
                faultMessage.Name = "ErrorInfo";
                service.Messages.Add(faultMessage);

                MessagePart mp = new MessagePart();
                mp.Name = "ErrorInfo";
                XmlQualifiedName qn = new XmlQualifiedName("ErrorInfo", faultnamespace);
                mp.Element = qn;
                faultMessage.Parts.Add(mp); 
            }
            
            
            
            Binding binding = new Binding();
            service.Bindings.Add(binding);
            binding.Name = component.Name;
            binding.Type = new XmlQualifiedName(component.Name, wsdlNamespace);

            Soap12Binding soapBinding = new Soap12Binding();
            binding.Extensions.Add(soapBinding);
            soapBinding.Transport = "http://schemas.xmlsoap.org/soap/http";

            PortType portType = new PortType();
            portType.Name = component.Name;
            service.PortTypes.Add(portType);
                    
            foreach (EA.Method m in component.Methods)
            {
                {
                    

                }   
                
                {
                    Message inMessage = new Message();
                    inMessage.Name = m.Name + "SoapIn";
                    service.Messages.Add(inMessage);

                    MessagePart mp = new MessagePart();
                    mp.Name = "Header";
                    XmlQualifiedName qn = new XmlQualifiedName("ApplicationContext", iaaNamespace);
                    mp.Element = qn;
                    inMessage.Parts.Add(mp);

                    MessagePart ip = new MessagePart();
                    ip.Name = m.Name+"Part";
                    XmlQualifiedName iqn = new XmlQualifiedName(m.Name, wsdlNamespace);
                    ip.Element = iqn;
                    inMessage.Parts.Add(ip);
                }
                {
                    Message outMessage = new Message();
                    outMessage.Name = m.Name + "SoapOut";
                    service.Messages.Add(outMessage);
                    
                    MessagePart ip = new MessagePart();
                    ip.Name = m.Name + "ResponsePart";
                    XmlQualifiedName iqn = new XmlQualifiedName(m.Name+"Response", wsdlNamespace);
                    ip.Element = iqn;
                    outMessage.Parts.Add(ip);
                }

                {
                    Operation operation = new Operation();
                    portType.Operations.Add(operation);
                    operation.Name = m.Name;

                    OperationInput oi = new OperationInput();
                    oi.Message = new XmlQualifiedName(component.Name + "SoapIn", component.Version);
                    operation.Messages.Add(oi);


                    OperationOutput oo = new OperationOutput();
                    oo.Message = new XmlQualifiedName(component.Name + "SoapOut", component.Version);
                    operation.Messages.Add(oo);

                    OperationFault of = new OperationFault();
                    of.Name = faultMessage.Name;
                    of.Message = new XmlQualifiedName("ErrorInfo", component.Version);
                    operation.Faults.Add(of);
                }

                {
                    OperationBinding opBinding = new OperationBinding();
                    binding.Operations.Add(opBinding);
                    opBinding.Input = new InputBinding();
                    opBinding.Output = new OutputBinding();
                    FaultBinding faultBinding = new FaultBinding();
                    opBinding.Faults.Add(faultBinding);

                    SoapHeaderBinding headerBinding = new SoapHeaderBinding();
                    opBinding.Input.Extensions.Add(headerBinding);
                    headerBinding.Message = new XmlQualifiedName(m.Name + "SoapIn", wsdlNamespace);
                    headerBinding.Part="Header";
                    headerBinding.Use=SoapBindingUse.Literal;

                    SoapBodyBinding bodyBinding = new SoapBodyBinding();
                    opBinding.Input.Extensions.Add(bodyBinding);                    
                    bodyBinding.PartsString= m.Name+"Part";


                    SoapBodyBinding outBinding = new SoapBodyBinding();
                    opBinding.Output.Extensions.Add(outBinding);
                    outBinding.Use = SoapBindingUse.Literal;

                    faultBinding.Name = "ErrorResponseType";
                    SoapFaultBinding soapFaultBinding = new SoapFaultBinding();                    
                    faultBinding.Extensions.Add(soapFaultBinding);
                    soapFaultBinding.Use = SoapBindingUse.Literal;
                }

            }            
            return service;
        }
    }
}
