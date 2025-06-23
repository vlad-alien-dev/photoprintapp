using System.ComponentModel.DataAnnotations;

namespace PhotoPrintApp.Api.Models
{
    public class UploadedPhoto
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; } = null!;
        [Required]
        public string FilePath { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
