using Microsoft.EntityFrameworkCore;
using SMS.Infrastructure.Data;
using SMS.Infrastructure.Dto;
using SMS.Services.Interfaces;
using SMS.WebApi.Domain;

namespace SMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _db;

        public StudentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<StudentResponseDto> CreateAsync(StudentCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("FirstName and LastName are required.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");
            if (!new[] { "M", "F", "O" }.Contains(dto.Gender))
                throw new ArgumentException("Gender must be one of M/F/O.");

            var emailExists = await _db.Students.AnyAsync(s => s.Email == dto.Email);
            if (emailExists) throw new ArgumentException("Email already exists.");

            var entity = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Enrollments = dto.CourseIds.Select(courseId => new Enrollment
                {
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow,
                    IsActive = true
                }).ToList()
            };

            _db.Students.Add(entity);
            await _db.SaveChangesAsync();

          
            var response = new StudentResponseDto
            {
                StudentId = entity.StudentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Enrollments = entity.Enrollments.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Course = _db.Courses
                        .Where(c => c.CourseId == e.CourseId)
                        .Select(c => new CourseDto
                        {
                            CourseId = c.CourseId,
                            CourseName = c.CourseName,
                            Credits = c.Credits
                        })
                        .FirstOrDefault()
                }).ToList()
            };

            return response;
        }

        public async Task<List<StudentResponseDto>> GetAllAsync()
        {
            var students = await _db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            return students.Select(MapToResponse).ToList();
        }

        public async Task<StudentResponseDto> GetByIdAsync(int id)
        {
            var entity = await _db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (entity == null) throw new KeyNotFoundException($"Student with id {id} not found.");
            return MapToResponse(entity);
        }

        public async Task<StudentResponseDto> UpdateAsync(int id, StudentUpdateDto dto)
        {
            var entity = await _db.Students
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (entity == null) throw new KeyNotFoundException($"Student with id {id} not found.");

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.DateOfBirth = dto.DateOfBirth;
            entity.Gender = dto.Gender;

            _db.Enrollments.RemoveRange(entity.Enrollments);

            entity.Enrollments = dto.CourseIds.Select(courseId => new Enrollment
            {
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true
            }).ToList();

            await _db.SaveChangesAsync();

            
            var response = new StudentResponseDto
            {
                StudentId = entity.StudentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Enrollments = entity.Enrollments.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Course = _db.Courses
                        .Where(c => c.CourseId == e.CourseId)
                        .Select(c => new CourseDto
                        {
                            CourseId = c.CourseId,
                            CourseName = c.CourseName,
                            Credits = c.Credits
                        })
                        .FirstOrDefault()
                }).ToList()
            };

            return response;
        }

        public async Task<StudentResponseDto> PatchAsync(int id, StudentPatchDto dto)
        {
            var entity = await _db.Students
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (entity == null) throw new KeyNotFoundException($"Student with id {id} not found.");

            if (!string.IsNullOrWhiteSpace(dto.FirstName)) entity.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) entity.LastName = dto.LastName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) entity.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Gender)) entity.Gender = dto.Gender;
            if (dto.DateOfBirth.HasValue) entity.DateOfBirth = dto.DateOfBirth.Value;

            await _db.SaveChangesAsync();

            return MapToResponse(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Students.FindAsync(id);
            if (entity == null) return false;

            _db.Students.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static StudentResponseDto MapToResponse(Student entity)
        {
            return new StudentResponseDto
            {
                StudentId = entity.StudentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Enrollments = entity.Enrollments.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Course = e.Course == null ? null : new CourseDto
                    {
                        CourseId = e.Course.CourseId,
                        CourseName = e.Course.CourseName,
                        Credits = e.Course.Credits
                    }
                }).ToList()
            };
        }
    }
}
