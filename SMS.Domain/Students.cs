using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.WebApi.Domain
{
    public class Student
    {

        [Key]
        public int StudentId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        
        [Required, MaxLength(1)]
        public string Gender { get; set; } = "O";

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        public int PhoneNumber { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();

    }
}
