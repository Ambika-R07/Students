using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
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
        private Mock<AppDbContext> _mockContext;
        private IStudentService _service;
        private List<Student> _studentData;

        [TestInitialize]
        public void Setup()
        {
            _studentData = new List<Student>
            {
                new Student
                {
                    StudentId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    Gender = "M",
                    DateOfBirth = DateTime.UtcNow,
                    Enrollments = new List<Enrollment>()
                }
            };

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _mockContext = new Mock<AppDbContext>(options);

            
            _mockContext.Setup(c => c.Students).ReturnsDbSet(_studentData);
            _mockContext.Setup(c => c.Courses).ReturnsDbSet(new List<Course>());
            _mockContext.Setup(c => c.Enrollments).ReturnsDbSet(new List<Enrollment>());

          
            _mockContext.Setup(c => c.Students.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((object[] ids) => _studentData.FirstOrDefault(s => s.StudentId == (int)ids[0]));

            _service = new StudentService(_mockContext.Object);
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

            Assert.AreEqual("Jane", result.FirstName);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingFirstName_ThrowsException()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "",
                LastName = "Doe",
                Email = "a@b.com",
                Gender = "M",
                CourseIds = new List<int>()
            };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingLastName_ThrowsException()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "Jane",
                LastName = "",
                Email = "a@b.com",
                Gender = "F",
                CourseIds = new List<int>()
            };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_MissingEmail_ThrowsException()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "",
                Gender = "F",
                CourseIds = new List<int>()
            };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_InvalidGender_ThrowsException()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "test@example.com",
                Gender = "X",
                CourseIds = new List<int>()
            };
            await _service.CreateAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_DuplicateEmail_ThrowsException()
        {
            var dto = new StudentCreateDto
            {
                FirstName = "New",
                LastName = "Student",
                Email = "john@example.com",
                Gender = "M",
                CourseIds = new List<int>()
            };
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
                DateOfBirth = DateTime.UtcNow,
                CourseIds = new List<int>()
            };

            var result = await _service.UpdateAsync(1, dto);

            Assert.AreEqual("Updated", result.FirstName);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateAsync_InvalidId_ThrowsException()
        {
            var dto = new StudentUpdateDto
            {
                FirstName = "Updated",
                LastName = "Name",
                Email = "updated@example.com",
                Gender = "F",
                DateOfBirth = DateTime.UtcNow,
                CourseIds = new List<int>()
            };
            await _service.UpdateAsync(99, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task PatchAsync_InvalidId_ThrowsException()
        {
            var dto = new StudentPatchDto
            {
                FirstName = "Patch"
            };
            await _service.PatchAsync(99, dto);
        }

        [TestMethod]
        public async Task PatchAsync_ValidId_UpdatesOnlyProvidedFields()
        {
            var dto = new StudentPatchDto
            {
                FirstName = "Patched"
            };

            var result = await _service.PatchAsync(1, dto);
            Assert.AreEqual("Patched", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            var result = await _service.DeleteAsync(1);
            Assert.IsTrue(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            var result = await _service.DeleteAsync(99);
            Assert.IsFalse(result);
        }

    }
}
