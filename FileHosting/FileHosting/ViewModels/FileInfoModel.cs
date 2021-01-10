using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class FileInfoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name should not be empty")]
        [StringLength(100, ErrorMessage = "Max name length is 100 symbols")]
        public string Name { get; set; }

        [StringLength(30, ErrorMessage = "Max category length is 30 symbols")]
        public string Category { get; set; }

        [StringLength(200, ErrorMessage = "Max description length is 200 symbols")]
        public string Description { get; set; }
    }
}
