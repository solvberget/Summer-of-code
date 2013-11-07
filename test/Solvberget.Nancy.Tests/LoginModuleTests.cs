using FakeItEasy;
using Nancy;
using Should;
using Solvberget.Nancy.Authentication;
using Solvberget.Nancy.Modules;

using Nancy.Testing;
using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class LoginModuleTests
    {
        private readonly Browser _browser;
        private readonly IAuthenticationProvider _provider;

        public LoginModuleTests()
        {
            _provider = A.Fake<IAuthenticationProvider>();

            _browser = new Browser(config =>
            {
                config.Module<LoginModule>();
                config.Dependency(_provider);
            });
        }
        
        [Fact]
        public void Should_fail_when_no_credentials()
        {
            A.CallTo(() => _provider.Authenticate(null,null)).Returns(null);

            var response = _browser.Post("/login", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public void Should_fail_when_incorrect_credentials()
        {
            A.CallTo(() => _provider.Authenticate("username", "incorrect password")).Returns(null);

            var response = _browser.Post("/login", with =>
            {
                with.FormValue("username", "username");
                with.FormValue("password", "incorrect password");
                with.Accept("application/json");
                with.HttpRequest();
            });

            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Should_succeed_when_correct_crredentials()
        {
            A.CallTo(() => _provider.Authenticate("username", "correct password")).Returns(new UserIdentity("username"));

            var response = _browser.Post("/login", with =>
            {
                with.FormValue("username", "username");
                with.FormValue("password", "correct password");
                with.Accept("application/json");
                with.HttpRequest();
            });

            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }
    }
}