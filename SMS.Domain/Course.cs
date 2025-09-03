using System.ComponentModel.DataAnnotations;

namespace SMS.WebApi.Domain
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        public int Credits { get; set; }


        [Required, MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CourseDescription { get; set; }

        [Required]
        public int DurationMonths { get; set; }

        public DateTime CreatedAt { get; set; } 

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
