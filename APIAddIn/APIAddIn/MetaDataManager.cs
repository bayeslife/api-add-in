using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIAddIn
{
   

    public class MetaDataManager
    {

        static Logger logger = new Logger();

        static public void setLogger(Logger l)
        {
            logger = l;
        }

        static public IList<EA.Element> diagramSamples(EA.Repository Repository,EA.Diagram diagram)
        {
            List<EA.Element> samples = new List<EA.Element>();             
            foreach (EA.DiagramObject diagramObject in diagram.DiagramObjects)
            {
                EA.Element el = Repository.GetElementByID(diagramObject.ElementID);
                if (el.Type.Equals(APIAddinClass.EA_TYPE_OBJECT))
                    samples.Add(el);
            }
            return samples;
        }

        public static IList<EA.Element> diagramClasses(EA.Repository Repository,EA.Diagram diagram)
        {
            List<EA.Element> samples = new List<EA.Element>();
            
            foreach (EA.DiagramObject diagramObject in diagram.DiagramObjects)
            {
                EA.Element el = Repository.GetElementByID(diagramObject.ElementID);
                if (el.Type == null)
                    continue;
                if (el.Type.Equals(APIAddinClass.EA_TYPE_CLASS) || el.Type.Equals(APIAddinClass.EA_TYPE_ENUMERATION))
                    samples.Add(el);
            }
            return samples;
        }


        public static IList<EA.Element> diagramElements(EA.Repository Repository)
        {
            List<EA.Element> samples = new List<EA.Element>();
            EA.Diagram diagram = null;
            if (Repository.GetContextItemType() == EA.ObjectType.otDiagram)
                diagram = Repository.GetContextObject(); 


            foreach (EA.DiagramObject diagramObject in diagram.DiagramObjects)
            {
                EA.Element el = Repository.GetElementByID(diagramObject.ElementID);                
                samples.Add(el);
            }
            return samples;
        }


        /* Finds the Objects with a classifier of API on the diagram */
        public static EA.Element diagramAPI(EA.Repository Repository,EA.Diagram diagram)
        {            
            foreach (EA.DiagramObject diagramObject in diagram.DiagramObjects)
            {
                EA.Element el = Repository.GetElementByID(diagramObject.ElementID);
                if (el.Type.Equals(APIAddinClass.EA_TYPE_OBJECT))
                {
                    EA.Element classifier = Repository.GetElementByID(el.ClassifierID);
                    if (classifier.Name.Equals(APIAddinClass.METAMODEL_API))
                    {
                        return el;
                    }
                }
            }
            return null;
        }

        public static void setAsAPIDiagram(EA.Repository Repository, EA.Diagram diagram)
        {
            diagram.Stereotype = APIAddinClass.EA_STEREOTYPE_APIDIAGRAM;
            diagram.Update();
        }
        public static void setAsSchemaDiagram(EA.Repository Repository, EA.Diagram diagram)
        {
            diagram.Stereotype = APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM;
            diagram.Update();            
        }
        public static void setAsSampleDiagram(EA.Repository Repository, EA.Diagram diagram)
        {
            diagram.Stereotype = APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM;
            diagram.Update();
        }

        public static bool filterMethod(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddinClass.METAMODEL_METHOD))
                return true;
            return false;
        }
        public static bool filterQueryParameter(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddinClass.METAMODEL_QUERY_PARAMETER))
                return true;
            return false;
        }

        public static bool filterPermission(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddinClass.METAMODEL_PERMISSION))
                return true;
            return false;
        }

        public static bool filterResponse(EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddinClass.METAMODEL_RESPONSE))
                return true;
            return false;
        }

        public static bool filterResource(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_RESOURCE))
                return true;
            return false;
        }

        public static bool filterTypeForResource(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_TYPE_FOR_RESOURCE))
                return true;
            return false;
        }

        public static bool filterResourceType(EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_RESOURCETYPE))
                return true;
            return false;
        }

        public static bool filterSecurity(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier == null)
            {
                //logger.log("Filtering for Security: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_SECURITYSCHEME))
                    return true;
                else
                {
                    // If the classifier is inherited from the Security Scheme in the meta model then return true
                    EA.Collection baseClasses = classifier.BaseClasses;
                    foreach( EA.Element baseClass in baseClasses) {
                        if (baseClass.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_SECURITYSCHEME))
                            return true;
                    }

                }
                    
            }
            return false;
        }

        public static bool filterCommunity(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier == null)
            {
                //logger.log("Filtering for Community: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_COMMUNITY))
                    return true;
                else
                {
                    //logger.log("FFS" + classifier.Name + " " + APIAddinClass.APIAddinClassClass.METAMODEL_COMMUNITY);
                }
                    
            }
            return false;
        }

        public static bool filterTrait(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier == null)
            {
                //logger.log("Filtering for Community: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_TRAIT))
                    return true;
                else
                {
                    //logger.log("FFS" + classifier.Name + " " + APIAddinClass.APIAddinClassClass.METAMODEL_COMMUNITY);
                }

            }
            return false;
        }

        public static bool filterRequestExample(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {            
            if (classifier != null)
            {                
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_PLACEHOLDER))
                {                    
                    if (con.SupplierEnd.Role == "Request")
                    {                 
                        return true;
                    }
                        
                }
            }
            return false;
        }
        public static bool filterResponseExample(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {            
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_PLACEHOLDER))
                {
                    if(con.SupplierEnd.Role=="Response")
                        return true;
                }                                    
            }
            return false;
        }


        public static bool filterReleasePipeline(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier == null)
            {
                //logger.log("Filtering for Community: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_RELEASEPIPELINE))
                    return true;
                else
                {
                    //logger.log("FFS" + classifier.Name + " " + APIAddinClass.APIAddinClassClass.METAMODEL_COMMUNITY);
                }

            }
            return false;
        }

        public static bool filterEnvironment(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier == null)
            {
                //logger.log("Filtering for Community: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_ENVIRONMENT))
                    return true;
                else
                {
                    //logger.log("FFS" + classifier.Name + " " + APIAddinClass.APIAddinClassClass.METAMODEL_COMMUNITY);
                }

            }
            return false;
        }


        public static bool filterSample(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (!filterObject(Repository, con, e, classifier))
                return false;
            if (classifier == null)
            {
                //logger.log("Filtering for Sample: Ignoring" + e.Name);
            }
            if (classifier != null)
            {
                if (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_SAMPLE))
                    return true;
                else
                {
                    //logger.log("FFS" + classifier.Name + " " + APIAddinClass.APIAddinClassClass.METAMODEL_SAMPLE);
                }
                    
            }
            return false;
        }


        public static bool filterSchema(EA.Repository Repository,EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (e.Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_CLASS))
                return true;
            return false;
        }

        public static bool filterClass(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (e.Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_CLASS))
                return true;
            return false;
        }

        public static bool filterObject(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && e.Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_OBJECT))
                return true;
            return false;
        }

        public static bool filterObjectNotClassifiedAsMethod(EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null)
            {
                //logger.log("FilterObjectNotClassifiedAsMethod Classifier:" + classifier.Name);
                if (e.Type.Equals(APIAddIn.APIAddinClass.EA_TYPE_OBJECT) && (classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_METHOD)))
                    return true;
            }
            return false;
        }

        public static bool filterContentType(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (classifier != null && classifier.Name.Equals(APIAddIn.APIAddinClass.METAMODEL_CONTENTTYPE))
                return true;
            return false;
        }

        public static bool filterDataItem(EA.Repository Repository, EA.Connector con, EA.Element e, EA.Element classifier)
        {
            if (e != null)
            {
                if(DiagramManager.isVisible(con))
                    if (e.Stereotype.Contains(APIAddIn.APIAddinClass.METAMODEL_DATAITEM))
                        return true;
            }
            return false;
        }


        static public Boolean isCDMPackage(EA.Repository Repository, EA.Package package)
        {
            //MessageBox.Show("Checking CDM Package:" + package.Name);
            if (package.Name == "CommonDataModel"){
                //MessageBox.Show("Is CDM Pkg");
                return true;
            } else if (package.ParentID == null || package.ParentID == 0){
                //MessageBox.Show("Is not CDM Pkg");
                return false;
            }                
            else
            {
                EA.Package parentPackage = Repository.GetPackageByID(package.ParentID);
                return isCDMPackage(Repository, parentPackage);
            }
        }
    }
}
