using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;

namespace SMQCoreManager.Services.Interfaces
{
    public interface IQueueService
    {
        Task<string> Dequeue(string queue, string token);

        Task<List<string>> DequeueAll(string queue, string token);

        Task<bool> Enqueue(string queue, string message, string token);

        Task<bool> Remove(string queue, string token);

        Task<List<MessageDto>> View(string queue, string token);

        Task<List<MessageDto>> ViewAll(string token);
    }
}