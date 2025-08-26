//using SMS.Services.Interfaces;
//using SMS.Domain.Entities;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace SMS.Services.Implementations
//{
//    public class StudentService : IStudentService
//    {
//        private readonly IStudentRepository _repo;
//        public StudentService(IStudentRepository repo) => _repo = repo;

//        public async Task<Student> CreateAsync(Student student) => await _repo.CreateAsync(student);

//        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

//        public async Task<IEnumerable<Student>> GetAllAsync() => await _repo.GetAllAsync();

//        public async Task<Student?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

//        public async Task<bool> UpdateAsync(int id, Student student)
//        {
//            var existing = await _repo.GetByIdAsync(id);
//            if (existing == null) return false;

//            // map changes
//            existing.FirstName = student.FirstName;
//            existing.LastName = student.LastName;
//            existing.Age = student.Age;

//            return await _repo.UpdateAsync(existing);
//        }
//    }
//}
