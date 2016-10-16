using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.EAFacade
{
    public class EARepository : EA.Repository
    {
        public static int nextAvailableId = 1;

        public static EARepository Repository = null;
        public static EADiagram currentDiagram = null;
        public static EAPackage currentPackage = null;


        public Dictionary<int, EAAttribute> attributes = new Dictionary<int, EAAttribute>();
        public Dictionary<int, EAElement> elements = new Dictionary<int, EAElement>();
        public Dictionary<int, EAPackage> packages = new Dictionary<int, EAPackage>();

        public static int NextAvailableId()
        {
            return ++nextAvailableId;
        }

        public EARepository()
        {
        }

        public void ActivateDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public bool ActivatePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public void ActivateTab(string Name)
        {
            throw new NotImplementedException();
        }

        public bool ActivateTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public bool ActivateToolbox(string Toolbox, int Options)
        {
            throw new NotImplementedException();
        }

        public bool AddDefinedSearches(string sXML)
        {
            throw new NotImplementedException();
        }

        public bool AddDocumentationPath(object Name, object Path, int Type)
        {
            throw new NotImplementedException();
        }

        public bool AddPerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public dynamic AddTab(string TabName, string ControlID)
        {
            throw new NotImplementedException();
        }

        public dynamic AddWindow(string TabName, string ControlID)
        {
            throw new NotImplementedException();
        }

        public void AdviseConnectorChange(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public void AdviseElementChange(int ElementID)
        {
            throw new NotImplementedException();
        }

        public EA.App App
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Authors
        {
            get { throw new NotImplementedException(); }
        }

        public bool BatchAppend
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

        public bool ChangeLoginUser(string Name, string Password)
        {
            throw new NotImplementedException();
        }

        public bool ClearAuditLogs(object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public void ClearOutput(string Name)
        {
            throw new NotImplementedException();
        }

        public EA.Collection Clients
        {
            get { throw new NotImplementedException(); }
        }

        public void CloseAddins()
        {
            throw new NotImplementedException();
        }

        public void CloseDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void CloseFile()
        {
            throw new NotImplementedException();
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }

        public EA.DocumentGenerator CreateDocumentGenerator()
        {
            throw new NotImplementedException();
        }

        public bool CreateModel(EA.CreateModelType CreateType, string FilePath, int ParentWnd)
        {
            throw new NotImplementedException();
        }

        public EA.ModelWatcher CreateModelWatcher()
        {
            throw new NotImplementedException();
        }

        public void CreateOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public string CustomCommand(string ClassName, string MethodName, string Parameters)
        {
            throw new NotImplementedException();
        }

        public EA.Collection Datatypes
        {
            get { throw new NotImplementedException(); }
        }

        public bool DeletePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public EA.EAEditionTypes EAEdition
        {
            get { throw new NotImplementedException(); }
        }

        public EA.EAEditionTypes EAEditionEx
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnableCache
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

        public int EnableEventFlags
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

        public bool EnableUIUpdates
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

        public void EnsureOutputVisible(string Name)
        {
            throw new NotImplementedException();
        }

        public void Execute(string SQL)
        {
            throw new NotImplementedException();
        }

        public void ExecutePackageBuildScript(int ScriptOptions, string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public string ExtractImagesFromNote(object Notes, object absPath, object imagePath)
        {
            throw new NotImplementedException();
        }

        public bool FlagUpdate
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

        public string GetActivePerspective()
        {
            throw new NotImplementedException();
        }

        public EA.Attribute GetAttributeByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public EA.Attribute GetAttributeByID(int AttributeID)
        {
            return EARepository.Repository.attributes[AttributeID];
        }

        public EA.Connector GetConnectorByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public EA.Connector GetConnectorByID(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public EA.ObjectType GetContextItem(out object Item)
        {
            throw new NotImplementedException();
        }

        public EA.ObjectType GetContextItemType()
        {
            return EA.ObjectType.otDiagram;
            //throw new NotImplementedException();
        }

        public dynamic GetContextObject()
        {
            return currentDiagram;
            //throw new NotImplementedException();
        }

        public string GetCounts()
        {
            throw new NotImplementedException();
        }

        public EA.Diagram GetCurrentDiagram()
        {
            return currentDiagram;
        }

        public string GetCurrentLoginUser(bool GetGuid = false)
        {
            throw new NotImplementedException();
        }

        public dynamic GetDiagramByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public EA.Diagram GetDiagramByID(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public EA.Element GetElementByGuid(string GUID)
        {
            throw new NotImplementedException();
        }


        public EA.Element GetElementByID(int ElementID)
        {
            return elements[ElementID];
        }

        public EA.Collection GetElementSet(string IDList, int Unused)
        {
            throw new NotImplementedException();
        }

        public EA.Collection GetElementsByQuery(string QueryName, string SearchTerm)
        {
            throw new NotImplementedException();
        }

        public string GetFieldFromFormat(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public string GetFormatFromField(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public string GetFormattedName(object GUID, int FlagInclude, object Separator, int FlagFormat)
        {
            throw new NotImplementedException();
        }

        public string GetGapAnalysisMatrix()
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public EA.IDualMailInterface GetMailInterface()
        {
            throw new NotImplementedException();
        }

        public EA.Method GetMethodByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public EA.Method GetMethodByID(int MethodID)
        {
            throw new NotImplementedException();
        }

        public EA.Package GetPackageByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public EA.Package GetPackageByID(int PackageID)
        {
            return packages[PackageID];
        }

        public EA.Project GetProjectInterface()
        {
            throw new NotImplementedException();
        }

        public EA.Reference GetReferenceList(string ListName)
        {
            throw new NotImplementedException();
        }

        public string GetRelationshipMatrix()
        {
            throw new NotImplementedException();
        }

        public string GetTechnologyVersion(string ID)
        {
            throw new NotImplementedException();
        }

        public EA.Collection GetTreeSelectedElements()
        {
            throw new NotImplementedException();
        }

        public EA.ObjectType GetTreeSelectedItem(out object Item)
        {
            throw new NotImplementedException();
        }

        public EA.ObjectType GetTreeSelectedItemType()
        {
            throw new NotImplementedException();
        }

        public dynamic GetTreeSelectedObject()
        {
            throw new NotImplementedException();
        }

        public EA.Package GetTreeSelectedPackage()
        {
            throw new NotImplementedException();
        }

        public string GetTreeXML(int RootPackageID)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXMLByGUID(string GUID)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXMLForElement(int ElementID)
        {
            throw new NotImplementedException();
        }

        public string HasPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public void HideAddinWindow()
        {
            throw new NotImplementedException();
        }

        public void ImportPackageBuildScripts(string PackageGUID, string BuildScriptXML)
        {
            throw new NotImplementedException();
        }

        public bool ImportTechnology(string Technology)
        {
            throw new NotImplementedException();
        }

        public string InstanceGUID
        {
            get { throw new NotImplementedException(); }
        }

        public int InvokeConstructPicker(object ConstructType)
        {
            throw new NotImplementedException();
        }

        public string InvokeFileDialog(object FilterString, int DefaultFilterIndex, int Flags)
        {
            throw new NotImplementedException();
        }

        public bool IsSecurityEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public int IsTabOpen(string TabName)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyEnabled(string ID)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyLoaded(string ID)
        {
            throw new NotImplementedException();
        }

        public EA.Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public string LastUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public int LibraryVersion
        {
            get { throw new NotImplementedException(); }
        }

        public void LoadAddins()
        {
            throw new NotImplementedException();
        }

        public EA.Collection Models
        {
            get { throw new NotImplementedException(); }
        }

        public EA.ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public void OpenDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public bool OpenFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public bool OpenFile2(string FilePath, string Username, string Password)
        {
            throw new NotImplementedException();
        }

        public bool OpenFileInEditor(object Name)
        {
            throw new NotImplementedException();
        }

        public string ProjectGUID
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection ProjectRoles()
        {
            throw new NotImplementedException();
        }

        public EA.Collection PropertyTypes
        {
            get { throw new NotImplementedException(); }
        }

        public void RefreshModelView(int PackageID)
        {
            throw new NotImplementedException();
        }

        public void RefreshOpenDiagrams(bool FullReload)
        {
            throw new NotImplementedException();
        }

        public void ReloadDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void RemoveOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void RemoveTab(string Name)
        {
            throw new NotImplementedException();
        }

        public bool RemoveWindow(object TabName)
        {
            throw new NotImplementedException();
        }

        public string RepositoryType()
        {
            throw new NotImplementedException();
        }

        public EA.Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public void RunModelSearch(string QueryName, string SearchTerm, string SearchOptions, string SearchData)
        {
            throw new NotImplementedException();
        }

        public string SQLQuery(string SQL)
        {
            return "<root/>";
        }

        public void SaveAllDiagrams()
        {
            throw new NotImplementedException();
        }

        public bool SaveAuditLogs(string FilePath, object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public void SaveDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public bool ScanXMIAndReconcile()
        {
            throw new NotImplementedException();
        }

        public EA.SchemaComposer SchemaComposer
        {
            get { throw new NotImplementedException(); }
        }

        public void SetUIPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public bool ShowAddinWindow(object TabName)
        {
            throw new NotImplementedException();
        }

        public void ShowBrowser(string TabName, string URL)
        {
            throw new NotImplementedException();
        }

        public void ShowDynamicHelp(string Topic)
        {
            throw new NotImplementedException();
        }

        public void ShowInProjectView(object Object)
        {
            throw new NotImplementedException();
        }

        public void ShowProfileToolbox(string Technology, string Profile, bool Show)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new NotImplementedException();
        }

        public EA.Simulation Simulation
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Stereotypes
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuppressEADialogs
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

        public bool SuppressSecurityDialog
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

        public bool SynchProfile(object Profile, object Stereotype)
        {
            throw new NotImplementedException();
        }

        public EA.Collection Tasks
        {
            get { throw new NotImplementedException(); }
        }

        public EA.Collection Terms
        {
            get { throw new NotImplementedException(); }
        }

        public void VersionControlResynchPkgStatuses(bool ClearSettings)
        {
            throw new NotImplementedException();
        }

        public void WriteOutput(string Name, string String, int ID)
        {
            throw new NotImplementedException();
        }

        public int __TempDebug(int No, DateTime No2, out int pNo3)
        {
            throw new NotImplementedException();
        }
    }
}
