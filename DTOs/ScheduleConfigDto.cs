using System;
using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.DTOs
{
    public class ScheduleConfigDto
    {
        [Required(ErrorMessage = "Mã lớp học không được để trống")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Mã phòng học không được để trống")]
        public int ClassroomId { get; set; }

        [Range(0, 6, ErrorMessage = "Thứ trong tuần phải từ 0 (Chủ Nhật) đến 6 (Thứ Bảy)")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Ca học không được để trống (ví dụ: Ca sáng, Ca chiều, Ca tối)")]
        [MaxLength(50)]
        public string Shift { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giờ bắt đầu không được để trống (định dạng HH:mm)")]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Giờ bắt đầu phải ở định dạng HH:mm")]
        public string StartTime { get; set; } = string.Empty; // e.g. "08:00"

        [Required(ErrorMessage = "Giờ kết thúc không được để trống (định dạng HH:mm)")]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Giờ kết thúc phải ở định dạng HH:mm")]
        public string EndTime { get; set; } = string.Empty; // e.g. "10:30"
    }
}
