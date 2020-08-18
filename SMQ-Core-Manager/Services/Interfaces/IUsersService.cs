using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;

namespace SMQCoreManager.Services.Interfaces
{
    public interface IUsersService
    {
        Task<bool> Add(UserDto user, string token);

        Task<UserDto> Get(int userId, string token);

        Task<List<UserDto>> Get(string token);

        Task<UserDto> Login(UserDto user);

        Task<bool> Remove(int userId, string token);

        Task<bool> Update(UserDto user, string token);
    }
}