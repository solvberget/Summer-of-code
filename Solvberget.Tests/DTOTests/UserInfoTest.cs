using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.DTO;

namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class UserInfoTest
    {

        [Test]
        public void PropertiesTest()
        {
            const string userId = "159222";
            var user = new UserInfo { BorrowerId = userId };
            user.FillProperties(UserXml);
            Assert.AreEqual(userId, user.BorrowerId);
            Assert.AreEqual("STV000060009", user.Id);

            Assert.AreEqual("60.00", user.Balance);
            Assert.AreEqual("350.00", user.CashLimit);
            Assert.AreEqual("91562303", user.CellPhoneNumber);
            Assert.AreEqual("4019 STAVANGER", user.CityAddress);
            Assert.AreEqual("09.04.1989", user.DateOfBirth);
            Assert.AreEqual("haaland@gmail.com", user.Email);
            Assert.AreEqual("Hovedbibl.", user.HomeLibrary);
            Assert.AreEqual("51536695", user.HomePhoneNumber);
            Assert.AreEqual("Sindre Haaland", user.Name);
            Assert.AreEqual("Sindre Haaland", user.PrefixAddress);
            Assert.AreEqual("Bakkesvingen 8", user.StreetAddress);
            Assert.AreEqual("4019", user.Zip); 

        }

        [Test]
        public void FineTest()
        {
            const string userId = "159222";
            var user = new UserInfo { BorrowerId = userId };
            user.FillProperties(UserXml);

            Assert.AreEqual("20070208", user.Fines.ElementAt(0).Date);
            Assert.AreEqual("Not paid by/credited to patron", user.Fines.ElementAt(0).Status);
            Assert.AreEqual('D', user.Fines.ElementAt(0).CreditDebit);
            Assert.AreEqual(30.00 , user.Fines.ElementAt(0).Sum);
            Assert.AreEqual("Nytt lånekort", user.Fines.ElementAt(0).Description);


            Assert.AreEqual(2, user.Fines.Count());
            Assert.AreEqual("20070319", user.Fines.ElementAt(1).Date);
            Assert.AreEqual("Lånt 20070208 Forf. 20070308 Kat. 01 - 1-2 ukes forsink.:", user.Fines.ElementAt(1).Description);
            Assert.AreEqual("230544", user.Fines.ElementAt(1).DocumentNumber);
            Assert.AreEqual("Gift", user.Fines.ElementAt(1).DocumentTitle);


        }



        private const string UserXml = @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
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
}
