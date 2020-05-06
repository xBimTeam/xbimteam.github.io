Basic model operations
======================

To get started the easiest way to get xbim into your project is to use [NuGet](https://www.nuget.org/packages?q=xbim). To do anything you want with the data you only need 
[Xbim Essentials](https://www.nuget.org/packages/Xbim.Essentials/). If you need anything to do with geometry (like visualization) you will also need 
[Xbim Geometry Engine](https://www.nuget.org/packages/Xbim.Geometry/). There are many years of development behind xbim and both these packages are mature and pretty much stable.


Now it's probably time to have a look at some very basic code examples representing how xbim can be used. We will present four basic functions of persistent storage well known as 
[CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) (Create, Retrieve, Update and Delete). Following examples usually work with IFC4 but you can also do the same
with IFC2x3. Actually most of the code is IFC version agnostic as it uses IFC4 interfaces which work for IFC2x3 as well. Sample data used to develop these examples can be downloaded
[here](/data/SampleHouse.zip).

Create
------

Following example will create simple IFC model without any geometry. As you can see `IfcStore` takes a configuration object `XbimEditorCredentials` representing
current application and user and uses it to maintain `OwnerHistory` of root entities. This is a requirement and
makes it easier to handle one of the many aspects needed to create compliant IFC models. This IFC doesn't define any [Model View Definition (MVD)](http://www.buildingsmart-tech.org/specifications/ifc-view-definition)
so there are no additional restrictions apart from `WHERE` rules and required properties. You should always set editor credentials and fill it in with initials of your application
and your current user.

```cs
var editor = new XbimEditorCredentials
{
    ApplicationDevelopersName = "xbim developer",
    ApplicationFullName = "xbim toolkit",
    ApplicationIdentifier = "xbim",
    ApplicationVersion = "4.0",
    EditorsFamilyName = "Santini Aichel",
    EditorsGivenName = "Johann Blasius",
    EditorsOrganisationName = "Independent Architecture"
};
```

All implementations of `IModel` in xbim are `IDisposable` so you should always use them inside of `using` statement like this

```cs
using (var model = IfcStore.Create(editor, IfcSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
{
	//...do something with the model
}
```

If you are going to create or modify anything in the model you have to use transactions. These should also be used inside of `using` statement
so they have a proper scope for eventual roll-back operation in case something happens. You have to commit transaction explicitly to keep the changes.
Transactions can't be nested so there has always be just one transaction at the time.

```cs
using (var txn = model.BeginTransaction("Hello Wall"))
{
    //....do something in the scope of this transaction
    txn.Commit()
}
```

All operations related to entities are accessible through `IModel.Instances`. That is your point of access to get, change and create new
entities in the model. To create any new object you use this templated function. You always have to specify a non-abstract type to create. This is
built in xbim in a way where you get a compile time error if you don't. 
Every model is schema specific so it is either IFC2x3 or IFC4 or other specific schema. `IfcStore` makes it easier because it can open both IFC versions 
and will tell you what it is but when you want to create data make sure you don't mess up your `using` statements. If you try to create IFC4 entity
with model initialized to IFC2x3 it will throw a runtime exception. 

```cs
var newWall = mode.Instances.New<IfcWall>();
```

It isn't possible to create new entities in any other way than with this function. You will see in the code above that this function takes optional 
typed object initializer to set up values of the object. It is not necessary to use them but I personally like it because I can see the structure of
the resulting entity.

Using all these basic pieces we can build the first wall.
This wall doesn't have any geometry so most of IFC viewers won't show you anything. But this is just a basic example. Here is the complete code:

```cs
var editor = new XbimEditorCredentials
{
    ApplicationDevelopersName = "xbim developer",
    ApplicationFullName = "xbim toolkit",
    ApplicationIdentifier = "xbim",
    ApplicationVersion = "4.0",
    EditorsFamilyName = "Santini Aichel",
    EditorsGivenName = "Johann Blasius",
    EditorsOrganisationName = "Independent Architecture"
};
using (var model = IfcStore.Create(editor, IfcSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
{
    using (var txn = model.BeginTransaction("Hello Wall"))
    {
        //there should always be one project in the model
        var project = model.Instances.New<IfcProject>(p => p.Name = "Basic Creation");
        //our shortcut to define basic default units
        project.Initialize(ProjectUnits.SIUnitsUK);

        //create simple object and use lambda initializer to set the name
        var wall = model.Instances.New<IfcWall>(w => w.Name = "The very first wall");

        //set a few basic properties
        model.Instances.New<IfcRelDefinesByProperties>(rel => {
            rel.RelatedObjects.Add(wall);
            rel.RelatingPropertyDefinition = model.Instances.New<IfcPropertySet>(pset => {
                pset.Name = "Basic set of properties";
                pset.HasProperties.AddRange(new[] {
                    model.Instances.New<IfcPropertySingleValue>(p => 
                    {
                        p.Name = "Text property";
                        p.NominalValue = new IfcText("Any arbitrary text you like");
                    }),
                    model.Instances.New<IfcPropertySingleValue>(p => 
                    {
                        p.Name = "Length property";
                        p.NominalValue = new IfcLengthMeasure(56.0);
                    }),
                    model.Instances.New<IfcPropertySingleValue>(p => 
                    {
                        p.Name = "Number property";
                        p.NominalValue = new IfcNumericMeasure(789.2);
                    }),
                    model.Instances.New<IfcPropertySingleValue>(p => 
                    {
                        p.Name = "Logical property";
                        p.NominalValue = new IfcLogical(true);
                    })
                });
            });
        });

        txn.Commit();
    }
    model.SaveAs("BasicWall.ifc");
}
```

Resulting simple IFC will look like this:

```step
ISO-10303-21;
HEADER;
FILE_DESCRIPTION ((''), '2;1');
FILE_NAME ('', '2016-10-27T13:14:43', (''), (''), 'Xbim File Processor version 3.2.0.0', 'Xbim version 3.2.0.0', '');
FILE_SCHEMA (('IFC4'));
ENDSEC;
DATA;
#1=IFCPROJECT('2t0OftVsP8UBH3rtAB$yJv',#2,'Basic Creation',$,$,$,$,(#20,#23),#8);
#2=IFCOWNERHISTORY(#5,#6,$,.ADDED.,$,$,$,0);
#3=IFCPERSON($,'Santini Aichel','Johann Blasius',$,$,$,$,$);
#4=IFCORGANIZATION($,'Independent Architecture',$,$,$);
#5=IFCPERSONANDORGANIZATION(#3,#4,$);
#7=IFCORGANIZATION($,'xbim developer',$,$,$);
#6=IFCAPPLICATION(#7,$,'xbim toolkit','xbim');
#8=IFCUNITASSIGNMENT((#9,#10,#11,#12,#13,#14,#15,#16,#17));
#9=IFCSIUNIT(*,.LENGTHUNIT.,.MILLI.,.METRE.);
#10=IFCSIUNIT(*,.AREAUNIT.,$,.SQUARE_METRE.);
#11=IFCSIUNIT(*,.VOLUMEUNIT.,$,.CUBIC_METRE.);
#12=IFCSIUNIT(*,.SOLIDANGLEUNIT.,$,.STERADIAN.);
#13=IFCSIUNIT(*,.PLANEANGLEUNIT.,$,.RADIAN.);
#14=IFCSIUNIT(*,.MASSUNIT.,$,.GRAM.);
#15=IFCSIUNIT(*,.TIMEUNIT.,$,.SECOND.);
#16=IFCSIUNIT(*,.THERMODYNAMICTEMPERATUREUNIT.,$,.DEGREE_CELSIUS.);
#17=IFCSIUNIT(*,.LUMINOUSINTENSITYUNIT.,$,.LUMEN.);
#18=IFCCARTESIANPOINT((0.,0.,0.));
#19=IFCAXIS2PLACEMENT3D(#18,$,$);
#20=IFCGEOMETRICREPRESENTATIONCONTEXT('Building Model','Model',3,1.E-05,#19,$);
#21=IFCCARTESIANPOINT((0.,0.));
#22=IFCAXIS2PLACEMENT2D(#21,$);
#23=IFCGEOMETRICREPRESENTATIONCONTEXT('Building Plan View','Plan',2,1.E-05,#22,$);
#24=IFCWALL('1YTVCro6L0$OJQL2X7wICY',#2,'The very first wall',$,$,$,$,$,$);
#27=IFCPROPERTYSINGLEVALUE('Text property',$,IFCTEXT('Any arbitrary text you like'),$);
#28=IFCPROPERTYSINGLEVALUE('Length property',$,IFCLENGTHMEASURE(56.),$);
#29=IFCPROPERTYSINGLEVALUE('Number property',$,IFCNUMERICMEASURE(789.2),$);
#30=IFCPROPERTYSINGLEVALUE('Logical property',$,IFCLOGICAL(.T.),$);
#26=IFCPROPERTYSET('2u_olyjv13oRt0GvSVSxHS',#2,'Basic set of properties',$,(#27,#28,#29,#30));
#25=IFCRELDEFINESBYPROPERTIES('3I5GuvWn95PRXcxoFGfJAL',#2,$,$,(#24),#26);
ENDSEC;
END-ISO-10303-21;
```

Retrieve
--------

Retrieving data from the model is very easy and it uses `IModel.Instances` again to access all entities we need.

```cs
var firstWall = mode.Instances.FirtsOrDefault<IfcWall>();
var allWalls = model.Instances.OfType<IfcWall>();
var specificWall = model.Instances.Where<IfcWall>(w => w.Name == "Brick wall");
```

You can see that all these functions are templated so they use type of the object as the first level filter. If you know the type you want you should
always specify it to increase a performance. For all search queries you can also use interfaces to retrieve entities. We have implemented **IFC4 interfaces
on IFC2x3 entities** which means you can query IFC2x3 and IFC4 with a **single codebase**. 

Following example only needs these usings:
```cs
using System;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
```

If you are interested in the structure of the entities we recommend you to have a look at IFC documentation [here](http://www.buildingsmart-tech.org/ifc/).

```cs
const string fileName = "SampleHouse.ifc";
using (var model = IfcStore.Open(fileName))
{
    //get all doors in the model (using IFC4 interface of IfcDoor this will work both for IFC2x3 and IFC4)
    var allDoors = model.Instances.OfType<IIfcDoor>();

    //get only doors with defined IIfcTypeObject
    var someDoors = model.Instances.Where<IIfcDoor>(d => d.IsTypedBy.Any());

    //get one single door 
    var id = "2AswZfru1AdAiKfEdrNPnu";
    var theDoor = model.Instances.FirstOrDefault<IIfcDoor>(d => d.GlobalId == id);
    Console.WriteLine($"Door ID: {theDoor.GlobalId}, Name: {theDoor.Name}");

    //get all single-value properties of the door
    var properties = theDoor.IsDefinedBy
        .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
        .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
        .OfType<IIfcPropertySingleValue>();
    foreach (var property in properties)
        Console.WriteLine($"Property: {property.Name}, Value: {property.NominalValue}");
}
```

This will write to console:

    Door ID: 3cUkl32yn9qRSPvBJVyWYp, Name: Doors_ExtDbl_Flush:1810x2110mm:285860
    Property: IsExternal, Value: true
    Property: Reference, Value: 1810x2110mm
    Property: Level, Value: Level: Ground Floor
    Property: Sill Height, Value: 0
    Property: Area, Value: 4.9462127188431
    Property: Volume, Value: 0.193819981582386
    Property: Mark, Value: 1
    Property: Category, Value: Doors
    Property: Family, Value: Doors_ExtDbl_Flush: 1810x2110mm
    Property: Family and Type, Value: Doors_ExtDbl_Flush: 1810x2110mm
    Property: Head Height, Value: 2110
    Property: Host Id, Value: Basic Wall: Wall-Ext_102Bwk-75Ins-100LBlk-12P
    Property: Type, Value: Doors_ExtDbl_Flush: 1810x2110mm
    Property: Type Id, Value: Doors_ExtDbl_Flush: 1810x2110mm
    Property: Phase Created, Value: New Construction

Update
------

Update is similar to the previous two. Notice the transaction has to be open and should be enclosed in `using` statements
or model will throw an exception at the moment you create or change any object.

```cs
const string fileName = "SampleHouse.ifc";
var editor = new XbimEditorCredentials
{
    ApplicationDevelopersName = "xbim developer",
    ApplicationFullName = "xbim toolkit",
    ApplicationIdentifier = "xbim",
    ApplicationVersion = "4.0",
    EditorsFamilyName = "Santini Aichel",
    EditorsGivenName = "Johann Blasius",
    EditorsOrganisationName = "Independent Architecture"
};

using (var model = IfcStore.Open(fileName, editor, true))
{
    //get existing door from the model
    var id = "3cUkl32yn9qRSPvBJVyWYp";
    var theDoor = model.Instances.FirstOrDefault<IfcDoor>(d => d.GlobalId == id);

    //open transaction for changes
    using (var txn = model.BeginTransaction("Doors modification"))
    {
        //create new property set with two properties
        var pSetRel = model.Instances.New<IfcRelDefinesByProperties>(r =>
        {
            r.GlobalId = Guid.NewGuid();
            r.RelatingPropertyDefinition = model.Instances.New<IfcPropertySet>(pSet =>
            {
                pSet.Name = "New property set";
                //all collections are always initialized
                pSet.HasProperties.Add(model.Instances.New<IfcPropertySingleValue>(p =>
                {
                    p.Name = "First property";
                    p.NominalValue = new IfcLabel("First value");
                }));
                pSet.HasProperties.Add(model.Instances.New<IfcPropertySingleValue>(p =>
                {
                    p.Name = "Second property";
                    p.NominalValue = new IfcLengthMeasure(156.5);
                }));
            });
        });

        //change the name of the door
        theDoor.Name += "_checked";
        //add properties to the door
        pSetRel.RelatedObjects.Add(theDoor);
        
        //commit changes
        txn.Commit();
    }

}
```

Delete
------

Delete is kind of the most complicated operation with the model. First thing to know is that it is only completely implemented in MemoryModel (October 2016).
The reason why this is complicated is that IFC data model is very complex and it isn't hierarchy or directional graph. So our implementation of delete only makes sure
that no object in the model is referencing the object you delete so the model stays consistent. But it doesn't automatically delete any objects referenced from it or 
referring to it. There are choices to be made by programmer or user. However, low level infrastructure for delete can be used very simply:

```cs
using (var model = IfcStore.Open(fileName))
{
    //get existing door from the model
    var id = "3cUkl32yn9qRSPvBJVyWYp";  //use some existing ID from your model
    var theDoor = model.Instances.FirstOrDefault<IIfcDoor>(d => d.GlobalId == id);

    //open transaction for changes
    using (var txn = model.BeginTransaction("Delete the door"))
    {
        //delete the door
        model.Delete(theDoor);
        //commit changes
        txn.Commit();
    }

}
```