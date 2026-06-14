using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;
using CourseCatalog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAll()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByClassId(int classId)
        {
            var schedules = await _scheduleService.GetSchedulesByClassIdAsync(classId);
            return Ok(schedules);
        }

        [HttpGet("teacher/{teacherName}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByTeacher(string teacherName)
        {
            var schedules = await _scheduleService.GetSchedulesByTeacherAsync(teacherName);
            return Ok(schedules);
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> ConfigureSchedule(ScheduleConfigDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _scheduleService.ConfigureScheduleAsync(dto);
                return Created(string.Empty, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict represents scheduling conflicts perfectly!
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSchedule(int id)
        {
            var succeeded = await _scheduleService.RemoveScheduleAsync(id);
            if (!succeeded)
            {
                return NotFound(new { message = $"Không tìm thấy lịch học với Id {id} để xóa" });
            }
            return NoContent();
        }
    }
}
