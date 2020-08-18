using System;

namespace SMQCore.Shared.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        public string Sender { get; set; }

        public string Queue { get; set; }

        public DateTime DateTime { get; set; }

        public string Payload { get; set; }

        public App App { get; set; }
    }
}