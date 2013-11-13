using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services;
using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class AuthenticationTest
    {


        [Fact]
        public async Task Should_be_able_to_post_formdata()
        {
            IStringDownloader downloader = new HttpBodyDownloader(new UserAuthenticationDataService());

            var data = new Dictionary<string, string>
            {
                {"Username", "164916"},
                {"Password", "9236"}
            };

            var result = await downloader.PostForm("http://localhost:39465/login", data);
            Console.WriteLine(result);
        }
    }
}
