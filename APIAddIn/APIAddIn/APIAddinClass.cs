using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using EA;
using SVGExport;

namespace APIAddIn
{
    public class APIAddinClass
    {
        // define menu constants
        const string menuHeader = "-&API MDG";
        const string menuHeaderExperimental = "-&API Experimental";

        const string menuWeb = "&Call Web";

        const string menuToggleLogging = "Toggle Logging";

        const string menuValidateDiagram = "&ValidateDiagram";

        const string menuExportDiagram = "&ExportDiagram";

        const string menuExportAll = "&ExportAll";
        const string menuExportPackage = "&ExportPackage";

        const string menuImportSOA = "&ImportSexportAllOA";
        const string menuExportSOA = "&ExportSOA";
        const string menuExportAPI = "&ExportAPI";
        const string menuExportSchema = "&ExportSchemas";
        const string menuExportSample = "&ExportSamples";
        const string menuSyncSample = "&SyncDiagramSample";
        //const string menuExportCanonical = "&ExportCanonical";

        const string menuUpdateClassFromInstance = "&UpdateClassFromInstance";
        const string menuUpdateInstanceFromClass = "&UpdateInstanceFromClass";

        const string menuCreateSample = "&GenerateSample";


        const string menuSqlQuery = "&SqlQuery";

        static Logger logger = new Logger();

        static FileManager fileManager = new FileManager(APIAddinClass.logger);

        public static string EA_TYPE_BOOLEAN = "boolean";
        public static string EA_TYPE_INT = "int";
        public static string EA_TYPE_DECIMAL = "decimal";
        public static string EA_TYPE_FLOAT = "float";
        public static string EA_TYPE_DATE = "date";
        public static string EA_TYPE_STRING = "String";
        public static string EA_TYPE_CURRENCY = "currency";
        public static string EA_TYPE_ATTRIBUTE = "Attribute";
        public static string EA_TYPE_ASSOCIATION = "Association";
        public static string EA_TYPE_CLASS = "Class";
        public static string EA_TYPE_OBJECT = "Object";
        public static string EA_TYPE_ENUMERATION = "Enumeration";
        public static string EA_TYPE_PACKAGE = "Package";

        public static string EA_STEREOTYPE_NONE = "";
        public static string EA_STEREOTYPE_APIDIAGRAM = "APIDiagram";
        public static string EA_STEREOTYPE_SOADIAGRAM = "SOADiagram";
        public static string EA_STEREOTYPE_SCHEMADIAGRAM = "SchemaDiagram";
        public static string EA_STEREOTYPE_SAMPLEDIAGRAM = "SampleDiagram";
        //public static string EA_STEREOTYPE_CANONICALDIAGRAM = "CanonicalDiagram";

        public static string EA_STEREOTYPE_SAMPLE = "Sample";
        public static string EA_STEREOTYPE_REQUEST = "Request";
        public static string EA_STEREOTYPE_DATAITEM = "DataItem";

        public static string EA_STEREOTYPE_EMBEDDED = "Embedded";

        public static string EA_TAGGEDVALUE_PATTERN = "Pattern";
        public static string EA_TAGGEDVALUE_DEFAULT = "Default";

        public static string API_PACKAGE_SCHEMAS = "Schemas";
        public static string API_PACKAGE_SAMPLES = "Samples";

        public static string METAMODEL_API = "API";
        public static string METAMODEL_CONTENTTYPE = "ContentType";
        public static string METAMODEL_RESPONSE = "Response";
        public static string METAMODEL_RESOURCE = "Resource";
        public static string METAMODEL_RESOURCETYPE = "ResourceType";
        public static string METAMODEL_ITEMGET = "ItemGet";
        public static string METAMODEL_SECURITYSCHEME = "SecurityScheme";
        public static string METAMODEL_QUERY_PARAMETER = "QueryParameter";
        public static string METAMODEL_COMMUNITY = "Community";
        public static string METAMODEL_SAMPLE = "Sample";
        public static string METAMODEL_DATAITEM = "DataItem";
        public static string METAMODEL_TRAIT = "Trait";
        public static string METAMODEL_RELEASEPIPELINE = "ReleasePipeline";
        public static string METAMODEL_ENVIRONMENT = "Environment";

        public static string METAMODEL_PLACEHOLDER = "PlaceHolder";

        public static string RESOURCETYPE_ITEMGET = "item-get";
        public static string RESOURCETYPE_ITEMPOST = "item-post";
        public static string RESOURCETYPE_ITEMPOST_SYNC = "item-post-sync";
        public static string RESOURCETYPE_ITEMPOST_ONEWAY = "item-post-oneway";
        public static string RESOURCETYPE_COLLECTIONGETPOST = "collection-get-post";

