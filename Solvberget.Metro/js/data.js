(function () {
    "use strict";

    var groupDescription = "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante";
    var itemDescription = "Item Description: Pellentesque porta mauris quis interdum vehicula urna sapien ultrices velit nec venenatis dui odio in augue cras posuere enim a cursus convallis neque turpis malesuada erat ut adipiscing neque tortor ac erat";
    var itemContent = "<p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat";
    var eventsDescription = "Oversikt over kommende arragementer på Sølvberget";
    var libraryListsDescription = "Lister fra Biblioteket";

    // These three strings encode placeholder images. You will want to set the
    // backgroundImage property in your real data to be URLs to images.
    var lightGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY7h4+cp/AAhpA3h+ANDKAAAAAElFTkSuQmCC";
    var mediumGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY5g8dcZ/AAY/AsAlWFQ+AAAAAElFTkSuQmCC";
    var darkGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY3B0cPoPAANMAcOba1BlAAAAAElFTkSuQmCC";
    var search = "/images/icon/Icon Pack 1/png/search.png";
    var events = "/images/icon/Icon Pack 1/png/calend.png";
    var info = "/images/icon/Icon Pack 1/png/info.png";
    var music = "/images/icon/Icon Pack 1/png/music.png";
    var home = "/images/icon/Icon Pack 1/png/home.png";
    var tasks = "/images/icon/Icon Pack 1/png/tasks.png";



    // Each of these sample groups must have a unique key to be displayed
    // separately.
    var sampleGroups = [
        { key: "lists", title: "Lister fra Biblioteket", subtitle: "Mest lest, nyheter etc.", backgroundImage: tasks, description: libraryListsDescription },
        { key: "group2", title: "Musikk", subtitle: "Min musikk", backgroundImage: music, description: groupDescription },
        { key: "group3", title: "Min Side", subtitle: "", backgroundImage: home, description: groupDescription },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, description: eventsDescription },
        { key: "group5", title: "Informasjon", subtitle: "Kontaktinfo og åpningstider", backgroundImage: info, description: groupDescription },
        { key: "group6", title: "Søk", subtitle: "Søk etter bøker, filmer eller lydbøker", backgroundImage: search, description: groupDescription }
    ];

    var searchResultItems = [

    ];

    // Each of these sample items should have a reference to a particular
    // group.
    var sampleItems = [
        { group: sampleGroups[0], title: "Drageløperen", subtitle: "av Khaled Hosseini", description: "Gjennomsnittlig terningkast: 5.03 av 6, basert på 1101 terningkast.", content: "Omtale fra Den Norske Bokdatabasen: <p>Året er 1975, og tolv år gamle Amir fra Kabul ønsker å vinne den årlige drageturneringen og dermed sin fars anerkjennelse. Amirs venn Hassan lover å hjelpe til. Hassan er tjenerens sønn, og tilhører en mindre ansett folkegruppe. Amir misunner likevel hans naturlige mot, og ikke minst hans plass i farens hjerte. På turneringsdagen skjer det noe katastrofalt som gjør at guttene mister kontakten med hverandre. Etter den sovjetiske invasjonen flykter Amir og faren til USA, og først etter mange år vender Amir tilbake til Afghanistan. Kanskje dette er hans sjanse til å gjøre opp for seg.", backgroundImage: "/images/dummydata/drageloperen.jpeg" },
        { group: sampleGroups[0], title: "Menn som hater kvinner", subtitle: "av Stieg Larsson", description: "Gjennomsnittlig terningkast: 5.14 av 6, basert på 1033 terningkast.", content: "Omtale fra Den Norske Bokdatabasen: <p>Finansjournalisten Mikael Blomkvist trenger en pause, og bestemmer seg for å ta et avbrekk fra jobben i tidsskriftet Millennium. Samtidig med dette, får han et oppdrag fra Henrik Vanger, tidligere en av Sveriges fremste industriledere. Han ønsker at Blomkvist skal skrive Vanger-slektens historie, men det viser seg raskt at dette er et skalkeskjul for at han ønsker å finne ut hva som skjedde med sin unge slektning, Harriet, som har vært sporløst forsvunnet i snart førti år. Sammen med Lisbeth Salander graver Blomkvist i familien Vangers fortid, og de avdekker mørkere og blodigere hemmeligheter enn noen av dem kunne ane. Dette er den første romanen om Blomkvist og Salander.  <p>Omtale fra forlaget: <p>Stieg Larssons første kriminalroman hylles av en enstemmig svensk presse. Økonomijournalisten Mikael Blomkvist trenger et avbrekk. Han er nettopp dømt til tre måneders fengsel for å ha ærekrenket finansmannen Hans-Erik Wennerström. Han bestemmer seg for å ta en pause fra jobben som redaktør i tidsskriftet Millennium. Omtrent samtidig får han et uvanlig oppdrag: Henrik Vanger, tidligere en av Sveriges fremste industriledere, vil at Blomkvist skal skrive Vanger-slektens historie. Men det viser seg snart at familiekrøniken bare er et dekke for Blomkvists virkelige oppdrag: å finne ut hva som hendte med Henrik Vangers unge slektning Harriet, som har vært sporløst forsvunnet i snart førti år, noe Henrik Vanger aldri har kunnet slutte å gruble over. Blomkvist påtar seg motvillig oppdraget og får hjelp av Lisbeth Salander, en mutt, tatovert kvinne på 24, med piercing og et stort talent for å irritere medarbeidere. Men Salander er også en fremragende researcher og en av landets fremste hackere. Sammen graver Blomkvist og Salander stadig dypere i familien Vangers fortid og finner en historie som er både mørkere og blodigere enn noen av dem kunne ane.«Genuin og genial» er blant de uttrykkene som brukes av en enstemmig svensk presse om Stieg Larssons debut som kriminalforfatter. Menn som hater kvinner er den første i en serie på tre kriminalromaner om Mikael Blomkvist og Lisbeth Salander.", backgroundImage: "/images/dummydata/mennsomhaterkvinner.jpeg" },
        { group: sampleGroups[0], title: "Harry Potter og de vises sten", subtitle: "av J.K. Rowling", description: "Gjennomsnittlig terningkast: 5.27 av 6, basert på 907 terningkast.", content: "Omtale fra Den Norske Bokdatabasen:<p>Harry Potter tror han er en helt vanlig 11 år gammel gutt til han blir reddet av ei ugle, tatt med til skolen for hekser og trollmenn og vinner en duell som kunne fått en dødelig utgang. Harry Potter er nemlig en trollmann! Dette er den første boken om Harry Potter.<p>Omtale fra forlaget: <p>Harry Potter kan ikke trylle, har aldri spilt rumpeldunk eller hjulpet til med å klekke ut en drage. Alt han kjenner til, er et trist liv hos teite onkel Wiktor og tante Petunia. Han bor i et kott under trappa og har ikke feiret bursdagen sin på 11 år. Men alt dette endrer seg når ei ugle leverer et mystisk brev med innbydelse til et utrolig sted som Harry - og alle som leser om ham - aldri vil glemme.", backgroundImage: "/images/dummydata/hp1.jpeg" },
        { group: sampleGroups[0], title: "Da Vinci-koden", subtitle: "av Dan Brown", description: "Gjennomsnittlig terningkast: 4.21 av 6, basert på 958 terningkast.", content: "Omtale fra Den Norske Bokdatabasen:<p>Når museumskurator på Louvre, Jacques Sauniere, blir brutalt myrdet, tilkalles Robert Langdon, som er professor i religiøs symbologi ved Harvard-universitetet. Ved liket er det en uforståelig kode, og sammen med den franske kryptologen Sophie Neveu finner Langdon spor som fører dem til Da Vincis verker og den hemmelige Sion-ordenen. Boka er filmatisert. Dette er den andre boka om Robert Langdon.", backgroundImage: "/images/dummydata/davincikoden.jpeg" },
        { group: sampleGroups[0], title: "Jenta som lekte med ilden", subtitle: "av Stieg Larsson", description: "Gjennomsnittlig terningkast: 5.14 av 6, basert på 890 terningkast.", content: "Omtale fra Den Norske Bokdatabasen:<p>Etter et lengre opphold i utlandet vender Lisbeth Salander hjem til Sverige. Hun har skaffet seg en stor sum penger og er for første gang i sitt liv økonomisk uavhengig. På samme tid har Mikael Blomkvist i tidsskriftet Millennium fått tilgang på brennhett nyhetsstoff. Journalisten Dag Svensson og hans samboer Mia Bergman sitter på opplysninger om en omfattende sexhandel mellom Øst-Europa og Sverige. Flere av dem som er innblandet i saken, har fremtredende posisjoner i samfunnet. For Salander begynner dystre hendelser i hennes fortid å gjøre seg gjeldende. Da Dag og Mia myrdes, rettes mistanken mot henne. Dermed bestemmer hun seg for å gjøre opp med fortiden og sørge for at de som fortjener det, får sin straff. Dette er den andre romanen om Blomqvist og Salander.<p>Omtale fra forlaget:<p>Kritikerroste Stieg Larssons andre kriminalroman om Lisbeth Salander og Mikael Blomkvist Lisbeth Salander vender tilbake til Sverige etter et lengre opphold i utlandet. Hun har skaffet seg en stor sum penger og er for første gang i sitt liv økonomisk uavhengig. Samtidig har Mikael Blomkvist i tidsskriftet Millennium fått tilgang på brennhett nyhetsstoff: Journalisten Dag Svensson og hans samboer Mia Bergman har kommet over avslørende opplysninger om en omfattende sex-handel mellom Øst-Europa og Sverige. Mange av dem som er innblandet i saken, har fremtredende posisjoner i samfunnet. Dystre hendelser i Salanders fortid begynner også å gjøre seg gjeldende. Da Dag og Mia blir brutalt myrdet, rettes mistanken mot Salander, og en storstilt politijakt settes i gang. Salander bestemmer seg for å gjøre opp med fortiden en gang for alle og sørge for at de som fortjener det, får sin straff. Blomkvists og Salanders veier krysses nok en gang.Jenta som lekte med ilden er den andre i en serie på tre kriminalromaner om Mikael Blomkvist og Lisbeth Salander. Den første, Menn som hater kvinner, ble hyllet av en enstemmig norsk presse da den utkom våren 2006.", backgroundImage: "/images/dummydata/jentasomlektemedilden.jpeg" },

        { group: sampleGroups[1], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[1], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[1], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: lightGray },

        { group: sampleGroups[2], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[2], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[2], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[2], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[2], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[2], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[2], title: "Item Title: 7", subtitle: "Item Subtitle: 7", description: itemDescription, content: itemContent, backgroundImage: mediumGray },

        { group: sampleGroups[3], title: "Detter er objekt 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[3], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[3], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[3], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[3], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[3], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: lightGray },

        { group: sampleGroups[4], title: "Dette er item 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[4], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[4], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[4], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: mediumGray },

        { group: sampleGroups[5], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[5], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[5], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[5], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[5], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: lightGray },
        { group: sampleGroups[5], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
        { group: sampleGroups[5], title: "Item Title: 7", subtitle: "Item Subtitle: 7", description: itemDescription, content: itemContent, backgroundImage: darkGray },
        { group: sampleGroups[5], title: "Item Title: 8", subtitle: "Item Subtitle: 8", description: itemDescription, content: itemContent, backgroundImage: lightGray }
    ];

    // Get a reference for an item, using the group key and item title as a
    // unique reference to the item that can be easily serialized.
    function getItemReference(item) {
        return [item.group.key, item.title];
    }

    function resolveGroupReference(key) {
        for (var i = 0; i < groupedItems.groups.length; i++) {
            if (groupedItems.groups.getAt(i).key === key) {
                return groupedItems.groups.getAt(i);
            }
        }
    }

    function resolveItemReference(reference) {
        for (var i = 0; i < groupedItems.length; i++) {
            var item = groupedItems.getAt(i);
            if (item.group.key === reference[0] && item.title === reference[1]) {
                return item;
            }
        }
    }

    // This function returns a WinJS.Binding.List containing only the items
    // that belong to the provided group.
    function getItemsFromGroup(group) {
        return list.createFiltered(function (item) { return item.group.key === group.key; });
    }

    var list = new WinJS.Binding.List();
    var groupedItems = list.createGrouped(
        function groupKeySelector(item) { return item.group.key; },
        function groupDataSelector(item) { return item.group; }
    );

    // TODO: Replace the data with your real data.
    // You can add data from asynchronous sources whenever it becomes available.
    sampleItems.forEach(function (item) {
        list.push(item);
    });

    WinJS.Namespace.define("Data", {
        items: groupedItems,
        groups: groupedItems.groups,
        getItemsFromGroup: getItemsFromGroup,
        getItemReference: getItemReference,
        resolveGroupReference: resolveGroupReference,
        resolveItemReference: resolveItemReference
    });
})();
