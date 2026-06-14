using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseCatalog.Api.Data;
using CourseCatalog.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CourseCatalog.Api.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly CourseDbContext _context;

        public ClassroomService(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassroomDto>> GetAllAsync()
        {
            return await _context.Classrooms
                .Select(c => new ClassroomDto
                {
                    Id = c.Id,
                    RoomName = c.RoomName,
                    Capacity = c.Capacity
                })
                .ToListAsync();
        }

        public async Task<ClassroomDto?> GetByIdAsync(int id)
        {
            var r = await _context.Classrooms.FindAsync(id);
            if (r == null) return null;

            return new ClassroomDto
            {
                Id = r.Id,
                RoomName = r.RoomName,
                Capacity = r.Capacity
            };
        }
    }
}
