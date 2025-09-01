public class EnrollmentDto
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public CourseDto Course { get; set; } = new();
   // public int StudentId { get; set; }
}

public class CourseDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int Credits { get; set; }
}
