using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCoreManager.Services.Interfaces;

namespace SMQCoreManager.Services
{
    public class QueueService : ServicesBase, IQueueService
    {
        public QueueService(Task<Settings> getsettingsTask, HttpClient httpClient) :
            base(getsettingsTask, httpClient)
        {
        }

        public async Task<bool> Enqueue(string queue, string message, string token)
        {
            return await Post($"{settings.Api}/Queue/{queue}", message, token);
        }

        public async Task<string> Dequeue(string queue, string token)
        {
            return await Get<string>($"{settings.Api}/Queue/{queue}", token);
        }

        public async Task<List<string>> DequeueAll(string queue, string token)
        {
            return await Get<List<string>>($"{settings.Api}/Queue/all/{queue}", token);
        }

        public async Task<List<MessageDto>> View(string queue, string token)
        {
            return await Get<List<MessageDto>>($"{settings.Api}/Queue/view/{queue}", token);
        }

        public async Task<List<MessageDto>> ViewAll(string token)
        {
            return await Get<List<MessageDto>>($"{settings.Api}/Queue/view", token);
        }

        public async Task<bool> Remove(string queue, string token)
        {
            return await Delete($"{settings.Api}/Queue/{queue}", token);
        }
    }
}