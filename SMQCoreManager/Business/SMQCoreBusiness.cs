using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCoreManager.Business.Interfaces;
using SMQCoreManager.Services.Interfaces;

namespace SMQCoreManager.Business
{
    public class SMQCoreBusiness : ISMQCoreBusiness
    {
        private readonly IUsersService usersService;
        private readonly IQueueService queueService;
        private readonly IAppsService appsService;

        public SMQCoreBusiness(IUsersService usersService,
            IQueueService queueService,
            IAppsService appsService)
        {
            this.usersService = usersService;
            this.queueService = queueService;
            this.appsService = appsService;
        }

        public UserDto LoggedUser { get; private set; } = null;

        #region User

        public async Task<bool> LoginAsync(string login, string password)
        {
            bool result;
            var user = new UserDto()
            {
                Login = login,
                PasswordHash = GetMd5Hash(password)
            };

            LoggedUser = null;
            LoggedUser = await usersService.Login(user);
            result = true;

            return result;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            bool result;

            var user = new UserDto()
            {
                PasswordHash = GetMd5Hash(oldPassword),
                NewPasswordHash = GetMd5Hash(newPassword)
            };

            LoggedUser = await usersService.ChangePassword(user, LoggedUser?.Token);
            result = true;

            return result;
        }

        public async Task<UserDto> GetUser(int userId)
        {
            return await usersService.Get(userId, LoggedUser?.Token);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await usersService.Get(LoggedUser?.Token);
        }

        public async Task<bool> AddUser(UserDto user)
        {
            return await usersService.Add(user, LoggedUser?.Token);
        }

        public async Task<bool> UpdateUser(UserDto user)
        {
            return await usersService.Update(user, LoggedUser?.Token);
        }

        public async Task<bool> RemoveUser(int userId)
        {
            return await usersService.Remove(userId, LoggedUser?.Token);
        }

        #endregion User

        #region Apps

        public async Task<AppDto> GetApp(int appId)
        {
            return await appsService.Get(appId, LoggedUser?.Token);
        }

        public async Task<List<AppDto>> GetApps()
        {
            return await appsService.Get(LoggedUser?.Token);
        }

        public async Task<bool> AddApp(AppDto user)
        {
            return await appsService.Add(user, LoggedUser?.Token);
        }

        public async Task<bool> UpdateApp(AppDto app)
        {
            return await appsService.Update(app, LoggedUser?.Token);
        }

        public async Task<bool> RemoveApp(int appId)
        {
            return await appsService.Remove(appId, LoggedUser?.Token);
        }

        #endregion Apps

        #region Queue

        public async Task<bool> Enqueue(string queue, string message)
        {
            return await queueService.Enqueue(queue, message, LoggedUser?.Token);
        }

        public async Task<string> Dequeue(string queue)
        {
            return await queueService.Dequeue(queue, LoggedUser?.Token);
        }

        public async Task<List<string>> DequeueAll(string queue)
        {
            return await queueService.DequeueAll(queue, LoggedUser?.Token);
        }

        public async Task<List<MessageDto>> ViewQueue(string queue)
        {
            return await queueService.View(queue, LoggedUser?.Token);
        }

        public async Task<List<MessageDto>> ViewAllQueues()
        {
            return await queueService.ViewAll(LoggedUser?.Token);
        }

        public async Task<bool> ClearQueue(string queue)
        {
            return await queueService.Remove(queue, LoggedUser?.Token);
        }

        #endregion Queue

        private string GetMd5Hash(string input)
        {
            StringBuilder sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }
    }
}