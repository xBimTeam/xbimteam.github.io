Insert Copy Function
====================

Merging and deleting entities in IFC models is a non-trivial task because IFC is not a hierarchical structure. It is a complex structure with 
potential cyclic relations an bi-directional navigation. Where it is not a problem to perform these tasks on a single entity (which you can imagine as a single line in STEP21 file)

    #144= IFCBUILDINGSTOREY('026ajlHVj1HBm_osQm7IDT',#47,'Lower Roof - Slab Level',$,$,#143,$,'Lower Roof - Slab Level',.ELEMENT.,3199.99999999704);

it becomes increasingly difficult once you want to isolate complete data islands defining the entity and you want to either delete it without side effects on other entities outside
the data island or you want to merge it so it blends into existing data without creating duplicities and inconsistencies. For these reasons we prefer the third option which is to 
choose what you want and to copy it over into an empty model. This is obviously potentially complex and complicated task as well but it is easier to keep 
things under your control at least. The core function which is now member of `IModel` interface is `InsertCopy()`:

```cs
T InsertCopy<T>(T toCopy, XbimInstanceHandleMap mappings, PropertyTranformDelegate propTransform, bool includeInverses, bool keepLabels);
```

Just as a brief description of all arguments are:

  - *toCopy*: Entity to be copied
  - *mappings*: Mappings of previous inserts. There should always only be **one instance for all insertions between two models**.
  - *propTransform*: Optional delegate which you can use to filter the content which will get coppied over or transform it before it gets copied. This is very powerful. 
  - *includeInverses*: Option to bring in all inverse entities. This is potentially dangerous as it might easily bring over almost entire model if not further constrained by propTransform delegate.
  - *keepLabels*: Option to keep entity labels the same. It might be useful to keep labels the same sometimes. **Never** use this option if the target model is not a new model or if you insert objects from multiple models.


From all these `PropertyTranformDelegate` might seem to be little bit cryptic. But it is a fundamental part of the approach described above because it allows to control extent of
the data being copied over. If you allow inverses and don't provide any additional filtering you will most probably end up with the model containing 98% of the original model
even if you just try to copy over a single wall. To use it properly you will need to understand structure of IFC very well. Here is a simple example of a powerful transform which 
will leave out all the geometry and placement and will only allow inverse relations describing type of the product and its properties. Geometry takes typically about 90% of the file
so if you are not interested in graphics or analyses based on geometry you can use this to create very small IFC file containing just descriptive data.

```cs
PropertyTranformDelegate semanticFilter = (property, parentObject) =>
{
    //leave out geometry and placement
    if (parentObject is IIfcProduct &&
        (property.PropertyInfo.Name == nameof(IIfcProduct.Representation) ||
        property.PropertyInfo.Name == nameof(IIfcProduct.ObjectPlacement)))
        return null;

	//leave out mapped geometry
    if (parentObject is IIfcTypeProduct && 
        property.PropertyInfo.Name == nameof(IIfcTypeProduct.RepresentationMaps))
        return null;

    //only bring over IsDefinedBy and IsTypedBy inverse relationships which will take over all properties and types
    if (property.EntityAttribute.Order < 0 && !(
        property.PropertyInfo.Name == nameof(IIfcProduct.IsDefinedBy) ||
        property.PropertyInfo.Name == nameof(IIfcProduct.IsTypedBy)
        ))
        return null;

    return property.PropertyInfo.GetValue(parentObject, null);
};
```

`PropertyTranformDelegate` takes two arguments where the first is `ExpressMetaProperty` and the second is an `object` which represents `IPersistEntity`.
`ExpressMetaProperty` is a cached object which is part of our own reflection meta model which we use for some data operations.
The delegate is used in other code which uses C# reflection to inspect the data and copy values over. If you don't specify a delegate `InsertCopy()`
will use all properties in the entity and copy them over. 

```cs
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace BasicExamples
{
    class InsertCopy
    {
        public void CopyWallsOver()
        {
            const string original = "SampleHouse.ifc";
            const string inserted = "SampleHouseWalls.ifc";

            PropertyTranformDelegate semanticFilter = (property, parentObject) =>
            {
                //leave out geometry and placement
                if (parentObject is IIfcProduct &&
                    (property.PropertyInfo.Name == nameof(IIfcProduct.Representation) ||
                    property.PropertyInfo.Name == nameof(IIfcProduct.ObjectPlacement)))
                    return null;

			   //leave out mapped geometry
               if (parentObject is IIfcTypeProduct && 
					property.PropertyInfo.Name == nameof(IIfcTypeProduct.RepresentationMaps))
					return null;



                //only bring over IsDefinedBy and IsTypedBy inverse relationships which will take over all properties and types
                if (property.EntityAttribute.Order < 0 && !(
                    property.PropertyInfo.Name == nameof(IIfcProduct.IsDefinedBy) ||
                    property.PropertyInfo.Name == nameof(IIfcProduct.IsTypedBy)
                    ))
                    return null;

                return property.PropertyInfo.GetValue(parentObject, null);
            };

            using (var model = IfcStore.Open(original))
            {
                var walls = model.Instances.OfType<IIfcWall>();
                using (var iModel = IfcStore.Create(model.IfcSchemaVersion, XbimStoreType.InMemoryModel))
                {
                    using (var txn = iModel.BeginTransaction("Insert copy"))
                    {
                        //single map should be used for all insertions between two models
                        var map = new XbimInstanceHandleMap(model, iModel);

                        foreach (var wall in walls)
                        {
                            iModel.InsertCopy(wall, map, semanticFilter, true, false);
                        }

                        txn.Commit();
                    }

                    iModel.SaveAs(inserted);
                }
            }
        }
    }
}
```