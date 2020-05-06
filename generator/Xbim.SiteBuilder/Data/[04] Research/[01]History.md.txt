# History

[Xbim Toolkit](/) was started by [prof. Steve Lockley](https://github.com/orgs/xBimTeam/people/SteveLockley) in 2007. Steve made it initially as a personal project while on a career break for a year. 
When he joined [Northumbria University](https://www.northumbria.ac.uk/) in 2009 the core of xbim was already
developed and it implemented most of [IFC2x3](http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/). This means Steve developed the core methodology of mapping between EXPRESS schema
and C#. He also developed the core infrastructure for data management of any IFC model. All this is good to know when you read our [license](/license/license.html);

Steve took xbim to university with him and since than it is being actively developed mostly as a part of successful [government funded](https://www.gov.uk/government/organisations/innovate-uk)
research projects with industrial partners mainly from UK. It evolved over time into a lot more than just an implementation
of IFC2x3. Current release supports 100% IFC2x3 and [IFC4](http://www.buildingsmart-tech.org/ifc/IFC4/Add1/html/). We use code generation to keep all the code consistent and complete.
Xbim allows developers to program against IFC2x3 models using IFC4 interfaces so it is possible to develop and maintain single codebase
which is agnostic to the IFC version. You can choose between Esent DB or in-memory model based on your needs. 
We have [geometry engine](https://github.com/xBimTeam/XbimGeometry) which can process complex IFC geometry and make it usable
for 3D analyses or visualization. We have our own JavaScript library using [WebGL](https://www.khronos.org/registry/webgl/specs/1.0/)
for visualization on a web without any plug-ins. 
There are also many projects we developed over time which use xbim core libraries ([Xbim Essentials](https://github.com/xBimTeam/XbimEssentials) 
and [Xbim Geometry](https://github.com/xBimTeam/XbimGeometry)) for some interesting tasks.

There are about 10 developers in the core of xbim team. All of us are keen to keep xbim ready for enterprise deployment. Part of the
team is based on Northumbria University the rest is spread over enterprises who have adopted xbim as an engine behind
the BIM related functionality of their products.

Our [GitHub page](https://github.com/orgs/xBimTeam/people) lists public members of the core team. There are other internal
members from the enterprises. If you want to contact us to ask something about xbim source code we prefer 
[GitHub issues](https://guides.github.com/features/issues/) as a way to talk about the code.

## Recent Research Projects Using xbim

  - [Tier2Tier: A collaboration interface between construction main contractors and their supply chain specialist sub-contractors](http://gtr.rcuk.ac.uk/projects?ref=102053) (Apr 15 - Sep 16)
  - [DECC-MR](http://gtr.rcuk.ac.uk/projects?ref=102062) (Apr 15 - Mar 16)
  - [A digital tool for building information modelling](http://gtr.rcuk.ac.uk/projects?ref=972199) (Oct 14 - Apr 15 )
  - [4BIM](http://gtr.rcuk.ac.uk/projects?ref=101150) (Feb 12 - Jan 14)
  - [interoperable Carbon Assessment Toolkit (iCAT)](http://gtr.rcuk.ac.uk/projects?ref=400144) (Mar 10 - May 12)

## The rise of a company: xbim ltd.

10 years after writing the first line of code Steve Lockley decided to set up a new company with Martin Cerny. 
Martin became the second major contributor to [xbim toolkit](/) and over time has also worked on all of the most recent research projects. 
They were later joined by Andy Ward, former solution architect of 4Projects. xbim ltd. currently maintains the [xbim toolkit](/) 
and will continue to develop it for the benefit of the community as well as do their own product development. Nothing will change 
for xbim users in terms of licenses. xbim ltd. will always keep the [xbim toolkit](/) public and open source. xbim Ltd. continues to grow 
and is currently focusing on developing **[Flex](https://www.xbim.net)**: a cloud platform based on the [xbim toolkit](/) which provides highly scalable distribution 
and processing of models with random access to data. [Flex](https://www.xbim.net) will be available to other developers as a commercial service (PaaS). 
If you are interested, [get in touch](mailto:info@xbim.net).