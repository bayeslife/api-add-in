using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EAParameter : EA.Parameter
    {
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

        public string ClassifierID {get;set;}
        

        public string Default
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

        public bool IsConst
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

        public string Kind {get;set;}
        

        public string Name{ get;set;}
        

        public string Notes
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

        public EA.ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int OperationID
        {
            get { throw new NotImplementedException(); }
        }

        public string ParameterGUID
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

        public int Position
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

        public string Stereotype
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

        public string Style
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

        public EA.Collection TaggedValues
        {
            get { throw new NotImplementedException(); }
        }

        public string Type{ get; set;}
        
        public bool Update()
        {
            return true;
        }
    }
}
