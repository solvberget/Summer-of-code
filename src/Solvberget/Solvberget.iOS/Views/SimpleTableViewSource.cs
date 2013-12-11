using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using SlidingPanels.Lib;
using System.Collections.Generic;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Threading;

namespace Solvberget.iOS
{

	public class SimpleTableViewSource<T> : MvxTableViewSource where T : class
	{

		Action<ISimpleCell, T> _binder;

		Task DownloadTask { get; set; }

		public SimpleTableViewSource(UITableView tableView, Action<ISimpleCell, T> binder) : base(tableView)
		{
			_binder = binder;
		
			tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;


			tableView.RegisterNibForCellReuse(UINib.FromName(SimpleCell.Key, NSBundle.MainBundle), SimpleCell.Key);

			DownloadTask = Task.Factory.StartNew (() => { });

		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 95;
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			var key = GetNibNameForVM(item);

			var cell = TableView.DequeueReusableCell(key, indexPath) as ISimpleCell;
			cell.TableWidth = tableView.Bounds.Width;

			cell.SetImage(null);
			_binder(cell, item as T);

			if (null != cell.ImageUrl)
			{
				UIImage cachedImage;
				Cache.Images.TryGetValue(cell.ImageUrl, out cachedImage);

				if (null != cachedImage) cell.SetImage(cachedImage);
				else  BeginDownloadingImage(cell.ImageUrl);
			}

			return cell as UITableViewCell;
		}

		NSString GetNibNameForVM(object item)
		{
			return SimpleCell.Key;
		}

		void BeginDownloadingImage (string imageUrl)
		{
			// Queue the image to be downloaded. This task will execute
			// as soon as the existing ones have finished.
			byte[] data = null;
			DownloadTask = DownloadTask.ContinueWith (prevTask => {
				try {
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					using (var c = new WebClient ())
						data = c.DownloadData (imageUrl);
				} finally {
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				}
			});

			// When the download task is finished, queue another task to update the UI.
			// Note that this task will run only if the download is successful and it
			// uses the CurrentSyncronisationContext, which on MonoTouch causes the task
			// to be run on the main UI thread. This allows us to safely access the UI.
			DownloadTask = DownloadTask.ContinueWith (t => {
				// Load the image from the byte array.
				var tag = imageUrl;
				var image = UIImage.LoadFromData (NSData.FromArray (data));

				Cache.Images.Add(tag, image);

				// Retrieve the cell which corresponds to the current entry. If the cell is null, it means the user
				// has already scrolled that entry off-screen.
				var cell = TableView.VisibleCells.Where (c => ((ISimpleCell)c).ImageUrl == tag).FirstOrDefault () as ISimpleCell;
				if (cell != null)
					cell.SetImage(image);
			}, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext ());
		}
	}
	
}
