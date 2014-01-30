(function () {

    "use strict";

    var groups = [];

    var groupedItems;

    function getItemReference(item) {
        return [getGroupByKey(item.TypeId).key, item.Name];
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
            if (getGroupByKey(item.TypeId).key === reference[0] && item.Name === reference[1]) {
                return item;
            }
        }
    }
    
    function itemAlreadyExist(id) {
        for(var i = 0; i < groupedItems.length; i++) {
            var item = groupedItems.getAt(i);
            if (item.Id == id)
                return true;
        }
        return false;
    }

    function getItemsFromGroup(group) {
        return list.createFiltered(function (item) { return getGroupByKey(item.TypeId).key === group.key; });
    }


    var list = new WinJS.Binding.List();

    groupedItems = list.createGrouped(
            function groupKeySelector(item) { return getGroupByKey(item.TypeId).key; },
            function groupDataSelector(item) { return getGroupByKey(item.TypeId); }
        );

    function setData(items) {
        items.forEach(function (item) {
            addGroupFor(item);
            if (!itemAlreadyExist(item.Id))
                list.push(item);
        });
        
    }

    function addGroupFor(item) {
        if (groups[item.TypeId] !== undefined) return;
        groups[item.TypeId] = { key: item.TypeId, Name: item.TypeName, Description: "Arrangement med kategori: " + item.TypeName };
    }

    function getGroupByKey(key) {
        return groups[key];
    }

    WinJS.Namespace.define("EventData", {
        items: groupedItems,
        groups: groupedItems.groups,
        getItemsFromGroup: getItemsFromGroup,
        getItemReference: getItemReference,
        resolveGroupReference: resolveGroupReference,
        resolveItemReference: resolveItemReference,
        setData: setData,
        getGroupByKey: getGroupByKey
    });




    //TODO: Sort by month
    //(Map month to key, generate groups, add AppBar sort switching, Set new GroupDataSource for ListView) 

    //function getGroupByKey(key) {
    //    return groupToKey[key];
    //}

    //function pushData(items) {
    //    eventItems = items;
    //    groups = new Array();
    //    eventItems.forEach(function (item) {
    //        addGroupFor(item);
    //        list.push(item);
    //    });
    //}

    //function addGroupFor(item) {
    //    for (var i = 0; i < groups.length; i++) {
    //        if (groups[i].title === groupToMonth[item.Group]) {
    //            return;
    //        }
    //    }
    //    var key = groups.length < 1 ? 0 : groups[groups.length - 1].key + 1;
    //    groups.push({ key: key, title: groupToMonth[item.Group] });
    //    groupToKey[item.Group] = key;
    //}

    //var groupToMonth = { 0: "Januar", 1: "Februar", 2: "Mars", 3: "April", 4: "Mai", 5: "Juni", 6: "Juli", 7: "August", 8: "September", 9: "Oktober", 10: "November", 11: "Desember" };
    //var groupToKey = [];



})();
