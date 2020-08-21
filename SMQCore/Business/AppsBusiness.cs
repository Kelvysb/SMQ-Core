using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMQCore.Business.Interfaces;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business
{
    public class AppsBusiness : IAppsBusiness
    {
        private IAppsRepository appsRepository;

        public AppsBusiness(IAppsRepository appsRepository)
        {
            this.appsRepository = appsRepository;
        }

        public async Task AddApp(AppDto appDto)
        {
            var app = AppDtoToApp(appDto);
            app.Id = 0;
            app.IsMain = false;
            await appsRepository.AddApp(app);
        }

        public async Task<List<AppDto>> GetAllApps()
        {
            var result = await appsRepository.GetAllApps();
            return result.Select(a => AppToAppDto(a)).ToList();
        }

        public async Task<AppDto> GetApp(int appId)
        {
            var result = await appsRepository.GetApp(appId);
            return AppToAppDto(result);
        }

        public async Task RemoveApp(int appId)
        {
            var app = await appsRepository.GetApp(appId);
            if (app != null)
            {
                if (!app.IsMain)
                {
                    await appsRepository.RemoveApp(app);
                }
                else
                {
                    throw new AccessViolationException();
                }
            }
            else
            {
                throw new KeyNotFoundException(appId.ToString());
            }
        }

        public async Task UpdateApp(AppDto appDto)
        {
            var current = await appsRepository.GetApp(appDto.Id);
            if (current != null)
            {
                var app = AppDtoToApp(appDto);
                current.Key = app.Key;
                current.Secret = app.Secret;
                current.Description = app.Description;
                await appsRepository.UpdateApp(current);
            }
            else
            {
                throw new KeyNotFoundException(appDto.Id.ToString());
            }
        }

        private App AppDtoToApp(AppDto app)
        {
            return new App()
            {
                Id = app.Id,
                Key = app.Key,
                Secret = app.Secret,
                Description = app.Description
            };
        }

        private AppDto AppToAppDto(App app)
        {
            return new AppDto()
            {
                Id = app.Id,
                Key = app.Key,
                Description = app.Description
            };
        }
    }
}