using System;
using Cirrious.CrossCore.Plugins;

namespace Solvberget.iOS
{
	public class FilePluginBootstrap  : MvxLoaderPluginBootstrapAction<Cirrious.MvvmCross.Plugins.File.PluginLoader, Cirrious.MvvmCross.Plugins.File.Touch.Plugin>
    {
		public FilePluginBootstrap()
        {
        }
    }
}

