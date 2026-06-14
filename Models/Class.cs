using System;
using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.Models
{
    public class Class
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TeacherName { get; set; } = string.Empty; // Teacher in charge

        [Range(1, 1000)]
        public int MaxCapacity { get; set; }

        public int CurrentEnrollment { get; set; } = 0; // Sĩ số hiện tại

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Opened"; // e.g. Opened, Closed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
