using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Capacity { get; set; }
    }
}
