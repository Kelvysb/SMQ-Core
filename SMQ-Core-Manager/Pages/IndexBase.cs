using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SMQCoreManager.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private Task<Settings> getsettingsTask { get; set; }

        private Settings settings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            settings = await getsettingsTask;
            await base.OnInitializedAsync();
        }
    }
}