using System.ComponentModel.DataAnnotations;

namespace SMS.Infrastructure.Dto
{
    public class StudentUpdateDto
    {
       

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

        public List<int> CourseIds { get; set; } = new List<int>();
    }
}
