using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.DTOs
{
    public class CourseUpdateDto
    {
        [Required(ErrorMessage = "Tên khóa học không được để trống")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Học phí phải lớn hơn hoặc bằng 0")]
        public decimal Fee { get; set; }

        [Range(1, 1000, ErrorMessage = "Số buổi học phải từ 1 đến 1000")]
        public int NumberOfSessions { get; set; }

        [Required(ErrorMessage = "Trình độ không được để trống")]
        [MaxLength(50)]
        public string Level { get; set; } = string.Empty;
    }
}
