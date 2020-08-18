using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess
{
    public class MessagesRepository : IMessagesRepository
    {
        protected ISMQContext context;

        public MessagesRepository(ISMQContext context)
        {
            this.context = context;
        }

        public Task Enqueue(Message input)
        {
            context.Messages.Add(input);
            return context.SaveChangesAsync();
        }

        public Task<List<Message>> GetAllMessages(string queue, int userId)
        {
            return context.Messages
                .AsNoTracking()
                .Where(m => m.Queue.ToUpper().Equals(queue.ToUpper())
                            && m.App.Users.Any(u => u.Id == userId))
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public Task<List<Message>> GetAllMessages(int userId)
        {
            return context.Messages
                .AsNoTracking()
                .Where(m => m.App.Users.Any(u => u.Id == userId))
                .OrderBy(m => m.Queue)
                .ThenBy(m => m.Id)
                .ToListAsync();
        }

        public Task<Message> GetFirstMessage(string queue, int userId)
        {
            return context.Messages
                .AsNoTracking()
                .Where(m => m.Queue.ToUpper().Equals(queue.ToUpper())
                            && m.App.Users.Any(u => u.Id == userId))
                .OrderBy(m => m.Id)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveAllMessages(string queue, int userId)
        {
            context.Messages
                .RemoveRange(context.Messages
                    .Where(m => m.Queue.ToUpper().Equals(queue.ToUpper())));
            await context.SaveChangesAsync();
        }

        public async Task RemoveMessage(int id)
        {
            var message = await context.Messages.Where(m => m.Id == id).FirstOrDefaultAsync();
            context.Messages.Remove(message);
            await context.SaveChangesAsync();
        }
    }
}