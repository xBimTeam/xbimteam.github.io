Converting STEP physical file to XML and vice versa
===================================================

[Xbim toolkit](/) implements 100% of IFC2x3 and IFC4 schemas and it supports all physical representations. 
[STEP21](https://en.wikipedia.org/wiki/ISO_10303-21) is a physical representation of data defined in [EXPRESS](https://en.wikipedia.org/wiki/EXPRESS_(data_modeling_language)). It is
the original definition of IFC and it is a perfect fit for product modelling. It might not be obvious from this simple example but EXPRESS is a lot more conscious and easier to read
than any XSD. EXPRESS definitions look like this:

```
ENTITY IfcAppliedValue
 SUPERTYPE OF (ONEOF
    (IfcCostValue));
	Name : OPTIONAL IfcLabel;
	Description : OPTIONAL IfcText;
	AppliedValue : OPTIONAL IfcAppliedValueSelect;
	UnitBasis : OPTIONAL IfcMeasureWithUnit;
	ApplicableDate : OPTIONAL IfcDate;
	FixedUntilDate : OPTIONAL IfcDate;
	Category : OPTIONAL IfcLabel;
	Condition : OPTIONAL IfcLabel;
	ArithmeticOperator : OPTIONAL IfcArithmeticOperatorEnum;
	Components : OPTIONAL LIST [1:?] OF IfcAppliedValue;
 INVERSE
	HasExternalReference : SET [0:?] OF IfcExternalReferenceRelationship FOR RelatedResourceObjects;
END_ENTITY;
```

STEP representation is also very compact and conscious. It is also easy to read and navigate once you get used to it.

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

IFC is also defined by [XSD](https://en.wikipedia.org/wiki/XML_Schema_(W3C)) which is derived from 
the EXPRESS definition and its physical representation is well known XML. XSD definition doesn't contain all IFC features like `WHERE` rules and `INVERSE` attributes which
are very handy for a bi-directional navigation in the data. IFC4 also makes some inverse relations inversed again and uses different mapping rules between EXPRESS and XSD so any
tool written for IFC2x3 XML is completely useless for IFC4. Code written for IFC2x3 with xbim is very easy to upgrade to be IFC4 compatible. 

**Also note that because of the nature of XML data and complexity of even simple IFC model XML model will *always* use more memory and CPU resources.**

XSD definitions look like this:

```xml
<xs:element name="IfcAppliedValue" type="ifc:IfcAppliedValue" substitutionGroup="ifc:Entity" nillable="true"/>
 <xs:complexType name="IfcAppliedValue">
  <xs:complexContent>
   <xs:extension base="ifc:Entity">
    <xs:sequence>
     <xs:element name="AppliedValue" nillable="true" minOccurs="0">
      <xs:complexType>
       <xs:group ref="ifc:IfcAppliedValueSelect"/>
      </xs:complexType>
     </xs:element>
     <xs:element name="UnitBasis" type="ifc:IfcMeasureWithUnit" nillable="true" minOccurs="0"/>
     <xs:element name="Components" nillable="true" minOccurs="0">
      <xs:complexType>
       <xs:sequence>
        <xs:element ref="ifc:IfcAppliedValue" maxOccurs="unbounded"/>
       </xs:sequence>
       <xs:attribute ref="ifc:itemType" fixed="ifc:IfcAppliedValue"/>
       <xs:attribute ref="ifc:cType" fixed="list"/>
       <xs:attribute ref="ifc:arraySize" use="optional"/>
      </xs:complexType>
     </xs:element>
    </xs:sequence>
    <xs:attribute name="Name" type="ifc:IfcLabel" use="optional"/>
    <xs:attribute name="Description" type="ifc:IfcText" use="optional"/>
    <xs:attribute name="ApplicableDate" type="ifc:IfcDate" use="optional"/>
    <xs:attribute name="FixedUntilDate" type="ifc:IfcDate" use="optional"/>
    <xs:attribute name="Category" type="ifc:IfcLabel" use="optional"/>
    <xs:attribute name="Condition" type="ifc:IfcLabel" use="optional"/>
    <xs:attribute name="ArithmeticOperator" type="ifc:IfcArithmeticOperatorEnum" use="optional"/>
   </xs:extension>
  </xs:complexContent>
 </xs:complexType>
```

The same example as above will look like this as IFCXML. Please note, that IFC4 XML is a lot less verbose than IFC2x3 XML. Bust still the resulting file is *always* bigger.

```xml
<?xml version="1.0" encoding="utf-8"?>
<ifcXML xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:ifc="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add1" xsi:schemaLocation="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add1 http://www.buildingsmart-tech.org/ifcXML/IFC4/Add1/IFC4_ADD1.xsd" id="uos_1" express="http://www.buildingsmart-tech.org/ifc/IFC4/Add1/IFC4_ADD1.exp" configuration="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add1/IFC4_ADD1_config.xml" xmlns="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add1">
  <header>
    <time_stamp>2016-10-31T09:35:30</time_stamp>
    <preprocessor_version>Xbim File Processor version 4.0.0.0</preprocessor_version>
    <originating_system>Xbim version 4.0.0.0</originating_system>
  </header>
  <IfcProject id="i1" GlobalId="1Ozgvj0H5Bd8HqZRp$$1gG" Name="Basic Creation">
    <OwnerHistory id="i2" xsi:type="IfcOwnerHistory" ChangeAction="added" CreationDate="0">
      <OwningUser id="i5" xsi:type="IfcPersonAndOrganization">
        <ThePerson id="i3" xsi:type="IfcPerson" FamilyName="Santini Aichel" GivenName="Johann Blasius" MiddleNames="" PrefixTitles="" SuffixTitles="" />
        <TheOrganization id="i4" xsi:type="IfcOrganization" Name="Independent Architecture" />
      </OwningUser>
      <OwningApplication id="i6" xsi:type="IfcApplication" Version="4.0" ApplicationFullName="xbim toolkit" ApplicationIdentifier="xbim">
        <ApplicationDeveloper id="i7" xsi:type="IfcOrganization" Name="xbim developer" />
      </OwningApplication>
    </OwnerHistory>
    <RepresentationContexts>
      <IfcGeometricRepresentationContext id="i20" pos="0" ContextIdentifier="Building Model" ContextType="Model" CoordinateSpaceDimension="3" Precision="1E-05">
        <WorldCoordinateSystem>
          <IfcAxis2Placement3D id="i19">
            <Location id="i18" xsi:type="IfcCartesianPoint" Coordinates="0 0 0" />
          </IfcAxis2Placement3D>
        </WorldCoordinateSystem>
      </IfcGeometricRepresentationContext>
      <IfcGeometricRepresentationContext id="i23" pos="1" ContextIdentifier="Building Plan View" ContextType="Plan" CoordinateSpaceDimension="2" Precision="1E-05">
        <WorldCoordinateSystem>
          <IfcAxis2Placement2D id="i22">
            <Location id="i21" xsi:type="IfcCartesianPoint" Coordinates="0 0" />
          </IfcAxis2Placement2D>
        </WorldCoordinateSystem>
      </IfcGeometricRepresentationContext>
    </RepresentationContexts>
    <UnitsInContext id="i8" xsi:type="IfcUnitAssignment">
      <Units>
        <IfcSIUnit id="i9" pos="0" UnitType="lengthunit" Prefix="milli" Name="metre" />
        <IfcSIUnit id="i10" pos="1" UnitType="areaunit" Name="square_metre" />
        <IfcSIUnit id="i11" pos="2" UnitType="volumeunit" Name="cubic_metre" />
        <IfcSIUnit id="i12" pos="3" UnitType="solidangleunit" Name="steradian" />
        <IfcSIUnit id="i13" pos="4" UnitType="planeangleunit" Name="radian" />
        <IfcSIUnit id="i14" pos="5" UnitType="massunit" Name="gram" />
        <IfcSIUnit id="i15" pos="6" UnitType="timeunit" Name="second" />
        <IfcSIUnit id="i16" pos="7" UnitType="thermodynamictemperatureunit" Name="degree_celsius" />
        <IfcSIUnit id="i17" pos="8" UnitType="luminousintensityunit" Name="lumen" />
      </Units>
    </UnitsInContext>
  </IfcProject>
  <IfcWall id="i24" GlobalId="0CYq5lt8fES8dUWMOwav6U" Name="The very first wall">
    <OwnerHistory ref="i2" xsi:type="IfcOwnerHistory" xsi:nil="true" />
    <IsDefinedBy>
      <IfcRelDefinesByProperties id="i25" pos="0" GlobalId="1Wt5lOOef8C8PngqD19enP">
        <OwnerHistory ref="i2" xsi:type="IfcOwnerHistory" xsi:nil="true" />
        <RelatingPropertyDefinition>
          <IfcPropertySet id="i26" GlobalId="0JTSUXsqP9QRLClhbnQBnS" Name="Basic set of properties">
            <OwnerHistory ref="i2" xsi:type="IfcOwnerHistory" xsi:nil="true" />
            <HasProperties>
              <IfcPropertySingleValue id="i27" pos="0" Name="Text property">
                <NominalValue>
                  <IfcText-wrapper>Any arbitrary text you like</IfcText-wrapper>
                </NominalValue>
              </IfcPropertySingleValue>
              <IfcPropertySingleValue id="i28" pos="1" Name="Length property">
                <NominalValue>
                  <IfcLengthMeasure-wrapper>56</IfcLengthMeasure-wrapper>
                </NominalValue>
              </IfcPropertySingleValue>
              <IfcPropertySingleValue id="i29" pos="2" Name="Number property">
                <NominalValue>
                  <IfcNumericMeasure-wrapper>789.2</IfcNumericMeasure-wrapper>
                </NominalValue>
              </IfcPropertySingleValue>
              <IfcPropertySingleValue id="i30" pos="3" Name="Logical property">
                <NominalValue>
                  <IfcLogical-wrapper>true</IfcLogical-wrapper>
                </NominalValue>
              </IfcPropertySingleValue>
            </HasProperties>
          </IfcPropertySet>
        </RelatingPropertyDefinition>
      </IfcRelDefinesByProperties>
    </IsDefinedBy>
  </IfcWall>
</ifcXML>
```

I hope I made my point in saying that IFC is a lot better to be stored and handled as STEP21. But we support XML as well and
you might need to convert it sometimes. (I hope you will only ever convert from XML to STEP21 ;-)

```cs
using System;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace BasicExamples
{
    public class StepToXmlExample
    {
        public static void Convert()
        {
            //open STEP21 file
            using (var stepModel = IfcStore.Open("SampleHouse.ifc"))
            {
                //save as XML
                stepModel.SaveAs("SampleHouse.ifcxml");

                //open XML file
                using (var xmlModel = IfcStore.Open("SampleHouse.ifcxml"))
                {
                    //just have a look that it contains the same number of entities and walls.
                    var stepCount = stepModel.Instances.Count;
                    var xmlCount = xmlModel.Instances.Count;

                    var stepWallsCount = stepModel.Instances.CountOf<IIfcWall>();
                    var xmlWallsCount = xmlModel.Instances.CountOf<IIfcWall>();

                    Console.WriteLine($"STEP21 file has {stepCount} entities. XML file has {xmlCount} entities.");
                    Console.WriteLine($"STEP21 file has {stepWallsCount} walls. XML file has {xmlWallsCount} walls.");
                }
            }

        }
    }
}
```