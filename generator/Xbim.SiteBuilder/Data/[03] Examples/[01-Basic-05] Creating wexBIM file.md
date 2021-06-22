﻿Creating WexBIM File for a Web Presentation
===========================================

<canvas id="x-view" style="width:50%;float:right;"></canvas>

You may have realized that one of our projects is [web viewer for IFC](http://docs.xbim.net/XbimWebUI/). We have developed it from scratch based on our previous experience with some webGL libraries.
We know it is not very obvious for the first time where to have a look when you want to convert IFC into compact wexBIM so here is the code. 
You will need [Xbim Essentials](https://www.nuget.org/packages/Xbim.Essentials/) and [Xbim Geometry](https://www.nuget.org/packages/Xbim.Geometry/).
Here is very simple code to be used for a conversion of complete IFC file into wexBIM file. Because xbim is a toolkit there are ways how to customize/filter
the wexBIM file for advanced deployments but that is far more complicated than this simple conversion:

<div class="clearfix"></div>

```cs
using System.IO;
using Xbim.Ifc;
using Xbim.ModelGeometry.Scene;

namespace CreateWexBIM
{
    class Program
    {
        public static void Main()
        {
            const string fileName = "SampleHouse.ifc";
            using (var model = IfcStore.Open(fileName))
            {
                var context = new Xbim3DModelContext(model);
                context.CreateContext();

                var wexBimFilename = Path.ChangeExtension(fileName, "wexBIM");
                using (var wexBiMfile = File.Create(wexBimFilename))
                {
                    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                    {
                        model.SaveAsWexBim(wexBimBinaryWriter);
                        wexBimBinaryWriter.Close();
                    }
                    wexBiMfile.Close();
                }
            }
        }
    }
}
```

<script type="text/javascript" src="/js/xbim-viewer.js"></script>
<script type="text/javascript" src="/js/sample-house.js"></script>
