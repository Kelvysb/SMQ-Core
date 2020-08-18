using System.Collections.Generic;

namespace SMQCore.Shared.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public List<UserPermission> Permissions { get; set; }

        public App App { get; set; }
    }
}