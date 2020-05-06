Fast upgrade track to IFC version agnostic solution
===================================================

So you invested your time into xbim v3 and we broke your code. Sorry for that. We believe you will like all the goodies coming with xbim v4 (like **ability to work with IFC2x3 like if it was IFC4**). 
To learn more about what is new in xbim v4 read [this](/xbim-4/xbim-4-release-notes.html). If you already have an idea about what has happened in xbim and you just want simple guidelines for fast 
upgrade here is how we did it most of the refactoring of our own code depending on Xbim Essentials:

  - If you want to make your code IFC schema independent remove all `using` statements referring to IFC. Change names of all classes from `IfcNameOfTheClass` to interfaces `IIfcNameOfTheClass`. 
  - Let your IDE to help you to resolve all the references to `Xbim.Ifc4.xxx` namespaces. Make sure you don't reference IFC2x3 namespaces. There are interfaces as well but these won't work for IFC4.
  - Try to remove reference to `Xbim.Ifc2x3.dll` and make sure your solution doesn't complain about missing references in using statements. Put it back than.
  - Replace all `XbimModel` references with `IfcStore`. It should have most of the functions you were used to in `XbimModel` but can use all combinations of storage, IFC versions and physical formats.
  - Run all your tests to make sure all works as expected!
