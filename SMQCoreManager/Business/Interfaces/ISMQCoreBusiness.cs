using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;

namespace SMQCoreManager.Business.Interfaces
{
    public interface ISMQCoreBusiness
    {
        public UserDto LoggedUser { get; }

        Task<bool> AddApp(AppDto user);

        Task<bool> AddUser(UserDto user);

        Task<bool> ChangePassword(string oldPassword, string newPassword);

        Task<bool> ClearQueue(string queue);

        Task<string> Dequeue(string queue);

        Task<List<string>> DequeueAll(string queue);

        Task<bool> Enqueue(string queue, string message);

        Task<AppDto> GetApp(int appId);

        Task<List<AppDto>> GetApps();

        Task<UserDto> GetUser(int userId);

        Task<List<UserDto>> GetUsers();

        Task<bool> LoginAsync(string login, string password);

        Task<bool> RemoveApp(int appId);

        Task<bool> RemoveUser(int userId);

        Task<bool> UpdateApp(AppDto app);

        Task<bool> UpdateUser(UserDto user);

        Task<List<MessageDto>> ViewAllQueues();

        Task<List<MessageDto>> ViewQueue(string queue);
    }
}