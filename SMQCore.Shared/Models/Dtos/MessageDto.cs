using System;

namespace SMQCore.Shared.Models.Dtos
{
    public class MessageDto
    {
        public string Queue { get; set; }

        public string Sender { get; set; }

        public DateTime DateTime { get; set; }

        public string Payload { get; set; }
    }
}