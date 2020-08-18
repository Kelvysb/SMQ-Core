using System.Collections.Generic;

namespace SMQCore.Shared.Models.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        public int Level { get; set; }

        public string Value { get; set; }

        public bool Enabled { get; set; }

        public List<UserPermission> Users { get; set; }
    }
}