using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EAConnector : EA.Connector
    {

        public EAConnector()
        {
            this.SupplierEnd = new EAConnectorEnd();
            this.ConnectorID = EARepository.NextAvailableId();
        }

        public string Alias
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public EA.ConnectorEnd ClientEnd
        {
            get { throw new NotImplementedException(); }
        }

        public int ClientID { get; set; }


        public int Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ConnectorGUID
        {
            get { throw new NotImplementedException(); }
        }

        public int ConnectorID{ get;set;}
        

        public EA.Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection ConveyedItems
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection CustomProperties
        {
            get { throw new NotImplementedException(); }
        }

        public int DiagramID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Direction{get;set;}
        

        public int EndPointX
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int EndPointY
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string EventFlags
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public bool IsLeaf
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsRoot
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSpec
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string MetaType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name { get; set; }


        public string Notes{get;set;}
        

        public EA.ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Properties Properties
        {
            get { throw new NotImplementedException(); }
        }

        public int RouteStyle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int SequenceNo
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int StartPointX
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int StartPointY
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string StateFlags
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Stereotype{ get;set;}        

        public string StereotypeEx
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string StyleEx
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Subtype
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public EA.ConnectorEnd SupplierEnd { get; set; }


        public int SupplierID { get; set; }


        public EA.Collection TaggedValues
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection TemplateBindings
        {
            get { throw new NotImplementedException(); }
        }

        public string TransitionAction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TransitionEvent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TransitionGuard
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Update()
        {
            return true;
        }

        public string VirtualInheritance
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string get_MiscData(int index)
        {
            throw new NotImplementedException();
        }
    }

}
