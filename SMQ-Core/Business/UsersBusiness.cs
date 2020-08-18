using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SMQCore.Business.Interfaces;
using SMQCore.DataAccess;
using SMQCore.Helpers;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business
{
    public class UsersBusiness : IUsersBusiness
    {
        private IUsersRepository usersRepository;
        private IConfiguration configuration;

        public UsersBusiness(IUsersRepository usersRepository,
                                IConfiguration configuration)
        {
            this.usersRepository = usersRepository;
            this.configuration = configuration;
        }

        public async Task AddUser(UserDto userDto, User loggedUser)
        {
            User user = UserDtoToUser(userDto);
            user.Id = 0;
            user = await RetrievePermisions(user);
            user.Permissions = FilterPermissions(user.Permissions, loggedUser.Permissions.Max(p => p.Permission.Level));
            await usersRepository.AddUser(user);
        }

        public async Task<List<UserDto>> GetAllUsers(User loggedUser)
        {
            var result = await usersRepository.GetAllUsers(loggedUser.Id);
            return result.Select(u => UserToUserDto(u)).ToList();
        }

        public async Task<UserDto> GetUser(int userId)
        {
            var result = await usersRepository.GetUser(userId);
            return UserToUserDto(result);
        }

        public async Task RemoveUser(UserDto user)
        {
            await usersRepository.RemoveUser(UserDtoToUser(user));
        }

        public async Task UpdateUser(UserDto userDto, User loggedUser)
        {
            User user = UserDtoToUser(userDto);
            User current = await usersRepository.GetUser(user.Id);
            if (current != null)
            {
                user.PasswordHash = current.PasswordHash;
                user.Login = current.Login;
                user.AppId = current.AppId;
                await RetrievePermisions(user);
                user.Permissions = FilterPermissions(user.Permissions, loggedUser.Permissions.Max(p => p.Permission.Level));
                await usersRepository.ClearPermissions(user.Id);
                await usersRepository.UpdateUser(user);
            }
            else
            {
                throw new KeyNotFoundException(user.Id.ToString());
            }
        }

        private List<UserPermission> FilterPermissions(List<UserPermission> permissions, int maxLevel)
        {
            permissions.RemoveAll(p => p.Permission.Level > maxLevel);
            return permissions;
        }

        public async Task<UserDto> Login(UserDto login)
        {
            User user = await usersRepository.GetUserByLogin(login.Login);
            if (user != null && user.PasswordHash.Equals(login.PasswordHash, StringComparison.InvariantCultureIgnoreCase))
            {
                UserDto result = UserToUserDto(user);
                result.Token = AuthHelper.GetToken(user.Id.ToString(),
                    configuration.GetSection("Auth").GetValue<string>("Secret"),
                    configuration.GetSection("Auth").GetValue<string>("Issuer"));
                return result;
            }
            else
            {
                throw new UnauthorizedAccessException(login.Login);
            }
        }

        public async Task<User> RetrievePermisions(User user)
        {
            List<Permission> permissions = await usersRepository.RetrievePermissions();

            if (user.Permissions.Any())
            {
                foreach (var permission in user.Permissions)
                {
                    permission.Permission = permissions.Find(p =>
                        p.Value.Equals(permission.Permission.Value, StringComparison.InvariantCultureIgnoreCase));
                    if (permission.Permission != null)
                    {
                        permission.PermissionId = permission.Permission.Id;
                    }
                }
            }
            else
            {
                user.Permissions.Add(new UserPermission()
                {
                    UserId = user.Id,
                    PermissionId = permissions.Find(p => p.Value.Equals(Permissions.User)).Id
                });
            }

            return user;
        }

        public async Task ChangePassword(UserDto userDto, User loggedUser)
        {
            User current = await usersRepository.GetUser(userDto.Id);

            if (current != null && current.PasswordHash.Equals(userDto.PasswordHash, StringComparison.InvariantCultureIgnoreCase))
            {
                if ((current.Permissions.Max(p => p.Permission.Level) < loggedUser.Permissions.Max(p => p.Permission.Level)) ||
                    current.Id == loggedUser.Id)
                {
                    current.PasswordHash = userDto.NewPasswordHash;
                    await usersRepository.UpdateUser(current);
                }
                else
                {
                    throw new AccessViolationException(userDto.Login);
                }
            }
            else
            {
                throw new AccessViolationException(userDto.Login);
            }
        }

        private User UserDtoToUser(UserDto user)
        {
            return new User
            {
                Id = user.Id,
                AppId = user.AppId,
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                Permissions = user.Permissions
                    .Select(p => new UserPermission() { Permission = new Permission() { Value = p } })
                    .ToList()
            };
        }

        private UserDto UserToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                AppId = user.AppId,
                Login = user.Login,
                PasswordHash = null,
                NewPasswordHash = null,
                Token = null,
                Permissions = user.Permissions.Select(p => p.Permission.Value).ToList()
            };
        }
    }
}