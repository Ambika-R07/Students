//using SMS.Domain.Entities;
//using SMS.Services.Interfaces;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SMS.Infrastructure.Repositories
//{
//    public class StudentRepository : IStudentRepository
//    {
//        private readonly List<Student> _store = new();
//        private int _nextId = 1;

//        public StudentRepository()
//        {
//            // Seed some dummy students
//            _store.Add(new Student { Id = _nextId++, FirstName = "Alice", LastName = "Doe", Age = 20 });
//            _store.Add(new Student { Id = _nextId++, FirstName = "Bob", LastName = "Smith", Age = 22 });
//        }

//        public Task<Student> CreateAsync(Student student)
//        {
//            student.Id = _nextId++;
//            _store.Add(student);
//            return Task.FromResult(student);
//        }

//        public Task<bool> DeleteAsync(int id)
//        {
//            var removed = _store.RemoveAll(s => s.Id == id) > 0;
//            return Task.FromResult(removed);
//        }

//        public Task<IEnumerable<Student>> GetAllAsync() => Task.FromResult<IEnumerable<Student>>(_store.ToList());

//        public Task<Student?> GetByIdAsync(int id) => Task.FromResult(_store.FirstOrDefault(s => s.Id == id));

//        public Task<bool> UpdateAsync(Student student)
//        {
//            var index = _store.FindIndex(s => s.Id == student.Id);
//            if (index < 0) return Task.FromResult(false);

//            _store[index] = student;
//            return Task.FromResult(true);
//        }
//    }
//}
