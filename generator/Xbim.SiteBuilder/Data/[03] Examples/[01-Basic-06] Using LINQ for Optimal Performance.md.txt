# Using [LINQ](https://msdn.microsoft.com/en-us/library/mt693052.aspx) for Optimal Performance

[LINQ](https://msdn.microsoft.com/en-us/library/mt693052.aspx) stands for a Language-Integrated Query and it is part .NET Framework
since version 3.5. It implements deferred execution which means you can chain the query statements and it will do nothing until you 
actually iterate over the result. You can either use LINQ as a [specific language](https://msdn.microsoft.com/en-us/library/bb397927.aspx) or
you can use [extension methods](https://msdn.microsoft.com/en-us/library/9eekhta0.aspx) from `System.Linq` which extend `IEnumerable<T>` 
interface and can get arguments as lambda expressions. We prefer the later approach but it is equivalent. 
Following example shows both variants to do the same. The result of both queries is enumeration of the globally unique IDs of the walls which have any opening.

```cs
//expression using LINQ
var ids =
    from wall in model.Instances.OfType<IIfcWall>()
    where wall.HasOpenings.Any()
    select wall.GlobalId;
```

```cs
//equivalent expression using chained extensions of IEnumerable and lambda expressions
var ids =
    model.Instances
    .Where<IIfcWall>(wall => wall.HasOpenings.Any())
    .Select(wall => wall.GlobalId);
```

You can see in the code that we called `Where()` function directly on `IModel.Instances`. `IEntityCollection` implementations  implement overloads for most of Linq 
data retrieval methods like `Where<T>()`, `Count<T>()`, `FirstOrDefault<T>()` and `OfType<T>()` and it is optimized on the lowest level for fast data access.
All these methods return `IEnumerable<T>` so you can chain it with other methods to perform further selections, aggregation, sorting and other operations. 
`IEntityCollection` functions also use deferred execution so it fits very well with Linq concepts. You should only force it to enumerate if you are going to use
the result more than once. You can do that by calling one of `ToList<T>()`, `ToArray<T>()`or `ToDictionary<T>()` methods.

Xbim uses type of entities internally as the first level filter so you should always ask for the most specific type you can. 
Keep in mind that `IModel.Instances` contains **all** entities in the model which is typically hundreds of thousands of objects! 
So you don't want to iterate over all of them to do anything. See following good and bad example which do the same but not quite the same:

```cs
public static void SelectionWithLinq()
{
    const string ifcFilename = "SampleHouse.ifc";
    var model = IfcStore.Open(ifcFilename);
    using (var txn = model.BeginTransaction())
    {
        var requiredProducts = new IIfcProduct[0]
            .Concat(model.Instances.OfType<IIfcWallStandardCase>())
            .Concat(model.Instances.OfType<IIfcDoor>())
            .Concat(model.Instances.OfType<IIfcWindow>());

        //This will only iterate over entities you really need (9 in this case)
        foreach (var product in requiredProducts)
        {
            //Do anything you want here...
        }

        txn.Commit();
    }
}
```

<div class="alert alert-danger" role="alert"> 
	<p>
		The Following code example is about <strong>4.5 times slower!!!</strong>
	</p>
	<strong>Please, never use this kind of code:</strong>
</div>

```cs
public static void SelectionWithoutLinqIsSLOW()
{
    const string ifcFilename = "SampleHouse.ifc";
    var model = IfcStore.Open(ifcFilename);
    using (var txn = model.BeginTransaction())
    {
        //this will iterate over 47309 entities instead of just 9 you need in this case!
        foreach (var entity in model.Instances)
        {
            if (entity is IIfcWallStandardCase ||
                entity is IIfcDoor ||
                entity is IIfcWindow)
            {
                //You may want to do something here. Please DON'T!
            }
        }
        txn.Commit();
    }
}
```

## Inverse attributes search

IFC has a concept of inverse attributes which is used to access object relations bi-directionally. Common example is 
[IfcObject.IsDefinedBy](https://standards.buildingsmart.org/IFC/RELEASE/IFC4/ADD2_TC1/HTML/link/ifcobject.htm). When you access this 
(read-only) property, xbim has to run this query internally: `model.Instances.Where<IfcRelDefinesByProperties>(r => r.RelatedObjects.Contains(this))`. 
So, this has to go through **all** instances of `IfcRelDefinesByProperties` in the model and compare objects in `RelatedObjects` list with the current entity. 
This is fine is you do it once, but is computing-expensive if you do it repeatedly. Because you are searching in a list of lists, it is actually exponential. 
One option is to restructure your code so that it doesn't use inverse attributes at all and is maintaining own lookups for performance. `model.BeginInverseCaching()` 
is doing this for you. So, once you access `IfcObject.IsDefinedBy` it creates lookup dictionary for all `IfcRelDefinesByProperties`. 
You pay some cost with the first call, but all subsequent calls on any instance of `IfcObject.IsDefinedBy` are almost instant. 
You can run a simple test on some large model:

```cs
private static void RunInverseSearch(IModel model)
{

    void usingInverseAttributes()
    {
        var noObjects = 0;
        var noRelations = 0;
        foreach (var obj in model.Instances.OfType<IIfcObject>())
        {
            var relCount = obj.IsDefinedBy.Count();
            if (relCount > 0)
            {
                noObjects++;
                noRelations += relCount;
            }
        }
        Log.Information($"Number of IfcObject instances with properties: {noObjects}");
    }

    void notUsingInverseAttributes()
    {
        var result = new HashSet<int>();
        foreach (var rel in model.Instances.OfType<IIfcRelDefinesByProperties>())
        {
            foreach (var obj in rel.RelatedObjects.OfType<IIfcObject>())
            {
                result.Add(obj.EntityLabel);
            }
        }
        Log.Information($"Number of IfcObject instances with properties: {result.Count}");
    }

    var w = Stopwatch.StartNew();
    using (var cache = model.BeginInverseCaching())
    {
        usingInverseAttributes();
        w.Stop();
        Log.Information($"Task duration WITH inverse caching, using inverse attributes: {w.ElapsedMilliseconds}ms");
    }

    using (var cache = model.BeginInverseCaching())
    {
        w.Restart();
        notUsingInverseAttributes();
        w.Stop();
        Log.Information($"Task duration WITH inverse caching, NOT using inverse attributes: {w.ElapsedMilliseconds}ms");
    }

    w.Restart();
    usingInverseAttributes();
    w.Stop();
    Log.Information($"Task duration WITHOUT caching, using inverse attributes: {w.ElapsedMilliseconds}ms");

    w.Restart();
    notUsingInverseAttributes();
    w.Stop();
    Log.Information($"Task duration WITHOUT caching, NOT using inverse attributes: {w.ElapsedMilliseconds}ms");

}
```

```
[09:19:05 INF] File size: 104.06MB
[09:19:13 INF] Number of IfcObject instances with properties: 968
[09:19:13 INF] Task duration WITH inverse caching, USING inverse attributes: 35ms
[09:19:13 INF] Task duration WITH inverse caching, NOT using inverse attributes: 14ms
[09:19:17 INF] Task duration WITHOUT caching, USING inverse attributes: 3609ms
[09:19:17 INF] Task duration WITHOUT caching, NOT using inverse attributes: 5ms
```

This kind of caching is obviously mutually exclusive with transactions. And it has a cost on the initial call and consumes memory. 
If you design your code not to use inverse attributes, you don't need it at all. That is why you have to decide based on your 
use case and use it explicitly as and when needed.

Following charts show performance for models with 100, 200, 500, 1000 and 2000 objects, each having 50 property sets:

![Inverse attributes search performance](images/inverse-search-performance.png)