        public static string METAMODEL_SCHEMA = "Schema";
        public static string METAMODEL_METHOD = "Method";
        public static string METAMODEL_TYPE_FOR_RESOURCE = "TypeForResource";        

        public static string CARDINALITY_0_TO_MANY = "0..*";
        public static string CARDINALITY_1_TO_MANY = "1..*";
        public static string CARDINALITY_0_TO_ONE = "0..1";
        public static string CARDINALITY_ONE = "1";

        public static string DIRECTION_SOURCE_TARGET = "Source -> Destination";
        
        ///
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        ///
        /// <param name="Repository" />the EA repository
        /// a string
        public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.
            
            logger.setRepository(Repository);

            DiagramManager.setLogger(logger);
            APIManager.setLogger(logger);
            APIManager.setFileManager(fileManager);
            SchemaManager.setLogger(logger);
            SchemaManager.setFileManager(fileManager);
            SampleManager.setLogger(logger);
            SampleManager.setFileManager(fileManager);
            WSDLManager.setLogger(logger);

            return "a string";
        }
 
        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            logger.log("location:" + Location);
            logger.log("MenuName:" + MenuName);

            EA.Diagram diagram = Repository.GetCurrentDiagram();    

                switch (MenuName)
                {
                    // defines the top level menu option
                    case "":                        
                        return menuHeader;
                                                                
                    case menuHeader:
                        string[] subMenusOther = { menuExportPackage,menuExportAll, menuExportDiagram, menuToggleLogging};
                        string[] subMenusAPI = { menuExportPackage, menuExportAll, menuExportDiagram, menuExportAPI, menuValidateDiagram, menuUpdateClassFromInstance, menuUpdateInstanceFromClass, menuToggleLogging };
                        string[] subMenusSOA = { menuExportPackage, menuExportSOA, menuExportDiagram, menuImportSOA };
                        string[] subMenusSchema = { menuExportPackage, menuExportAll, menuExportDiagram, menuValidateDiagram, menuExportSchema, menuCreateSample, menuUpdateClassFromInstance, menuUpdateInstanceFromClass, menuToggleLogging };
                        string[] subMenusSample = { menuExportPackage, menuExportAll, menuExportDiagram, menuExportSample, menuValidateDiagram, menuSyncSample, menuUpdateClassFromInstance, menuUpdateInstanceFromClass, menuToggleLogging };
                        //string[] subMenusCanonical = { menuExportAll, menuExportDiagram, menuExportCanonical, menuCreateSample, menuUpdateClassFromInstance, menuUpdateInstanceFromClass, menuToggleLogging };

                        if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_APIDIAGRAM))
                        {
                            logger.log("API Menus");
                            return subMenusAPI;
                        }
                        else if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SOADIAGRAM))
                        {
                            logger.log("SOA Menus");
                            return subMenusSOA;
                        }
                        else if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM))
                        {
                            logger.log("Schema Menus");
                            return subMenusSchema;
                        }
                        else if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM))
                        {
                            logger.log("Sample Menus");
                            return subMenusSample;
                        }
                        //else if (diagram != null && diagram.Stereotype.Equals(APIAddInClass.EA_STEREOTYPE_CANONICALDIAGRAM))
                        //{
                        //    logger.log("Canonical Menus");
                        //    return subMenusCanonical;
                        //}
            
                        return subMenusOther;

                    case menuHeaderExperimental:
                        string[] subMenus2 = { menuSqlQuery, menuWeb, };
                        //EA.Element apiEl = diagramAPI(Repository);
                        //if (apiEl == null)
                        //{
                        //    return new string[] { menuGenerate, menuGenerateSamples, menuGenerateAPI, menuValidateDiagram, };
                        //}                        
                        return subMenus2;
                }
 
            return "";
        }
 
        ///
        /// returns true if a project is currently opened
        ///
        /// <param name="Repository" />the repository
        /// true if a project is opened in EA
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;

                return true;
            }
            catch
            {
                return false;
            }
        }
 
        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {

            logger.log("Get Menu State:" + MenuName+":"+ItemName);

            if (IsProjectOpen(Repository))
            {
            
                EA.Diagram diagram = Repository.GetCurrentDiagram();    
                

                switch (ItemName)
                {
                    case menuWeb:
                        IsEnabled = true;
                        break;
                
                    
                    case menuValidateDiagram:
                        IsEnabled = true;
                        break;
                    // there shouldn't be any other, but just in case disable it.


                    case menuExportAPI:
                        IsEnabled = false;
                        if (diagram != null && 
                                (
                                    diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_APIDIAGRAM)
                                ||
                                    diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SOADIAGRAM)
                                )
                            )                            
                            IsEnabled = true;
                        break;

                    case menuExportSchema:
                    case menuCreateSample:
                        IsEnabled = false;
                        if (diagram != null &&  diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM))
                            IsEnabled = true;
                        break;


                    case menuExportSample:
                    case menuSyncSample:
                        IsEnabled = false;
                        if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM))
                            IsEnabled = true;
                        break;

                     case menuExportAll:
                        IsEnabled = false;
                        if (diagram != null && diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_APIDIAGRAM))           
                            IsEnabled = true;
                        break;

                     //case menuExportCanonical:
                     //   IsEnabled = false;
                     //   if (diagram != null && diagram.Stereotype.Equals(APIAddInClass.EA_STEREOTYPE_CANONICALDIAGRAM))
                     //       IsEnabled = true;
                     //   break;

                    default:
                        IsEnabled = true;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                IsEnabled = false;
            }
        }
 
        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            logger.enable(Repository);

            EA.Diagram diagram = Repository.GetCurrentDiagram();
                                   
            switch (ItemName)
            {
                case menuExportAll:                    
                    exportAll(Repository);
                    break;

                case menuExportPackage:
                    exportPackage(Repository);
                    break;

                case menuExportDiagram:
                    exportDiagram(Repository);
                    break;

                case menuExportSchema:
                    try
                    {
                        SchemaManager.exportSchema(Repository, diagram);
                        MetaDataManager.setAsSchemaDiagram(Repository, diagram);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }                    
                    break;

                //case menuExportCanonical:
                //    try
                //    {
                //        SchemaManager.exportCanonical(Repository, diagram);
                //        //MetaDataManager.setAsCanonicalDiagram(Repository, diagram);
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(ex.Message);
                //    }
                //    break;
                
                case menuExportSample:                    
                    SampleManager.exportSample(Repository,diagram);
                    MetaDataManager.setAsSampleDiagram(Repository, diagram);
                    break;

                case menuSyncSample:
                    SampleManager.syncSample(Repository,diagram);
                    break;

                case menuCreateSample:
                    SchemaManager.generateSample(Repository);
                    break;

                case menuUpdateClassFromInstance:
                    SchemaManager.updateClassFromInstance(Repository);
                    break;

                case menuUpdateInstanceFromClass:
                    SchemaManager.operateOnSample(Repository, SchemaManager.updateSampleFromClass);                    
                    break;

                case menuValidateDiagram:
                    if (diagram != null)
                        if(diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM))
                            SampleManager.validateDiagram(Repository,diagram);
                        else if (diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM))
                        {
                            SchemaManager.validateDiagram(Repository, diagram);
                        }                            
                        else if (diagram.Stereotype.Equals(APIAddinClass.EA_STEREOTYPE_APIDIAGRAM))
                            APIManager.validateDiagram(Repository, diagram);
                    break;

                case menuExportAPI:                    
                    APIManager.exportAPI(Repository,diagram);
                    MetaDataManager.setAsAPIDiagram(Repository, diagram);
                    break;

                
                case menuToggleLogging:
                    logger.toggleLogging(Repository);                    
                    break;

                case menuWeb:
                    this.callWeb(Repository);
                    break;

                case menuImportSOA:
                    WSDLManager.importWSDLs(Repository, diagram);
                    break;

                case menuExportSOA:
                    WSDLManager.exportWSDLs(Repository, diagram);
                    break;
            }
        }

        private void exportPackage(EA.Repository Repository)
        {
            EA.Package pkg = Repository.GetTreeSelectedPackage();
            exportPackage(Repository, pkg);
        }

        private void exportPackage(EA.Repository Repository, EA.Package pkg)
        {
            foreach (EA.Package p in pkg.Packages)
            {
                exportAPIPackage(Repository, p);
                exportPackage(Repository, p);//recurse
            }
        }

        private void exportAll(EA.Repository Repository)
        {
            EA.Diagram diagram = Repository.GetCurrentDiagram();
            APIManager.exportAPI(Repository, diagram);

            EA.Package apiPackage = Repository.GetPackageByID(diagram.PackageID);

            exportAPIPackage(Repository, apiPackage);
        }

        private void exportAPIPackage(EA.Repository Repository, EA.Package apiPackage)
        {
            EA.Package samplePackage = null;
            EA.Package schemasPackage = null;
            foreach (EA.Package p in apiPackage.Packages)
            {
                logger.log("Package:" + p.Name);
                if (p.Name.Equals(APIAddinClass.API_PACKAGE_SAMPLES))
                {
                    samplePackage = p;
                }
                else if (p.Name.Equals(APIAddinClass.API_PACKAGE_SCHEMAS))
                {
                    schemasPackage = p;
                }
            }

            if (samplePackage == null || schemasPackage == null)
            {
                logger.log("No an api/model package:" + apiPackage);
                return;
            }

            logger.log("Found api/model package:" + apiPackage);
            if (samplePackage != null)
            {
                List<EA.Package> pkgs = new List<EA.Package>();
                pkgs.Add(samplePackage);
                foreach (EA.Package child in samplePackage.Packages)
                {
                    pkgs.Add(child);
                }

                foreach (EA.Package sp in pkgs)
                {
                    logger.log("Exporting Samples:" + sp.Name);

                    foreach (object obj in sp.Diagrams)
                    {
                        EA.Diagram samplediagram = (EA.Diagram)obj;
                        logger.log("Exporting Schema Diagram:" + samplediagram.Name);
                        SampleManager.exportSample(Repository, samplediagram);

                    }
                }

            }
            if (schemasPackage != null)
            {
                logger.log("Exporting Schemas:" + schemasPackage.Name);
                foreach (EA.Diagram schemadiagram in schemasPackage.Diagrams)
                {
                    logger.log("Exporting Sample Diagram:" + schemadiagram.Name);
                    SchemaManager.exportSchema(Repository, schemadiagram);
                }
            }
        }

        private void exportDiagram(EA.Repository Repository)
        {
            EA.Diagram diagram = Repository.GetCurrentDiagram();

            string confluencedata = null;

            if (diagram.Version != "1.0") {
                confluencedata = diagram.Version;            
            }else{
                confluencedata = diagram.Notes;            
            }
            if(confluencedata==null || confluencedata.Length==0){
                MessageBox.Show("Please define the diagram version as <confluence page name>");
            }
            else
            {
                char[] delimiter = { ',' };
                string[] pages = confluencedata.Split(delimiter);
                foreach(string page in pages){
                    string file = @"d:\tmp\content\" + page + "---" + diagram.Name + ".svg";
                    logger.log(file);
                    SVGExport.EAPlugin.SaveDiagramAsSvg(Repository, diagram, file);
                }                
            }
                
        }


        private void exportAllGlobal(EA.Repository Repository)
        {
            {
                List<string> diagrams = DiagramManager.queryAPIDiagrams(Repository);
                foreach (string diagramId in diagrams)
                {
                    EA.Diagram diagram = Repository.GetDiagramByGuid(diagramId);
                    logger.log("Exporting Diagram:" + diagram.Name);
                    APIManager.exportAPI(Repository, diagram);
                    logger.log("Exported Diagram:" + diagram.Name);
                }
            }
            {
                List<string> diagrams = DiagramManager.querySchemaDiagrams(Repository);
                foreach (string diagramId in diagrams)
                {
                    EA.Diagram diagram = Repository.GetDiagramByGuid(diagramId);
                    logger.log("Exporting Schema Diagram:" + diagram.Name);
                    SchemaManager.exportSchema(Repository, diagram);
                }
            }
            {
                List<string> diagrams = DiagramManager.querySampleDiagrams(Repository);
                foreach (string diagramId in diagrams)
                {
                    EA.Diagram diagram = Repository.GetDiagramByGuid(diagramId);

                    EA.Package samplePackage = Repository.GetPackageByID(diagram.PackageID);
                    EA.Package apiPackage = Repository.GetPackageByID(samplePackage.ParentID);

                    logger.log("Exporting Sample Diagram:" + diagram.Name + " from api package:" + apiPackage.Name);
                    SampleManager.exportSample(Repository, diagram);
                }
            }
        }

        //public List<string> queryAPIDiagrams2(EA.Repository Repository)
        //{
            
        //}

        //public List<string> queryAPIDiagrams2(EA.Repository Repository)
        //{
        //    EA.Collection diagrams = Repository.GetElementsByQuery(
        //        "StateMachine Diagrams", "");
        //    MessageBox.Show("here");
        //    List<string> result = new List<string>();
        //    foreach (object dia in diagrams)
        //    {
        //        EA.Diagram d = (EA.Diagram)dia;
        //        result.Add(d.DiagramGUID);                
        //    }
        //    return result;
        //}
    

        private void callWeb(EA.Repository Repository)
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create("http://xceptionale.com");
            // If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            string status = ((HttpWebResponse)response).StatusDescription;

            MessageBox.Show("Status:"+status);

            // Get the stream containing content returned by the server.
            //Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            //string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            //reader.Close();
            response.Close();
        }

        private void callWeb2(EA.Repository Repository)
        {
            object o;
            EA.ObjectType type = Repository.GetContextItem(out o);
            MessageBox.Show("Type:" + type);

            EA.Element e = (EA.Element)o;
            MessageBox.Show("Name:" + e.Name);

        }

        ///
        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        ///
        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
 
    }

   
   
}
