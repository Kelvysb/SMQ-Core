using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business.Interfaces
{
    public interface IUsersBusiness
    {
        Task AddUser(UserDto userDto, User loggedUser);

        Task<List<UserDto>> GetAllUsers(User loggedUser);

        Task<UserDto> GetUser(int userId);

        Task<UserDto> Login(UserDto login);

        Task RemoveUser(int userId);

        Task UpdateUser(UserDto userDto, User loggedUser);

        Task ChangePassword(UserDto userDto, User loggedUser);
    }
}