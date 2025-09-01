using Microsoft.AspNetCore.Mvc;
using SMS.Infrastructure.Dto;
using SMS.Services.Interfaces;

namespace SMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, created);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StudentResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _service.GetByIdAsync(id);
            return s is null ? NotFound() : Ok(s);
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(StudentResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(StudentResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(int id, [FromBody] StudentPatchDto dto)
        {
            try
            {
                var updated = await _service.PatchAsync(id, dto);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound($"Student with ID {id} not found.");

            return NoContent(); 
        }

    }
}
