<!-- {
  "UseContainer" : true,
  "Order" : -1,
  "ShowBanner" : true,
  "BannerContent" : "<h1><img alt='xbim flex' src='/img/xBim_flex_logo_white.svg' style='height: 2em;'><br /><small>making building information flow </small></h1>",
  "Title": "xbim flex",
  "Template" : "Layout"
} -->

# xbim in the cloud

After many years developing the [xbim toolkit](/) we helped help other members of the xbimteam to develop  web or cloud applications 
built on our core technology. This has given us insights into specific issues and challenges which are involved in cloud data 
processing and 3D presentation. IFC files are essentially serialized object databases of a building. It is quite common that 
these files are hundreds of megabytes. Even if you compress it to IFCZIP (about 90% compression rate), you still need to expand 
it once you want to process it. When it's all loaded in memory, it can easily consume several GB of RAM. This is certainly not scalable 
for cloud based multi-user application. Although some progress can be made using our [Esent model](https://www.nuget.org/packages/Xbim.IO.Esent/), 
initial processing has the same hit of conversion of geometry form and storage and cannot be fanned out to multiple machines. 
That is not very responsive and is only scalable to a certain degree.

When you start processing geometry, it consumes essentially all CPU available on the machine. That is great to get it done as 
quick as possible but probably not desirable if you run your API or the application itself on the same machine. Dedicated virtual 
machine with a lot of RAM and CPU might be used to serve a queue of models but some people might wait ages until their models 
get processed and sometimes you will be paying a lot for the VM which is just sitting there, waiting for some models to digest.
That is scalable in a way but far from being optimal or cost efficient.

Based on all our findings we have developed a new cloud platform **[xbim flex](https://www.xbim.net)**, which uses [xbim toolkit](/)
under the hood and provides access to building data in a completely scalable way as a commercial service. It can process any number 
of models at any time and can digest models which make many other platforms choke. If you are interested, 
download the Flex Revit Add-In [here](https://www.xbim.net/download-addin/) or get in touch at [info@xbim.net](mailto:info@xbim.net).

Our first application on the platform is helping people to communicate around BIM models using open standards 
in the cloud: [www.xbim.net](https://www.xbim.net). We have also found and fixed several issues with IFC production, 
you can download our Revit  Add-in for free and see how the platform works at [www.xbim.net/download-addin](https://www.xbim.net/download-addin).

Web application for communication around models:

![xbim flex comms](/img/tugendhat.jpg)

**This needs changing!**

**NOW**
