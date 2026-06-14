using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;

namespace CourseCatalog.Api.Services
{
    public interface IClassroomService
    {
        Task<IEnumerable<ClassroomDto>> GetAllAsync();
        Task<ClassroomDto?> GetByIdAsync(int id);
    }
}
