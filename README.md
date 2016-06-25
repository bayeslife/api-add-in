# APIAddIn

An Add In to Sparx Enterprise Architect to allows modelling and exporting of RAML,Json Schema and Json artefacts.

Blogs about this functionality
 - [Modelling an API in SparxEA and building runtime artefacts](https://xceptionale.wordpress.com/2016/03/09/modelling-an-api-in-sparkea-and-building-runtime-artefacts/)
 - [Editing Samples and Updating the Enterprise Architect Model](https://xceptionale.wordpress.com/2016/02/21/editing-samples-and-updating-the-enterprise-architect-model/)
 - [Generating Object Diagram Samples from UML Class Models](https://xceptionale.wordpress.com/2016/02/21/generating-object-diagram-samples-from-uml-class-models/)
 - [Modelling API Schema as Class Diagrams in Enterprise Architect](https://xceptionale.wordpress.com/2016/02/21/modelling-api-schema-as-class-diagrams-in-enterprise-architect/)

Videos introducing this functionality.  
- [Introduction](https://www.youtube.com/watch?v=uzPYato5kCk)
- [Exporting class diagrams as json schemas](https://www.youtube.com/watch?v=FKTzZ6ObkGo)
- [Generating samples](https://www.youtube.com/watch?v=A0gYgGENT6U)
- [Synchronizing Json files into EA](https://www.youtube.com/watch?v=RnohxHbeX4w)
- Validating RAML API,Json Schemas and Samples (no video as yet)


# Installation

- Build the solution in the APIAddIn folder using visual studio
- Using Regedit add the registry key to list Sparx EA to the class.
![Synchronizing Json files into EA](./images/registry.png)
- Start EA.
- You should see the extension menus in Extensions-API MDG.
  -   The extensions are context sensitive


# FAQs
 - The extensions by default export into d:\generated folder on your file system.
  - See the FileManager.cs where you can change this.

- Be sure to set your diagrams to a stereotype of APIDiagram, SchemaDiagram or SampleDiagram to enable the appropriate menus.

- Is there a sample EAP project file available
  - Yes I can send you a sample EAP project.  Get in contact with me at phil@xceptionale.com.


- How is the Add In used.
  - This AddIn is used in conjunction with the maven pom file to build and validate the exported raml,json schema and json files.  
    - It builds a library of reusable traits, resource types into a jar. (See the PanAPI directory)
    - It copes the src into target and extracts the PanApi artefacts.
    - Using a number of tools it validates the raml, validates the samples against the schema and then build html documentation, java pojo jars and raml.zip files.
    - I have not included each of the tools used.

- How are changes made
  - Make the changes to the source
  - Build in Visual studio
  - Then restart EA.

  - There are unit tests to be able to validate changes before having to run in Spark EA.


# Thanks
  This project makes use of the [Yaml.NET](https://github.com/aaubry/YamlDotNet). I have had to clone the source code into this project as without changes I couldnt get the raml syntax to be generated correctly.

# Useful

If this is useful to you or you would be interested to some other features/capabilities. Let me know.
