using System.Collections.Generic;
using System.Threading.Tasks;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess.Interfaces
{
    public interface IMessagesRepository
    {
        Task Enqueue(Message input);

        Task<List<Message>> GetAllMessages(int userId);

        Task<List<Message>> GetAllMessages(string queue, int userId);

        Task<Message> GetFirstMessage(string queue, int userId);

        Task RemoveAllMessages(string queue, int userId);

        Task RemoveMessage(int id);
    }
}