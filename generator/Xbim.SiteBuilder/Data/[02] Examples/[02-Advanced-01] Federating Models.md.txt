Federation of multiple models
=============================

  - Overlaying multiple models to behave like a one
  - Uniform access to the data as if it was a single model
  - Read-only. To modify the content of the model you have to work with the specific model
  - Not constrained to a single schema (federate IFC2x3 + IFC4 → query it as IFC4 interfaces)
  - IFC federation can be stored as an IFC file (in IfcStore)

```cs
using System;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace BasicExamples
{
    public class FederationExample
    {
        public void CreateFederation()
        {
            var editor = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "You",
                ApplicationFullName = "Your app",
                ApplicationIdentifier = "Your app ID",
                ApplicationVersion = "4.0",
                //your user
                EditorsFamilyName = "Santini Aichel",
                EditorsGivenName = "Johann Blasius",
                EditorsOrganisationName = "Independent Architecture"
            };
            using (var federation = IfcStore.Create(editor, IfcSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
            {

                federation.AddModelReference("SampleHouse.ifc", "Bob The Builder", "Original Constructor"); //IFC4
                federation.AddModelReference("SampleHouseExtension.ifc", "Tyna", "Extensions Builder"); //IFC2x3

                Console.WriteLine($"Model is federation: {federation.IsFederation}");
                Console.WriteLine($"Number of overall entities: {federation.FederatedInstances.Count}");
                Console.WriteLine($"Number of walls: {federation.FederatedInstances.CountOf<IIfcWall>()}");
                foreach (var refModel in federation.ReferencedModels)
                {
                    Console.WriteLine();
                    Console.WriteLine($"    Referenced model: {refModel.Name}");
                    Console.WriteLine($"    Referenced model organization: {refModel.OwningOrganisation}");
                    Console.WriteLine($"    Number of walls: {refModel.Model.Instances.CountOf<IIfcWall>()}");

                }

                //you can save information about the federation for a future use
                federation.SaveAs("federation.ifc");
            }
        }
    }
}

```

Console output will look like this:

```
Model is federation: True
Number of overall entities: 50303
Number of walls: 8

    Referenced model: SampleHouse.ifc
    Referenced model organization: Bob The Builder
    Number of walls: 5

    Referenced model: SampleHouseExtension.ifc
    Referenced model organization: Tyna
    Number of walls: 3
```

Federation settings saved as IFC file look like this. This convention is purely our invention so it won't work with other tools. But it will allows you
to create federations in your system and reopen them again once needed.

```STEP
ISO-10303-21;
HEADER;
FILE_DESCRIPTION ((''), '2;1');
FILE_NAME ('', '2016-10-27T13:14:43', (''), (''), 'Xbim File Processor version 3.2.0.0', 'Xbim version 3.2.0.0', '');
FILE_SCHEMA (('IFC4'));
ENDSEC;
DATA;
#1=IFCACTORROLE(.USERDEFINED.,'Original Constructor',$);
#2=IFCORGANIZATION($,'Bob The Builder',$,(#1),$);
#3=IFCDOCUMENTINFORMATION('1','SampleHouse.ifc',$,$,$,'XbimReferencedModel',$,$,#2,$,$,$,$,$,$,$,$);
#4=IFCACTORROLE(.USERDEFINED.,'Extensions Builder',$);
#5=IFCORGANIZATION($,'Tyna',$,(#4),$);
#6=IFCDOCUMENTINFORMATION('2','SampleHouseExtension.ifc',$,$,$,'XbimReferencedModel',$,$,#5,$,$,$,$,$,$,$,$);
ENDSEC;
END-ISO-10303-21;

```