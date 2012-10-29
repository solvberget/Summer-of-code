using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Document
    {

        //Base properties
        public int StandardLoanTime { get { return 32; } }
        public string DocType { get { return GetType().Name; } }
        public string DocumentNumber { get; set; }
        public string TargetGroup { get; set; }
        public string IsFiction { get; set; }
        public string Language { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> DocumentType { get; set; }
        public string LocationCode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<string> ResponsiblePersons { get; set; }
        public object MainResponsible { get; set; }
        public string PlacePublished { get; set; }
        public string Publisher { get; set; }
        public string PublisherType { get; set; }
        public int PublishedYear { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesNumber { get; set; }
        public string CompressedSubTitle { get { return GetCompressedString(); } set { } }
        
        //Location and availability info for each branch
        public List<AvailabilityInformation> AvailabilityInfo { get; private set; }

        //Images
        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }

        protected virtual void FillProperties(string xml)
        {

            FillPropertiesLight(xml);

            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");

                var targetGrpString = GetFixfield(nodes, "008", 22, 22);
                if (!string.IsNullOrEmpty(targetGrpString))
                    TargetGroup = targetGrpString[0].Equals('j') ? "Barn og ungdom" : "Voksne";
                IsFiction = GetFixfield(nodes, "008", 33, 33).Equals("1") ? "Fiksjon" : "Fakta";

                //Only get languages (041a) if language in base equals "Flerspråklig" 
                if (Language.Equals("Flerspråklig"))
                {

                    var languages = GetVarfield(nodes, "041", "a").SplitByLength(3).ToList();
                    for (var i = 0; i < languages.Count(); i++)
                    {
                        string languageLookupValue = null;
                        LanguageDictionary.TryGetValue(languages[i], out languageLookupValue);
                        languages[i] = languageLookupValue ?? "Språk er ikke registrert";
                    }
                    Languages = languages;
                }

                LocationCode = GetVarfield(nodes, "090", "d");

                var involvedPersonsAsString = GetVarfield(nodes, "245", "c");
                if (involvedPersonsAsString != null)
                {
                    ResponsiblePersons = involvedPersonsAsString.Split(';');
                }

                PlacePublished = GetVarfield(nodes, "260", "a");
                Publisher = GetVarfield(nodes, "260", "b");

                var publisherType = DocType;
                string publisherTypeLookup = null;
                if (publisherType != null) PublisherTypeDictionary.TryGetValue(publisherType, out publisherTypeLookup);

                PublisherType = publisherTypeLookup ?? publisherType;

                SeriesTitle = GetVarfield(nodes, "440", "a");
                SeriesNumber = GetVarfield(nodes, "440", "v");

            }
        }

        protected virtual void FillPropertiesLight(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                DocumentNumber = xmlDoc.Root.Descendants("doc_number").Select(x => x.Value).FirstOrDefault();

                var nodes = xmlDoc.Root.Descendants("oai_marc");
                var language = GetFixfield(nodes, "008", 35, 37);

                string languageLookupValue = null;
                if (language != null)
                    LanguageDictionary.TryGetValue(language, out languageLookupValue);

                Language = languageLookupValue ?? "Språk er ikke registrert";

                var docTypeString = GetVarfield(nodes, "019", "b");
                if (docTypeString != null)
                    DocumentType = docTypeString.Split(';');



                Title = GetVarfield(nodes, "245", "a");
                SubTitle = GetVarfield(nodes, "245", "b");

                var publishedYearString = GetVarfield(nodes, "260", "c");
                if (publishedYearString != null)
                {
                    //Format may be "[2009]" or "2009.", trim if so
                    var regExp = new Regex(@"[a-zA-Z.\[\]]*(\d+)[a-zA-Z.\[\]]*");
                    var foundValue = regExp.Match(publishedYearString).Groups[1].ToString();
                    if (!string.IsNullOrEmpty(foundValue))
                        PublishedYear = int.Parse(foundValue);
                }

            }
        }


        protected virtual string GetCompressedString()
        {
            string docTypeLookupValue = null;
            if (DocType != null)
            {
                DocumentDictionary.TryGetValue(DocType, out docTypeLookupValue);
            }

            var temp = docTypeLookupValue ?? DocType;
            if (PublishedYear != 0)
                temp += " (" + PublishedYear + ")";
            return temp;

        }

        public static Document GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new Document();
            document.FillProperties(xml);
            return document;
        }

        public static Document GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new Document();
            document.FillPropertiesLight(xml);
            return document;
        }

        public void GenerateLocationAndAvailabilityInfo(IEnumerable<DocumentItem> docItems)
        {

            var items = docItems.ToList();
            if (!items.Any()) return;

            AvailabilityInfo = new List<AvailabilityInformation>();
            
            foreach (var availabilityInfo in AvailabilityInformation.BranchesToHandle.Select(branch => AvailabilityInformation.GenerateInfoFor(this, branch, items)).Where(availabilityInfo => availabilityInfo != null))
            {
                AvailabilityInfo.Add(availabilityInfo);
            }

        }

        private static string GetFixfield(IEnumerable<XElement> nodes, string id, int fromPos, int toPos)
        {
            var fixfield = nodes.Elements("fixfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Select(x => x.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(fixfield) && toPos < fixfield.Length)
            {
                if (fromPos == toPos)
                {
                    return fixfield.ElementAt(fromPos).ToString();
                }
                else
                {
                    return fixfield.Substring(fromPos, (toPos - fromPos) + 1);
                }
            }
            else
            {
                return "";
            }
        }

        public static string GetVarfield(IEnumerable<XElement> nodes, string id, string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return
                varfield.Where(x => ((string)x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).FirstOrDefault();
        }

        protected static IEnumerable<string> TrimContentList(string list)
        {
            var returnList = new List<string>();
            var temp = new List<string>();

            var cleanedList = list.Replace("Innhold: ", "");
            //cleanedList = cleanedList.Replace(":", ";");
            cleanedList = cleanedList.Replace("; ", ";");
            cleanedList = cleanedList.Replace(" CD", ";;CD");
            

            temp = cleanedList.Split(';').ToList();

            for (var i = 0; i < temp.Count(); i++){
                if (temp.ElementAt(i).Contains("CD"))
                {
                    returnList.Insert(i, temp.ElementAt(i).Split(':').ToList()[0].Trim());
                    i++;
                    returnList.Insert(i, temp.ElementAt(i - 1).Split(':').ToList()[1].Trim());
                }
                else
                {
                    returnList.Insert(i, temp.ElementAt(i).Trim());
                }
            }
  
            return returnList;
        } 

        protected static IEnumerable<string> GetVarfieldAsList(IEnumerable<XElement> nodes, string id,
                                                               string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return varfield.Where(x => ((string)x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).ToList();
        }

        private static string GetSubFieldValue(XElement varfield, string label)
        {
            return
                varfield.Elements("subfield").Where(x => ((string)x.Attribute("label")).Equals(label)).Select(
                    x => x.Value).FirstOrDefault();
        }

        protected static IEnumerable<Person> GeneratePersonsFromXml(IEnumerable<XElement> nodes, string id)
        {

            var persons = new List<Person>();

            var varfields = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).ToList();

            foreach (var varfield in varfields)
            {
                var nationality = GetSubFieldValue(varfield, "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);

                var role = GetSubFieldValue(varfield, "e");
                string roleLookupValue = null;
                if (role != null)
                    RoleDictionary.TryGetValue(role, out roleLookupValue);

                var person = new Person()
                                 {
                                     Name = GetSubFieldValue(varfield, "a"),
                                     LivingYears = GetSubFieldValue(varfield, "d"),
                                     Nationality = nationalityLookupValue ?? nationality,
                                     Role = roleLookupValue ?? role,
                                     ReferredWork = GetSubFieldValue(varfield, "t")
                                 };

                string tempName = GetSubFieldValue(varfield, "a");
                if (tempName != null)
                    person.InvertName(tempName);
                
                if (!string.IsNullOrEmpty(person.Name) && !persons.Any(x => x.Name.Equals(person.Name)))
                {
                    persons.Add(person);
                }

            }

            return persons;

        }

        protected static IEnumerable<Organization> GenerateOrganizationsFromXml(IEnumerable<XElement> nodes, string id)
        {

            var organizations = new List<Organization>();

            var varfields = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).ToList();

            foreach (var varfield in varfields)
            {
                var org = new Organization()
                              {
                                  Name = GetSubFieldValue(varfield, "a"),
                                  UnderOrganization = GetSubFieldValue(varfield, "b"),
                                  Role = GetSubFieldValue(varfield, "e"),
                                  FurtherExplanation = GetSubFieldValue(varfield, "q"),
                                  ReferencedPublication = GetSubFieldValue(varfield, "t")
                              };

                organizations.Add(org);

            }

            return organizations;

        }

        protected static readonly Dictionary<string, string> PublisherTypeDictionary = new Dictionary<string, string>
                                {
                                    {"Document", "Utgiver"},
                                    {"Book", "Forlag"},
                                    {"Film", "Utgiver"},
                                    {"AudioBook", "Forlag"},
                                    {"Cd", "Label/utgiver"},
                                    {"SheetMusic", "Forlag"},
                                    {"Journal", "Forlag"},
                                    {"LanguageCourse", "Forlag"}
                                }; 

        protected static readonly Dictionary<string, string> DocumentDictionary = new Dictionary<string, string>
                                {
                                    {"^^^", "Dokumenttype er ikke registrert"},
                                    {typeof(Document).Name, "Annet"},
                                    {typeof(AudioBook).Name, "Lydbok"},
                                    {typeof(Book).Name, "Bok"},                       
                                    {typeof(Cd).Name, "Cd"},
                                    {typeof(Film).Name, "Film"},
                                    {typeof(Journal).Name, "Tidsskrift"},
                                    {typeof(LanguageCourse).Name, "Språkkurs"},
                                    {typeof(SheetMusic).Name, "Note"},
                                    {typeof(Game).Name, "Spill"}
                                };

        protected static readonly Dictionary<string, string> LanguageDictionary = new Dictionary<string, string>
                                {
                                    {"^^^", ""},
                                    {"ing", ""},
                                    {"ace", "Aceh"},
                                    {"afr", "Afrikaans"},
                                    {"akk", "Akkadisk"},
                                    {"alb", "Albansk"},
                                    {"amh", "Amharisk"},
                                    {"ang", "Engelsk"},
                                    {"ara", "Arabisk"},
                                    {"arc", "Arameisk"},
                                    {"arm", "Armensk"},
                                    {"aze", "Aserbajdsjansk"},
                                    {"bam", "Bambara"},
                                    {"baq", "Baskisk"},
                                    {"bel", "Hviterussisk"},
                                    {"ben", "Bengali"},
                                    {"bis", "Bislama"},
                                    {"bnt", "Bantuspråk"},
                                    {"bos", "Bosnisk"},
                                    {"bre", "Bretonsk"},
                                    {"bul", "Bulgarsk"},
                                    {"bur", "Burmesisk"},
                                    {"byn", "Bilin"},
                                    {"cat", "Katalansk"},
                                    {"ceb", "Cebuano"},
                                    {"chi", "Kinesisk"},
                                    {"chu", "Kirkeslavisk"},
                                    {"cop", "Koptisk"},
                                    {"cus", "Kusjittiske"},
                                    {"cze", "Tsjekkisk"},
                                    {"dan", "Dansk"},
                                    {"dut", "Nederlandsk, Hollandsk"},
                                    {"dzo", "Dzongkha"},
                                    {"egy", "Egyptisk"},
                                    {"eng", "Engelsk"},
                                    {"enm", "Engelsk"},
                                    {"epo", "Esperanto"},
                                    {"est", "Estisk"},
                                    {"fao", "Færøysk"},
                                    {"fij", "Fijiansk"},
                                    {"fin", "Finsk"},
                                    {"fiu", "Finsk-ugriske"},
                                    {"fkv", "Kvensk"},
                                    {"fre", "Fransk"},
                                    {"frm", "Fransk"},
                                    {"fro", "Fransk"},
                                    {"fry", "Frisisk"},
                                    {"ful", "Fulfulde"},
                                    {"geo", "Georgisk"},
                                    {"ger", "Tysk"},
                                    {"gez", "Etiopisk"},
                                    {"gle", "Irsk"},
                                    {"glg", "Galegisk, Galisisk"},
                                    {"gmh", "Tysk"},
                                    {"got", "Gotisk"},
                                    {"grc", "Gresk"},
                                    {"gre", "Gresk"},
                                    {"guj", "Gujarati"},
                                    {"hau", "Hausa"},
                                    {"heb", "Hebraisk"},
                                    {"hil", "Hiligaynon"},
                                    {"hin", "Hindi"},
                                    {"hit", "Hettittisk"},
                                    {"hrv", "Kroatisk"},
                                    {"hun", "Ungarsk"},
                                    {"ibo", "Ibo"},
                                    {"ice", "Islandsk"},
                                    {"iku", "Inuittisk"},
                                    {"ilo", "Iloko"},
                                    {"ind", "Indonesisk"},
                                    {"ipk", "Inupiak"},
                                    {"ira", "Iranske"},
                                    {"ita", "Italiensk"},
                                    {"jpn", "Japansk"},
                                    {"kal", "Kalaalisut"},
                                    {"khm", "Khmer"},
                                    {"kho", "Khotanesisk"},
                                    {"kin", "Kinyarwanda"},
                                    {"kon", "Kongo"},
                                    {"kor", "Koreansk"},
                                    {"kur", "Kurdisk"},
                                    {"lat", "Latin"},
                                    {"lav", "Latvisk"},
                                    {"lit", "Litauisk"},
                                    {"mac", "Makedonsk"},
                                    {"mao", "Maori"},
                                    {"mar", "Marathi"},
                                    {"may", "Malayisk"},
                                    {"mlg", "Madagassisk"},
                                    {"mol", "Moldavisk"},
                                    {"mon", "Mongolsk"},
                                    {"mul", "Flerspråklig"},
                                    {"myn", "Maya"},
                                    {"nbl", "Ndebele"},
                                    {"nde", "Ndebele"},
                                    {"nds", "Nedertysk"},
                                    {"nep", "Nepali"},
                                    {"nic", "Niger-Kongospråk"},
                                    {"nno", "Nynorsk"},
                                    {"nob", "Norsk bokmål"},
                                    {"nom", "Mellomnorsk, ca. 1350 - 1550"},
                                    {"non", "Gammelnorsk, norrønt, frem til ca 1350"},
                                    {"nor", "Norsk"},
                                    {"oci", "Provencalsk"},
                                    {"orm", "Oromo"},
                                    {"pan", "Panjabi"},
                                    {"per", "Farsi"},
                                    {"pli", "Pali"},
                                    {"pol", "Polsk"},
                                    {"por", "Portugisisk"},
                                    {"pra", "Prakrit"},
                                    {"pus", "Pashto"},
                                    {"roh", "Retoromansk"},
                                    {"rom", "Romani"},
                                    {"rum", "Rumensk"},
                                    {"rus", "Russisk"},
                                    {"sah", "Jakutisk"},
                                    {"san", "Sanskrit"},
                                    {"sat", "Santali"},
                                    {"sgn", "Tegnspråk"},
                                    {"sin", "Singalesisk"},
                                    {"sit", "Sino-tibetanske"},
                                    {"slo", "Slovakisk"},
                                    {"slv", "Slovensk"},
                                    {"sma", "Sørsamisk"},
                                    {"sme", "Nordsamisk"},
                                    {"smi", "Samiske"},
                                    {"smj", "Lulesamisk"},
                                    {"smn", "Inarisamisk"},
                                    {"smo", "Samoansk"},
                                    {"sms", "Skoltesamisk"},
                                    {"sna", "Shona"},
                                    {"snd", "Sindhi"},
                                    {"som", "Somali"},
                                    {"spa", "Spansk"},
                                    {"srp", "Serbisk"},
                                    {"sux", "Sumerisk"},
                                    {"swa", "Swahili"},
                                    {"swe", "Svensk"},
                                    {"syr", "Syrisk"},
                                    {"tam", "Tamil"},
                                    {"tgl", "Tagalog"},
                                    {"tha", "Thai"},
                                    {"tib", "Tibetansk"},
                                    {"tir", "Tigrinja"},
                                    {"tkl", "Tokelaisk"},
                                    {"ton", "Tongansk"},
                                    {"tur", "Tyrkisk"},
                                    {"uig", "Uigurisk"},
                                    {"ukr", "Ukrainsk"},
                                    {"und", "Ubestemt"},
                                    {"urd", "Urdu"},
                                    {"uzb", "Usbekisk"},
                                    {"vie", "Vietnamesisk"},
                                    {"wel", "Walisisk"},
                                    {"wen", "Sorbiske, Vendisk"},
                                    {"yid", "Jiddish"},
                                    {"ypk", "Yupikspråk"},
                                    {"zul", "Zulu"}
                                };

        protected static readonly Dictionary<string, string> NationalityDictionary = new Dictionary<string, string>
                                {
                                    {"afr.", "Afrikansk"},
                                    {"afg.", "Afghansk"},
                                    {"alb.", "Albansk"},
                                    {"alg.", "Algerisk"},
                                    {"am.", "Amerikansk"},
                                    {"andor.", "Andorransk"},
                                    {"angol.", "Angolsk"},
                                    {"antig.", "Antiguansk"},
                                    {"arab.", "Arabisk"},
                                    {"argen.", "Argentinsk"},
                                    {"arm.", "Armensk"},
                                    {"aserb.", "Aserbajdsjansk"},
                                    {"au.", "Australsk"},
                                    {"babyl.", "Babylonsk"},
                                    {"baham.", "Bahamansk"},
                                    {"bahr.", "Bahrainsk"},
                                    {"bangl.", "Bangladeshisk"},
                                    {"barb.", "Barbadisk"},
                                    {"belg.", "Belgisk"},
                                    {"beliz.", "Belizisk"},
                                    {"benin.", "Beninsk"},
                                    {"bhut.", "Bhutansk"},
                                    {"boliv.", "Boliviansk"},
                                    {"bosn.", "Bosnisk"},
                                    {"botsw.", "Botswansk"},
                                    {"bras.", "Brasiliansk"},
                                    {"brun.", "Bruneisk"},
                                    {"bulg.", "Bulgarsk"},
                                    {"burkin.", "Burkinsk"},
                                    {"burm.", "Burmesisk"},
                                    {"burund.", "Burundisk"},
                                    {"chil.", "Chilensk"},
                                    {"colomb.", "Columbiansk"},
                                    {"costaric.", "Costaricansk"},
                                    {"cub.", "Cubansk"},
                                    {"d.", "Dansk"},
                                    {"dominik.", "Dominikansk"},
                                    {"ecuad.", "Ecuadoriansk"},
                                    {"egypt.", "Egyptisk"},
                                    {"emiratarab.", "Emiratarabisk"},
                                    {"eng.", "Engelsk"},
                                    {"eritr.", "Eritreisk"},
                                    {"est.", "Estisk"},
                                    {"etiop.", "Etiopisk"},
                                    {"fi.", "Finsk"},
                                    {"fr.", "Fransk"},
                                    {"fær.", "Færøyisk"},
                                    {"gabon.", "Gabonsk"},
                                    {"gamb.", "Gambisk"},
                                    {"gas.", "Gassisk"},
                                    {"georg.", "Georgisk"},
                                    {"ghan.", "Ghanesisk"},
                                    {"grenad.", "Grenadisk"},
                                    {"gr.", "Gresk"},
                                    {"grønl.", "Grønlandsk"},
                                    {"guadel.", "Guadeloupisk"},
                                    {"guatem.", "Guatemalsk"},
                                    {"guin.", "Guineansk"},
                                    {"guyan.", "Guyansk"},
                                    {"hait.", "Haitiansk"},
                                    {"hond.", "Honduransk"},
                                    {"hviter.", "Hviterussisk"},
                                    {"indian.", "Indianer"},
                                    {"ind.", "Indisk"},
                                    {"indon.", "Indonesisk"},
                                    {"irak.", "Irakisk"},
                                    {"iran.", "Iransk"},
                                    {"ir.", "Irsk"},
                                    {"isl.", "Islandsk"},
                                    {"isr.", "Israelsk"},
                                    {"it.", "Italiensk"},
                                    {"ivor.", "Elfenbenskysten (Ivoriansk)"},
                                    {"jam.", "Jamaicansk"},
                                    {"jap.", "Japansk"},
                                    {"jemen.", "Jemenittisk"},
                                    {"jord.", "Jordansk"},
                                    {"jug.", "Jugoslavisk"},
                                    {"kamb.", "Kambodsjansk"},
                                    {"kamer.", "Kamerunsk"},
                                    {"kan.", "Kanadisk"},
                                    {"kappverd.", "Kappverdisk"},
                                    {"kas.", "Kasakstansk"},
                                    {"katal.", "Katalonsk"},
                                    {"ken.", "Kenyansk"},
                                    {"kin.", "Kinesisk"},
                                    {"kirg.", "Kirgisisk"},
                                    {"komor.", "Komorisk"},
                                    {"kongol.", "Kongolesisk"},
                                    {"kor.", "Koreansk"},
                                    {"kroat.", "Kroatisk"},
                                    {"kurd.", "Kurdisk"},
                                    {"kuw.", "Kuwaitisk"},
                                    {"kypr.", "Kypriotisk"},
                                    {"laot.", "Laotisk"},
                                    {"latv.", "Latvisk"},
                                    {"lesot.", "Lesothisk"},
                                    {"liban.", "Libanesisk"},
                                    {"liber.", "Liberiansk"},
                                    {"liby.", "Libysk"},
                                    {"liecht.", "Liechtensteinsk"},
                                    {"lit.", "Litauisk"},
                                    {"lux.", "Luxembourgsk"},
                                    {"maked.", "Makedonsk"},
                                    {"malaw.", "Malawisk"},
                                    {"malay.", "Malayisk"},
                                    {"mali.", "Malisk"},
                                    {"malt.", "Maltesisk"},
                                    {"marok.", "Marokkansk"},
                                    {"mauret.", "Mauretansk"},
                                    {"maurit.", "Mauritisk"},
                                    {"mesop.", "Mesopotamisk"},
                                    {"mex.", "Mexikansk"},
                                    {"mold.", "Moldovsk"},
                                    {"moneg.", "Monegaskisk"},
                                    {"mong.", "Mongolsk"},
                                    {"montenegr.", "Montenegrinsk"},
                                    {"mosam.", "Mosambikisk"},
                                    {"namib.", "Namibisk"},
                                    {"ned.", "Nederlandsk"},
                                    {"nep.", "Nepalsk"},
                                    {"newzeal.", "Newzealandsk"},
                                    {"nicarag.", "Nicaraguansk"},
                                    {"niger.", "Nigeriansk"},
                                    {"nordir.", "Nordirsk"},
                                    {"n.", "Norsk"},
                                    {"pak.", "Pakistansk"},
                                    {"pal.", "Palestinsk"},
                                    {"panam.", "Panamansk"},
                                    {"pap.", "Papuansk"},
                                    {"parag.", "Paraguyansk"},
                                    {"pers.", "Persisk"},
                                    {"peru.", "Peruansk"},
                                    {"pol.", "Polsk"},
                                    {"portug.", "Portugisisk"},
                                    {"puert.", "Puertoricansk"},
                                    {"rom.", "Romersk"},
                                    {"rum.", "Rumensk"},
                                    {"r.", "Russisk"},
                                    {"rwand.", "Rwandisk"},
                                    {"salvad.", "Salvadoriansk"},
                                    {"sam.", "Samisk"},
                                    {"samoan.", "Samoansk"},
                                    {"sanktluc.", "Sanktlicianer"},
                                    {"saudiarab.", "Saudiarabisk"},
                                    {"senegal.", "Senegalesisk"},
                                    {"serb.", "Serbisk"},
                                    {"sierral.", "Sierraleonsk"},
                                    {"sk.", "Skotsk"},
                                    {"slovak.", "Slovakisk"},
                                    {"sloven.", "Slovensk"},
                                    {"somal.", "Somalisk"},
                                    {"sp.", "Spansk"},
                                    {"srilank.", "Srilankisk"},
                                    {"sudan.", "Sudanesisk"},
                                    {"surin.", "Surinamsk"},
                                    {"sveits.", "Sveitsisk"},
                                    {"sv.", "Svensk"},
                                    {"syr.", "Syrisk"},
                                    {"swazil.", "Swazilandsk"},
                                    {"sørafr.", "Sørafrikansk"},
                                    {"tadsj.", "Tadsjikisk"},
                                    {"tanz.", "Tanzaniansk"},
                                    {"tahit.", "Tahitisk"},
                                    {"taiw.", "Taiwansk"},
                                    {"tchad.", "Tchadisk"},
                                    {"thai.", "Thailandsk"},
                                    {"tib.", "Tibetansk"},
                                    {"tsjet.", "Tsjetsjensk"},
                                    {"togo.", "Togolesisk"},
                                    {"trinid.", "Trinidadisk"},
                                    {"tsj.", "Tsjekkisk"},
                                    {"tun.", "Tunesisk"},
                                    {"turkm.", "Turkmensk"},
                                    {"tyrk.", "Tyrkisk"},
                                    {"t.", "Tysk"},
                                    {"ugand.", "Ugandisk"},
                                    {"ukr.", "Ukrainsk"},
                                    {"ung.", "Ungarsk"},
                                    {"urug.", "Uruguyansk"},
                                    {"usb.", "Usbekisk"},
                                    {"venez.", "Venezuelansk"},
                                    {"vestind.", "Vestindisk"},
                                    {"viet.", "Vietnamesisk"},
                                    {"wal.", "Walisisk"},
                                    {"zair.", "Zairisk"},
                                    {"zamb.", "Zambisk"},
                                    {"zimb.", "Zimbabwisk"},
                                    {"øst.", "Østerriksk"},
                                };

        protected static readonly Dictionary<string, string> RoleDictionary = new Dictionary<string, string>
                                {
                                    {"arr.", "arrangør"},
                                    {"bearb.", "bearbeider"},
                                    {"dir.", "dirigent"},
                                    {"forf.", "forfatter"},
                                    {"fotogr.", "fotograf"},
                                    {"illustr.", "illustratør"},
                                    {"innl.", "innleser"},
                                    {"komm.", "kommentator"},
                                    {"komp.", "komponist"},
                                    {"manusforf.", "manusforfatter"},
                                    {"medarb.", "medarbeider"},
                                    {"medforf.", "medforfatter"},
                                    {"oppr.forf.", "opprinnelig forfatter"},
                                    {"overs.", "oversetter"},
                                    {"red.", "redaktør"},
                                    {"regissør", "regissør"},
                                    {"skuesp.", "skuespiller"},
                                    {"utg.", "utgiver"},
                                    {"utøv.", "utøver"},
                                };
    }
}



