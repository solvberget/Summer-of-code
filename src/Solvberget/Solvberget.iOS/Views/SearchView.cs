using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Solvberget.iOS
{
	public partial class SearchView : NamedViewController
    {
        public SearchView() : base("SearchView", null)
        {
        }

		public new SearchViewModel ViewModel
		{
			get
			{
				return base.ViewModel as SearchViewModel;
			}
		}

		SimpleTableViewSource<SearchResultViewModel> _resultsSource;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			ViewModel.EnableListEmptyResult = false;
			UpdateResultCount();

			ViewModel.PropertyChanged += (sender, e) => UpdateResultCount();
		
			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Organize, HandleRightBarButtonItemClicked), true);

			Query.SearchButtonClicked += HandleSearchButtonClicked;
			Query.TextChanged += HandleTextChanged;

			_resultsSource = new SimpleTableViewSource<SearchResultViewModel>(Results, CellBindings.SearchResults);
			Results.Source = _resultsSource;

			LoadingOverlay.LoadingText = "Søker...";

			var set = this.CreateBindingSet<SearchView, SearchViewModel>();
			set.Bind(_resultsSource).To(vm => vm.Results);
			set.Bind(_resultsSource).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			set.Bind(LoadingOverlay).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			Results.ReloadData();

			Query.BecomeFirstResponder();
		}

		UIView _overlay = new UIView();
		UIPickerView _filterOptions;
		bool _filterShowing;
		MvxPickerViewModel _filterModel;

        void HandleRightBarButtonItemClicked (object sender, EventArgs e)
        {
			ToggleFilterPanel();
        }

		void ToggleFilterPanel()
		{
			EnsureFilterPanelCreated();

			if (!_filterShowing) ShowFilterPanel();
			else HideFilterPanel();

			_filterShowing = !_filterShowing;
		}

		void EnsureFilterPanelCreated()
		{
			if (null == _filterOptions)
			{
				_overlay = new UIView();
				_overlay.AddGestureRecognizer(new UITapGestureRecognizer(ToggleFilterPanel));
				_overlay.Frame = new RectangleF(PointF.Empty, View.Frame.Size);
				_filterOptions = new UIPickerView();
				_filterOptions.BackgroundColor = UIColor.White;
				_overlay.AddSubview(_filterOptions);
				_filterModel = new MvxPickerViewModel(_filterOptions);
				_filterModel.ItemsSource = new[] {
					"Alle",
					"Bøker",
					"CDer",
					"Filmer",
					"Journaler",
					"Lydbøker",
					"Noter",
					"Spill",
					"Annet"
				};
				_filterModel.SelectedItem = "Alle";
				_filterModel.SelectedItemChanged += HandleFilterChanged;
				_filterOptions.Model = _filterModel;
			}
		}

		void ShowFilterPanel()
		{
			Query.ResignFirstResponder();

			_overlay.BackgroundColor = new UIColor(0f, 0f, 0f, 0f);
			_overlay.Frame = new RectangleF(PointF.Empty, View.Frame.Size);

			_filterOptions.Center = new PointF(View.Frame.Width / 2, View.Frame.Height + (_filterOptions.Frame.Height / 2));
			_filterOptions.Frame = new RectangleF(0, _filterOptions.Frame.Top, View.Frame.Width, _filterOptions.Frame.Height);
			View.AddSubview(_overlay);
			UIView.Animate(0.25, 0, UIViewAnimationOptions.CurveEaseInOut, () => 
			{
				_filterOptions.Center = new PointF(View.Frame.Width / 2, View.Frame.Height - (_filterOptions.Frame.Height / 2));
				_overlay.BackgroundColor = new UIColor(0f, 0f, 0f, 0.75f);
			}, null);
		}

		void HideFilterPanel()
		{
			UIView.Animate(0.25, 0, UIViewAnimationOptions.CurveEaseInOut, () => 
			{
				_filterOptions.Center = new PointF(View.Frame.Width / 2, View.Frame.Height + (_filterOptions.Frame.Height / 2));
				_overlay.BackgroundColor = new UIColor(0f, 0f, 0f, 0f);
			}, () => 
			{
				_overlay.RemoveFromSuperview();
			});
		}

		void HandleFilterChanged (object sender, EventArgs e)
		{
			UpdateBinding();
		}

		void UpdateBinding()
		{
			var set = this.CreateBindingSet<SearchView, SearchViewModel>();
			switch (_filterModel.SelectedItem as string)
			{
				case "Alle":
					set.Bind(_resultsSource).To(vm => vm.Results);
					break;
				case "Bøker":
					set.Bind(_resultsSource).To(vm => vm.BookResults);
					break;
				case "Filmer":
					set.Bind(_resultsSource).To(vm => vm.MovieResults);
					break;
				case "CDer":
					set.Bind(_resultsSource).To(vm => vm.CDResults);
				case "Lydbøker":
					set.Bind(_resultsSource).To(vm => vm.AudioBookResults);
					break;
				case "Noter":
					set.Bind(_resultsSource).To(vm => vm.SheetMusicResults);
					break;
				case "Spill":
					set.Bind(_resultsSource).To(vm => vm.GameResults);
					break;
				case "Journaler":
					set.Bind(_resultsSource).To(vm => vm.MagazineResults);
					break;
				case "Annet":
					set.Bind(_resultsSource).To(vm => vm.OtherResults);
					break;
			}

			set.Apply();
			Results.ReloadData();
			UpdateResultCount();
		}

		void UpdateResultCount()
		{
			if (null != ViewModel.Results && Results.NumberOfRowsInSection(0) == 0)
			{
				View.AddSubview(NoResultsLabel);
			}
			else
			{
				NoResultsLabel.RemoveFromSuperview();
			}
		}

        void HandleTextChanged (object sender, UISearchBarTextChangedEventArgs e)
        {
			ViewModel.ClearResults();
        }

        void HandleSearchButtonClicked (object sender, EventArgs e)
        {
			Query.ResignFirstResponder();

			ViewModel.Query = Query.Text;
			ViewModel.SearchAndLoad();

			Results.BecomeFirstResponder();
        }
    }
}

