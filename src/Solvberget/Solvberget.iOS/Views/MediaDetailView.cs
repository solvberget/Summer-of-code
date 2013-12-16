using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;
using System.Threading;
using System.Linq;
using Solvberget.Core.DTOs;
using System.Web;
using MonoTouch.Twitter;
using MonoTouch.FacebookConnect;

namespace Solvberget.iOS
{
	public partial class MediaDetailView : NamedViewController
    {
		public new MediaDetailViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MediaDetailViewModel;
			}
		}


        public MediaDetailView() : base("MediaDetailView", null)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter detaljer...";
		}

		BoxRenderer _boxes;

		protected override void ViewModelReady()
		{
			base.ViewModelReady();

			foreach (var s in ScrollView.Subviews.Skip(1)) // leave the header view
				s.RemoveFromSuperview();
		
			_boxes = new BoxRenderer(ScrollView);

			RatingSourceLabel.Text = HeaderLabel.Text = SubtitleLabel.Text = TypeLabel.Text = String.Empty;

			Style();
			Update();
		}

		private void Style()
        {
            StarsContainer.BackgroundColor = UIColor.Clear;
			HeaderView.BackgroundColor = Application.ThemeColors.Main2;

			HeaderLabel.Font = Application.ThemeColors.TitleFont1;
			HeaderLabel.TextColor = Application.ThemeColors.MainInverse;

			SubtitleLabel.Font = Application.ThemeColors.DefaultFont;
			SubtitleLabel.TextColor = Application.ThemeColors.MainInverse;

			TypeLabel.Font = Application.ThemeColors.DefaultFont;
			TypeLabel.TextColor = Application.ThemeColors.MainInverse;
		}

        private void OnToggleFavorite(object sender, EventArgs e)
        {
			NavigationItem.RightBarButtonItem.Enabled = false;
			ToggleFavorite();
        }

		private async void ToggleFavorite()
		{
			if (ViewModel.IsFavorite) await ViewModel.RemoveFavorite();
			else await ViewModel.AddFavorite();

			InvokeOnMainThread(() => {
				UpdateFavoriteButtonState();
			});
		}

		UIBarButtonItem _favButton;
		UIBarButtonItem _shareButton;

		private void UpdateFavoriteButtonState()
		{
			var favStateImage = UIImage.FromBundle("/Images/star.on.png").Scale(new SizeF(26, 26));

			if(!ViewModel.IsFavorite && !UIHelpers.MinVersion7)
			{
				favStateImage = UIImage.FromBundle("/Images/star.off.png").Scale(new SizeF(26, 26));
			}

			if (null == _favButton)
			{
				_favButton = new UIBarButtonItem(favStateImage, UIBarButtonItemStyle.Plain, OnToggleFavorite);
				_shareButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, OnShare);

				NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{_shareButton,_favButton}, false);
			}

			if (UIHelpers.MinVersion7)
			{
				_favButton.TintColor = ViewModel.IsFavorite ? Application.ThemeColors.FavoriteColor : Application.ThemeColors.MainInverse;
			}
			else
			{
				_favButton.Image = favStateImage;
			}
		}

		private void OnShare(object sender, EventArgs e)
		{
			var shareView = new UIAlertView(View.Frame);

			shareView.Title = "Del " + ViewModel.Title;

			shareView.AddButton("Del på Facebook");
			shareView.AddButton("Del på Twitter");
			shareView.AddButton("Avbryt");

			shareView.Clicked += (ss, se) =>
			{
				string launchUri = null;

				var shareMessage = "Se hva jeg fant på Sølvberget: " + ViewModel.Title;

				if(null == ViewModel.RawDto.WebAppUrl) return;

				switch(se.ButtonIndex)
				{
					case 0:

						if(!FBDialogs.CanPresentOSIntegratedShareDialog(FBSession.ActiveSession))
						{
							UIAlertView alert = new UIAlertView(View.Frame);
							alert.Title = "Facebook oppsett mangler";
							alert.Message = "Du må koble din iPhone/iPad til Facebook før du kan dele (selv om du kanskje har installet Facebook appen). Gå til Instillinger - Facebook.";
							alert.AddButton("Ok");
							alert.Show();
							return;
						}

						FBDialogs.PresentOSIntegratedShareDialogModally(this,
							shareMessage, null, new NSUrl(ViewModel.RawDto.WebAppUrl),new FBOSIntegratedShareDialogHandler((res,err) => {

								var ex = err;

							}));


						break;
					case 1:

						var tvc = new TWTweetComposeViewController();
						tvc.SetInitialText(shareMessage);
						tvc.AddUrl(new NSUrl(ViewModel.RawDto.WebAppUrl));
						PresentModalViewController(tvc, true);

						break;
				}

				if(null == launchUri) return;

				bool success = UIApplication.SharedApplication.OpenUrl(new NSUrl(launchUri));
			};

			shareView.CancelButtonIndex = shareView.ButtonCount - 1;

			shareView.Show();
		}

		private void Update()
		{
            UpdateFavoriteButtonState();

			HeaderLabel.Text = ViewModel.Title;
			SubtitleLabel.Text = ViewModel.SubTitle;
			TypeLabel.Text = ViewModel.Type;

			RenderRating();
			RenderAvailability();

			switch (ViewModel.Type)
			{
				case "Book":
					RenderBook();
					break;

				case "Film":
					RenderMovie();
					break;

				case "SheetMusic":
					RenderSheetMusic();
					break;

				case "Game":
					RenderGame();
					break;

				case "Journal":
					RenderJournal();
					break;

				case "Cd":
					RenderCd();
					break;
			}

			Image.Image = UIHelpers.ImageFromUrl(ViewModel.Image);

			if (null != Image.Image)
			{
				var imageScale = Image.Frame.Width / Image.Image.Size.Width;
				var imageHeight = Math.Min(Image.Image.Size.Height * imageScale, Image.Frame.Height);
				var imageSize = new SizeF(Image.Frame.Width, imageHeight);
				Image.Frame = new RectangleF(Image.Frame.Location, imageSize);
			}

			Position();

			LoadingOverlay.Hide();
		}

		void RenderAvailability()
		{
			if (null == ViewModel.Availabilities || ViewModel.Availabilities.Length == 0) return;

			_boxes.AddSectionHeader("Tilgjengelighet");

			foreach (var availability in ViewModel.Availabilities)
			{
				var box = _boxes.StartBox();
				new LabelAndValue(box, "Filial", availability.Branch, true);
				new LabelAndValue(box, "Finnes på hylle", availability.Location);
				new LabelAndValue(box, "Avdeling", availability.Department);
				new LabelAndValue(box, "Samling", availability.Collection);

				var availabilityText = availability.AvailableCount + " av " + availability.TotalCount + " tilgjengelig for utlån.";

				if (availability.EstimatedAvailableDate.HasValue)
				{
					availabilityText += " Tidligst tilgjengelig " + availability.EstimatedAvailableText;
				}

				new LabelAndValue(box, "Tilgjengelighet", availabilityText, colspan : 3);

				var reserve = new UIButton();

				reserve.TouchUpInside += (s, e) => availability.PlaceHoldRequestCommand.Execute(null);

				var set = this.CreateBindingSet<MediaDetailView, MediaDetailViewModel>();
				set.Bind(reserve).For("Title").To(vm => vm.ButtonText);
				set.Bind(reserve).For("Enabled").To(vm => vm.ButtonEnabled);
				set.Apply();


				Application.ThemeColors.Style(reserve);

				var btnPadding = UIHelpers.MinVersion7 ? 0f : padding;

				reserve.Frame = new RectangleF(padding, box.Subviews.Last().Frame.Bottom+padding, 165f, reserve.SizeThatFits(new SizeF(0f,0f)).Height + btnPadding);

				box.Add(reserve);
				box.Frame = new RectangleF(box.Frame.Location, new SizeF(box.Frame.Width, reserve.Frame.Bottom+padding));

			}
		}


		void RenderRating()
		{
			if (null != ViewModel.Rating)
			{
				RatingSourceLabel.Text = "Fra " + ViewModel.Rating.Source;
			
				var x = 0;
				for (int i = 0; i < (int)ViewModel.Rating.MaxScore; i++)
				{
					var star = new UIImageView(new RectangleF(x, 0, 14, 14));
					if (i < (int)ViewModel.Rating.Score)// add star.half.on.png for better precision?
					{
						star.Image = UIImage.FromBundle("/Images/star.on.png");
					}
					else
					{
						star.Image = UIImage.FromBundle("/Images/star.off.png");
					}
					StarsContainer.Add(star);
					x += 14;
				}
			}
		}

		void RenderBook()
		{
			if (!String.IsNullOrEmpty(ViewModel.Review))
			{
				_boxes.AddSectionHeader("Bokbasens omtale");
				var box = _boxes.StartBox();
				new LabelAndValue(box, String.Empty, ViewModel.Review, colspan : 3);
			}

			_boxes.AddSectionHeader("Fakta om boka");

			var dto = ViewModel.RawDto as BookDto;

			var facts = _boxes.StartBox();

			if(!String.IsNullOrEmpty(dto.AuthorName)) new LabelAndValue(facts, "Forfatter", dto.AuthorName);
			if(!String.IsNullOrEmpty(dto.Classification)) new LabelAndValue(facts, "Sjanger", dto.Classification);
			if(null != dto.Series) new LabelAndValue(facts, "Del av serie", dto.Series.Title);
			if(null != dto.Series) new LabelAndValue(facts, "Nummber i serie", dto.Series.SequenceNo);

			if(!String.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Forlag", ViewModel.Publisher);
			if(!String.IsNullOrEmpty(ViewModel.Language)) new LabelAndValue(facts, "Språk", ViewModel.Language);
		}

		void RenderCd()
		{
			_boxes.AddSectionHeader("Fakta om CDen");

			var dto = ViewModel.RawDto as CdDto;

			var facts = _boxes.StartBox();

			if(!String.IsNullOrEmpty(dto.ArtistOrComposerName)) new LabelAndValue(facts, "Artist eller komponist", dto.ArtistOrComposerName);
			if(null != dto.CompositionTypesOrGenres && dto.CompositionTypesOrGenres.Length > 0) new LabelAndValue(facts, "Komposisjonstype eller sjanger", String.Join(", ", dto.CompositionTypesOrGenres));
			if(!String.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Språk", dto.Language);
			if(!String.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Label/utgiver", ViewModel.Publisher);
			if(!String.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);

		}

		void RenderGame()
		{
			_boxes.AddSectionHeader("Fakta om spillet");

			var dto = ViewModel.RawDto as GameDto;

			var facts = _boxes.StartBox();
			if(!String.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Platform", dto.Platform);

			if(!String.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
			if(!String.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
		}

		void RenderJournal()
		{
			_boxes.AddSectionHeader("Fakta om journalen");

			var dto = ViewModel.RawDto as JournalDto;

			var facts = _boxes.StartBox();
			if(null != dto.Subjects && dto.Subjects.Length > 0) new LabelAndValue(facts, "Kategorier", String.Join(", ", dto.Subjects));

			if(!String.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
			if(!String.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
		}

		void RenderSheetMusic()
		{
			_boxes.AddSectionHeader("Fakta om notehefte");

			var dto = ViewModel.RawDto as SheetMusicDto;

			var facts = _boxes.StartBox();

			if(!String.IsNullOrEmpty(dto.ComposerName)) new LabelAndValue(facts, "Komponist", dto.ComposerName);
			if(!String.IsNullOrEmpty(dto.CompositionType)) new LabelAndValue(facts, "Komposisjonstype", dto.CompositionType);
			if(!String.IsNullOrEmpty(dto.NumberOfPagesAndParts)) new LabelAndValue(facts, "Sidetall, type noter og antall stemmer", dto.NumberOfPagesAndParts);

			if(null != dto.MusicalLineup && dto.MusicalLineup.Length > 0) new LabelAndValue(facts, "Beseting", String.Join(", ", dto.MusicalLineup));

		}


		void RenderMovie()
		{
			_boxes.AddSectionHeader("Fakta om filmen");

			var dto = ViewModel.RawDto as FilmDto;

			var facts = _boxes.StartBox();

			if(!String.IsNullOrEmpty(dto.AgeLimit)) new LabelAndValue(facts, "Aldersgrense", dto.AgeLimit.Replace("Aldersgrense:", String.Empty).Trim());
			if(!String.IsNullOrEmpty(dto.MediaInfo)) new LabelAndValue(facts, "Format", dto.MediaInfo);
			if(null != dto.ActorNames && dto.ActorNames.Length > 0) new LabelAndValue(facts, "Skuespillere", String.Join(", ",dto.ActorNames));
			if(!String.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Språk", dto.Language);
			if(null != dto.SubtitleLanguages && dto.SubtitleLanguages.Length > 0) new LabelAndValue(facts, "Undertekster", String.Join(", ",dto.SubtitleLanguages));
			if(null != dto.ReferredPeopleNames && dto.ReferredPeopleNames.Length > 0) new LabelAndValue(facts, "Refererte personer", String.Join(", ",dto.ReferredPeopleNames));
			if(null != dto.ReferencedPlaces && dto.ReferencedPlaces.Length > 0) new LabelAndValue(facts, "Omtalte steder", String.Join(", ",dto.ReferencedPlaces));
			if(null != dto.Genres && dto.Genres.Length > 0) new LabelAndValue(facts, "Sjanger", String.Join(", ",dto.Genres));if(null != dto.ActorNames && dto.ActorNames.Length > 0) new LabelAndValue(facts, "Skuespillere", String.Join(",",dto.ActorNames));

			if(null != dto.InvolvedPersonNames && dto.InvolvedPersonNames.Length > 0) new LabelAndValue(facts, "Involverte personer", String.Join(", ",dto.InvolvedPersonNames));
			if(null != dto.ResponsiblePersonNames && dto.ResponsiblePersonNames.Length > 0) new LabelAndValue(facts, "Ansvarlige personer", String.Join(", ",dto.ResponsiblePersonNames));

			if(!String.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
			if(!String.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);




		}

		float padding = 10.0f;

		private void Position()
		{
			var headerSize = HeaderLabel.SizeThatFits(new SizeF(HeaderLabel.Frame.Width, 0));

			HeaderLabel.Frame = new RectangleF(HeaderLabel.Frame.Location, headerSize);

			var subtitleSize = SubtitleLabel.SizeThatFits(new SizeF(SubtitleLabel.Frame.Width, 0));
			var subtitlePos = new PointF(SubtitleLabel.Frame.X, HeaderLabel.Frame.Bottom);

			SubtitleLabel.Frame = new RectangleF(subtitlePos, subtitleSize);

			var typeSize = TypeLabel.SizeThatFits(new SizeF(TypeLabel.Frame.Width, 0));
			var typePos = new PointF(TypeLabel.Frame.X, SubtitleLabel.Frame.Bottom);

			TypeLabel.Frame = new RectangleF(typePos, typeSize);

			ScrollView.ContentSize = new SizeF(320, ScrollView.Subviews.Last().Frame.Bottom + padding);
		}
    }
}

