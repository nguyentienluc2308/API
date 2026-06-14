using System.ComponentModel.DataAnnotations;

namespace CourseCatalog.Api.DTOs
{
    public class ClassOpenDto
    {
        [Required(ErrorMessage = "Mã khóa học không được để trống")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Tên lớp học không được để trống")]
        [MaxLength(200)]
        public string ClassName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên giáo viên phụ trách không được để trống")]
        [MaxLength(200)]
        public string TeacherName { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Sĩ số tối đa phải từ 1 đến 1000")]
        public int MaxCapacity { get; set; }
    }
}
