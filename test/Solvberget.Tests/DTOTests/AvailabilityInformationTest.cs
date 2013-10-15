using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class AvailabilityInformationTest
    {

        private Document _document;

        [SetUp]
        public void InitDocument()
        {
            _document = new Document();
            var docItems = DocumentItem.GetDocumentItemsFromXml(getDocumentItemsXml(), getDocumentCircItemsXml(), new RulesRepository());
            _document.GenerateLocationAndAvailabilityInfo(docItems); 
        }

        [Test]
        public void TestDocumentAvailability()
        {
            Assert.AreEqual(2, _document.AvailabilityInfo.ElementAt(0).AvailableCount);
            Assert.AreEqual(4, _document.AvailabilityInfo.ElementAt(0).TotalCount);
            Assert.AreEqual(0, _document.AvailabilityInfo.ElementAt(1).AvailableCount);
            Assert.AreEqual(1, _document.AvailabilityInfo.ElementAt(1).TotalCount);
            Assert.NotNull(_document.AvailabilityInfo.ElementAt(1).EarliestAvailableDateFormatted);
        }

        [Test]
        public void TestDocumentShouldOnlyIncludeTwoBranches()
        {
            Assert.AreEqual(2, _document.AvailabilityInfo.Count());
        }

        private string getDocumentItemsXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<item-data>
    <item>
        <rec-key>000611167000110</rec-key>
        <barcode>11031180135</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000160</rec-key>
        <barcode>11031180136</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000170</rec-key>
        <barcode>11031180133</barcode>
        <sub-library>Stavanger fengsel</sub-library>
        <collection>FENGS</collection>
        <item-status>80</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000180</rec-key>
        <barcode>11031180134</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120813</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <item>
        <rec-key>000611167000190</rec-key>
        <barcode>11031180131</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120804</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <item>
        <rec-key>000611167000200</rec-key>
        <barcode>11031186112</barcode>
        <sub-library>Madla</sub-library>
        <collection>VOKS</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120808</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <session-id>CHJMYTFX3E2NUK4KMTSQ2J5KTEPSFJLGS4NMM1QF836MB7KRCD</session-id>
</item-data>";
        }

        private string getDocumentCircItemsXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<circ-status>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>08/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Madla</sub-library>
        <collection>VOKS</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031186112</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>On Shelf</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180135</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>On Shelf</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180136</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>13/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180134</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>04/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180131</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>Stavanger fengsel</loan-status>
        <due-date>Stavanger fengsel</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Stavanger fengsel</sub-library>
        <collection>FENGS</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180133</barcode>
        <opac-note></opac-note>
    </item-data>
    <session-id>IDEQKF24FAEP5FQRAC4P2TCYCIRAMY4K16E185AUE2UYPKA81K</session-id>
</circ-status>
 ";
        }

    }
}
