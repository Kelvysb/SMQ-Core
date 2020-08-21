using Microsoft.AspNetCore.Components;
using SMQCoreManager.Business.Interfaces;

namespace SMQCoreManager.Shared
{
    public class NavMenuBase : ComponentBase
    {
        [Inject]
        public ISMQCoreBusiness business { get; set; }

        private bool collapseNavMenu = true;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}