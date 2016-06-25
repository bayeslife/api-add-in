using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{

    public class EACollection : EA.Collection
    {
        int parent = 0;
        //public List<object> collection = new List<object>();
        public Dictionary<string,object> collection = new Dictionary<string,object>();

        string type = "";

        public EACollection(string type)
        {
            this.type = type;
        }

        public void setParent(int p)
        {
            parent = p;
        }
       

        public dynamic AddNew(string Name, string Type)
        {
            if (this.type.Equals("AttributeTag"))
            {
                EATaggedValue p = new EATaggedValue();
                collection.Add(Name, p);
                return p;

            }

            if (this.type.Equals("Parameters"))
            {
                EAParameter p = new EAParameter();
                collection.Add(Name, p);
                return p;

            }

            if (this.type.Equals("Method"))
            {
                EAMethod m = new EAMethod();
                collection.Add(Name, m);
                return m;

            }
            if (this.type.Equals("DiagramLink"))
            {
                EADiagramLink dia = new EADiagramLink();
                 collection.Add(Name, dia);
                return dia;

            }
            if (this.type.Equals("Diagram"))
            {
                EADiagram dia = new EADiagram();
                dia.PackageID = parent;
                collection.Add(Name,dia);
                return dia;

            }
            if(this.type.Equals("DiagramObject")){                  
                EADiagramObject diaObj = new EADiagramObject();                
                collection.Add(Name,diaObj);
                return diaObj;
            
            }else 
            {
                if (Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_PACKAGE))
            {
                EAPackage package = new EAPackage(Name);
                package.ParentID = parent;
                collection.Add(Name,package);
                return package;
            }
            else if (Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_ASSOCIATION))
            {
                EAConnector connector = new EAConnector();
                connector.Name = Name;
                collection.Add(Name,connector);

                return connector;
            }
            else if (Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_ATTRIBUTE))
            {
                EAAttribute attr = new EAAttribute();
                attr.ParentID = parent;
                attr.Name = Name;
                attr.LowerBound = "0";
                attr.UpperBound = "*";
                collection.Add(Name,attr);

                return attr;
            }
            else if (Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_CLASS) || Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_OBJECT))
            {
                EAElement element = new EAElement();
                element.Type = Type;
                element.Name = Name;
                collection.Add(Name,element);

                return element;
            }

            if (this.type.Equals(APIAddIn.APIAddinClass.EA_TYPE_ATTRIBUTE))
            {
                if (Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_STRING))
                {
                    EAAttribute attr = new EAAttribute();
                    attr.Name = Name;
                    collection.Add(Name, attr);

                    return attr;
                }
            }
           
            throw new Exception("Not supported");
         }
            
            throw new NotImplementedException();
        }

        public short Count
        {
            get { return (short)collection.Count; }
        }

        public void Delete(short index)
        {
            throw new NotImplementedException();
        }

        public void DeleteAt(short index, bool Refresh)
        {
            throw new NotImplementedException();
        }

        public dynamic GetAt(short index)
        {
            return collection.Values.ElementAt(index);
        }

        public dynamic GetByName(string Name)
        {
            if (collection.ContainsKey(Name))
                return collection[Name];
            return null;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return collection.Values.GetEnumerator();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public EA.ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public void Refresh()
        {
            
        }
    }
    
   
}
