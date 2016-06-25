using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APIAddIn;

namespace UnitTestProject1.EAFacade
{

    public class EAFactory
    {
        public EAElement clientElement;

        public EAFactory setupClient(string name, string classifier, string stereotype, int classifierId, string[] states)
        {
            clientElement = new EAElement();
            clientElement.Name = name;
            clientElement.Type = classifier;
            clientElement.ClassifierID = classifierId;
            clientElement.Stereotype = stereotype;

            clientElement.PackageID = EARepository.currentPackage.PackageID;

            string state = "";
            if (states != null)
            {
                for (int i = 0; i < states.Length; i += 2)
                {
                    state = ObjectManager.addRunState(state, states[i], states[i + 1],0);
                }
            }
            clientElement.RunState = state;

            EADiagram d = EARepository.currentDiagram;
            object obj = d.DiagramObjects.AddNew(name, "");
            EADiagramObject diaObj = (EADiagramObject)obj;
            diaObj.ElementID = clientElement.ElementID;

            EAFactory client = new EAFactory();
            client.clientElement = clientElement;

            return client;
        }

        public EAFactory addAttribute(string name, string eatype,string lowerBound = "0",string upperBound = "1")        
        {
            object a = this.clientElement.Attributes.AddNew(name, APIAddinClass.EA_TYPE_ATTRIBUTE);
            EA.Attribute attr = (EA.Attribute)a;
            attr.Type = eatype;
            attr.LowerBound = lowerBound;
            attr.UpperBound = upperBound;
            return this;
        }

        public EAFactory addSupplier(EAElement element, string supplierRoleName, string cardinality, String stereotype)
        {
            object obj = EARepository.currentDiagram.DiagramObjects.AddNew(element.Name, "");
            EADiagramObject diaObj = (EADiagramObject)obj;
            diaObj.ElementID = element.ElementID;

            object o = clientElement.Connectors.AddNew(element.Name, APIAddinClass.EA_TYPE_ASSOCIATION);
            EA.Connector connector = (EA.Connector)o;

            connector.ClientID = clientElement.ElementID;
            connector.SupplierID = element.ElementID;

            connector.SupplierEnd.Cardinality = cardinality;
            connector.SupplierEnd.Role = supplierRoleName;
            connector.Stereotype = stereotype;

            object dobj = EARepository.currentDiagram.DiagramLinks.AddNew(element.Name+supplierRoleName, "");
            EADiagramLink diaLink = (EADiagramLink)dobj;
            diaLink.ConnectorID = connector.ConnectorID;

            return this;
        }

        public EAFactory addSupplier(string name, string type, int classifierId, string targetStereotype, string[] states, string supplierRoleName, string cardinality,String stereotype)
        {
            string state = "";
            if (states != null)
                for (int i = 0; i < states.Length; i += 2)
                {
                    state = ObjectManager.addRunState(state, states[i], states[i + 1],0);
                }

            EAElement supplier = new EAElement();
            supplier.Name = name;
            supplier.Type = type;
            supplier.ClassifierID = classifierId;
            supplier.RunState = state;
            if(targetStereotype!=null)
                supplier.Stereotype = targetStereotype;

            addSupplier(supplier, supplierRoleName, cardinality,stereotype);
            //object obj = EARepository.currentDiagram.DiagramObjects.AddNew(name,"");
            //EADiagramObject diaObj = (EADiagramObject)obj;
            //diaObj.ElementID = supplier.ElementID;

            //object o = clientElement.Connectors.AddNew(name, APIAddInClass.EA_TYPE_ASSOCIATION);
            //EA.Connector connector = (EA.Connector)o;

            //connector.ClientID = clientElement.ElementID;
            //connector.SupplierID = supplier.ElementID;

            //connector.SupplierEnd.Cardinality = cardinality;
            //connector.SupplierEnd.Role = supplierRoleName;

            EAFactory client = new EAFactory();
            client.clientElement = supplier;
            return client;
        }

    }
   
}
