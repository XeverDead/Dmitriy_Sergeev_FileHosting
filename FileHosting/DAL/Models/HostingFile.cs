using LinqToDB;
using LinqToDB.Mapping;
using System;

namespace DAL.Models
{
    public class HostingFile
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ulong Size { get; set; }
        public ulong AuthorId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Link { get; set; }
    }
}
