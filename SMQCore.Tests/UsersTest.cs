using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMQCore.Business;
using SMQCore.Controllers;
using SMQCore.DataAccess;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Tests.Helpers;

namespace SMQCore.Tests
{
    [TestClass]
    public class UsersTest
    {
        private UsersController usersController;

        public UsersTest()
        {
            IUsersRepository usersRepository = UserRepositoryMock.Build();
            IConfiguration configuration = ConfigurationMock.Build();

            usersController = new UsersController(
                new UsersBusiness(usersRepository, configuration),
                new PermissionCheck(usersRepository));
        }

        [TestMethod]
        public void Login()
        {
            var result = usersController.Login(new UserDto() { Login = "user1", PasswordHash = "098f6bcd4621d373cade4e832627b4f6" }).Result;
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result.Result).Value.Should().BeOfType<UserDto>();
            ((UserDto)((OkObjectResult)result.Result).Value).Login.Should().Be("User1");
            ((UserDto)((OkObjectResult)result.Result).Value).Token.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void GetAllUsers()
        {
            usersController.ControllerContext = ContextMock.Build(1);
            var result = usersController.GetAllUsers().Result;
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result.Result).Value.Should().BeOfType<List<UserDto>>();
            ((List<UserDto>)((OkObjectResult)result.Result).Value).Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void GetUserById()
        {
            usersController.ControllerContext = ContextMock.Build(1);
            var result = usersController.GetUser(1).Result;
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result.Result).Value.Should().BeOfType<UserDto>();
            ((UserDto)((OkObjectResult)result.Result).Value).Login.Should().Be("User1");
        }

        [TestMethod]
        public void GetUserByLogin()
        {
            UserDto intput = new UserDto()
            {
                AppId = 1,
                Login = "TestUser",
                PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                Permissions = new List<string>() { "User" }
            };
            usersController.ControllerContext = ContextMock.Build(1);

            var result = usersController.AddUser(intput).Result;
            result.Should().NotBeNull();
            //result.Should().BeOfType<OkObjectResult>();
            //((OkObjectResult)result.Result).Value.Should().BeOfType<UserDto>();
            //((UserDto)((OkObjectResult)result.Result).Value).Login.Should().Be("User1");
        }
    }
}