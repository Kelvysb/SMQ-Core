using System.Collections.Generic;

namespace SMQCore.Shared.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string NewPasswordHash { get; set; }

        public string Token { get; set; }

        public List<string> Permissions { get; set; }
    }
}