using System.Linq;
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
                    return GenerateAlbumFromXml(element);
                }
            }
            return null;
        }

        public static LastFmAlbum GenerateAlbumFromXml(XElement element)
        {
            var album = new LastFmAlbum();
            var name = element.Element("name");
            if (name != null) album.Name = name.Value;

            var artist = element.Element("artist");
            if (artist != null) album.Artist = artist.Value;

            var smallimage = element.Elements("image").Where(x => ((string)x.Attribute("size")).Equals("medium")).Select(x => x.Value).FirstOrDefault();
            album.SmallImageUrl = smallimage;

            var largeimage = element.Elements("image").Where(x => ((string)x.Attribute("size")).Equals("extralarge")).Select(x => x.Value).FirstOrDefault();
            album.LargeImageUrl = largeimage;

            return album;
        }
    }
}
