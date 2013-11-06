using FakeItEasy;

using Nancy.Testing;

using Should;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Users;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class UserModuleTests
    {
        private readonly IRepository _repository;

        private readonly Browser _browser;

        public UserModuleTests()
        {
            _repository = A.Fake<IRepository>();
            _browser = new Browser(config =>
            {
                config.Module<UserModule>();
                config.Dependency(_repository);
            });
        }

        [Fact]
        public void GetInfoShouldFetchUserInformationFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetUserInformation("1234", "verification")).Returns(new UserInfo
            {
                Name = "Chuck Norris",
                HomeLibrary = "Sølvberget KF"
            });

            // When
            var response = _browser.Get("/user/1234/info", with =>
            {
                with.Query("verify", "verification");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<UserInfoDto>().Name.ShouldEqual("Chuck Norris");
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
            var response = _browser.Get("/user/1234/pin", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();       
            });

            // Then
            response.Body.DeserializeJson<RequestReply>().Reply.ShouldEqual("Great success!");
        }
    }
}