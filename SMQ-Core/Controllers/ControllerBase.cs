using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMQCore.Business.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private IPermissionCheck permissionCheck;

        protected ControllerBase(IPermissionCheck permissionCheck)
        {
            this.permissionCheck = permissionCheck;
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