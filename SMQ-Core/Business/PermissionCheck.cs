using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using SMQCore.Business.Interfaces;
using SMQCore.DataAccess;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business
{
    public class PermissionCheck : IPermissionCheck
    {
        private IUsersRepository usersRepository;

        public PermissionCheck(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<User> Check(int userId, params string[] permissions)
        {
            var user = await usersRepository.GetUser(userId);

            foreach (string permission in permissions)
            {
                if (!user.Permissions.Any(p =>
                     p.Permission.Value.Equals(permission, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new AccessViolationException(user.Login);
                }
            }

            return user;
        }
    }
}