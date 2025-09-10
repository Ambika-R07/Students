using System.ComponentModel.DataAnnotations;

namespace SMS.WebApi.Domain
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; } = default!;

        public int CourseId { get; set; }

        public Course Course { get; set; } = default!;
       
        public DateTime EnrollmentDate { get; set; } 

        public bool IsActive { get; set; } = true;
    }
}
