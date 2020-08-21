using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        protected ISMQContext context;

        public UsersRepository(ISMQContext context)
        {
            this.context = context;
        }

        public Task<User> GetUserByLogin(string login)
        {
            return context.Users
                .AsNoTracking()
                .Include(u => u.Permissions)
                .ThenInclude(p => p.Permission)
                .Where(u => u.Login.ToUpper().Equals(login.ToUpper()))
                .OrderBy(m => m.Id)
                .FirstOrDefaultAsync();
        }

        public Task<List<User>> GetAllUsers(int userId)
        {
            return context.Users
                .AsNoTracking()
                .Include(u => u.Permissions)
                .ThenInclude(p => p.Permission)
                .Where(u => u.App.Users.Any(a => a.Id == userId))
                .ToListAsync();
        }

        public Task<User> GetUser(int userId)
        {
            return context.Users
                .AsNoTracking()
                .Include(u => u.App)
                .Include(u => u.Permissions)
                .ThenInclude(p => p.Permission)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public Task AddUser(User user)
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task UpdateUser(User user)
        {
            context.Users.Update(user);
            return context.SaveChangesAsync();
        }

        public Task RemoveUser(User user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public Task<List<Permission>> RetrievePermissions()
        {
            return context.Permissions.ToListAsync();
        }

        public Task ClearPermissions(int userId)
        {
            context.UserPermissions
                .RemoveRange(context.UserPermissions
                .Where(p => p.UserId == userId));
            return context.SaveChangesAsync();
        }
    }
}