using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;

namespace CourseCatalog.Api.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
        Task<IEnumerable<ScheduleDto>> GetSchedulesByClassIdAsync(int classId);
        Task<IEnumerable<ScheduleDto>> GetSchedulesByTeacherAsync(string teacherName);
        Task<ScheduleDto> ConfigureScheduleAsync(ScheduleConfigDto dto);
        Task<bool> RemoveScheduleAsync(int scheduleId);
    }
}
