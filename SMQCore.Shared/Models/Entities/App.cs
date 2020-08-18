using System.Collections.Generic;

namespace SMQCore.Shared.Models.Entities
{
    public class App
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }

        public string Description { get; set; }

        public bool IsMain { get; set; }

        public List<Message> Messages { get; set; }

        public List<User> Users { get; set; }
    }
}