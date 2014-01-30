using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using FakeItEasy;

using Nancy.Testing;

using Should;

using Solvberget.Core.DTOs;
using Solvberget.Domain.Info;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class InformationModuleTests
    {
        private readonly IInformationRepository _repository;

        private readonly Browser _browser;

        public InformationModuleTests()
        {
            _repository = A.Fake<IInformationRepository>();
            _browser = new Browser(config =>
            {
                config.Module<InformationModule>();
                config.Dependency(_repository);
            });
        }

        [Fact]
        public void GetContactInfoShouldFetchContactInfoFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetContactInformation()).Returns(new List<ContactInformation>
            {
                new ContactInformation { Title = "CEO" },
                new ContactInformation { Title = "CTO" }
            });

            // When
            var response = _browser.Get("/info/contact", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<ContactInfoDto>>().Count().ShouldEqual(2);
        }

        [Fact]
        public void GetOpeningHoursShouldFetchOpeningHoursFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetOpeningHoursInformation()).Returns(new List<OpeningHoursInformation>
            {
                new OpeningHoursInformation { Title = "Monday - Friday" },
                new OpeningHoursInformation { Title = "Saturday - Sunday" }
            });

            // When
            var response = _browser.Get("/info/opening-hours", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<OpeningHoursInformation>>().Count.ShouldEqual(2);
        }
    }
}