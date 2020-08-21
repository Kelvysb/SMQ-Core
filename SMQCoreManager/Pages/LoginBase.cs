using Microsoft.AspNetCore.Components;
using SMQCoreManager.Business.Interfaces;

namespace SMQCoreManager.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public ISMQCoreBusiness business { get; set; }
    }
}