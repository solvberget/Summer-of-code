using System.Linq;

using Solvberget.Domain.DTO;

using Xunit;

namespace Solvberget.Service.Tests.DTOTests
{
    public class UserInfoTest
    {
        [Fact]
        public void PropertiesTest()
        {
            const string userId = "159222";
            var user = new UserInfo { BorrowerId = userId };
            user.FillProperties(getUserXml());
            Assert.Equal(userId, user.BorrowerId);
            Assert.Equal("STV000060009", user.Id);

            Assert.Equal("60.00", user.Balance);
            Assert.Equal("350.00", user.CashLimit);
            Assert.Equal("91562303", user.CellPhoneNumber);
            Assert.Equal("4019 STAVANGER", user.CityAddress);
            Assert.Equal("09.04.1989", user.DateOfBirth);
            Assert.Equal("haaland@gmail.com", user.Email);
            Assert.Equal("Hovedbibl.", user.HomeLibrary);
            Assert.Equal("51536695", user.HomePhoneNumber);
            Assert.Equal("Sindre Haaland", user.Name);
            Assert.Equal("Sindre Haaland", user.PrefixAddress);
            Assert.Equal("Bakkesvingen 8", user.StreetAddress);
            Assert.Equal("4019", user.Zip); 

        }

        [Fact]
        public void FineTest()
        {
            const string userId = "159222";
            var user = new UserInfo { BorrowerId = userId };
            user.FillProperties(getUserXml());

            Assert.Equal("08.02.2007", user.Fines.ElementAt(0).Date);
            Assert.Equal("Ikke betalt ", user.Fines.ElementAt(0).Status);
            Assert.Equal('D', user.Fines.ElementAt(0).CreditDebit);
            Assert.Equal(30.00 , user.Fines.ElementAt(0).Sum);
            Assert.Equal("Nytt lånekort", user.Fines.ElementAt(0).Description);


            Assert.Equal(2, user.Fines.Count());
            Assert.Equal("19.03.2007", user.Fines.ElementAt(1).Date);
            Assert.Equal("For sent levert", user.Fines.ElementAt(1).Description);
            Assert.Equal("000230544", user.Fines.ElementAt(1).DocumentNumber);
            Assert.Equal("Gift", user.Fines.ElementAt(1).DocumentTitle);


        }

        [Fact]
        public void LoanTest()
        {
            const string userId = "164916";
            var user = new UserInfo { BorrowerId = userId };
            user.FillProperties(getUserLoansXml());

            Assert.Equal("ellenwiig@gmail.com", user.Email);
            Assert.Equal("530185", user.Loans.ElementAt(0).AdminisrtativeDocumentNumber);
            Assert.Equal("1001 filmer du må se før du dør", user.Loans.ElementAt(0).DocumentTitle);
            Assert.Equal("Hovedbiblioteket", user.Loans.ElementAt(0).SubLibrary);
            Assert.Equal("15.08.2012", user.Loans.ElementAt(0).OriginalDueDate);
            Assert.Equal("4 uker", user.Loans.ElementAt(0).ItemStatus);
            Assert.Equal("18.07.2012", user.Loans.ElementAt(0).LoanDate);
            Assert.Equal("15:04", user.Loans.ElementAt(0).LoanHour);
            Assert.Equal("15.08.2012", user.Loans.ElementAt(0).DueDate);

        }



