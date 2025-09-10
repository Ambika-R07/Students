using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMS.Infrastructure.Data;
using SMS.Infrastructure.Dto;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.WebApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Tests
{
    [TestClass]
    public class StudentServiceTests
    {
        private AppDbContext _context;
        private IStudentService _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);

            _context.Students.Add(new Student
            {
                StudentId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Gender = "M",
                DateOfBirth = DateTime.UtcNow,
                Enrollments = new List<Enrollment>()
            });
            _context.SaveChanges();

            _service = new StudentService(_context);
        }

        [TestMethod]
        public async Task CreateAsync_ValidStudent_ReturnsDto()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Gender = "F",
                DateOfBirth = DateTime.UtcNow,
                CourseIds = new List<int>()
            };

            var result = await _service.CreateAsync(dto);
            var all = await _service.GetAllAsync();

            Assert.AreEqual("Jane", result.FirstName);
            Assert.AreEqual(2, all.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingFirstName_ThrowsException()
        {
            var dto = new StudentCreateDto { FirstName = "", LastName = "Doe", Email = "a@b.com", Gender = "M" };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingLastName_ThrowsException()
        {
            var dto = new StudentCreateDto { FirstName = "Jane", LastName = "", Email = "a@b.com", Gender = "M" };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingEmail_ThrowsException()
        {
            var dto = new StudentCreateDto { FirstName = "Jane", LastName = "Doe", Email = "", Gender = "F" };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_InvalidGender_ThrowsException()
        {
            var dto = new StudentCreateDto { FirstName = "Test", LastName = "Doe", Email = "test@test.com", Gender = "X" };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_DuplicateEmail_ThrowsException()
        {
            var dto = new StudentCreateDto { FirstName = "Dup", LastName = "Student", Email = "john@example.com", Gender = "M" };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsStudents()
        {
            var result = await _service.GetAllAsync();
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ValidId_ReturnsStudent()
        {
            var result = await _service.GetByIdAsync(1);
            Assert.AreEqual("John", result.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetByIdAsync_InvalidId_ThrowsException()
        {
            await _service.GetByIdAsync(99);
        }

        [TestMethod]
        public async Task UpdateAsync_ValidId_UpdatesStudent()
        {
            var dto = new StudentUpdateDto
            {
                FirstName = "Updated",
                LastName = "Name",
                Email = "updated@example.com",
                Gender = "F",
                DateOfBirth = DateTime.UtcNow
            };

            var result = await _service.UpdateAsync(1, dto);

            Assert.AreEqual("Updated", result.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateAsync_InvalidId_ThrowsException()
        {
            var dto = new StudentUpdateDto { FirstName = "X", LastName = "Y", Email = "x@y.com", Gender = "M", DateOfBirth = DateTime.UtcNow };
            await _service.UpdateAsync(99, dto);
        }

        
        [TestMethod]
        public async Task PatchAsync_ValidId_UpdatesOnlyProvidedFields()
        {
            var dto = new StudentPatchDto { FirstName = "Patched" };
            var result = await _service.PatchAsync(1, dto);

            Assert.AreEqual("Patched", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task PatchAsync_InvalidId_ThrowsException()
        {
            var dto = new StudentPatchDto { FirstName = "Nope" };
            await _service.PatchAsync(99, dto);
        }

        
        [TestMethod]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            var result = await _service.DeleteAsync(1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            var result = await _service.DeleteAsync(99);
            Assert.IsFalse(result);
        }
    }
}
