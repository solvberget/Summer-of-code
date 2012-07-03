(function () {
    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/itemDetail/itemDetail.html", {

        item: undefined,
        documentId: undefined,
        viewModel: undefined,
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            this.item = options.item;
            this.documentId = options.key;
            this.defaultScript();
            element.querySelector(".content").focus();
        },

        defaultScript: function () {
            this.fragmentsDiv = this.element.querySelector("#fragments");
            this.fragmentsDiv.innerHTML = "";
            var self = this;

            var createViewModel = function (item) {

                if (ViewModel.DocumentList[self.documentId] != undefined) {
                    self.viewModel = ViewModel.DocumentList[self.documentId];
                } else {
                    if (item.DocType == "Book") {
                        self.viewModel = ViewModel.Book;
                        self.viewModel.viewPath = "/pages/itemDetail/fragments/bookFragment/bookFragment.html";
                        self.viewModel.fragment = Book_Fragment;
                    }
                    if (item.DocType == "Film") {
                        self.viewModel = ViewModel.Movie;
                        self.viewModel.viewtPath = "/pages/itemDetail/fragments/movieFragment/movieFragment.html";
                        self.viewModel.fragment = Movie_Fragment;
                    }
                    self.viewModel.fillProperties(item);
                    ViewModel.DocumentList[self.documentId] = self.viewModel;
                }
            }

            var renderItem = function (item) {
                createViewModel(item);
                // Read fragment from the HMTL file and load it into the div.  This
                // fragment also loads linked CSS and JavaScript specified in the fragment
                WinJS.UI.Fragments.renderCopy(self.viewModel.viewPath,
              self.fragmentsDiv)
              .done(function (fragment) {
                  // After the fragment is loaded into the target element,
                  // CSS and JavaScript referenced in the fragment are loaded.  The
                  // fragment loads script that defines an initialization function,
                  // so we can now call it to initialize the fragment's contents.
                  WinJS.Binding.processAll(self.fragmentsDiv, self.viewModel);
                  self.viewModel.fragment.fragmentLoad(fragment);
                  WinJS.log && WinJS.log("successfully loaded fragment.", "sample", "status");
              }
          );
            }

            renderItem(self.item);


        }
    });


})();


