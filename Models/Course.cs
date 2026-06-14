using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Fee { get; set; }

        [Range(1, 1000)]
        public int NumberOfSessions { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; } = string.Empty; // e.g. Beginner, Intermediate, Advanced
    }
}
