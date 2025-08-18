using SMS.Domain.Entities;
using SMS.Services.Interfaces;

namespace SMS.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly List<Student> _students = new();

        public Task<IEnumerable<Student>> GetAllAsync()
            => Task.FromResult(_students.AsEnumerable());

        public Task<Student?> GetByIdAsync(int id)
            => Task.FromResult(_students.FirstOrDefault(s => s.Id == id));

        public Task<Student> CreateAsync(Student student)
        {
            student.Id = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);
            return Task.FromResult(student);
        }

        public Task<bool> UpdateAsync(Student student)
        {
            var existing = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existing == null) return Task.FromResult(false);

            existing.FirstName = student.FirstName;
            existing.LastName = student.LastName;
            existing.Age = student.Age;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return Task.FromResult(false);

            _students.Remove(student);
            return Task.FromResult(true);
        }
    }
}
