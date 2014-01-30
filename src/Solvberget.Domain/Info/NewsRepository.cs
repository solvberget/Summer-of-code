using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Solvberget.Domain.Info
{
    public class NewsRepository : INewsRepository
    {

        public List<NewsItem> GetNewsItems(int? limitCount)
        {

            var newsItems = new List<NewsItem>();
            var reader = XmlReader.Create(Properties.Settings.Default.NewsRssUrl);
            var feed = SyndicationFeed.Load(reader);

            if (feed != null)
            {
                newsItems.AddRange(feed.Items.Select(MapItem));

                return limitCount != null ? newsItems.Take((int)limitCount).ToList() : newsItems;

            }

            return new List<NewsItem>();

        }

        private NewsItem MapItem(SyndicationItem item)
        {
            var eventImage = item.Links.FirstOrDefault(l => l.RelationshipType.Equals("enclosure"));

            return new NewsItem
            {
                Title = item.Title.Text, PublishedDateAsDateTime = item.PublishDate, Link = item.Links.First().Uri, DescriptionUnescaped = item.Summary.Text,
                EnclosureUrl = null == eventImage ? null : eventImage.Uri.OriginalString
            };
        }
    }
}
