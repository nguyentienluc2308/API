using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;

namespace CourseCatalog.Api.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CourseCreateDto dto);
        Task<CourseDto?> UpdateAsync(int id, CourseUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
