using System.Threading.Tasks;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business.Interfaces
{
    public interface IPermissionCheck
    {
        Task<User> Check(int userId, params string[] permissions);
    }
}