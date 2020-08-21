using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess
{
    public interface IUsersRepository
    {
        Task AddUser(User user);

        Task<List<User>> GetAllUsers(int userId);

        Task<User> GetUser(int userId);

        Task<User> GetUserByLogin(string login);

        Task RemoveUser(User user);

        Task UpdateUser(User user);

        Task<List<Permission>> RetrievePermissions();

        Task ClearPermissions(int Id);
    }
}