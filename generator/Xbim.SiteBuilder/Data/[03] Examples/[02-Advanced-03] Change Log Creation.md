Change Log Creation
===================

This is one of the interesting aspects of xbim. It is in a core of our design that every change happening in the model is
a part of a transaction. All transactions are created by an implementation of `IModel` and are weakly referenced from there
so when you use `using` statement model only keeps reference to the the transaction as long as you keep it. This means that 
there is a single point where all changes are happening and we can do something with them.

One obvious thing to do is to record all changes, previous states and next states. Combining all this together you can create 
either back-log or forward-log. To simplify this task we implemented a `Xbim.IO.Delta.TransactionLog` class. In the following example
we will have a look on how to use it.

```cs
using System;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.IO.Delta;
using Xbim.IO.Step21;
```

```cs
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

using (var model = IfcStore.Open("SampleHouse.ifc", editor, true))
{
    using (var txn = model.BeginTransaction("Modification"))
    {
        using (var log = new TransactionLog(txn))
        {
            //change to existing wall
            var wall = model.Instances.FirstOrDefault<IIfcWall>();
            wall.Name = "Unexpected name";
            wall.GlobalId = Guid.NewGuid().ToPart21();
            wall.Description = "New and more descriptive description";

            //print all changes caused by this
            PrintChanges(log);
            txn.Commit();
        }
        Console.WriteLine();
    }
}
```

Previous code uses this simple function to report changes on the objects:

```cs
private static void PrintChanges(TransactionLog log)
{
    foreach (var change in log.Changes)
    {
        switch (change.ChangeType)
        {
            case ChangeType.New:
                Console.WriteLine(@"New entity: {0}", change.CurrentEntity);
                break;
            case ChangeType.Deleted:
                Console.WriteLine(@"Deleted entity: {0}", change.OriginalEntity);
                break;
            case ChangeType.Modified:
                Console.WriteLine(@"Changed Entity: #{0}={1}", change.Entity.EntityLabel, change.Entity.ExpressType.ExpressNameUpper);
                foreach (var prop in change.ChangedProperties)
                    Console.WriteLine(@"        Property '{0}' changed from {1} to {2}", prop.Name, prop.OriginalValue, prop.CurrentValue);
                break;
            default:
                break;
        }
    }
}
```

Resulting change log will look like this. It contains more changes because xbim handles owner history automatically for you when you change or create any `IFCROOT` entity.

```step
Changed Entity: #1229=IFCWALL
        Property 'Name' changed from 'Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285330' to 'Unexpected name'
        Property 'OwnerHistory' changed from #42 to #83873
        Property 'GlobalId' changed from '3cUkl32yn9qRSPvBJVyWw5' to '0zxW3$9z95n8U_H9YOcyiE'
        Property 'Description' changed from $ to 'New and more descriptive description'
New entity: #83873=IFCOWNERHISTORY(#83876,#83877,$,.MODIFIED.,$,$,$,0);
New entity: #83874=IFCPERSON($,'Santini Aichel','Johann Blasius',$,$,$,$,$);
New entity: #83875=IFCORGANIZATION($,'Independent Architecture',$,$,$);
New entity: #83876=IFCPERSONANDORGANIZATION(#83874,#83875,$);
New entity: #83878=IFCORGANIZATION($,'You',$,$,$);
New entity: #83877=IFCAPPLICATION(#83878,$,'Your app','Your app ID');
```