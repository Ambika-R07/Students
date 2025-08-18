using System.ComponentModel.DataAnnotations;

namespace SMS.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 120)]
        public int Age { get; set; }
    }
}
