namespace SMQCore.Shared.Models.Dtos
{
    public class AppDto
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }

        public string Description { get; set; }

        public bool IsMain { get; set; }
    }
}