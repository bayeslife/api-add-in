using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EATaggedValue : EA.TaggedValue
    {
        public int AttributeID
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

        public string FQName
        {
            get { throw new NotImplementedException(); }
        }

        public string GetAttribute(string PropName)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public bool HasAttributes()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        
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

        public bool SetAttribute(string PropName, string PropValue)
        {
            throw new NotImplementedException();
        }

        public string TagGUID
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

        public int TagID
        {
            get { throw new NotImplementedException(); }
        }

        public bool Update()
        {
            return true;
        }

        public string Value {get;set;}


        public int ElementID
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

        public int ParentID
        {
            get { throw new NotImplementedException(); }
        }

        public string PropertyGUID
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

        public int PropertyID
        {
            get { throw new NotImplementedException(); }
        }
    }
}
