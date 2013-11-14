using FakeItEasy;
using Nancy.Authentication.Stateless;
using Nancy.Testing;

using Should;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Users;
using Solvberget.Nancy.Authentication;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class UserModuleTests
    {
        private readonly IRepository _repository;
        private IAuthenticationProvider _provider;

        private readonly Browser _browser;

        public UserModuleTests()
        {
            _provider = A.Fake<IAuthenticationProvider>();
            _repository = A.Fake<IRepository>();

            _browser = new Browser(config =>
            {
                config.Module<UserModule>();
                config.Dependency(_repository);
                config.Dependency(_provider);
                
                config.ApplicationStartup((ioc, pipelines) =>
                {
                    var statelessAuthConfiguration = new StatelessAuthenticationConfiguration(ctx => ioc.Resolve<NancyContextAuthenticator>().Authenticate(ctx));
                    StatelessAuthentication.Enable(pipelines, statelessAuthConfiguration);
                });
            });

        }

        [Fact]
        public void GetInfoShouldFetchUserInformationFromRepository()
        {

            A.CallTo(() => _provider.Authenticate("1234", "verification"))
                .Returns(new AlephUserIdentity("1234", "verification", new UserInfo
                {
                    Name = "Chuck Norris",
                    HomeLibrary = "Sølvberget KF"
                }));

            // When
            var response = _browser.Get("/user/info", with =>
            {
                with.Header("Authorization", "1234:verification");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<UserInfoDto>().Name.ShouldEqual("Chuck Norris");
        }
    }
}