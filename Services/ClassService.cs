using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseCatalog.Api.Data;
using CourseCatalog.Api.DTOs;
using CourseCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCatalog.Api.Services
{
    public class ClassService : IClassService
    {
        private readonly CourseDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public ClassService(CourseDbContext context, IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public async Task<IEnumerable<ClassDto>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Course)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    CourseId = c.CourseId,
                    CourseName = c.Course != null ? c.Course.Name : string.Empty,
                    ClassName = c.ClassName,
                    TeacherName = c.TeacherName,
                    MaxCapacity = c.MaxCapacity,
                    CurrentEnrollment = c.CurrentEnrollment,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ClassDto?> GetByIdAsync(int id)
        {
            var c = await _context.Classes
                .Include(cl => cl.Course)
                .FirstOrDefaultAsync(cl => cl.Id == id);

            if (c == null) return null;

            return new ClassDto
            {
                Id = c.Id,
                CourseId = c.CourseId,
                CourseName = c.Course != null ? c.Course.Name : string.Empty,
                ClassName = c.ClassName,
                TeacherName = c.TeacherName,
                MaxCapacity = c.MaxCapacity,
                CurrentEnrollment = c.CurrentEnrollment,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            };
        }

        public async Task<ClassDto> OpenClassAsync(ClassOpenDto dto)
        {
            // Verify Course exists
            var courseExists = await _context.Courses.AnyAsync(co => co.Id == dto.CourseId);
            if (!courseExists)
            {
                throw new System.ArgumentException($"Không tìm thấy khóa học với Id {dto.CourseId}");
            }

            var newClass = new Class
            {
                CourseId = dto.CourseId,
                ClassName = dto.ClassName,
                TeacherName = dto.TeacherName,
                MaxCapacity = dto.MaxCapacity,
                CurrentEnrollment = 0,
                Status = "Opened",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Classes.AddAsync(newClass);
            await _context.SaveChangesAsync();

            // Load Course detail for Event
            var course = await _context.Courses.FindAsync(dto.CourseId);

            // Publish class.opened event
            var eventPayload = new
            {
                ClassId = newClass.Id,
                ClassName = newClass.ClassName,
                CourseId = newClass.CourseId,
                CourseName = course?.Name ?? string.Empty,
                TeacherName = newClass.TeacherName,
                MaxCapacity = newClass.MaxCapacity,
                Status = newClass.Status,
                OpenedAt = newClass.CreatedAt
            };

            await _eventPublisher.PublishAsync("class.opened", eventPayload);

            return new ClassDto
            {
                Id = newClass.Id,
                CourseId = newClass.CourseId,
                CourseName = course?.Name ?? string.Empty,
                ClassName = newClass.ClassName,
                TeacherName = newClass.TeacherName,
                MaxCapacity = newClass.MaxCapacity,
                CurrentEnrollment = newClass.CurrentEnrollment,
                Status = newClass.Status,
                CreatedAt = newClass.CreatedAt
            };
        }

        public async Task<IEnumerable<ClassDto>> GetClassesByTeacherAsync(string teacherName)
        {
            return await _context.Classes
                .Include(c => c.Course)
                .Where(c => c.TeacherName.ToLower() == teacherName.ToLower())
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    CourseId = c.CourseId,
                    CourseName = c.Course != null ? c.Course.Name : string.Empty,
                    ClassName = c.ClassName,
                    TeacherName = c.TeacherName,
                    MaxCapacity = c.MaxCapacity,
                    CurrentEnrollment = c.CurrentEnrollment,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ClassDto?> EnrollStudentAsync(int classId)
        {
            var targetClass = await _context.Classes
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (targetClass == null) return null;

            if (targetClass.CurrentEnrollment >= targetClass.MaxCapacity)
            {
                throw new InvalidOperationException($"Lớp học '{targetClass.ClassName}' đã đạt sĩ số tối đa ({targetClass.MaxCapacity}/{targetClass.MaxCapacity}). Không thể đăng ký thêm.");
            }

            targetClass.CurrentEnrollment++;
            await _context.SaveChangesAsync();

            return new ClassDto
            {
                Id = targetClass.Id,
                CourseId = targetClass.CourseId,
                CourseName = targetClass.Course != null ? targetClass.Course.Name : string.Empty,
                ClassName = targetClass.ClassName,
                TeacherName = targetClass.TeacherName,
                MaxCapacity = targetClass.MaxCapacity,
                CurrentEnrollment = targetClass.CurrentEnrollment,
                Status = targetClass.Status,
                CreatedAt = targetClass.CreatedAt
            };
        }

        public async Task<ClassDto?> UnenrollStudentAsync(int classId)
        {
            var targetClass = await _context.Classes
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (targetClass == null) return null;

            if (targetClass.CurrentEnrollment <= 0)
            {
                throw new InvalidOperationException($"Lớp học '{targetClass.ClassName}' hiện tại chưa có học viên nào. Không thể hủy đăng ký.");
            }

            targetClass.CurrentEnrollment--;
            await _context.SaveChangesAsync();

            return new ClassDto
            {
                Id = targetClass.Id,
                CourseId = targetClass.CourseId,
                CourseName = targetClass.Course != null ? targetClass.Course.Name : string.Empty,
                ClassName = targetClass.ClassName,
                TeacherName = targetClass.TeacherName,
                MaxCapacity = targetClass.MaxCapacity,
                CurrentEnrollment = targetClass.CurrentEnrollment,
                Status = targetClass.Status,
                CreatedAt = targetClass.CreatedAt
            };
        }
    }
}
