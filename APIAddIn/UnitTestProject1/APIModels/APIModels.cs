using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1.EAFacade;
using APIAddIn;

namespace UnitTestProject1.APIModels
{
    public class APIModel
    {
        static public EAFactory createInvalidAPI(EAMetaModel meta)
        {
            EAFactory factory = new EAFactory();

            meta.setupSchemaPackage();

            EAFactory typesForResource = factory.setupClient("item-get", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_NONE, meta.META_TYPE_FOR_RESOURCE.ElementID, null);

            //Root class has lower case name
            EAFactory rootclass = typesForResource.addSupplier("rootClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "requestSchema", APIAddinClass.CARDINALITY_ONE, null);
            
            //Invalid supplier end role
            EAFactory childClass = rootclass.addSupplier("ChildClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, ""/*Invalid supplierEndRole is empty string*/, "0..*"/*cardinality*/, null);

            //No cardinality
            EAFactory childClass2 = childClass.addSupplier("ChildClass2", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "child", ""/*invalid no cardinality*/, null);
            
            meta.setupAPIPackage();

            EAFactory factoryresult = factory.setupClient("APITitle", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "APITitle", "version", "1", "baseUri", "http://localhost", "mediaType", "application/json" });

            EAFactory resource = factory.addSupplier("/resource/{id}", APIAddinClass.EA_TYPE_OBJECT, meta.META_RESOURCE.ElementID, null/*target stereotype*/, new string[] { "test", "testvalue" }/*runstate*/, "resource"/*supplierEndRole*/, ""/*relationshipName*/, null);

            EAFactory schema = factory.addSupplier(rootclass.clientElement, "quoteSchema"/*supplier end role*/, "",null);
                        
            resource.addSupplier(typesForResource.clientElement, "item-get", "",null);

            typesForResource.addSupplier("sample200Resp", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "sample200Resp", "", null);
            
            return factoryresult;
        }

        static public EAFactory createAPI1(EAMetaModel meta)
        {                       
            EAFactory factory = new EAFactory();

            meta.setupSchemaPackage();

            EAFactory typesForResource = factory.setupClient("item-get", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_NONE,meta.META_TYPE_FOR_RESOURCE.ElementID,null);

            EAFactory rootclass = typesForResource.addSupplier("RootClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "requestSchema", APIAddinClass.CARDINALITY_ONE, null);

            rootclass.addAttribute("listStringAttr", APIAddinClass.EA_TYPE_STRING, "0", "*");

            rootclass.addAttribute("booleanAttr", APIAddinClass.EA_TYPE_BOOLEAN);
            rootclass.addAttribute("floatAttr", APIAddinClass.EA_TYPE_FLOAT);
            rootclass.addAttribute("intAttr", APIAddinClass.EA_TYPE_INT);
            rootclass.addAttribute("dateAttr", APIAddinClass.EA_TYPE_DATE,"0","1","2016-01-01");
            rootclass.addAttribute("currencyAttr", APIAddinClass.EA_TYPE_DECIMAL);

            EAFactory childClass = rootclass.addSupplier("ChildClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "0OrMoreAttribute"/*supplierEndRole*/, "0..*"/*cardinality*/, null);

            EAFactory enumExample = rootclass.addSupplier("EnumExample", APIAddinClass.EA_TYPE_ENUMERATION, 0, null/*target stereotype*/, null, "enumExample"/*supplierEndRole*/, "1"/*cardinality*/, null);
            enumExample.clientElement.Attributes.AddNew("Enum1", APIAddinClass.EA_TYPE_ATTRIBUTE);
            enumExample.clientElement.Attributes.AddNew("Enum2", APIAddinClass.EA_TYPE_ATTRIBUTE);

            meta.setupAPIPackage();

            EAFactory api = factory.setupClient("APITitle", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "APITitle", "version", "1", "baseUri", "http://localhost", "mediaType", "application/json" });

            EAFactory resource = factory.addSupplier("/resource/{someuriparameter}", APIAddinClass.EA_TYPE_OBJECT, meta.META_RESOURCE.ElementID, null/*target stereotype*/, new string[] { "test", "testvalue" }/*runstate*/, "resource"/*supplierEndRole*/, ""/*relationshipName*/, null);
                      
            resource.addSupplier("propertyClass", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_STEREOTYPE_DATAITEM + "," + APIAddinClass.EA_TYPE_STRING/*target stereotype*/, null, "propClass"/*supplierEndRole*/, "1"/*cardinality*/, APIAddinClass.EA_TYPE_STRING);

            EAFactory schema = factory.addSupplier(rootclass.clientElement, "quoteSchema"/*supplier end role*/, "", null);

            resource.addSupplier(typesForResource.clientElement, "item-get", "", null);

            EAFactory getMethod = resource.addSupplier("get", APIAddinClass.EA_TYPE_OBJECT, meta.META_METHOD.ElementID, null/*target stereotype*/, null/*run state*/, "", "", null);

            EAFactory queryMethod = getMethod.addSupplier("somequeryparameter", APIAddinClass.EA_TYPE_OBJECT, meta.META_QUERY_PARAMETER.ElementID, null/*Target stereotype*/, null/*run state*/, "", "", null);
            EAFactory dataitem = queryMethod.addSupplier("somequeryparameter_type", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_STEREOTYPE_DATAITEM, null/*run state*/, "", "", null);
            dataitem.clientElement.Notes = "data_item_description";

            getMethod.addSupplier("notcacheable", APIAddinClass.EA_TYPE_OBJECT, meta.META_TRAIT.ElementID, null/*target stereotype*/, null/*run state*/, "", "", null);

            getMethod.addSupplier("PERMISSION_EXECUTE", APIAddinClass.EA_TYPE_OBJECT, meta.META_PERMISSION.ElementID, null/*target stereotype*/, null/*run state*/, "", "", null);

            typesForResource.addSupplier("sample200Resp", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "sample200Resp", "", null);


            meta.setupSamplePackage();
            EAFactory sampleRoot = factory.setupClient("ObjectWithListAttribute", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_REQUEST, rootclass.clientElement.ElementID, null);

            sampleRoot.addSupplier("ChildObjects", APIAddinClass.EA_TYPE_OBJECT, childClass.clientElement.ElementID, null/*target stereotype*/, null, "0OrMoreAttribute"/*supplierEndRole*/, "0..*"/*cardinality*/, null);

            EAFactory releasePipeline = api.addSupplier("ReleasePipeline", APIAddinClass.EA_TYPE_OBJECT, meta.META_RELEASEPIPELINE.ElementID, null/*target stereotype*/, null, ""/*supplierEndRole*/, ""/*cardinality*/, null);
            releasePipeline.addSupplier("dev-environment", APIAddinClass.EA_TYPE_OBJECT, meta.META_ENVIRONMENT.ElementID, null/*target stereotype*/, null, ""/*supplierEndRole*/, ""/*cardinality*/, null);
            releasePipeline.addSupplier("prod-environment", APIAddinClass.EA_TYPE_OBJECT, meta.META_ENVIRONMENT.ElementID, null/*target stereotype*/, null, ""/*supplierEndRole*/, ""/*cardinality*/, null);

            return api;
        }

        static public EAFactory createAPI2(EAMetaModel meta)
        {
            EAFactory factory = new EAFactory();

            meta.setupSchemaPackage();

            EAFactory typesForResource = factory.setupClient("item-get", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_NONE, meta.META_TYPE_FOR_RESOURCE.ElementID, null);

            EAFactory rootclass = typesForResource.addSupplier("RootClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "requestSchema", APIAddinClass.CARDINALITY_ONE, null);

            EAFactory inlineObject = rootclass.addSupplier("IdentifierClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "identifierObject"/*supplierEndRole*/, "1"/*cardinality*/, APIAddinClass.EA_STEREOTYPE_EMBEDDED);

            inlineObject.addAttribute("objectAttr", APIAddinClass.EA_TYPE_STRING);

            meta.setupAPIPackage();

            EAFactory factoryresult = factory.setupClient("APITitle", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "APITitle", "version", "1", "baseUri", "http://localhost", "mediaType", "application/json" });
            
            return factoryresult;
        }


        static public EAFactory createHomeQuote(EAMetaModel meta)
        {
            EAFactory api = new EAFactory();
            meta.setupAPIPackage();

            api.setupClient("api", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "House And Content Quotes Web API", "version", "v1511", "baseUri", "https://api.live.iag.co.nz", "mediaType", "application/json" });

            EAFactory resource = api.addSupplier("/quotes/home/{version}", APIAddinClass.EA_TYPE_OBJECT, meta.META_RESOURCE.ElementID, null/*target stereotype*/, new string[] { "description", "This Web API is used to submit a house and content insurance quote" }/*runstate*/, "resource"/*supplierEndRole*/, ""/*relationshipName*/, null);

            EAFactory schema = api.addSupplier("homeQuote", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null/*runstate*/, "homeQuote"/*supplier end role*/, "", null);

            //api.addSupplier("resource-types", APIAddinClass.EA_TYPE_OBJECT, meta.metaResourceType.ElementID, null/*runstate*/, "resourceTypes"/*supplierEndRole*/, ""/*relationshipName*/);
            EAFactory typesForResource = resource.addSupplier("item-post", APIAddinClass.EA_TYPE_OBJECT, meta.META_TYPE_FOR_RESOURCE.ElementID, null/*target stereotype*/, null, "item-post", "", null);

            //typesForResource.addSupplier("homeQuote2", APIAddinClass.EA_TYPE_CLASS, meta.META_SCHEMA.ElementID, null, "postRespSchema", "");

            //typesForResource.addSupplier("responseInfo", APIAddinClass.EA_TYPE_CLASS, meta.META_SCHEMA.ElementID, null, "infoSchema", "");

            typesForResource.addSupplier("postSampleReq", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "postSampleReq", "", null);
            typesForResource.addSupplier("postSample200Req", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "postSample200Req", "", null);

            EAFactory method = resource.addSupplier("post", APIAddinClass.EA_TYPE_OBJECT, meta.META_METHOD.ElementID, null/*target stereotype*/, new string[] { "test", "testvalue" }/*runstate*/, "post", "", null);

            return api;
        }

        static public EAFactory createMOM(EAMetaModel meta)
        {
            EAFactory api = new EAFactory();
            meta.setupAPIPackage();

            api.setupClient("api", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "IAG House And Content Quotes Web API", "version", "v1511", "baseUri", "https://api.live.iag.co.nz", "mediaType", "application/json" });

            EAFactory resource = api.addSupplier("/mom", APIAddinClass.EA_TYPE_OBJECT, meta.META_RESOURCE.ElementID, null/*target stereotype*/, new string[] { "description", "This Web API is used to submit a house and content insurance quote" }/*runstate*/, "resource"/*supplierEndRole*/, ""/*relationshipName*/, null);

            EAFactory childresource = resource.addSupplier("/event", APIAddinClass.EA_TYPE_OBJECT, meta.META_RESOURCE.ElementID, null/*target stereotype*/, new string[] { "description", "This Web API is used to submit a house and content insurance quote" }/*runstate*/, "resource"/*supplierEndRole*/, ""/*relationshipName*/, null);

            EAFactory schema = api.addSupplier("eventSchema", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null/*runstate*/, "homeQuote"/*supplier end role*/, "", null);

            EAFactory typesForResource = resource.addSupplier("item-post", APIAddinClass.EA_TYPE_OBJECT, meta.META_TYPE_FOR_RESOURCE.ElementID, null/*target stereotype*/, null, "item-post", "", null);

            typesForResource.addSupplier("postSampleReq", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "postSampleReq", "", null);
            typesForResource.addSupplier("postSample200Req", APIAddinClass.EA_TYPE_OBJECT, schema.clientElement.ElementID, null/*target stereotype*/, null, "postSample200Req", "", null);

            EAFactory method = childresource.addSupplier("post", APIAddinClass.EA_TYPE_OBJECT, meta.META_METHOD.ElementID, null/*target stereotype*/, new string[] { "test", "testvalue" }/*runstate*/, "post", "", null);

            return api;
        }


        static public EAFactory createAPI_AttributesFirstClass(EAMetaModel meta)
        {
            EAFactory factory = new EAFactory();

            meta.setupSchemaPackage();

            EAFactory typesForResource = factory.setupClient("item-get", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_NONE, meta.META_TYPE_FOR_RESOURCE.ElementID, null);

            EAFactory rootclass = typesForResource.addSupplier("RootClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "requestSchema", APIAddinClass.CARDINALITY_ONE, null);

            EAFactory inlineObject = rootclass.addSupplier("IdentifierClass", APIAddinClass.EA_TYPE_CLASS, 0, null/*target stereotype*/, null, "identifierObject"/*supplierEndRole*/, "1"/*cardinality*/, APIAddinClass.EA_STEREOTYPE_EMBEDDED);

            //This is a property as a Data Item.  This property does have a supplier end role
            inlineObject.addSupplier("propertyClass", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_TYPE_STRING + "," + APIAddinClass.EA_STEREOTYPE_DATAITEM/*target stereotype*/, null, "propClass"/*supplierEndRole*/, "1"/*cardinality*/, APIAddinClass.EA_TYPE_STRING);

            //This is a property as a Data Item.  This property does not have a supplier end role
            inlineObject.addSupplier("propertyClass2", APIAddinClass.EA_TYPE_CLASS, 0, APIAddinClass.EA_TYPE_STRING + "," + APIAddinClass.EA_STEREOTYPE_DATAITEM/*target stereotype*/, null, ""/*supplierEndRole*/, "1"/*cardinality*/, APIAddinClass.EA_TYPE_STRING);
            
            meta.setupAPIPackage();

            EAFactory factoryresult = factory.setupClient("APITitle", APIAddinClass.EA_TYPE_OBJECT, APIAddinClass.EA_STEREOTYPE_SAMPLE, meta.META_API.ElementID, new string[] { APIManager.TITLE, "APITitle", "version", "1", "baseUri", "http://localhost", "mediaType", "application/json" });

            return factoryresult;
        }
    }
}
