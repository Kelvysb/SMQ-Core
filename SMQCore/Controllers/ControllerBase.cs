using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMQCore.Business.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Controllers
{
    public abstract class ControllerBase<T> : Controller
    {
        private IPermissionCheck permissionCheck;
        protected ILogger<T> logger;

        protected ControllerBase(IPermissionCheck permissionCheck,
                                 ILogger<T> logger)
        {
            this.permissionCheck = permissionCheck;
            this.logger = logger;
        }

        protected Task<User> CheckPermission(params string[] permissions)
        {
            return permissionCheck.Check(GetUserId(), permissions);
        }

        protected int GetUserId()
        {
            return int.Parse(User.FindFirst("User")?.Value);
        }
    }
}