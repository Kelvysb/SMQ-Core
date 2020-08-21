using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess.Interfaces
{
    public interface ISMQContext
    {
        DbSet<App> Apps { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Permission> Permissions { get; set; }

        DbSet<UserPermission> UserPermissions { get; set; }

        Task<int> SaveChangesAsync();
    }
}