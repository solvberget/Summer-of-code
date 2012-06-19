var sampleViewModel = {
    firstName: ko.observable("Jean-Luc"),
    lastName: ko.observable("Picard")
};

sampleViewModel.fullName = ko.dependentObservable(function () {
    // Knockout tracks dependencies automatically. It knows that fullName depends
    // on firstName and lastName, because these get called when evaluating fullName.
    return sampleViewModel.firstName() + " " + sampleViewModel.lastName();
});

// This makes Knockout get to work
ko.applyBindings(sampleViewModel);
