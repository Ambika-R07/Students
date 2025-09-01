using Microsoft.AspNetCore.Mvc;

namespace SMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DummyController : ControllerBase
    {
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
        }

        
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice", Age = 20 },
            new Student { Id = 2, Name = "Bob", Age = 22 }
        };

        [HttpGet]
        public IActionResult GetAllStudents() => Ok(students);

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            return student is null ? NotFound($"Student with Id {id} not found.") : Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            if (string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
                return BadRequest("Invalid data. Name cannot be empty and Age must be > 0.");

            if (students.Any(s => s.Id == student.Id))
                return BadRequest($"Student with Id {student.Id} already exists.");

            students.Add(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student updated)
        {
            var existing = students.FirstOrDefault(s => s.Id == id);
            if (existing is null) return NotFound($"Student with Id {id} not found.");

            if (string.IsNullOrWhiteSpace(updated.Name) || updated.Age <= 0)
                return BadRequest("Invalid data.");

            existing.Name = updated.Name;
            existing.Age = updated.Age;
            return Ok(existing);
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateStudent(int id, [FromBody] Student patch)
        {
            var existing = students.FirstOrDefault(s => s.Id == id);
            if (existing is null) return NotFound($"Student with Id {id} not found.");

            if (!string.IsNullOrWhiteSpace(patch.Name)) existing.Name = patch.Name;
            if (patch.Age > 0) existing.Age = patch.Age;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var existing = students.FirstOrDefault(s => s.Id == id);
            if (existing is null) return NotFound($"Student with Id {id} not found.");

            students.Remove(existing);
            return Ok($"Student with Id {id} deleted.");
        }
    }
}
