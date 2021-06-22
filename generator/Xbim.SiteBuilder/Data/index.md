﻿<!-- {
  "UseContainer" : true,
  "Order" : -1,
  "MenuGroup" : null,
  "MenuGroupOrder" : 0,
  "ShowBanner" : true,
  "BannerContent" : "<h1>xbim toolkit <br /> <small>making building information flow </small></h1>",
  "Title": "xbim toolkit",
  "Template" : "Layout"
} -->

# The toolkit

The **xbim toolkit** is a **.NET** [open-source](/license/license.html) software development BIM toolkit that 
supports the [BuildingSmart](http://www.buildingsmart-tech.org/) Data Model (aka the [Industry Foundation Classes IFC](https://technical.buildingsmart.org/standards/ifc/)).

Xbim allows .NET developers to read, create and view [Building Information (BIM)](http://en.wikipedia.org/wiki/Building_information_modeling) Models in the IFC format. 
There is full support for geometric, topological operations and visualisation. In addition xbim supports 
bi-directional translation between IFC and COBie formats. Core libraries for data manipulation are all written in C#, core of geometry engine is written in C++.

# Getting Started

Our main distribution channel is [NuGet](https://www.nuget.org/packages?q=xbim). Versions published there are assumed to be stable and safe. 
You can also use our development feeds as listed at the bottom of this page. Have a look at [this quick guide](/quick-start.html) to get up to speed
with basic xbim functions. Examples contain other code samples and snippets which present various xbim features. You will see that it is quite powerful.
 
Two core libraries [Xbim Essentials](https://github.com/xBimTeam/XbimEssentials) and [Xbim Geometry](https://github.com/xBimTeam/XbimGeometry) are to be used 
for the creation of complex applications, other [repositories](https://github.com/xBimTeam) include a number of example applications to 
demonstrate its capabilities:

* [Xbim Xplorer](https://github.com/xBimTeam/XbimWindowsUI) - a Windows WPF sample application that can open and render 3D IFC models (and native xbim models ) as well as displaying semantic data.
* [Xbim WebUI](https://github.com/xBimTeam/XbimWebUI) - a 3D web component that can open and render 3D models [processed by xbim](/examples/creating-wexbim-file.html). 
* [Xbim Utilities](https://github.com/xBimTeam/XbimUtilities) - a set of sample console applications to perform bulk functions on IFC files.
* [Xbim Exchange ](https://github.com/xBimTeam/XbimExchange) - Project containing libraries and sample application demonstrating various approaches to work with COBie. This includes [Xbim.Cobie](https://github.com/xBimTeam/XbimExchange/tree/master/Xbim.COBie) which represents spreadsheet view of the COBie model, [implementation](https://github.com/xBimTeam/XbimExchange/tree/master/Xbim.COBieLite) of [CobieLite](https://www.nibs.org/?page=bsa_cobielite),  [Xbim.CobieLiteUK](https://github.com/xBimTeam/XbimExchange/tree/master/Xbim.COBieLiteUK) which is XML model inspired by CobieLite but more rigorous and memory efficient and [CobieExpress](https://github.com/xBimTeam/XbimEssentials/tree/master/Xbim.CobieExpress) as an EXPRESS based model representing COBie. [XbimExchange ](https://github.com/xBimTeam/XbimExchange) contains sample code for conversions between IFC and various implementations of COBie.
* [Xbim Samples](https://github.com/xBimTeam/XbimSamples) - a sample console application demonstrating how to undertake simple IFC creation and other tasks with xbim.

Please note: all the applications except for [Xbim WebUI](https://github.com/xBimTeam/XbimWebUI) are provided to demonstrate how to use the xbim toolkit components, they are not intended for use in uncontrolled production environments.

# Stay in touch

Xbim Ltd occasionally send a newsletter with tips and tricks for the Toolkit, as well as information about the latest developments. 
[Sign up](https://share.hsforms.com/1IQizWxh-SGCOQ0CQnikEJw3myli) if you want to stay updated. 

# Current versions

|Toolkit Component| Latest Myget (develop) | Latest Myget (master) | Latest Nuget |
|----| ---- | ----| ---- |
|Essentials| ![develop](https://img.shields.io/myget/xbim-develop/vpre/Xbim.Essentials.svg) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.Essentials.svg) | [![Nuget](https://img.shields.io/nuget/v/Xbim.Essentials.svg)](https://www.nuget.org/packages/Xbim.Essentials/)
|Geometry| ![develop](https://img.shields.io/myget/xbim-develop/vpre/Xbim.Geometry.svg) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.Geometry.svg) | [![Nuget](https://img.shields.io/nuget/v/Xbim.Geometry.svg)](https://www.nuget.org/packages/Xbim.Geometry/)
|CobieExpress| ![develop](https://img.shields.io/myget/xbim-develop/vpre/Xbim.CobieExpress.svg) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.IO.CobieExpress.svg) | [![Nuget](https://img.shields.io/nuget/v/Xbim.IO.CobieExpress.svg)](https://www.nuget.org/packages/Xbim.CobieExpress/)
|Windows UI| ![develop](https://img.shields.io/myget/xbim-develop/vpre/Xbim.WindowsUI.svg) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.WindowsUI.svg) | [![Nuget](https://img.shields.io/nuget/v/Xbim.WindowsUI.svg)](https://www.nuget.org/packages/Xbim.WindowsUI/)
|Exchange| ![develop](https://img.shields.io/myget/xbim-develop/vpre/Xbim.Exchange.svg) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.Exchange.svg) | [![Nuget](https://img.shields.io/nuget/v/Xbim.Exchange.svg)](https://www.nuget.org/packages/Xbim.Exchange/)

# <a name="compilation"></a>Compilation

The toolkit uses the NuGet technology for the management of several required packages as well as for distributing the libraries.
If you wish to use the **development versions of xbim**, make sure to add our NuGet feeds for the master and develop branches of the solution.
NuGet can download all the required dependencies for you if you have the correct package source configuration.

If you use Visual Studio 2015+ add the following package sources:

  - https://www.myget.org/F/xbim-develop/api/v3/index.json
  - https://www.myget.org/F/xbim-master/api/v3/index.json

If you use Visual Studio 2013+ add the following package sources:

  - https://www.myget.org/F/xbim-develop/api/v2
  - https://www.myget.org/F/xbim-master/api/v2
  
<canvas id="x-view" style="width:100%;"></canvas>
<script type="text/javascript" src="/js/xbim-viewer.js"></script>
<script type="text/javascript" src="/js/sample-house.js"></script>