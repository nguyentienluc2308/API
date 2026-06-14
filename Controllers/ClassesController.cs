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
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetAll()
        {
            var classes = await _classService.GetAllAsync();
            return Ok(classes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetById(int id)
        {
            var c = await _classService.GetByIdAsync(id);
            if (c == null)
            {
                return NotFound(new { message = $"Không tìm thấy lớp học với Id {id}" });
            }
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult<ClassDto>> OpenClass(ClassOpenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _classService.OpenClassAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("teacher/{teacherName}")]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetByTeacher(string teacherName)
        {
            var classes = await _classService.GetClassesByTeacherAsync(teacherName);
            return Ok(classes);
        }

        [HttpPost("{id}/enroll")]
        public async Task<ActionResult<ClassDto>> Enroll(int id)
        {
            try
            {
                var result = await _classService.EnrollStudentAsync(id);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy lớp học với Id {id}" });
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/unenroll")]
        public async Task<ActionResult<ClassDto>> Unenroll(int id)
        {
            try
            {
                var result = await _classService.UnenrollStudentAsync(id);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy lớp học với Id {id}" });
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
