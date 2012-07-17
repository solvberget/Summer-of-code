using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.DTO;

namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class UserInfoTest
    {

        [Test]
        public void PropertiesTest()
        {
            const string userId = "159222";
            var user = new UserInfo { BorrowerId = userId };
            Assert.AreEqual(userId, user.BorrowerId);
        }

    }
}
