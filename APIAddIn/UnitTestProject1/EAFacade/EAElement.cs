using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EAElement : EA.Element
    {
        EACollection elements = new EACollection("");
        EACollection attributes = new EACollection("");
        EACollection methods = new EACollection("Method");
        EACollection tagged_Values = new EACollection("AttributeTag");

        public EAElement()
        {
            int e = EARepository.NextAvailableId();
            ElementID = e;
            EARepository.Repository.elements.Add(this.ElementID, this);
            Stereotype = "";
            attributes.setParent(e);
        }

        public string Abstract
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

        public string ActionFlags
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

        public bool ApplyGroupLock(string aGroupName)
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new NotImplementedException();
        }

        public int AssociationClassConnectorID
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Attributes
        {
            get { return attributes; }
        }

        public EA.Collection AttributesEx
        {
            get { throw new NotImplementedException(); }
        }

        public string Author
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

        public EA.Collection BaseClasses
        {
            get { return new EACollection(""); }
        }

        public int ClassfierID { get; set; }


        public int ClassifierID { get; set; }


        public string ClassifierName { get; set; }


        public string ClassifierType { get; set; }


        public string Complexity { get; set; }

        public dynamic CompositeDiagram
        {
            get { throw new NotImplementedException(); }
        }

        EACollection connectors = new EACollection("Connectors");
        public EA.Collection Connectors
        {
            get
            {
                return connectors;
            }
            set
            {
                connectors = (EACollection)value;
            }
        }

        public EA.Collection Constraints
        {
            get;
            set;
        }

        public EA.Collection ConstraintsEx
        {
            get { throw new NotImplementedException(); }
        }

        public bool CreateAssociationClass(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public DateTime Created
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

        public EA.Collection CustomProperties
        {
            get { throw new NotImplementedException(); }
        }

        public bool DeleteLinkedDocument()
        {
            throw new NotImplementedException();
        }

        public EA.Collection Diagrams
        {
            get { throw new NotImplementedException(); }
        }

        public string Difficulty
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

        public EA.Collection Efforts
        {
            get { throw new NotImplementedException(); }
        }

        public string ElementGUID
        {
            get { throw new NotImplementedException(); }
        }

        public int ElementID { get; set; }



        public EA.Collection Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = (EACollection)value;
            }
        }

        public EA.Collection EmbeddedElements
        {
            get { throw new NotImplementedException(); }
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

        public string ExtensionPoints
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

        public EA.Collection Files
        {
            get { throw new NotImplementedException(); }
        }

        public string Genfile
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

        public string Genlinks
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

        public string Gentype
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

        public string GetBusinessRules()
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string GetLinkedDocument()
        {
            throw new NotImplementedException();
        }

        public string GetRelationSet(EA.EnumRelationSetType Type)
        {
            throw new NotImplementedException();
        }

        public string GetStereotypeList()
        {
            return Stereotype;
        }

        public bool HasStereotype(string stereo)
        {
            throw new NotImplementedException();
        }

        public dynamic Header1
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

        public dynamic Header2
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

        public bool IsActive
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

        public bool IsAssociationClass()
        {
            throw new NotImplementedException();
        }

        public bool IsComposite
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

        public bool IsNew
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

        public EA.Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public bool LoadLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool Locked
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

        public EA.Collection Methods
        {
            get { return methods; }            
        }

        public EA.Collection MethodsEx
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Metrics
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime Modified
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

        public string Multiplicity
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


        public string Notes { get; set; }


        public EA.ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int PackageID { get; set; }


        public int ParentID { get; set; }

        public EA.Collection Partitions
        {
            get { throw new NotImplementedException(); }
        }

        public string Persistence
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

        public string Phase
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

        public string Priority
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

        public EA.Properties Properties
        {
            get { throw new NotImplementedException(); }
        }

        public int PropertyType
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

        public dynamic PropertyTypeName
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Realizes { get; set; }


        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseUserLock()
        {
            throw new NotImplementedException();
        }

        public EA.Collection Requirements
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection RequirementsEx
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Risks
        {
            get { throw new NotImplementedException(); }
        }

        public string RunState { get; set; }


        public bool SaveLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public EA.Collection Scenarios
        {
            get { throw new NotImplementedException(); }
        }

        public void SetAppearance(int Scope, int Item, int Value)
        {
            throw new NotImplementedException();
        }

        public bool SetCompositeDiagram(string sGUID)
        {
            throw new NotImplementedException();
        }

        public EA.Collection StateTransitions
        {
            get { throw new NotImplementedException(); }
        }

        public string Status
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

        public string Stereotype { get; set; }



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

        public int Subtype
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

        public bool SynchConstraints(string sProfile, string sStereotype)
        {
            throw new NotImplementedException();
        }

        public bool SynchTaggedValues(string sProfile, string sStereotype)
        {
            throw new NotImplementedException();
        }

        public string Tablespace
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

        public string Tag
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
            get { return tagged_Values; }
        }

        public EA.Collection TaggedValuesEx
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection TemplateParameters
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Tests
        {
            get { throw new NotImplementedException(); }
        }

        public int TreePos
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

        public string Type { get; set; }

        public bool UnlinkFromAssociation()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            return true;
        }

        public string Version { get; set;}        

        public string Visibility
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
