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
                newsItems.AddRange(feed.Items.Select(item => new NewsItem
                                                                 {
                                                                     Title = item.Title.Text,
                                                                     PublishedDateAsDateTime = item.PublishDate,
                                                                     Link = item.Links.First().Uri,
                                                                     DescriptionUnescaped = item.Summary.Text
                                                                 }));

                return limitCount != null ? newsItems.Take((int)limitCount).ToList() : newsItems;

            }

            return new List<NewsItem>();

        }

    }
}
