using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business.Interfaces
{
    public interface IQueueBusiness
    {
        Task Clear(string queue, User user);

        Task<MessageDto> Dequeue(string queue, User user);

        Task<List<MessageDto>> DequeueAll(string queue, User user);

        Task Enqueue(string queue, string message, User user);

        Task<List<MessageDto>> List(User user);

        Task<List<MessageDto>> List(string queue, User user);
    }
}