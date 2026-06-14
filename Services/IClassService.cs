using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;

namespace CourseCatalog.Api.Services
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetAllAsync();
        Task<ClassDto?> GetByIdAsync(int id);
        Task<ClassDto> OpenClassAsync(ClassOpenDto dto);
        Task<IEnumerable<ClassDto>> GetClassesByTeacherAsync(string teacherName);
        Task<ClassDto?> EnrollStudentAsync(int classId);
        Task<ClassDto?> UnenrollStudentAsync(int classId);
    }
}
