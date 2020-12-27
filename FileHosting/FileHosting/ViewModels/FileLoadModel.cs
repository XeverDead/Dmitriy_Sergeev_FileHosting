using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class FileLoadModel
    {
        [Required(ErrorMessage = "File must be loaded")]
        public IFormFile UploadedFile { get; set; }

        [StringLength(30, ErrorMessage = "Max category length is 30 symbols")]
        public string Category { get; set; }

        [StringLength(200, ErrorMessage = "Max description length is 200 symbols")]
        public string Description { get; set; }
    }
}
