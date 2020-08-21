using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SMQCore.DataAccess;
using SMQCore.Helpers;
using SMQCore.Models.Entities;

namespace SMQCore.Tests.Helpers
{
    internal static class UserRepositoryMock
    {
        private static Mock<IUsersRepository> userRepository;
        private static List<User> data;

        public static IUsersRepository Build()
        {
            userRepository = new Mock<IUsersRepository>();

            data = new List<User>()
            {
                new User() {
                    Id = 1,
                    AppId = 1,
                    Login = "User1",
                    PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                    App = new App() {Id =  1, Description = "App1", IsMain = true},
                    Permissions = new List<UserPermission>()
                    {
                       new UserPermission() {UserId = 1, PermissionId = 1, Permission = new Permission() { Id = 1, Value = Permissions.SuperUser}},
                       new UserPermission() {UserId = 1, PermissionId = 2, Permission = new Permission() { Id = 2, Value = Permissions.AppAdmin}},
                       new UserPermission() {UserId = 1, PermissionId = 3, Permission = new Permission() { Id = 3, Value = Permissions.User}}
                    }
                },
                new User() {
                    Id = 2,
                    AppId = 2,
                    Login = "User2",
                    PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                    App = new App() {Id =  2, Description = "App2", IsMain = true},
                    Permissions = new List<UserPermission>()
                    {
                       new UserPermission() {UserId = 2, PermissionId = 3, Permission = new Permission() { Id = 3, Value = Permissions.User}}
                    }
                },
                 new User() {
                    Id = 3,
                    AppId = 2,
                    Login = "User3",
                    PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                    App = new App() {Id =  2, Description = "App2", IsMain = true},
                    Permissions = new List<UserPermission>()
                    {
                       new UserPermission() {UserId = 3, PermissionId = 2, Permission = new Permission() { Id = 2, Value = Permissions.AppAdmin}},
                       new UserPermission() {UserId = 3, PermissionId = 3, Permission = new Permission() { Id = 3, Value = Permissions.User}}
                    }
                }
            };

            userRepository
                .Setup(r => r.GetUser(It.IsAny<int>()))
                .ReturnsAsync((int id) => data.Where(d => d.Id == id).FirstOrDefault());

            userRepository
                .Setup(r => r.GetUserByLogin(It.IsAny<string>()))
                .ReturnsAsync((string login) => data.Where(d => d.Login.Equals(login, System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault());

            userRepository
                .Setup(r => r.GetAllUsers(It.IsAny<int>()))
                .ReturnsAsync(() => data);

            userRepository
                .Setup(r => r.AddUser(It.IsAny<User>()))
                .Returns((User user) => Task.Run(() => data.Add(user)));

            userRepository
                .Setup(r => r.UpdateUser(It.IsAny<User>()))
                .Returns((User user) => Task.Run(() => data[data.FindIndex(u => u.Id == user.Id)] = user));

            userRepository
                .Setup(r => r.UpdateUser(It.IsAny<User>()))
                .Returns((User user) => Task.Run(() => data[data.FindIndex(u => u.Id == user.Id)] = user));

            userRepository
                .Setup(r => r.RemoveUser(It.IsAny<User>()))
                .Returns((User user) => Task.Run(() => data.RemoveAt(data.FindIndex(u => u.Id == user.Id))));

            return userRepository.Object;
        }
    }
}