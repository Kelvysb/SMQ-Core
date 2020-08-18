using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCoreManager.Services.Interfaces;

namespace SMQCoreManager.Services
{
    public class AppsService : ServicesBase, IAppsService
    {
        public AppsService(Task<Settings> getsettingsTask, HttpClient httpClient) :
            base(getsettingsTask, httpClient)
        {
        }

        public async Task<List<AppDto>> Get(string token)
        {
            return await Get<List<AppDto>>($"{settings.Api}/Apps", token);
        }

        public async Task<AppDto> Get(int userId, string token)
        {
            return await Get<AppDto>($"{settings.Api}/Apps/{userId}", token);
        }

        public async Task<bool> Add(AppDto user, string token)
        {
            return await Post($"{settings.Api}/Apps/", user, token);
        }

        public async Task<bool> Update(AppDto user, string token)
        {
            return await Put($"{settings.Api}/Apps/", user, token);
        }

        public async Task<bool> Remove(int userId, string token)
        {
            return await Delete($"{settings.Api}/Apps/{userId}", token);
        }
    }
}