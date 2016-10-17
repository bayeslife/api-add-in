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
        string path = "d:\\generated";
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

        public string apiDirectoryPath(string apiName,double version)
        {
            string result;
            if (version == APIAddinClass.RAML_0_8)
                result = path + @"\" + apiName+ @"\src\main\api\";
            else
            {
                String versionName = version.ToString("F1");
                result = path + @"\" + apiName + @"\src\main\api\";
                //result = path + @"\" + apiName + @"\src\main\api-" + versionName + @"\";
            }
                
            //if (logger != null)
            //    logger.log("FilePath:" + result);
            return result;
        }
        public string apiPath(string apiName,double version)
        {
            string result = apiDirectoryPath(apiName, version);
            if (version == APIAddinClass.RAML_0_8)
                result =  result + "api.raml";
            else
            {
                String versionName = version.ToString("F1");
                result = result + "api-" + versionName + ".raml";
            }
                
                ////if (logger != null)
                ////    logger.log("FilePath:" + result);
                return result;
        }

        public string diagramPath(string page, string name)
        {
            string result = path + @"\content\" + page + "---" + name + ".svg";
            if (logger != null)
                logger.log("FilePath:" + result);
            return result;
        }

        public string samplePath(string sampleName,string classifierName)
        {
            string result = apiDirectoryPath(apiPackageName, APIAddinClass.RAML_0_8) + @"samples\" + sampleName + "-sample."+classifierName+".json";
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
        public string schemaPath(string schemaName)
        {
            string result = apiDirectoryPath(apiPackageName, APIAddinClass.RAML_0_8) + @"schemas\" + schemaName + ".json";
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
            string directorypath = apiDirectoryPath(this.apiPackageName,version);
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

        public bool sampleExists(string sampleName,string classifierName)
        {
            return System.IO.File.Exists(samplePath(sampleName,classifierName));
        }
        public bool schemaExists(string schemaName)
        {
            return System.IO.File.Exists(schemaPath(schemaName));
        }
        public void exportAPI(string apiName, double version,string content)
        {
            string fullpath = apiPath(apiName,version);
            if(logger!=null)
                logger.log(fullpath);
            System.IO.File.WriteAllText(fullpath, content);
        }
        public void exportSchema(string schemaName, string content)
        {
            string fullpath = schemaPath(schemaName);
                if(logger!=null)
            logger.log(fullpath);
            System.IO.File.WriteAllText(fullpath, content);
        }
        public void exportSample(string sampleName, string classifierName,string content)
        {
            string fullpath = samplePath(sampleName,classifierName);
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
