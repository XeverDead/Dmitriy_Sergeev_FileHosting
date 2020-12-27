namespace Common.Models
{
    public class HostingFile : IHostingEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public long AuthorId { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Link { get; set; }
    }
}
