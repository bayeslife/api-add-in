using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EADiagram : EA.Diagram
    {
        //public List<EAElement> elements = new List<EAElement>();

        public EADiagram()
        {
            DiagramObjects = new EACollection("DiagramObject");
            DiagramID = EARepository.NextAvailableId();
            DiagramLinks = new EACollection("DiagramLink");
        }

        public bool ApplyGroupLock(string aGroupName)
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new NotImplementedException();
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

        public DateTime CreatedDate
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

        public string DiagramGUID
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

        public int DiagramID{get;set;}
        

        public EA.Collection DiagramLinks{get;set;}
        
        public EA.Collection DiagramObjects { get; set; }
        

        public string ExtendedStyle
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

        public EA.DiagramObject GetDiagramObjectByID(int nID, string sDUID)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public bool HighlightImports
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

        public bool IsLocked
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
            get { throw new NotImplementedException(); }
        }

        public DateTime ModifiedDate
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

        public string Name{get;set;}
        
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

        public string Orientation
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

        public int PackageID { get; set; }

        public int PageHeight
        {
            get { throw new NotImplementedException(); }
        }

        public int PageWidth
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID
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

        public string ReadStyle(string Style)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseUserLock()
        {
            throw new NotImplementedException();
        }

        public void ReorderMessages()
        {
            throw new NotImplementedException();
        }

        public bool SaveAsPDF(string sFilename)
        {
            throw new NotImplementedException();
        }

        public bool SaveImagePage(int x, int y, int sizeX, int sizeY, string FileName, int Flags)
        {
            throw new NotImplementedException();
        }

        public int Scale
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

        public EA.Connector SelectedConnector
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

        public EA.Collection SelectedObjects
        {
            get { throw new NotImplementedException(); }
        }

        public void ShowAsElementList(bool ShowAsList, bool Persist)
        {
            throw new NotImplementedException();
        }

        public int ShowDetails
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

        public bool ShowPackageContents
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

        public bool ShowPrivate
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

        public bool ShowProtected
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

        public bool ShowPublic
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

        public string Stereotype{get;set;}
        

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

        public EA.SwimlaneDef SwimlaneDef
        {
            get { throw new NotImplementedException(); }
        }

        public string Swimlanes
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
            get { throw new NotImplementedException(); }
        }

        public bool Update()
        {
            return true;
        }

        public string Version
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

        public void WriteStyle(string Style, string Value)
        {
            throw new NotImplementedException();
        }

        public int cx
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

        public int cy
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
    }

}
