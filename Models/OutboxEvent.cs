using System;
using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.Models
{
    public class OutboxEvent
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } = string.Empty; // e.g. class.opened

        [Required]
        public string Payload { get; set; } = string.Empty; // JSON serialized string

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
