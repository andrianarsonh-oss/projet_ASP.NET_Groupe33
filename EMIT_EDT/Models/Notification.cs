using System.ComponentModel.DataAnnotations;

namespace EMIT_EDT.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string DestinataireId { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Lien { get; set; }

        public bool Lu { get; set; } = false;
        public DateTime DateEnvoi { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string Canal { get; set; } = "InApp";
    }
}