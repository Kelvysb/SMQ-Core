using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess.Interfaces
{
    public interface IAppsRepository
    {
        Task AddApp(App app);

        Task<List<App>> GetAllApps();

        Task<App> GetApp(int appId);

        Task<App> GetAppByUserId(int userId);

        Task RemoveApp(App app);

        Task UpdateApp(App app);
    }
}