        private string getUserXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<bor-info>
<z303>
<z303-id>STV000060009</z303-id>
<z303-proxy-for-id></z303-proxy-for-id>
<z303-primary-id></z303-primary-id>
<z303-name-key>h}land sindre                         STV000060009</z303-name-key>
<z303-user-type></z303-user-type>
<z303-user-library>NOR50</z303-user-library>
<z303-open-date>19980204</z303-open-date>
<z303-update-date>20120621</z303-update-date>
<z303-con-lng>NOR</z303-con-lng>
<z303-alpha>L</z303-alpha>
<z303-name>Haaland, Sindre</z303-name>
<z303-title></z303-title>
<z303-delinq-1>00</z303-delinq-1>
<z303-delinq-n-1></z303-delinq-n-1>
<z303-delinq-1-update-date>00000000</z303-delinq-1-update-date>
<z303-delinq-1-cat-name></z303-delinq-1-cat-name>
<z303-delinq-2>00</z303-delinq-2>
<z303-delinq-n-2></z303-delinq-n-2>
<z303-delinq-2-update-date>00000000</z303-delinq-2-update-date>
<z303-delinq-2-cat-name></z303-delinq-2-cat-name>
<z303-delinq-3>00</z303-delinq-3>
<z303-delinq-n-3></z303-delinq-n-3>
<z303-delinq-3-update-date>00000000</z303-delinq-3-update-date>
<z303-delinq-3-cat-name></z303-delinq-3-cat-name>
<z303-budget></z303-budget>
<z303-profile-id></z303-profile-id>
<z303-ill-library>Hovedbibl.</z303-ill-library>
<z303-home-library>Hovedbibl.</z303-home-library>
<z303-field-1>E-POST /</z303-field-1>
<z303-field-2></z303-field-2>
<z303-field-3>IKKE_NASJONAL::090489::::2110300::0::0::0</z303-field-3>
<z303-note-1></z303-note-1>
<z303-note-2></z303-note-2>
<z303-salutation></z303-salutation>
<z303-ill-total-limit>9999</z303-ill-total-limit>
<z303-ill-active-limit>9999</z303-ill-active-limit>
<z303-dispatch-library></z303-dispatch-library>
<z303-birth-date>19890409</z303-birth-date>
<z303-export-consent>N</z303-export-consent>
<z303-proxy-id-type>00</z303-proxy-id-type>
<z303-send-all-letters>Y</z303-send-all-letters>
<z303-plain-html>P</z303-plain-html>
<z303-want-sms></z303-want-sms>
<z303-plif-modification></z303-plif-modification>
<z303-title-req-limit>0000</z303-title-req-limit>
<z303-gender></z303-gender>
<z303-birthplace></z303-birthplace>
</z303>
<z304>
<z304-id>STV000060009</z304-id>
<z304-sequence>01</z304-sequence>
<z304-address-0>Sindre Haaland</z304-address-0>
<z304-address-1>Bakkesvingen 8</z304-address-1>
<z304-address-3>4019 STAVANGER</z304-address-3>
<z304-zip>4019</z304-zip>
<z304-email-address>haaland@gmail.com</z304-email-address>
<z304-telephone>51536695</z304-telephone>
<z304-date-from>20120621</z304-date-from>
<z304-date-to>20991231</z304-date-to>
<z304-address-type>01</z304-address-type>
<z304-telephone-2></z304-telephone-2>
<z304-telephone-3>91562303</z304-telephone-3>
<z304-telephone-4></z304-telephone-4>
<z304-sms-number></z304-sms-number>
<z304-update-date>20120621</z304-update-date>
<z304-cat-name>BATCH</z304-cat-name>
</z304>
<z305>
<z305-id>STV000060009</z305-id>
<z305-sub-library>NOR50</z305-sub-library>
<z305-open-date>19980204</z305-open-date>
<z305-update-date>20120621</z305-update-date>
<z305-bor-type>Mann</z305-bor-type>
<z305-bor-status>Alm. lånere</z305-bor-status>
<z305-registration-date>00000000</z305-registration-date>
<z305-expiry-date>20991231</z305-expiry-date>
<z305-note></z305-note>
<z305-loan-permission>Y</z305-loan-permission>
<z305-photo-permission>N</z305-photo-permission>
<z305-over-permission>Y</z305-over-permission>
<z305-multi-hold>N</z305-multi-hold>
<z305-loan-check>Y</z305-loan-check>
<z305-hold-permission>Y</z305-hold-permission>
<z305-renew-permission>Y</z305-renew-permission>
<z305-rr-permission>Y</z305-rr-permission>
<z305-ignore-late-return>N</z305-ignore-late-return>
<z305-last-activity-date>20070319</z305-last-activity-date>
<z305-photo-charge>C</z305-photo-charge>
<z305-no-loan>0000</z305-no-loan>
<z305-no-hold>0000</z305-no-hold>
<z305-no-photo>0000</z305-no-photo>
<z305-no-cash>0000</z305-no-cash>
<z305-cash-limit>350.00</z305-cash-limit>
<z305-credit-debit>C</z305-credit-debit>
<z305-sum>0.00</z305-sum>
<z305-delinq-1>00</z305-delinq-1>
<z305-delinq-n-1></z305-delinq-n-1>
<z305-delinq-1-update-date>00000000</z305-delinq-1-update-date>
<z305-delinq-1-cat-name></z305-delinq-1-cat-name>
<z305-delinq-2>00</z305-delinq-2>
<z305-delinq-n-2></z305-delinq-n-2>
<z305-delinq-2-update-date>00000000</z305-delinq-2-update-date>
<z305-delinq-2-cat-name></z305-delinq-2-cat-name>
<z305-delinq-3>00</z305-delinq-3>
<z305-delinq-n-3></z305-delinq-n-3>
<z305-delinq-3-update-date>00000000</z305-delinq-3-update-date>
<z305-delinq-3-cat-name></z305-delinq-3-cat-name>
<z305-field-1></z305-field-1>
<z305-field-2></z305-field-2>
<z305-field-3></z305-field-3>
<z305-hold-on-shelf>Y</z305-hold-on-shelf>
<z305-end-block-date></z305-end-block-date>
<z305-booking-permission>N</z305-booking-permission>
<z305-booking-ignore-hours>N</z305-booking-ignore-hours>
</z305>
<balance>60.00</balance>
<sign>D</sign>
<fine>
<z31>
<z31-id>STV000060009</z31-id>
<z31-date>20070208</z31-date>
<z31-status>Not paid by/credited to patron</z31-status>
<z31-sub-library>Alle filialer</z31-sub-library>
<z31-type>8</z31-type>
<z31-credit-debit>D</z31-credit-debit>
<z31-sum>(30.00)</z31-sum>
<z31-vat-sum>0.00</z31-vat-sum>
<z31-net-sum>30.00</z31-net-sum>
<z31-payment-date></z31-payment-date>
<z31-payment-hour></z31-payment-hour>
<z31-payment-cataloger></z31-payment-cataloger>
<z31-payment-target>Alle filialer</z31-payment-target>
<z31-payment-ip></z31-payment-ip>
<z31-payment-receipt-number></z31-payment-receipt-number>
<z31-payment-mode></z31-payment-mode>
<z31-payment-identifier></z31-payment-identifier>
<z31-description>Nytt lånekort</z31-description>
<z31-transfer-department></z31-transfer-department>
<z31-transfer-date>00000000</z31-transfer-date>
<z31-transfer-number></z31-transfer-number>
<z31-recall-transfer-status></z31-recall-transfer-status>
<z31-recall-transfer-date>00000000</z31-recall-transfer-date>
<z31-recall-transfer-number></z31-recall-transfer-number>
</z31>
</fine>
<fine>
<z31>
<z31-id>STV000060009</z31-id>
<z31-date>20070319</z31-date>
<z31-status>Not paid by/credited to patron</z31-status>
<z31-sub-library>Hovedbibl.</z31-sub-library>
<z31-type>3</z31-type>
<z31-credit-debit>D</z31-credit-debit>
<z31-sum>(30.00)</z31-sum>
<z31-vat-sum>0.00</z31-vat-sum>
<z31-net-sum>30.00</z31-net-sum>
<z31-payment-date></z31-payment-date>
<z31-payment-hour></z31-payment-hour>
<z31-payment-cataloger></z31-payment-cataloger>
<z31-payment-target>Hovedbibl.</z31-payment-target>
<z31-payment-ip></z31-payment-ip>
<z31-payment-receipt-number></z31-payment-receipt-number>
<z31-payment-mode></z31-payment-mode>
<z31-payment-identifier></z31-payment-identifier>
<z31-description>Lånt 20070208 Forf. 20070308 Kat. 01 - 1-2 ukes forsink.:</z31-description>
<z31-transfer-department></z31-transfer-department>
<z31-transfer-date>00000000</z31-transfer-date>
<z31-transfer-number></z31-transfer-number>
<z31-recall-transfer-status></z31-recall-transfer-status>
<z31-recall-transfer-date>00000000</z31-recall-transfer-date>
<z31-recall-transfer-number></z31-recall-transfer-number>
</z31>
<z13>
<z13-doc-number>230544</z13-doc-number>
<z13-year>1991</z13-year>
<z13-open-date>19971213</z13-open-date>
<z13-update-date>20070809</z13-update-date>
<z13-call-no-key></z13-call-no-key>
<z13-call-no-code>090</z13-call-no-code>
<z13-call-no></z13-call-no>
<z13-author-code>100</z13-author-code>
<z13-author>Kielland, Alexander L</z13-author>
<z13-title-code>245</z13-title-code>
<z13-title>Gift</z13-title>
<z13-imprint-code>019</z13-imprint-code>
<z13-imprint>l</z13-imprint>
<z13-isbn-issn-code>020</z13-isbn-issn-code>
<z13-isbn-issn>8205000042</z13-isbn-issn>
<z13-user-defined-1-code>LDR</z13-user-defined-1-code>
<z13-user-defined-1>00673cam^^22002411^^45^^</z13-user-defined-1>
<z13-user-defined-2-code>FMT</z13-user-defined-2-code>
<z13-user-defined-2>BK</z13-user-defined-2>
<z13-user-defined-3-code>008</z13-user-defined-3-code>
<z13-user-defined-3>910307^1991^^^^^^^^^^^a^^^^^^^^^^1^nob^^</z13-user-defined-3>
<z13-user-defined-4-code></z13-user-defined-4-code>
<z13-user-defined-4></z13-user-defined-4>
<z13-user-defined-5-code>245</z13-user-defined-5-code>
<z13-user-defined-5></z13-user-defined-5>
</z13>
</fine>
<session-id>I3XLUU5UVTS9GS1F5H2ND5EFRBNMQEJVNDV42RECKFJC5RVTHY</session-id>
</bor-info>
 ";
        }
        
        private string getUserLoansXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<bor-info>
    <z303>
        <z303-id>STV000209626</z303-id>
        <z303-proxy-for-id></z303-proxy-for-id>
        <z303-primary-id></z303-primary-id>
        <z303-name-key>andresen ellen wiig                   STV000209626</z303-name-key>
        <z303-user-type></z303-user-type>
        <z303-user-library>NOR50</z303-user-library>
        <z303-open-date>20120621</z303-open-date>
        <z303-update-date>20120621</z303-update-date>
        <z303-con-lng>NOR</z303-con-lng>
        <z303-alpha>L</z303-alpha>
        <z303-name>Andresen, Ellen Wiig</z303-name>
        <z303-title></z303-title>
        <z303-delinq-1>00</z303-delinq-1>
        <z303-delinq-n-1></z303-delinq-n-1>
        <z303-delinq-1-update-date>00000000</z303-delinq-1-update-date>
        <z303-delinq-1-cat-name></z303-delinq-1-cat-name>
        <z303-delinq-2>00</z303-delinq-2>
        <z303-delinq-n-2></z303-delinq-n-2>
        <z303-delinq-2-update-date>00000000</z303-delinq-2-update-date>
        <z303-delinq-2-cat-name></z303-delinq-2-cat-name>
        <z303-delinq-3>00</z303-delinq-3>
        <z303-delinq-n-3></z303-delinq-n-3>
        <z303-delinq-3-update-date>00000000</z303-delinq-3-update-date>
        <z303-delinq-3-cat-name></z303-delinq-3-cat-name>
        <z303-budget></z303-budget>
        <z303-profile-id></z303-profile-id>
        <z303-ill-library>Hovedbibl.</z303-ill-library>
        <z303-home-library>Hovedbibl.</z303-home-library>
        <z303-field-1>E-POST /</z303-field-1>
        <z303-field-2></z303-field-2>
        <z303-field-3>IKKE_NASJONAL::010689::::2110300::0::0::0</z303-field-3>
        <z303-note-1></z303-note-1>
        <z303-note-2></z303-note-2>
        <z303-salutation></z303-salutation>
        <z303-ill-total-limit>9999</z303-ill-total-limit>
        <z303-ill-active-limit>9999</z303-ill-active-limit>
        <z303-dispatch-library></z303-dispatch-library>
        <z303-birth-date>19890601</z303-birth-date>
        <z303-export-consent>N</z303-export-consent>
        <z303-proxy-id-type>00</z303-proxy-id-type>
        <z303-send-all-letters>Y</z303-send-all-letters>
        <z303-plain-html>P</z303-plain-html>
        <z303-want-sms></z303-want-sms>
        <z303-plif-modification></z303-plif-modification>
        <z303-title-req-limit>0000</z303-title-req-limit>
        <z303-gender></z303-gender>
        <z303-birthplace></z303-birthplace>
    </z303>
    <z304>
        <z304-id>STV000209626</z304-id>
        <z304-sequence>01</z304-sequence>
        <z304-address-0>Ellen Wiig Andresen</z304-address-0>
        <z304-address-1>Odd Brochmanns vei 1 - 212</z304-address-1>
        <z304-address-3>7051 TRONDHEIM</z304-address-3>
        <z304-address-4>NO</z304-address-4>
        <z304-zip>7051</z304-zip>
        <z304-email-address>ellenwiig@gmail.com</z304-email-address>
        <z304-telephone></z304-telephone>
        <z304-date-from>20120621</z304-date-from>
        <z304-date-to>20991231</z304-date-to>
        <z304-address-type>01</z304-address-type>
        <z304-telephone-2></z304-telephone-2>
        <z304-telephone-3>99297919</z304-telephone-3>
        <z304-telephone-4></z304-telephone-4>
        <z304-sms-number></z304-sms-number>
        <z304-update-date>20120621</z304-update-date>
        <z304-cat-name>BATCH</z304-cat-name>
    </z304>
    <z305>
        <z305-id>STV000209626</z305-id>
        <z305-sub-library>NOR50</z305-sub-library>
        <z305-open-date>20120621</z305-open-date>
        <z305-update-date>20120718</z305-update-date>
        <z305-bor-type>Kvinne</z305-bor-type>
        <z305-bor-status>Alm. lånere</z305-bor-status>
        <z305-registration-date>00000000</z305-registration-date>
        <z305-expiry-date>20991231</z305-expiry-date>
        <z305-note></z305-note>
        <z305-loan-permission>Y</z305-loan-permission>
        <z305-photo-permission>N</z305-photo-permission>
        <z305-over-permission>Y</z305-over-permission>
        <z305-multi-hold>N</z305-multi-hold>
        <z305-loan-check>Y</z305-loan-check>
        <z305-hold-permission>Y</z305-hold-permission>
        <z305-renew-permission>Y</z305-renew-permission>
        <z305-rr-permission>Y</z305-rr-permission>
        <z305-ignore-late-return>N</z305-ignore-late-return>
        <z305-last-activity-date>20120718</z305-last-activity-date>
        <z305-photo-charge>C</z305-photo-charge>
        <z305-no-loan>0000</z305-no-loan>
        <z305-no-hold>0000</z305-no-hold>
        <z305-no-photo>0000</z305-no-photo>
        <z305-no-cash>0000</z305-no-cash>
        <z305-cash-limit>150.00</z305-cash-limit>
        <z305-credit-debit></z305-credit-debit>
        <z305-sum>0.00</z305-sum>
        <z305-delinq-1>00</z305-delinq-1>
        <z305-delinq-n-1></z305-delinq-n-1>
        <z305-delinq-1-update-date>00000000</z305-delinq-1-update-date>
        <z305-delinq-1-cat-name></z305-delinq-1-cat-name>
        <z305-delinq-2>00</z305-delinq-2>
        <z305-delinq-n-2></z305-delinq-n-2>
        <z305-delinq-2-update-date>00000000</z305-delinq-2-update-date>
        <z305-delinq-2-cat-name></z305-delinq-2-cat-name>
        <z305-delinq-3>00</z305-delinq-3>
        <z305-delinq-n-3></z305-delinq-n-3>
        <z305-delinq-3-update-date>00000000</z305-delinq-3-update-date>
        <z305-delinq-3-cat-name></z305-delinq-3-cat-name>
        <z305-field-1></z305-field-1>
        <z305-field-2></z305-field-2>
        <z305-field-3></z305-field-3>
        <z305-hold-on-shelf>Y</z305-hold-on-shelf>
        <z305-end-block-date></z305-end-block-date>
        <z305-booking-permission>N</z305-booking-permission>
        <z305-booking-ignore-hours>N</z305-booking-ignore-hours>
    </z305>
    <item-l>
        <z36>
            <z36-doc-number>000530185</z36-doc-number>
            <z36-item-sequence>000020</z36-item-sequence>
            <z36-number>005098054</z36-number>
            <z36-material>BOOK</z36-material>
            <z36-sub-library>Hovedbibl.</z36-sub-library>
            <z36-status>A</z36-status>
            <z36-loan-date>20120718</z36-loan-date>
            <z36-loan-hour>15:04</z36-loan-hour>
            <z36-effective-due-date>00000000</z36-effective-due-date>
            <z36-due-date>20120815</z36-due-date>
            <z36-due-hour>24:00</z36-due-hour>
            <z36-returned-date></z36-returned-date>
            <z36-returned-hour></z36-returned-hour>
            <z36-item-status>04</z36-item-status>
            <z36-bor-status>Alm. lånere</z36-bor-status>
            <z36-letter-number>0</z36-letter-number>
            <z36-letter-date></z36-letter-date>
            <z36-no-renewal>0</z36-no-renewal>
            <z36-note-1></z36-note-1>
            <z36-note-2></z36-note-2>
            <z36-loan-cataloger-name>STVGBIBL</z36-loan-cataloger-name>
            <z36-loan-cataloger-ip>10.61.1.69</z36-loan-cataloger-ip>
            <z36-return-cataloger-name></z36-return-cataloger-name>
            <z36-return-cataloger-ip></z36-return-cataloger-ip>
            <z36-renew-cataloger-name></z36-renew-cataloger-name>
            <z36-renew-cataloger-ip></z36-renew-cataloger-ip>
            <z36-renew-mode></z36-renew-mode>
            <z36-bor-type>KV</z36-bor-type>
            <z36-note-alpha></z36-note-alpha>
            <z36-recall-date></z36-recall-date>
            <z36-recall-due-date></z36-recall-due-date>
            <z36-last-renew-date></z36-last-renew-date>
            <z36-original-due-date>20120815</z36-original-due-date>
            <z36-process-status></z36-process-status>
            <z36-loan-type></z36-loan-type>
            <z36-proxy-id></z36-proxy-id>
            <z36-recall-type></z36-recall-type>
            <z36-return-location></z36-return-location>
            <z36-return-sub-location></z36-return-sub-location>
            <z36-source></z36-source>
            <z36-delivery-time></z36-delivery-time>
            <z36-tail-time></z36-tail-time>
        </z36>
        <z30>
            <z30-doc-number>530185</z30-doc-number>
            <z30-item-sequence>20</z30-item-sequence>
            <z30-barcode>11030991828</z30-barcode>
            <z30-sub-library>Hovedbibl.</z30-sub-library>
            <z30-material> BOK</z30-material>
            <z30-item-status>4 uker</z30-item-status>
            <z30-open-date>20041119</z30-open-date>
            <z30-update-date>20041201</z30-update-date>
            <z30-cataloger>STVGBIBL</z30-cataloger>
            <z30-date-last-return>20081011</z30-date-last-return>
            <z30-hour-last-return>10:40</z30-hour-last-return>
            <z30-ip-last-return>10.61.1.44</z30-ip-last-return>
            <z30-no-loans>021</z30-no-loans>
            <z30-alpha>L</z30-alpha>
            <z30-collection>Musikkbiblioteket</z30-collection>
            <z30-call-no-type></z30-call-no-type>
            <z30-call-no>Film</z30-call-no>
            <z30-call-no-key>  film</z30-call-no-key>
            <z30-call-no-2-type></z30-call-no-2-type>
            <xxx-tag>Referense</xxx-tag>
            <z30-call-no-2-key>  referense</z30-call-no-2-key>
            <z30-description></z30-description>
            <z30-note-opac></z30-note-opac>
            <z30-note-circulation></z30-note-circulation>
            <z30-note-internal></z30-note-internal>
            <z30-order-number>ORDER-53275</z30-order-number>
            <z30-inventory-number></z30-inventory-number>
            <z30-inventory-number-date></z30-inventory-number-date>
            <z30-last-shelf-report-date>00000000</z30-last-shelf-report-date>
            <z30-price>312.80</z30-price>
            <z30-shelf-report-number></z30-shelf-report-number>
            <z30-on-shelf-date>00000000</z30-on-shelf-date>
            <z30-on-shelf-seq>000000</z30-on-shelf-seq>
            <z30-doc-number-2>000000000</z30-doc-number-2>
            <z30-schedule-sequence-2>00000</z30-schedule-sequence-2>
            <z30-copy-sequence-2>00000</z30-copy-sequence-2>
            <z30-vendor-code></z30-vendor-code>
            <z30-invoice-number></z30-invoice-number>
            <z30-line-number>00000</z30-line-number>
            <z30-pages></z30-pages>
            <z30-issue-date></z30-issue-date>
            <z30-expected-arrival-date></z30-expected-arrival-date>
            <z30-arrival-date></z30-arrival-date>
            <z30-item-statistic></z30-item-statistic>
            <z30-item-process-status></z30-item-process-status>
            <z30-copy-id></z30-copy-id>
            <z30-hol-doc-number>000000000</z30-hol-doc-number>
            <z30-temp-location>No</z30-temp-location>
            <z30-enumeration-a></z30-enumeration-a>
            <z30-enumeration-b></z30-enumeration-b>
            <z30-enumeration-c></z30-enumeration-c>
            <z30-enumeration-d></z30-enumeration-d>
            <z30-enumeration-e></z30-enumeration-e>
            <z30-enumeration-f></z30-enumeration-f>
            <z30-enumeration-g></z30-enumeration-g>
            <z30-enumeration-h></z30-enumeration-h>
            <z30-chronological-i></z30-chronological-i>
            <z30-chronological-j></z30-chronological-j>
            <z30-chronological-k></z30-chronological-k>
            <z30-chronological-l></z30-chronological-l>
            <z30-chronological-m></z30-chronological-m>
            <z30-supp-index-o></z30-supp-index-o>
            <z30-85x-type></z30-85x-type>
            <z30-depository-id></z30-depository-id>
            <z30-linking-number>000000000</z30-linking-number>
            <z30-gap-indicator></z30-gap-indicator>
            <z30-maintenance-count>001</z30-maintenance-count>
        </z30>
        <z13>
            <z13-doc-number>524340</z13-doc-number>
            <z13-year>2004</z13-year>
            <z13-open-date>20041119</z13-open-date>
            <z13-update-date>20120302</z13-update-date>
            <z13-call-no-key></z13-call-no-key>
            <z13-call-no-code>090</z13-call-no-code>
            <z13-call-no>791.4303</z13-call-no>
            <z13-author-code></z13-author-code>
            <z13-author></z13-author>
            <z13-title-code>24500</z13-title-code>
            <z13-title>1001 filmer du må se før du dør</z13-title>
            <z13-imprint-code>019</z13-imprint-code>
            <z13-imprint>l</z13-imprint>
            <z13-isbn-issn-code>020</z13-isbn-issn-code>
            <z13-isbn-issn>82-458-0649-8</z13-isbn-issn>
            <z13-user-defined-1-code>LDR</z13-user-defined-1-code>
            <z13-user-defined-1>^^^^^nam^^^^^^^^^1</z13-user-defined-1>
            <z13-user-defined-2-code>FMT</z13-user-defined-2-code>
            <z13-user-defined-2>BK</z13-user-defined-2>
            <z13-user-defined-3-code>008</z13-user-defined-3-code>
            <z13-user-defined-3>041119s2004^^^^^^^^^^^a^^^^^^^^^^0^nob^^</z13-user-defined-3>
            <z13-user-defined-4-code>082</z13-user-defined-4-code>
            <z13-user-defined-4>791.4303</z13-user-defined-4>
            <z13-user-defined-5-code>24500</z13-user-defined-5-code>
            <z13-user-defined-5></z13-user-defined-5>
        </z13>
        <current-fine></current-fine>
        <due-date>20120815</due-date>
        <due-hour>24:00</due-hour>
    </item-l>
    <item-l>
        <z36>
            <z36-doc-number>000317309</z36-doc-number>
            <z36-item-sequence>000002</z36-item-sequence>
            <z36-number>005098062</z36-number>
            <z36-material>BOOK</z36-material>
            <z36-sub-library>Hovedbibl.</z36-sub-library>
            <z36-status>A</z36-status>
            <z36-loan-date>20120718</z36-loan-date>
            <z36-loan-hour>15:05</z36-loan-hour>
            <z36-effective-due-date>00000000</z36-effective-due-date>
            <z36-due-date>20120815</z36-due-date>
            <z36-due-hour>24:00</z36-due-hour>
            <z36-returned-date></z36-returned-date>
            <z36-returned-hour></z36-returned-hour>
            <z36-item-status>04</z36-item-status>
            <z36-bor-status>Alm. lånere</z36-bor-status>
            <z36-letter-number>0</z36-letter-number>
            <z36-letter-date></z36-letter-date>
            <z36-no-renewal>0</z36-no-renewal>
            <z36-note-1></z36-note-1>
            <z36-note-2></z36-note-2>
            <z36-loan-cataloger-name>STVGBIBL</z36-loan-cataloger-name>
            <z36-loan-cataloger-ip>10.61.1.69</z36-loan-cataloger-ip>
            <z36-return-cataloger-name></z36-return-cataloger-name>
            <z36-return-cataloger-ip></z36-return-cataloger-ip>
            <z36-renew-cataloger-name></z36-renew-cataloger-name>
            <z36-renew-cataloger-ip></z36-renew-cataloger-ip>
            <z36-renew-mode></z36-renew-mode>
            <z36-bor-type>KV</z36-bor-type>
            <z36-note-alpha></z36-note-alpha>
            <z36-recall-date></z36-recall-date>
            <z36-recall-due-date></z36-recall-due-date>
            <z36-last-renew-date></z36-last-renew-date>
            <z36-original-due-date>20120815</z36-original-due-date>
            <z36-process-status></z36-process-status>
            <z36-loan-type></z36-loan-type>
            <z36-proxy-id></z36-proxy-id>
            <z36-recall-type></z36-recall-type>
            <z36-return-location></z36-return-location>
            <z36-return-sub-location></z36-return-sub-location>
            <z36-source></z36-source>
            <z36-delivery-time></z36-delivery-time>
            <z36-tail-time></z36-tail-time>
        </z36>
        <z30>
            <z30-doc-number>317309</z30-doc-number>
            <z30-item-sequence>2</z30-item-sequence>
            <z30-barcode>11030410905</z30-barcode>
            <z30-sub-library>Hovedbibl.</z30-sub-library>
            <z30-material> BOK</z30-material>
            <z30-item-status>4 uker</z30-item-status>
            <z30-open-date>19971130</z30-open-date>
            <z30-update-date>19980325</z30-update-date>
            <z30-cataloger></z30-cataloger>
            <z30-date-last-return>20120117</z30-date-last-return>
            <z30-hour-last-return>17:38</z30-hour-last-return>
            <z30-ip-last-return>10.61.1.44</z30-ip-last-return>
            <z30-no-loans>021</z30-no-loans>
            <z30-alpha>L</z30-alpha>
            <z30-collection>Musikkbiblioteket</z30-collection>
            <z30-call-no-type></z30-call-no-type>
            <z30-call-no>Noter</z30-call-no>
            <z30-call-no-key>  noter</z30-call-no-key>
            <z30-call-no-2-type></z30-call-no-2-type>
            <xxx-tag></xxx-tag>
            <z30-call-no-2-key></z30-call-no-2-key>
            <z30-description></z30-description>
            <z30-note-opac></z30-note-opac>
            <z30-note-circulation></z30-note-circulation>
            <z30-note-internal></z30-note-internal>
            <z30-order-number></z30-order-number>
            <z30-inventory-number></z30-inventory-number>
            <z30-inventory-number-date></z30-inventory-number-date>
            <z30-last-shelf-report-date>00000000</z30-last-shelf-report-date>
            <z30-price></z30-price>
            <z30-shelf-report-number></z30-shelf-report-number>
            <z30-on-shelf-date>00000000</z30-on-shelf-date>
            <z30-on-shelf-seq>000000</z30-on-shelf-seq>
            <z30-doc-number-2>000000000</z30-doc-number-2>
            <z30-schedule-sequence-2>00000</z30-schedule-sequence-2>
            <z30-copy-sequence-2>00000</z30-copy-sequence-2>
            <z30-vendor-code></z30-vendor-code>
            <z30-invoice-number></z30-invoice-number>
            <z30-line-number>00000</z30-line-number>
            <z30-pages></z30-pages>
            <z30-issue-date></z30-issue-date>
            <z30-expected-arrival-date></z30-expected-arrival-date>
            <z30-arrival-date></z30-arrival-date>
            <z30-item-statistic></z30-item-statistic>
            <z30-item-process-status></z30-item-process-status>
            <z30-copy-id></z30-copy-id>
            <z30-hol-doc-number>000000000</z30-hol-doc-number>
            <z30-temp-location>No</z30-temp-location>
            <z30-enumeration-a></z30-enumeration-a>
            <z30-enumeration-b></z30-enumeration-b>
            <z30-enumeration-c></z30-enumeration-c>
            <z30-enumeration-d></z30-enumeration-d>
            <z30-enumeration-e></z30-enumeration-e>
            <z30-enumeration-f></z30-enumeration-f>
            <z30-enumeration-g></z30-enumeration-g>
            <z30-enumeration-h></z30-enumeration-h>
            <z30-chronological-i></z30-chronological-i>
            <z30-chronological-j></z30-chronological-j>
            <z30-chronological-k></z30-chronological-k>
            <z30-chronological-l></z30-chronological-l>
            <z30-chronological-m></z30-chronological-m>
            <z30-supp-index-o></z30-supp-index-o>
            <z30-85x-type></z30-85x-type>
            <z30-depository-id></z30-depository-id>
            <z30-linking-number>000000000</z30-linking-number>
            <z30-gap-indicator></z30-gap-indicator>
            <z30-maintenance-count>001</z30-maintenance-count>
        </z30>
        <z13>
            <z13-doc-number>317309</z13-doc-number>
            <z13-year>1993</z13-year>
            <z13-open-date>19971213</z13-open-date>
            <z13-update-date>20060818</z13-update-date>
            <z13-call-no-key></z13-call-no-key>
            <z13-call-no-code>090</z13-call-no-code>
            <z13-call-no>786.5</z13-call-no>
            <z13-author-code></z13-author-code>
            <z13-author></z13-author>
            <z13-title-code>245</z13-title-code>
            <z13-title>The Oxford book of wedding music : for manuals</z13-title>
            <z13-imprint-code>019</z13-imprint-code>
            <z13-imprint>c</z13-imprint>
            <z13-isbn-issn-code>020</z13-isbn-issn-code>
            <z13-isbn-issn>0193751232</z13-isbn-issn>
            <z13-user-defined-1-code>LDR</z13-user-defined-1-code>
            <z13-user-defined-1>02449ccm^^22007091^^45^^</z13-user-defined-1>
            <z13-user-defined-2-code>FMT</z13-user-defined-2-code>
            <z13-user-defined-2>MU</z13-user-defined-2>
            <z13-user-defined-3-code>008</z13-user-defined-3-code>
            <z13-user-defined-3>940412^1993^^^^^^^^^^^a^^^^^^^^^^0^^^^^^</z13-user-defined-3>
            <z13-user-defined-4-code>082</z13-user-defined-4-code>
            <z13-user-defined-4>786.5</z13-user-defined-4>
            <z13-user-defined-5-code>245</z13-user-defined-5-code>
            <z13-user-defined-5></z13-user-defined-5>
        </z13>
        <current-fine></current-fine>
        <due-date>20120815</due-date>
        <due-hour>24:00</due-hour>
    </item-l>
    <item-l>
        <z36>
            <z36-doc-number>000436295</z36-doc-number>
            <z36-item-sequence>000010</z36-item-sequence>
            <z36-number>005098044</z36-number>
            <z36-material>BOOK</z36-material>
            <z36-sub-library>Hovedbibl.</z36-sub-library>
            <z36-status>A</z36-status>
            <z36-loan-date>20120718</z36-loan-date>
            <z36-loan-hour>15:03</z36-loan-hour>
            <z36-effective-due-date>00000000</z36-effective-due-date>
            <z36-due-date>20120815</z36-due-date>
            <z36-due-hour>24:00</z36-due-hour>
            <z36-returned-date></z36-returned-date>
            <z36-returned-hour></z36-returned-hour>
            <z36-item-status>04</z36-item-status>
            <z36-bor-status>Alm. lånere</z36-bor-status>
            <z36-letter-number>0</z36-letter-number>
            <z36-letter-date></z36-letter-date>
            <z36-no-renewal>0</z36-no-renewal>
            <z36-note-1></z36-note-1>
            <z36-note-2></z36-note-2>
            <z36-loan-cataloger-name>STVGBIBL</z36-loan-cataloger-name>
            <z36-loan-cataloger-ip>10.61.1.69</z36-loan-cataloger-ip>
            <z36-return-cataloger-name></z36-return-cataloger-name>
            <z36-return-cataloger-ip></z36-return-cataloger-ip>
            <z36-renew-cataloger-name></z36-renew-cataloger-name>
            <z36-renew-cataloger-ip></z36-renew-cataloger-ip>
            <z36-renew-mode></z36-renew-mode>
            <z36-bor-type>KV</z36-bor-type>
            <z36-note-alpha></z36-note-alpha>
            <z36-recall-date></z36-recall-date>
            <z36-recall-due-date></z36-recall-due-date>
            <z36-last-renew-date></z36-last-renew-date>
            <z36-original-due-date>20120815</z36-original-due-date>
            <z36-process-status></z36-process-status>
            <z36-loan-type></z36-loan-type>
            <z36-proxy-id></z36-proxy-id>
            <z36-recall-type></z36-recall-type>
            <z36-return-location></z36-return-location>
            <z36-return-sub-location></z36-return-sub-location>
            <z36-source></z36-source>
            <z36-delivery-time></z36-delivery-time>
            <z36-tail-time></z36-tail-time>
        </z36>
        <z30>
            <z30-doc-number>436295</z30-doc-number>
            <z30-item-sequence>10</z30-item-sequence>
            <z30-barcode>11030517347</z30-barcode>
            <z30-sub-library>Hovedbibl.</z30-sub-library>
            <z30-material> BOK</z30-material>
            <z30-item-status>4 uker</z30-item-status>
            <z30-open-date>20020626</z30-open-date>
            <z30-update-date>20020703</z30-update-date>
            <z30-cataloger>STVGBIBL</z30-cataloger>
            <z30-date-last-return>20070924</z30-date-last-return>
            <z30-hour-last-return>12:27</z30-hour-last-return>
            <z30-ip-last-return>10.61.1.45</z30-ip-last-return>
            <z30-no-loans>003</z30-no-loans>
            <z30-alpha>L</z30-alpha>
            <z30-collection>Musikkbiblioteket</z30-collection>
            <z30-call-no-type></z30-call-no-type>
            <z30-call-no>Noter</z30-call-no>
            <z30-call-no-key>  noter</z30-call-no-key>
            <z30-call-no-2-type></z30-call-no-2-type>
            <xxx-tag></xxx-tag>
            <z30-call-no-2-key></z30-call-no-2-key>
            <z30-description>1996</z30-description>
            <z30-note-opac></z30-note-opac>
            <z30-note-circulation></z30-note-circulation>
            <z30-note-internal></z30-note-internal>
            <z30-order-number>ORDER-31372</z30-order-number>
            <z30-inventory-number></z30-inventory-number>
            <z30-inventory-number-date></z30-inventory-number-date>
            <z30-last-shelf-report-date>00000000</z30-last-shelf-report-date>
            <z30-price>5617.00</z30-price>
            <z30-shelf-report-number></z30-shelf-report-number>
            <z30-on-shelf-date>00000000</z30-on-shelf-date>
            <z30-on-shelf-seq>000000</z30-on-shelf-seq>
            <z30-doc-number-2>000000000</z30-doc-number-2>
            <z30-schedule-sequence-2>00000</z30-schedule-sequence-2>
            <z30-copy-sequence-2>00000</z30-copy-sequence-2>
            <z30-vendor-code></z30-vendor-code>
            <z30-invoice-number></z30-invoice-number>
            <z30-line-number>00000</z30-line-number>
            <z30-pages></z30-pages>
            <z30-issue-date></z30-issue-date>
            <z30-expected-arrival-date></z30-expected-arrival-date>
            <z30-arrival-date></z30-arrival-date>
            <z30-item-statistic></z30-item-statistic>
            <z30-item-process-status></z30-item-process-status>
            <z30-copy-id></z30-copy-id>
            <z30-hol-doc-number>000000000</z30-hol-doc-number>
            <z30-temp-location>No</z30-temp-location>
            <z30-enumeration-a></z30-enumeration-a>
            <z30-enumeration-b></z30-enumeration-b>
            <z30-enumeration-c></z30-enumeration-c>
            <z30-enumeration-d></z30-enumeration-d>
            <z30-enumeration-e></z30-enumeration-e>
            <z30-enumeration-f></z30-enumeration-f>
            <z30-enumeration-g></z30-enumeration-g>
            <z30-enumeration-h></z30-enumeration-h>
            <z30-chronological-i></z30-chronological-i>
            <z30-chronological-j></z30-chronological-j>
            <z30-chronological-k></z30-chronological-k>
            <z30-chronological-l></z30-chronological-l>
            <z30-chronological-m></z30-chronological-m>
            <z30-supp-index-o></z30-supp-index-o>
            <z30-85x-type></z30-85x-type>
            <z30-depository-id></z30-depository-id>
            <z30-linking-number>000000000</z30-linking-number>
            <z30-gap-indicator></z30-gap-indicator>
            <z30-maintenance-count>001</z30-maintenance-count>
        </z30>
        <z13>
            <z13-doc-number>424256</z13-doc-number>
            <z13-year>1996</z13-year>
            <z13-open-date>20020625</z13-open-date>
            <z13-update-date>20060818</z13-update-date>
            <z13-call-no-key></z13-call-no-key>
            <z13-call-no-code></z13-call-no-code>
            <z13-call-no></z13-call-no>
            <z13-author-code>100</z13-author-code>
            <z13-author>Händel, Georg Friedrich</z13-author>
            <z13-title-code>24510</z13-title-code>
            <z13-title>Water music</z13-title>
            <z13-imprint-code>019</z13-imprint-code>
            <z13-imprint>c</z13-imprint>
            <z13-isbn-issn-code>020</z13-isbn-issn-code>
            <z13-isbn-issn>086209867X</z13-isbn-issn>
            <z13-user-defined-1-code>LDR</z13-user-defined-1-code>
            <z13-user-defined-1>^^^^^ncm^^^^^^^^^1</z13-user-defined-1>
            <z13-user-defined-2-code>FMT</z13-user-defined-2-code>
            <z13-user-defined-2>MU</z13-user-defined-2>
            <z13-user-defined-3-code>008</z13-user-defined-3-code>
            <z13-user-defined-3>020625s1996^^^^^^^^^z^a^^^^^^^^^^0^ing^^</z13-user-defined-3>
            <z13-user-defined-4-code>082</z13-user-defined-4-code>
            <z13-user-defined-4>786.5</z13-user-defined-4>
            <z13-user-defined-5-code>24510</z13-user-defined-5-code>
            <z13-user-defined-5>note</z13-user-defined-5>
        </z13>
        <current-fine></current-fine>
        <due-date>20120815</due-date>
        <due-hour>24:00</due-hour>
    </item-l>
    <item-l>
        <z36>
            <z36-doc-number>000621630</z36-doc-number>
            <z36-item-sequence>000010</z36-item-sequence>
            <z36-number>005098073</z36-number>
            <z36-material>DIGIT</z36-material>
            <z36-sub-library>Hovedbibl.</z36-sub-library>
            <z36-status>A</z36-status>
            <z36-loan-date>20120718</z36-loan-date>
            <z36-loan-hour>15:07</z36-loan-hour>
            <z36-effective-due-date>00000000</z36-effective-due-date>
            <z36-due-date>20120718</z36-due-date>
            <z36-due-hour>23:59</z36-due-hour>
            <z36-returned-date></z36-returned-date>
            <z36-returned-hour></z36-returned-hour>
            <z36-item-status>30</z36-item-status>
            <z36-bor-status>Alm. lånere</z36-bor-status>
            <z36-letter-number>0</z36-letter-number>
            <z36-letter-date></z36-letter-date>
            <z36-no-renewal>0</z36-no-renewal>
            <z36-note-1></z36-note-1>
            <z36-note-2></z36-note-2>
            <z36-loan-cataloger-name>STVGBIBL</z36-loan-cataloger-name>
            <z36-loan-cataloger-ip>10.61.1.69</z36-loan-cataloger-ip>
            <z36-return-cataloger-name></z36-return-cataloger-name>
            <z36-return-cataloger-ip></z36-return-cataloger-ip>
            <z36-renew-cataloger-name></z36-renew-cataloger-name>
            <z36-renew-cataloger-ip></z36-renew-cataloger-ip>
            <z36-renew-mode></z36-renew-mode>
            <z36-bor-type>KV</z36-bor-type>
            <z36-note-alpha></z36-note-alpha>
            <z36-recall-date></z36-recall-date>
            <z36-recall-due-date></z36-recall-due-date>
            <z36-last-renew-date></z36-last-renew-date>
            <z36-original-due-date>20120718</z36-original-due-date>
            <z36-process-status></z36-process-status>
            <z36-loan-type></z36-loan-type>
            <z36-proxy-id></z36-proxy-id>
            <z36-recall-type></z36-recall-type>
            <z36-return-location></z36-return-location>
            <z36-return-sub-location></z36-return-sub-location>
            <z36-source></z36-source>
            <z36-delivery-time></z36-delivery-time>
            <z36-tail-time></z36-tail-time>
        </z36>
        <z30>
            <z30-doc-number>621630</z30-doc-number>
            <z30-item-sequence>10</z30-item-sequence>
            <z30-barcode>11031203665</z30-barcode>
            <z30-sub-library>Hovedbibl.</z30-sub-library>
            <z30-material> DIGITALE MEDIER</z30-material>
            <z30-item-status>Video,1 uke</z30-item-status>
            <z30-open-date>20111102</z30-open-date>
            <z30-update-date>20111103</z30-update-date>
            <z30-cataloger>KATALOG</z30-cataloger>
            <z30-date-last-return>20120409</z30-date-last-return>
            <z30-hour-last-return>12:32</z30-hour-last-return>
            <z30-ip-last-return>10.61.1.44</z30-ip-last-return>
            <z30-no-loans>003</z30-no-loans>
            <z30-alpha>L</z30-alpha>
            <z30-collection>Musikkbiblioteket</z30-collection>
            <z30-call-no-type></z30-call-no-type>
            <z30-call-no>Film Regissører</z30-call-no>
            <z30-call-no-key>  film regissører</z30-call-no-key>
            <z30-call-no-2-type></z30-call-no-2-type>
            <xxx-tag></xxx-tag>
            <z30-call-no-2-key></z30-call-no-2-key>
            <z30-description></z30-description>
            <z30-note-opac></z30-note-opac>
            <z30-note-circulation></z30-note-circulation>
            <z30-note-internal></z30-note-internal>
            <z30-order-number></z30-order-number>
            <z30-inventory-number></z30-inventory-number>
            <z30-inventory-number-date></z30-inventory-number-date>
            <z30-last-shelf-report-date>00000000</z30-last-shelf-report-date>
            <z30-price></z30-price>
            <z30-shelf-report-number></z30-shelf-report-number>
            <z30-on-shelf-date>00000000</z30-on-shelf-date>
            <z30-on-shelf-seq>000000</z30-on-shelf-seq>
            <z30-doc-number-2>000000000</z30-doc-number-2>
            <z30-schedule-sequence-2>00000</z30-schedule-sequence-2>
            <z30-copy-sequence-2>00000</z30-copy-sequence-2>
            <z30-vendor-code></z30-vendor-code>
            <z30-invoice-number></z30-invoice-number>
            <z30-line-number>00000</z30-line-number>
            <z30-pages></z30-pages>
            <z30-issue-date></z30-issue-date>
            <z30-expected-arrival-date></z30-expected-arrival-date>
            <z30-arrival-date></z30-arrival-date>
            <z30-item-statistic></z30-item-statistic>
            <z30-item-process-status></z30-item-process-status>
            <z30-copy-id></z30-copy-id>
            <z30-hol-doc-number>000000000</z30-hol-doc-number>
            <z30-temp-location>No</z30-temp-location>
            <z30-enumeration-a></z30-enumeration-a>
            <z30-enumeration-b></z30-enumeration-b>
            <z30-enumeration-c></z30-enumeration-c>
            <z30-enumeration-d></z30-enumeration-d>
            <z30-enumeration-e></z30-enumeration-e>
            <z30-enumeration-f></z30-enumeration-f>
            <z30-enumeration-g></z30-enumeration-g>
            <z30-enumeration-h></z30-enumeration-h>
            <z30-chronological-i></z30-chronological-i>
            <z30-chronological-j></z30-chronological-j>
            <z30-chronological-k></z30-chronological-k>
            <z30-chronological-l></z30-chronological-l>
            <z30-chronological-m></z30-chronological-m>
            <z30-supp-index-o></z30-supp-index-o>
            <z30-85x-type></z30-85x-type>
            <z30-depository-id></z30-depository-id>
            <z30-linking-number>000000000</z30-linking-number>
            <z30-gap-indicator></z30-gap-indicator>
            <z30-maintenance-count>003</z30-maintenance-count>
        </z30>
        <z13>
            <z13-doc-number>605950</z13-doc-number>
            <z13-year>0000</z13-year>
            <z13-open-date>20111102</z13-open-date>
            <z13-update-date>20111103</z13-update-date>
            <z13-call-no-key></z13-call-no-key>
            <z13-call-no-code>090</z13-call-no-code>
            <z13-call-no></z13-call-no>
            <z13-author-code></z13-author-code>
            <z13-author></z13-author>
            <z13-title-code>24500</z13-title-code>
            <z13-title>What price glory</z13-title>
            <z13-imprint-code>019</z13-imprint-code>
            <z13-imprint>ee</z13-imprint>
            <z13-isbn-issn-code>025</z13-isbn-issn-code>
            <z13-isbn-issn>5035673006788</z13-isbn-issn>
            <z13-user-defined-1-code>LDR</z13-user-defined-1-code>
            <z13-user-defined-1>^^^^^ngm^^^^^^^^^1</z13-user-defined-1>
            <z13-user-defined-2-code>FMT</z13-user-defined-2-code>
            <z13-user-defined-2>VM</z13-user-defined-2>
            <z13-user-defined-3-code>008</z13-user-defined-3-code>
            <z13-user-defined-3>111102s^^^^^^^^^^^^^^^a^^^^^^^^^^1^eng^^</z13-user-defined-3>
            <z13-user-defined-4-code></z13-user-defined-4-code>
            <z13-user-defined-4></z13-user-defined-4>
            <z13-user-defined-5-code>24500</z13-user-defined-5-code>
            <z13-user-defined-5>video</z13-user-defined-5>
        </z13>
        <current-fine></current-fine>
        <due-date>20120718</due-date>
        <due-hour>23:59</due-hour>
    </item-l>
    <balance>12.00</balance>
    <sign>D</sign>
    <fine>
        <z31>
            <z31-id>STV000209626</z31-id>
            <z31-date>20120718</z31-date>
            <z31-status>Not paid by/credited to patron</z31-status>
            <z31-sub-library>Alle filialer</z31-sub-library>
            <z31-type>25</z31-type>
            <z31-credit-debit>D</z31-credit-debit>
            <z31-sum>(12.00)</z31-sum>
            <z31-vat-sum>0.00</z31-vat-sum>
            <z31-net-sum>12.00</z31-net-sum>
            <z31-payment-date></z31-payment-date>
            <z31-payment-hour></z31-payment-hour>
            <z31-payment-cataloger></z31-payment-cataloger>
            <z31-payment-target>Alle filialer</z31-payment-target>
            <z31-payment-ip></z31-payment-ip>
            <z31-payment-receipt-number></z31-payment-receipt-number>
            <z31-payment-mode></z31-payment-mode>
            <z31-payment-identifier></z31-payment-identifier>
            <z31-description>Diverse</z31-description>
            <z31-transfer-department></z31-transfer-department>
            <z31-transfer-date>00000000</z31-transfer-date>
            <z31-transfer-number></z31-transfer-number>
            <z31-recall-transfer-status></z31-recall-transfer-status>
            <z31-recall-transfer-date>00000000</z31-recall-transfer-date>
            <z31-recall-transfer-number></z31-recall-transfer-number>
        </z31>
    </fine>
    <session-id>HBDTYD84FQJFNEKJGBAEBXYK12XTQAD1EP1AEFUAQF6C5FBM8T</session-id>
</bor-info>
 
 ";
        }
    
    
    }
}
