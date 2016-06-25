using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EAPackage : EA.Package
    {        

        public EAPackage(string name)
        {
            Name = name;
            PackageID = EARepository.NextAvailableId();
            EARepository.Repository.packages.Add(PackageID, this);
            
            EACollection p = new EACollection("Packages");
            p.setParent(PackageID);
            Packages = p;

            EACollection d = new EACollection("Diagram");
            d.setParent(PackageID);
            Diagrams = d;

            EACollection e = new EACollection("Element");
            e.setParent(PackageID);
            Elements = e;
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

        public bool ApplyGroupLockRecursive(string aGroupName, bool IncludeElements, bool IncludeDiagrams, bool IncludeSubPackages)
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLockRecursive(bool IncludeElements, bool IncludeDiagrams, bool IncludeSubPackages)
        {
            throw new NotImplementedException();
        }

        public int BatchLoad
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

        public int BatchSave
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

        public EA.Package Clone()
        {
            throw new NotImplementedException();
        }

        public string CodePath
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

        public EA.Collection Connectors
        {
            get { throw new NotImplementedException(); }
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

        public EA.Collection Diagrams{ get; set;}
        
        public EA.Element Element
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Elements{ get; set;}
        

        public dynamic FindObject(string DottedID)
        {
            throw new NotImplementedException();
        }

        public string Flags
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

        public void GenerateSourceCode()
        {
            throw new NotImplementedException();
        }

        public EA.CodeObject GetClassCodeObjects(string CodeIDs)
        {
            throw new NotImplementedException();
        }

        public EA.CodeObject GetCodeObject(string CodeID)
        {
            throw new NotImplementedException();
        }

        public void GetCodeProject(out string GUID, out string ProjectType)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public bool IsControlled
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

        public bool IsModel
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsNamespace
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

        public bool IsProtected
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

        public bool IsVersionControlled
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastLoadDate
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastSaveDate
        {
            get { throw new NotImplementedException(); }
        }

        public bool LogXML
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

        public string Owner
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

        public string PackageGUID
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

        public EA.Collection Packages{get;set;}
        
        public int ParentID { get; set; }


        public bool ReleaseUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseUserLockRecursive(bool IncludeElements, bool IncludeDiagrams, bool IncludeSubPackages)
        {
            throw new NotImplementedException();
        }

        public void SetCodeProject(string GUID, string ProjectType)
        {
            throw new NotImplementedException();
        }

        public void SetReadOnly(bool ReadOnly, bool IncludeSubPkgs)
        {
            throw new NotImplementedException();
        }

        public EA.CodeObject ShallowGetClassCodeObjects(string CodeIDs)
        {
            throw new NotImplementedException();
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

        public string UMLVersion
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

        public bool UseDTD
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

        public void VersionControlAdd(string ConfigGuid, string XMLFile, string Comment, bool KeepCheckedOut)
        {
            throw new NotImplementedException();
        }

        public void VersionControlCheckin(string Comment = "0")
        {
            throw new NotImplementedException();
        }

        public void VersionControlCheckinEx(string Comment, bool PreserveCrossPkgRefs)
        {
            throw new NotImplementedException();
        }

        public void VersionControlCheckout(string Comment = "0")
        {
            throw new NotImplementedException();
        }

        public void VersionControlGetLatest(bool ForceImport)
        {
            throw new NotImplementedException();
        }

        public int VersionControlGetStatus()
        {
            throw new NotImplementedException();
        }

        public void VersionControlPutLatest(string Comment = "0")
        {
            throw new NotImplementedException();
        }

        public void VersionControlRemove()
        {
            throw new NotImplementedException();
        }

        public void VersionControlResynchPkgStatus(bool ClearSettings)
        {
            throw new NotImplementedException();
        }

        public string XMLPath
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
