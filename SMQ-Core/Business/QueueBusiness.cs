using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMQCore.Business.Interfaces;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Dtos;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.Business
{
    public class QueueBusiness : IQueueBusiness
    {
        private IMessagesRepository messageRepository;

        public QueueBusiness(IMessagesRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public async Task<MessageDto> Dequeue(string queue, User user)
        {
            Message result = await messageRepository.GetFirstMessage(queue, user.Id);
            if (result != null)
            {
                await messageRepository.RemoveMessage(result.Id);
            }
            return result != null ? MessageToMessageDto(result) : null;
        }

        public async Task<List<MessageDto>> DequeueAll(string queue, User user)
        {
            List<Message> result = await messageRepository.GetAllMessages(queue, user.Id);
            if (result.Any())
            {
                await messageRepository.RemoveAllMessages(queue, user.Id);
            }
            return result.Select(m => MessageToMessageDto(m)).ToList();
        }

        public async Task Enqueue(string queue, string message, User user)
        {
            Message input = new Message()
            {
                AppId = user.App.Id,
                Queue = queue,
                Sender = user.Login,
                Payload = message,
                DateTime = DateTime.Now
            };
            await messageRepository.Enqueue(input);
        }

        public async Task Clear(string queue, User user)
        {
            await messageRepository.RemoveAllMessages(queue, user.Id);
        }

        public async Task<List<MessageDto>> List(User user)
        {
            List<Message> result = await messageRepository.GetAllMessages(user.Id);
            return result.Select(m => MessageToMessageDto(m)).ToList();
        }

        public async Task<List<MessageDto>> List(string queue, User user)
        {
            List<Message> result = await messageRepository.GetAllMessages(queue, user.Id);
            return result.Select(m => MessageToMessageDto(m)).ToList();
        }

        private MessageDto MessageToMessageDto(Message input)
        {
            return new MessageDto()
            {
                Queue = input.Queue,
                Sender = input.Sender,
                Payload = input.Payload,
                DateTime = input.DateTime
            };
        }
    }
}