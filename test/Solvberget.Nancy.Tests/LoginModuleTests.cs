using FakeItEasy;
using Nancy;
using Should;
using Solvberget.Domain.Aleph;
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
        private readonly IRepository _repository;

        public LoginModuleTests()
        {
            _provider = A.Fake<IAuthenticationProvider>();
            _repository = A.Fake<IRepository>();

            _browser = new Browser(config =>
            {
                config.Module<LoginModule>();
                config.Dependency(_provider);
                config.Dependency(_repository);
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
            A.CallTo(() => _provider.Authenticate("username", "correct password")).Returns(new AlephUserIdentity("username", "correct password", null));

            var response = _browser.Post("/login", with =>
            {
                with.FormValue("username", "username");
                with.FormValue("password", "correct password");
                with.Accept("application/json");
                with.HttpRequest();
            });

            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }


        [Fact]
        public void GetPinShouldRequestNewPinFromRepository()
        {
            // Given
            A.CallTo(() => _repository.RequestPinCodeToSms("1234")).Returns(new RequestReply
            {
                Success = true,
                Reply = "Great success!"
            });

            // When
            var response = _browser.Get("/login/forgot/1234", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<RequestReply>().Reply.ShouldEqual("Great success!");
        }
    }
}