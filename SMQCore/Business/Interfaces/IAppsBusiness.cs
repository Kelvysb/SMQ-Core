using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;

namespace SMQCore.Business.Interfaces
{
    public interface IAppsBusiness
    {
        Task AddApp(AppDto appDto);

        Task<List<AppDto>> GetAllApps();

        Task<AppDto> GetApp(int appId);

        Task RemoveApp(int appId);

        Task UpdateApp(AppDto appDto);
    }
}