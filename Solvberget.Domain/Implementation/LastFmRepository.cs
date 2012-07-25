using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    class LastFmRepository
    {
        public static string GetLastFmSearchQuery(Cd cd)
        {
            if ((cd.ArtistOrComposer != null || cd.MusicGroup != null) && cd.Title != null)
            {
                var trimmedTitle = cd.Title.Split(';').ToList()[0].Trim();

                var searchString = "autocorrect=1&artist=" + cd.ArtistOrGroupName + "&album=" + trimmedTitle;


                return searchString;
            }
            return string.Empty;
        }

        public static LastFmAlbum GetLastFmAlbumFromSeachQuery(string searchString)
        {
            var lastFmAlbumAsXml = RepositoryUtils.GetXmlFromStreamWithParam(Properties.Settings.Default.LastFmApiUrl, searchString);

            if (lastFmAlbumAsXml != null && lastFmAlbumAsXml.Root != null)
            {
                var element = lastFmAlbumAsXml.Root.Element("album");
                if (element != null)
                {
                    var lastFmAlbum = new LastFmAlbum();
                    lastFmAlbum.FillProperties(element);
                    return lastFmAlbum;
                }
            }
            return null;
        }
    }
}
