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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = $"Không tìm thấy khóa học với Id {id}" });
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> Update(int id, CourseUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _courseService.UpdateAsync(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"Không tìm thấy khóa học với Id {id} để cập nhật" });
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var succeeded = await _courseService.DeleteAsync(id);
                if (!succeeded)
                {
                    return NotFound(new { message = $"Không tìm thấy khóa học với Id {id} để xóa" });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
