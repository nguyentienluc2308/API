using System;

namespace CourseCatalog.Api.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public int CurrentEnrollment { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
