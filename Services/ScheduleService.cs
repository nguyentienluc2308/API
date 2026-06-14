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
    public class ScheduleService : IScheduleService
    {
        private readonly CourseDbContext _context;

        public ScheduleService(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
        {
            return await _context.ClassSchedules
                .Include(cs => cs.Class)
                .Include(cs => cs.Classroom)
                .Select(cs => new ScheduleDto
                {
                    Id = cs.Id,
                    ClassId = cs.ClassId,
                    ClassName = cs.Class != null ? cs.Class.ClassName : string.Empty,
                    TeacherName = cs.Class != null ? cs.Class.TeacherName : string.Empty,
                    ClassroomId = cs.ClassroomId,
                    RoomName = cs.Classroom != null ? cs.Classroom.RoomName : string.Empty,
                    DayOfWeek = cs.DayOfWeek,
                    Shift = cs.Shift,
                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                    EndTime = cs.EndTime.ToString(@"hh\:mm")
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByClassIdAsync(int classId)
        {
            return await _context.ClassSchedules
                .Include(cs => cs.Class)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.ClassId == classId)
                .Select(cs => new ScheduleDto
                {
                    Id = cs.Id,
                    ClassId = cs.ClassId,
                    ClassName = cs.Class != null ? cs.Class.ClassName : string.Empty,
                    TeacherName = cs.Class != null ? cs.Class.TeacherName : string.Empty,
                    ClassroomId = cs.ClassroomId,
                    RoomName = cs.Classroom != null ? cs.Classroom.RoomName : string.Empty,
                    DayOfWeek = cs.DayOfWeek,
                    Shift = cs.Shift,
                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                    EndTime = cs.EndTime.ToString(@"hh\:mm")
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByTeacherAsync(string teacherName)
        {
            return await _context.ClassSchedules
                .Include(cs => cs.Class)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.Class != null && cs.Class.TeacherName.ToLower() == teacherName.ToLower())
                .Select(cs => new ScheduleDto
                {
                    Id = cs.Id,
                    ClassId = cs.ClassId,
                    ClassName = cs.Class != null ? cs.Class.ClassName : string.Empty,
                    TeacherName = cs.Class != null ? cs.Class.TeacherName : string.Empty,
                    ClassroomId = cs.ClassroomId,
                    RoomName = cs.Classroom != null ? cs.Classroom.RoomName : string.Empty,
                    DayOfWeek = cs.DayOfWeek,
                    Shift = cs.Shift,
                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                    EndTime = cs.EndTime.ToString(@"hh\:mm")
                })
                .ToListAsync();
        }

        public async Task<ScheduleDto> ConfigureScheduleAsync(ScheduleConfigDto dto)
        {
            // 1. Parse times
            if (!TimeSpan.TryParse(dto.StartTime, out var startTime))
            {
                throw new ArgumentException("Giờ bắt đầu không hợp lệ.");
            }
            if (!TimeSpan.TryParse(dto.EndTime, out var endTime))
            {
                throw new ArgumentException("Giờ kết thúc không hợp lệ.");
            }

            if (endTime <= startTime)
            {
                throw new ArgumentException("Giờ kết thúc phải sau giờ bắt đầu.");
            }

            // 2. Load and verify Class and Teacher
            var targetClass = await _context.Classes.FindAsync(dto.ClassId);
            if (targetClass == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy lớp học với Id {dto.ClassId}");
            }

            // 3. Load and verify Classroom and Capacity
            var classroom = await _context.Classrooms.FindAsync(dto.ClassroomId);
            if (classroom == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy phòng học với Id {dto.ClassroomId}");
            }

            if (targetClass.MaxCapacity > classroom.Capacity)
            {
                throw new InvalidOperationException($"Sĩ số tối đa của lớp ({targetClass.MaxCapacity}) vượt quá sức chứa của phòng học {classroom.RoomName} ({classroom.Capacity} người).");
            }

            // 4. Check for overlapping schedules
            // Overlap condition:
            // Same DayOfWeek AND
            // (NewStart < ExistingEnd AND NewEnd > ExistingStart)
            // AND (Same Room OR Same Teacher)
            var conflictingSchedule = await _context.ClassSchedules
                .Include(cs => cs.Class)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.DayOfWeek == dto.DayOfWeek)
                .Where(cs => cs.StartTime < endTime && cs.EndTime > startTime)
                .Where(cs => cs.ClassroomId == dto.ClassroomId || (cs.Class != null && cs.Class.TeacherName == targetClass.TeacherName))
                .FirstOrDefaultAsync();

            if (conflictingSchedule != null)
            {
                string conflictType = conflictingSchedule.ClassroomId == dto.ClassroomId 
                    ? $"phòng học {classroom.RoomName}" 
                    : $"giáo viên phụ trách {targetClass.TeacherName}";

                string conflictDetails = $"Lịch học bị trùng vì {conflictType} đã có lịch xếp cho lớp '{conflictingSchedule.Class?.ClassName}' vào thứ {GetDayNameVietnamese(dto.DayOfWeek)} ca {conflictingSchedule.Shift} ({conflictingSchedule.StartTime.ToString(@"hh\:mm")} - {conflictingSchedule.EndTime.ToString(@"hh\:mm")}).";
                
                throw new InvalidOperationException(conflictDetails);
            }

            // 5. Create new ClassSchedule
            var newSchedule = new ClassSchedule
            {
                ClassId = dto.ClassId,
                ClassroomId = dto.ClassroomId,
                DayOfWeek = dto.DayOfWeek,
                Shift = dto.Shift,
                StartTime = startTime,
                EndTime = endTime
            };

            await _context.ClassSchedules.AddAsync(newSchedule);
            await _context.SaveChangesAsync();

            return new ScheduleDto
            {
                Id = newSchedule.Id,
                ClassId = newSchedule.ClassId,
                ClassName = targetClass.ClassName,
                TeacherName = targetClass.TeacherName,
                ClassroomId = newSchedule.ClassroomId,
                RoomName = classroom.RoomName,
                DayOfWeek = newSchedule.DayOfWeek,
                Shift = newSchedule.Shift,
                StartTime = newSchedule.StartTime.ToString(@"hh\:mm"),
                EndTime = newSchedule.EndTime.ToString(@"hh\:mm")
            };
        }

        public async Task<bool> RemoveScheduleAsync(int scheduleId)
        {
            var schedule = await _context.ClassSchedules.FindAsync(scheduleId);
            if (schedule == null) return false;

            _context.ClassSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GetDayNameVietnamese(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "Chủ Nhật",
                DayOfWeek.Monday => "Hai",
                DayOfWeek.Tuesday => "Ba",
                DayOfWeek.Wednesday => "Tư",
                DayOfWeek.Thursday => "Năm",
                DayOfWeek.Friday => "Sáu",
                DayOfWeek.Saturday => "Bảy",
                _ => day.ToString()
            };
        }
    }
}
