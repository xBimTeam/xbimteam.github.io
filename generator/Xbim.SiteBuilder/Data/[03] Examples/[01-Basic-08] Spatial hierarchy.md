# Spatial hierarchy from IFC

This very simple example shows how to retrieve spatial structure from the file. Spatial structure in IFC means nested structure of 
the hierarchy representing project, site, building, storey and space. If you have a look at [IFC documentation](http://www.buildingsmart-tech.org/ifc/IFC4/Add2/html/link/ifcspace.htm)
you will find that building can contain storeys as well as other buildings, storey can contain spaces as well as other storeys
and so on. Also this kind of relation is modeled using [IfcRelAggregates](http://www.buildingsmart-tech.org/ifc/IFC4/Add2/html/link/ifcrelaggregates.htm)
but if you want to find elements contained in the particular spatial structure it is modeled as 
[IfcRelContainedInSpatialStructure](http://www.buildingsmart-tech.org/ifc/IFC4/Add2/html/link/ifcrelcontainedinspatialstructure.htm)
so it depends what you want to find. Following example shows how to search and traverse the data to get complete hierarchy using the two relations mentioned above.


```cs
using System;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace BasicExamples
{
    class SpatialStructureExample
    {
        public static void Show()
        {
            const string file = "SampleHouse.ifc";
            
            using (var model = IfcStore.Open(file))
            {
                var project = model.Instances.FirstOrDefault<IIfcProject>();
                PrintHierarchy(project, 0);
            }
        }

        private static void PrintHierarchy(IIfcObjectDefinition o, int level)
        {
            Console.WriteLine(string.Format("{0}{1} [{2}]", GetIndent(level), o.Name, o.GetType().Name));
            
            //only spatial elements can contain building elements
            var spatialElement = o as IIfcSpatialStructureElement;
            if (spatialElement != null)
            {
                //using IfcRelContainedInSpatialElement to get contained elements
                var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                foreach (var element in containedElements)
                    Console.WriteLine(string.Format("{0}    ->{1} [{2}]", GetIndent(level), element.Name, element.GetType().Name));
            }

            //using IfcRelAggregares to get spatial decomposition of spatial structure elements
            foreach (var item in o.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
                PrintHierarchy(item, level +1);
        }

        private static string GetIndent(int level)
        {
            var indent = "";
            for (int i = 0; i < level; i++)
                indent += "  ";
            return indent;
        }
    }
}

```

This will give you following output for our sample model:

```
Project Number [IfcProject]
  Default [IfcSite]
     [IfcBuilding]
      Ground Floor [IfcBuildingStorey]
          ->Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285330 [IfcWall]
          ->Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285395 [IfcWall]
          ->Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285459 [IfcWall]
          ->Curtain Wall:Curtain_Wall-Exterior_Glazing:285582 [IfcCurtainWall]
          ->Curtain Wall:Curtain_Wall-Exterior_Glazing:285684 [IfcCurtainWall]
          ->Basic Wall:Wall-Partn_12P-70MStd-12P:285792 [IfcWallStandardCase]
          ->Basic Wall:Wall-Partn_12P-70MStd-12P:285846 [IfcWallStandardCase]
          ->Doors_ExtDbl_Flush:1810x2110mm:285860 [IfcDoor]
          ->Doors_IntSgl:810x2110mm:285959 [IfcDoor]
          ->Doors_IntSgl:810x2110mm:285996 [IfcDoor]
          ->Windows_Sgl_Plain:1810x1210mm:286105 [IfcWindow]
          ->Windows_Sgl_Plain:1810x1210mm:286188 [IfcWindow]
          ->Windows_Sgl_Plain:1810x1210mm:286238 [IfcWindow]
          ->Compound Ceiling:Plain:286319 [IfcCovering]
          ->Compound Ceiling:Plain:286329 [IfcCovering]
          ->Compound Ceiling:Plain:286337 [IfcCovering]
          ->Floor:Floor-Grnd-Susp_65Scr-80Ins-100Blk-75PC:286349 [IfcSlab]
          ->Windows_Sgl_Plain:1810x1210mm:287567 [IfcWindow]
        1 - Living room [IfcSpace]
            ->Furniture_Table_Dining_w-Chairs_Rectangular:2000x1000x750mm_w-6_Seats:289768 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:289769 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:289770 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:289771 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:289772 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:290097 [IfcFurniture]
            ->Chair - Dining:Chair - Dining:290098 [IfcFurniture]
            ->Furniture_Couch_Viper:2290x950x340mm:290852 [IfcFurniture]
            ->Furniture_Chair_Viper:1120x940x350mm:291916 [IfcFurniture]
            ->Furniture_Chair_Viper:1120x940x350mm:292127 [IfcFurniture]
            ->Furniture_Table_Coffee_1:1200x550x450mm:293046 [IfcFurniture]
            ->Furniture_Piano:1370x600x1170mm:293961 [IfcFurniture]
        2 - Bedroom [IfcSpace]
            ->Furniture_Desk:1525x762mm:287689 [IfcFurniture]
            ->Furniture_Bed_1:1525x2007x355mm-Queen:295878 [IfcFurniture]
        3 - Entrance hall [IfcSpace]
      Roof [IfcBuildingStorey]
          ->Basic Roof:Roof_Flat-4Felt-150Ins-50Scr-150Conc-12Plr:286419 [IfcRoof]
          ->Floor:Simple floor:295048 [IfcSlab]
        4 - Roof [IfcSpace]
```