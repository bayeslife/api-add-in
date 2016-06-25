using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIAddIn;

namespace UnitTestProject1.EAFacade
{
    public class EAMetaModel
    {
        public EAElement META_API;
        public EAElement META_RESOURCE;
        public EAElement META_RESOURCE_TYPE;
        public EAElement META_TYPE_FOR_RESOURCE;
        public EAElement META_SCHEMA;
        public EAElement META_METHOD;
        public EAElement META_TRAIT;
        //public EAElement META_DATAITEM;
        public EAElement META_RELEASEPIPELINE;
        public EAElement META_ENVIRONMENT;

        public EAPackage apiPackage = null;
        public EAPackage soaPackage = null;
        public EAPackage schemaPackage = null;
        public EAPackage samplesPackage = null;
        public EAPackage samplePackage = null;

        public EADiagram apiDiagram = null;
        public EADiagram soaDiagram = null;
        public EADiagram schemaDiagram = null;
        public EADiagram sampleDiagram = null;

        public EAMetaModel()
        {

            EARepository.Repository = new EARepository();

            META_API = new EAElement();
            META_RESOURCE = new EAElement();
            META_RESOURCE_TYPE = new EAElement();
            META_TYPE_FOR_RESOURCE = new EAElement();
            META_SCHEMA = new EAElement();
            META_METHOD = new EAElement();
            META_TRAIT = new EAElement();
            //META_DATAITEM = new EAElement();
            META_RELEASEPIPELINE = new EAElement();
            META_ENVIRONMENT = new EAElement();

            META_API.Name = APIAddinClass.METAMODEL_API;
            META_RESOURCE.Name = APIAddinClass.METAMODEL_RESOURCE;
            META_RESOURCE_TYPE.Name = APIAddinClass.METAMODEL_RESOURCETYPE;
            META_TYPE_FOR_RESOURCE.Name = APIAddinClass.METAMODEL_TYPE_FOR_RESOURCE;
            META_SCHEMA.Name = APIAddinClass.METAMODEL_SCHEMA;
            META_METHOD.Name = APIAddinClass.METAMODEL_METHOD;
            META_TRAIT.Name = APIAddinClass.METAMODEL_TRAIT;
            //META_DATAITEM.Name = APIAddInClass.METAMODEL_DATAITEM;
            META_RELEASEPIPELINE.Name = APIAddinClass.METAMODEL_RELEASEPIPELINE;
            META_ENVIRONMENT.Name = APIAddinClass.METAMODEL_ENVIRONMENT;

            apiPackage = new EAPackage("UnitTest");

            object o = apiPackage.Packages.AddNew(APIAddinClass.API_PACKAGE_SCHEMAS, APIAddinClass.EA_TYPE_PACKAGE);
            schemaPackage = (EAPackage)o;            
            schemaPackage.ParentID = apiPackage.PackageID;

            o = apiPackage.Packages.AddNew(APIAddinClass.API_PACKAGE_SAMPLES, APIAddinClass.EA_TYPE_PACKAGE);
            samplesPackage = (EAPackage)o;            
            samplesPackage.ParentID = apiPackage.PackageID;

            o = samplesPackage.Packages.AddNew("sample", APIAddinClass.EA_TYPE_PACKAGE);
            samplePackage = (EAPackage)o;            
            samplePackage.ParentID = samplesPackage.PackageID;

            o = apiPackage.Diagrams.AddNew("API Diagram","");
            apiDiagram = (EADiagram)o;
            apiDiagram.Stereotype = APIAddinClass.EA_STEREOTYPE_APIDIAGRAM;            

            o = schemaPackage.Diagrams.AddNew("Schema Diagram", "");
            schemaDiagram = (EADiagram)o;
            schemaDiagram.Stereotype = APIAddinClass.EA_STEREOTYPE_SCHEMADIAGRAM;
            
            o = samplePackage.Diagrams.AddNew("Sample Diagram", "");
            sampleDiagram = (EADiagram)o;
            sampleDiagram.Stereotype = APIAddinClass.EA_STEREOTYPE_SAMPLEDIAGRAM;

            soaPackage = new EAPackage("UnitTestSOA");
            o = soaPackage.Diagrams.AddNew("SOA Diagram", "");
            soaDiagram = (EADiagram)o;
            soaDiagram.Stereotype = APIAddinClass.EA_STEREOTYPE_SOADIAGRAM;
       }

        public EAMetaModel setupAPIPackage()
        {
            EARepository.currentDiagram = apiDiagram;
            EARepository.currentPackage = apiPackage;
            return this;
        }
        public EAMetaModel setupSchemaPackage()
        {
            EARepository.currentDiagram = schemaDiagram;
            EARepository.currentPackage = schemaPackage;
            return this;
        }
        public EAMetaModel setupSamplePackage()
        {
            EARepository.currentDiagram = sampleDiagram;
            EARepository.currentPackage = samplePackage;
            return this;
        }
        public EAMetaModel setupSOAPackage()
        {
            EARepository.currentDiagram = soaDiagram;
            EARepository.currentPackage = soaPackage;
            return this;
        }

    }
}
