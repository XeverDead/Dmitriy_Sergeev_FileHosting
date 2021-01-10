using BL;
using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Web.Controllers;

namespace Tests
{
    [TestFixture]
    public class HostingControllerTests
    {
        private HostingController _hostingController;

        [SetUp]
        public void Setup()
        {
            var hostingCoreMock = new Mock<IHostingCore>();
            hostingCoreMock.Setup(core => core.GetUsersByLogin("user")).Returns(GetTestUsers());

            var appEnvironmentMock = new Mock<IWebHostEnvironment>();

            _hostingController = new HostingController(hostingCoreMock.Object, appEnvironmentMock.Object);
        }

        [Test]
        public void IndexViewNotNull()
        {
            IActionResult indexView = _hostingController.Index();

            Assert.NotNull(indexView);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UserSearchViewModelWithEmptyQueryEqualNull(string query)
        {
            var usersSearchView = _hostingController.UsersSearch(query) as ViewResult;

            Assert.Null(usersSearchView.Model);
        }

        [Test]
        public void UserSearchViewModelWithProperQueryNotEqualNull()
        {
            var usersSearchView = _hostingController.UsersSearch("user") as ViewResult;

            Assert.NotNull(usersSearchView.Model);
        }

        private IEnumerable<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "1@1.1",
                    Login = "user",
                    Password = "uu",
                    Role = Common.Enums.Roles.User
                },
                new User
                {
                    Id = 2,
                    Email = "2@2.2",
                    Login = "user",
                    Password = "u",
                    Role = Common.Enums.Roles.User
                },
            };

            return users;
        }
    }
}