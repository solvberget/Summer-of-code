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

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
		}

		LoadingOverlay _loadingOverlay = new LoadingOverlay();

		BoxRenderer _boxes;


		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			_boxes = new BoxRenderer(ScrollView);

			RatingSourceLabel.Text = HeaderLabel.Text = SubtitleLabel.Text = TypeLabel.Text = String.Empty;

			Style();

			Add(_loadingOverlay);

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));
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

        private void UpdateFavoriteButtonState()
        {
			if (null == NavigationItem.RightBarButtonItem)
			{
				var image = UIImage.FromBundle("/Images/star.on.png").Scale(new SizeF(26,26));
				NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(image, UIBarButtonItemStyle.Plain, OnToggleFavorite), false);
			}

			NavigationItem.RightBarButtonItem.TintColor = ViewModel.IsFavorite ? Application.ThemeColors.FavoriteColor : Application.ThemeColors.MainInverse;
      }

        private void OnToggleFavorite(object sender, EventArgs e)
        {
			Add(_loadingOverlay);
			InvokeInBackground(ToggleFavorite);
        }

		private void ToggleFavorite()
		{
			if (ViewModel.IsFavorite) ViewModel.RemoveFavorite();
			else ViewModel.AddFavorite();

			InvokeOnMainThread(() => {
				UpdateFavoriteButtonState();
				_loadingOverlay.Hide();
			});
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

			_loadingOverlay.Hide();
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

				//reserve.TouchUpInside += (s,e) => ??
				reserve.SetTitle(availability.ButtonText, UIControlState.Normal);

				reserve.SetTitleColor(Application.ThemeColors.Main2, UIControlState.Highlighted);
				reserve.SetTitleColor(Application.ThemeColors.Main, UIControlState.Normal);
				reserve.Font = Application.ThemeColors.ButtonFont;

				reserve.Frame = new RectangleF(new PointF(padding, box.Subviews.Last().Frame.Bottom+padding), reserve.SizeThatFits(SizeF.Empty));

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

			if(!String.IsNullOrEmpty(dto.AgeLimit)) new LabelAndValue(facts, "Aldersgrense", dto.AgeLimit);
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
			var subtitlePos = new PointF(SubtitleLabel.Frame.X, HeaderLabel.Frame.Bottom+padding);

			SubtitleLabel.Frame = new RectangleF(subtitlePos, subtitleSize);

			var typeSize = TypeLabel.SizeThatFits(new SizeF(TypeLabel.Frame.Width, 0));
			var typePos = new PointF(TypeLabel.Frame.X, SubtitleLabel.Frame.Bottom+padding);

			TypeLabel.Frame = new RectangleF(typePos, typeSize);

			ScrollView.ContentSize = new SizeF(320, ScrollView.Subviews.Last().Frame.Bottom + padding);
		}
    }
}

