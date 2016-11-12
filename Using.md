# Using the API Add In


# FAQ
- The extensions by default export into d:\generated folder on your file system.
 - See the FileManager.cs where you can change this.

- Be sure to set your diagrams to a stereotype of APIDiagram, SchemaDiagram or SampleDiagram to enable the appropriate menus.

- Is there a sample EAP project file available
 - Yes I can send you a sample EAP project.  Get in contact with me at phil@xceptionale.com.


# How is the Add In used.
 - This AddIn is used in conjunction with the maven pom file to build and validate the exported raml,json schema and json files.  
   - It builds a library of reusable traits, resource types into a jar. (See the PanAPI directory)
   - It copes the src into target and extracts the PanApi artefacts.
   - Using a number of tools it validates the raml, validates the samples against the schema and then build html documentation, java pojo jars and raml.zip files.
   - I have not included each of the tools used.

# How are changes made
 - Make the changes to the source
 - Build in Visual studio
 - Then restart EA.

 - There are unit tests to be able to validate changes before having to run in Spark EA.


# Validating a sample

When viewing a sample, use the validate sample to check it conforms to the schema.

![Validate Sample ](./images/validate-sample.gif)


# Sync a sample from the file system

Update a sample outside of EA and bring the content into the sample.

![Sync Sample ](./images/sync-sample.gif)


# Raml 1.0

There is now an 'Export RAML 1.0' menu option.

![Export Raml 1.0 ](./images/export-raml-1.0.gif)

This will export a raml 1.0 formatted raml file.

~~~~
#%RAML 1.0
title: The Customer API
baseUri: https://{environment}
baseUriParameters:
  environment:
    enum: [api.test.corp.com,api.live.corp.com]
uses:
  extensions: annotations.1_0.raml
documentation:
- title: Description
  content: ''
- title: History
  content: !include documentation/history.md
- title: Effort
  content: !include documentation/effort.md
traits: !include traits.1_0.raml
resourceTypes: !include resource-types-direct.1_0.raml
~~~~

# Linking traits to methods

Link traits to methods to get them included in the raml

![Line ](./images/link-traits-to-method.gif)

~~~~
/{customerNumber}:  
  get:    
    is: [crossOrigin,notcacheable,traceable]
~~~~

## OAuth2 with scopes

An OAuth2 security schema with scopes can be configured using the OAuth2 meta class

![Security Schema OAuth2 with scopes](./images/security-schema-oauth2.png)

You would use this in the api definition in the following way.

![Security Schema OAuth2 with scopes example ](./images/security-scheme-oauth--example.png)

The following is added to the RAML.
~~~~
securitySchemes:
- OAuth2:
    type: OAuth2
    describedBy: security-schemas-oauth2-describedBy.raml
    description: The interface uses OAuth2 to authenticate requests
    settings:
      accessTokenUri: /api/token
      authorizationUri: /api/authorization
      authorizationGrants: [ code,token ]
      scopes: [ "TRADES","LOYALTY" ]
~~~~
