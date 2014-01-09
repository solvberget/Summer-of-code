using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using Nancy;

namespace Solvberget.Nancy.Modules
{
    public class TwitterModule : NancyModule
    {
        public TwitterModule() : base("/infoscreen/tweets")
        {
            Get["/{q}"] = args =>
            {
                string query = args.q.HasValue ? args.q : "#sølvberget";

                var auth = new SingleUserAuthorizer
                {
                    Credentials = new InMemoryCredentials
                    {
                        ConsumerKey = "a8SsPWed3GyxkFjnUh8vMA",
                        ConsumerSecret = "oN5p9jfXzVKVl107Txb2miOCebmX9pcERLssmxjM",
                        AccessToken = "4utDqnqoIBXIPi0TAtud1648QF2uC3hV5VRs6mNfYCyPT", // access token secret
                        OAuthToken = "14460916-ENyyzjPyZF4RfrtV3GK2Z44w9AE5ttLKWQs582SS1" // access token
                    }
                };

                var twitter = new TwitterContext(auth);


                var searchResponse = (from search in twitter.Search
                            where search.Type == SearchType.Search && search.Query == query
                            select search).FirstOrDefault();

                var tweets = new List<TweetDto>();

                if (searchResponse != null)
                {
                    tweets.AddRange(searchResponse.Statuses.Where(t => !t.PossiblySensitive).Select(tweet => new TweetDto
                    {
                        Text = tweet.Text, Username = tweet.User.Name, UserProfileImageUrl = tweet.User.ProfileImageUrl,
                        MediaUrl = tweet.Entities.MediaEntities.Count > 0 ? tweet.Entities.MediaEntities.First().MediaUrl + ":large" : null
                    }));

                }

                return Response.AsJson(tweets);
            };
        }
    }

    public class TweetDto
    {
        public string Username { get; set; }
        public string Text { get; set; }

        public string MediaUrl { get; set; }

        public string UserProfileImageUrl { get; set; }
    }
}