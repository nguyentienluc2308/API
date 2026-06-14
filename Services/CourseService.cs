using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseCatalog.Api.Data;
using CourseCatalog.Api.DTOs;
using CourseCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCatalog.Api.Services
{
    public class CourseService : ICourseService
    {
        private readonly CourseDbContext _context;

        public CourseService(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            return await _context.Courses
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Fee = c.Fee,
                    NumberOfSessions = c.NumberOfSessions,
                    Level = c.Level
                })
                .ToListAsync();
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Fee = course.Fee,
                NumberOfSessions = course.NumberOfSessions,
                Level = course.Level
            };
        }

        public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
        {
            var course = new Course
            {
                Name = dto.Name,
                Fee = dto.Fee,
                NumberOfSessions = dto.NumberOfSessions,
                Level = dto.Level
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Fee = course.Fee,
                NumberOfSessions = course.NumberOfSessions,
                Level = course.Level
            };
        }

        public async Task<CourseDto?> UpdateAsync(int id, CourseUpdateDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            course.Name = dto.Name;
            course.Fee = dto.Fee;
            course.NumberOfSessions = dto.NumberOfSessions;
            course.Level = dto.Level;

            await _context.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Fee = course.Fee,
                NumberOfSessions = course.NumberOfSessions,
                Level = course.Level
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            // Check if there are any classes registered for this course
            var hasClasses = await _context.Classes.AnyAsync(c => c.CourseId == id);
            if (hasClasses)
            {
                throw new System.InvalidOperationException("Không thể xóa khóa học vì đã có lớp học đăng ký khóa học này.");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
