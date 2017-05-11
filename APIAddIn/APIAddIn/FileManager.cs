using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAddIn
{
    /* This class manages the writing of documents to the file system */
    public class FileManager
    {
        public string path = "d:\\generated";
        public string diagrampath = "d:\\tmp";

        Logger logger = null;
        string apiPackageName;
        //string canonicalPackageName;


        public FileManager(Logger l)
        {
            this.logger = l;
        }

        public void setBasePath(string path)
        {
            this.path = path;
        }

        public void setDiagramPath(string path)
        {
            this.diagrampath = path;
        }

        public string getNamespace(EA.Repository Repository, EA.Package package)
        {
            EA.Package p = package;
            List<string> namespaceList = new List<string>();
            String res = "";

            bool done = false;
            while ( !done )
            {
                if (p != null)
                {
                    namespaceList.Insert(0, p.Name);
                    if ( p.IsNamespace == true)
                        done = true;
                    if (p.ParentID != 0)
                        p = Repository.GetPackageByID(p.ParentID);
                    else
                        return "";
                }
                else
                    done = true;
            }

            foreach(string s in namespaceList) {
                res = res + (s + @"\");
            }

            return res;
        }

        public string apiDirectoryPath(string apiName,double version, string namespacePath)
        {
            string result;
            string pathBase;

            if (namespacePath != "")
                pathBase = path + @"\" + namespacePath;
            else
                pathBase = path + @"\" + apiName;

            if (version == APIAddinClass.RAML_0_8)
                result = pathBase + @"\src\main\api\";
            else
            {
                String versionName = version.ToString("F1");
                result = pathBase + @"\src\main\api\";
            }

            return result;
        }
        public string apiPath(string apiName,double version, string namespacePath)
        {
            string result = apiDirectoryPath(apiName, version, namespacePath);
            System.IO.Directory.CreateDirectory(result);
            if (version == APIAddinClass.RAML_0_8)
                result =  result + "api.raml";
            else
            {
                String versionName = version.ToString("F1");
                result = result + "api-" + versionName + ".raml";
            }

            return result;
        }

        public string diagramPath(string page, string name)
        {
            string result = diagrampath + @"\content\" + page + "---" + name + ".svg";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }

        public string samplePath(string sampleName,string classifierName, string namespacePath)
        {
            string result = apiDirectoryPath(sampleName, APIAddinClass.RAML_0_8, namespacePath) + @"samples\";
            System.IO.Directory.CreateDirectory(result);
            result += sampleName + "-sample." + classifierName + ".json";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string sampleIncludePath(string sampleName,string classifierName)
        {
            string result = "samples/" + sampleName + "-sample."+classifierName+".json";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string examplesPath(string sampleName)
        {
            string result = "examples/" + sampleName + ".raml";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string samplePlaceholderPath(string sampleName)
        {
            string result = "samples/" + sampleName + ".json";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string schemaPath(string schemaName, string namespacePath)
        {
            string result = apiDirectoryPath(schemaName, APIAddinClass.RAML_0_8, namespacePath) + @"schemas\";
            System.IO.Directory.CreateDirectory(result);
            result += schemaName + ".json";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string resourceTypeIncludePath(string name)
        {
            string result = "resources/" + name + ".raml";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        public string schemaIncludePath(string schemaName)
        {
            string result = "schemas/" + schemaName + ".json";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }
        

        public void setup(double version)
        {
            string directorypath = apiDirectoryPath(this.apiPackageName,version,"");
            //if (logger != null)
            //    logger.log("Creating directory:" + directorypath);
            System.IO.Directory.CreateDirectory(directorypath);
            string schemapath = directorypath + @"\schemas";
            //if (logger != null)
            //    logger.log("Creating directory:" + schemapath);
            System.IO.Directory.CreateDirectory(schemapath);
            string samplepath = directorypath + @"\samples";
            System.IO.Directory.CreateDirectory(samplepath);
        }
        public void initializeAPI(string apiPkg)
        {
            this.apiPackageName = apiPkg;
        }

        public bool sampleExists(string sampleName,string classifierName, string namespacePath)
        {
            return System.IO.File.Exists(samplePath(sampleName,classifierName, namespacePath));
        }
        public bool schemaExists(string schemaName, string namespacePath)
        {
            return System.IO.File.Exists(schemaPath(schemaName, namespacePath));
        }
        public void exportAPI(string apiName, double version,string content, string namespacePath)
        {
            string fullpath = apiPath(apiName,version, namespacePath);
            if(logger!=null)
                logger.log(fullpath);
            System.IO.File.WriteAllText(fullpath, content);
        }
        public void exportSchema(string schemaName, string content, string namespacePath)
        {
            string fullpath = schemaPath(schemaName, namespacePath);
                if(logger!=null)
            logger.log(fullpath);
            System.IO.File.WriteAllText(fullpath, content);
        }
        public void exportSample(string sampleName, string classifierName,string content, string namespacePath)
        {
            string fullpath = samplePath(sampleName,classifierName, namespacePath);
            if(logger!=null)
                logger.log(fullpath);
            System.IO.File.WriteAllText(fullpath, content);
        }

        //  Canonical File Management Stuff
        //public void initializeCanonical(string pkg)
        //{
        //    this.canonicalPackageName = pkg;
        //}
        //public string canonicalDirectoryPath(string name)
        //{
        //    string result = path + @"\cdm\src\main\resources\"+name;
        //    if (logger != null)
        //        logger.log("FilePath:" + result);
        //    return result;
        //}
        //public void setupCanonical()
        //{
        //    string directorypath = canonicalDirectoryPath(this.canonicalPackageName);
        //    if (logger != null)
        //        logger.log("Creating directory:" + directorypath);
        //    System.IO.Directory.CreateDirectory(directorypath);
        //    string schemapath = directorypath + @"\schemas";
        //    if (logger != null)
        //        logger.log("Creating directory:" + schemapath);
        //    System.IO.Directory.CreateDirectory(schemapath);
        //    string samplepath = directorypath + @"\samples";
        //    System.IO.Directory.CreateDirectory(samplepath);
        //}        
        //public void exportCanonical(string schemaName, string content)
        //{
        //    string fullpath = canonicalSchemaPath(schemaName);
        //    if (logger != null)
        //        logger.log(fullpath);
        //    System.IO.File.WriteAllText(fullpath, content);
        //}
        // public string canonicalSchemaPath(string schemaName)
        //{
        //    string result = canonicalDirectoryPath(canonicalPackageName) + @"\schemas\" + schemaName + ".json";
        //    if (logger != null)
        //        logger.log("FilePath:" + result);
        //    return result;
        //}
    }
}
