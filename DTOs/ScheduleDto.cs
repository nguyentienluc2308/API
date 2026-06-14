using System;

namespace CourseCatalog.Api.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int ClassroomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public string DayOfWeekName => DayOfWeek.ToString(); // e.g. "Monday"
        public string Shift { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty; // "HH:mm"
        public string EndTime { get; set; } = string.Empty; // "HH:mm"
    }
}
