using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross;
using Solvberget.Core;
using Cirrious.CrossCore;
using Solvberget.Core.ViewModels;
using System.Diagnostics;
using System;

namespace Solvberget.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			return new App();
		}

		protected override IMvxTrace CreateDebugTrace() { return new MyDebugTrace(); }
	}

	public class MyDebugTrace : IMvxTrace
	{
		public void Trace(MvxTraceLevel level, string tag, Func<string> message)
		{
			Console.WriteLine(tag + ":" + level + ":" + message());
		}

		public void Trace(MvxTraceLevel level, string tag, string message)
		{
			Console.WriteLine(tag + ":" + level + ":" + message);
		}

		public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
		{
			try
			{
				Console.WriteLine(string.Format(tag + ":" + level + ":" + message, args));
			}
			catch (FormatException)
			{
				Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
			}
		}
	}
}