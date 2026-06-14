using System.Collections.Generic;
using System.Threading.Tasks;
using CourseCatalog.Api.DTOs;
using CourseCatalog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomsController : ControllerBase
    {
        private readonly IClassroomService _classroomService;

        public ClassroomsController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAll()
        {
            var classrooms = await _classroomService.GetAllAsync();
            return Ok(classrooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassroomDto>> GetById(int id)
        {
            var classroom = await _classroomService.GetByIdAsync(id);
            if (classroom == null)
            {
                return NotFound(new { message = $"Không tìm thấy phòng học với Id {id}" });
            }
            return Ok(classroom);
        }
    }
}
