using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCoreManager.Services.Interfaces;

namespace SMQCoreManager.Services
{
    public class UsersService : ServicesBase, IUsersService
    {
        public UsersService(Task<Settings> getsettingsTask, HttpClient httpClient) :
            base(getsettingsTask, httpClient)
        {
        }

        public async Task<UserDto> Login(UserDto user)
        {
            return await Post<UserDto, UserDto>($"{settings.Api}/Users/Login", user);
        }

        public async Task<UserDto> ChangePassword(UserDto user, string token)
        {
            return await Post<UserDto, UserDto>($"{settings.Api}/Users/ChangePassword", user, token);
        }

        public async Task<List<UserDto>> Get(string token)
        {
            return await Get<List<UserDto>>($"{settings.Api}/Users", token);
        }

        public async Task<UserDto> Get(int userId, string token)
        {
            return await Get<UserDto>($"{settings.Api}/Users/{userId}", token);
        }

        public async Task<bool> Add(UserDto user, string token)
        {
            return await Post($"{settings.Api}/Users/", user, token);
        }

        public async Task<bool> Update(UserDto user, string token)
        {
            return await Put($"{settings.Api}/Users/", user, token);
        }

        public async Task<bool> Remove(int userId, string token)
        {
            return await Delete($"{settings.Api}/Users/{userId}", token);
        }
    }
}