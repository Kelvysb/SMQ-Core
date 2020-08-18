using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;

namespace SMQCoreManager.Services.Interfaces
{
    public interface IAppsService
    {
        Task<bool> Add(AppDto user, string token);

        Task<AppDto> Get(int userId, string token);

        Task<List<AppDto>> Get(string token);

        Task<bool> Remove(int userId, string token);

        Task<bool> Update(AppDto user, string token);
    }
}