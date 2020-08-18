using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess
{
    public class AppsRepository : IAppsRepository
    {
        protected ISMQContext context;

        public AppsRepository(ISMQContext context)
        {
            this.context = context;
        }

        public Task<List<App>> GetAllApps()
        {
            return context.Apps
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<App> GetApp(int appId)
        {
            return context.Apps
                .AsNoTracking()
                .Where(a => a.Id == appId)
                .FirstOrDefaultAsync();
        }

        public Task<App> GetAppByUserId(int userId)
        {
            return context.Apps
                .AsNoTracking()
                .Where(a => a.Users.Any(u => u.Id == userId))
                .FirstOrDefaultAsync();
        }

        public Task AddApp(App app)
        {
            context.Apps.Add(app);
            return context.SaveChangesAsync();
        }

        public Task UpdateApp(App app)
        {
            context.Apps.Update(app);
            return context.SaveChangesAsync();
        }

        public Task RemoveApp(App app)
        {
            context.Apps.Remove(app);
            return context.SaveChangesAsync();
        }
    }
}