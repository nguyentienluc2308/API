using System;
using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }

        public int ClassId { get; set; }
        public Class? Class { get; set; }

        public int ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        public DayOfWeek DayOfWeek { get; set; } // Sunday = 0, Monday = 1, etc.

        [Required]
        [MaxLength(50)]
        public string Shift { get; set; } = string.Empty; // Morning, Afternoon, Evening (or Ca sáng, Ca chiều, Ca tối)

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
