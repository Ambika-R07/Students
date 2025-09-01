namespace SMS.Infrastructure.Dto;
public class StudentResponseDto
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = "O";
    public string Email { get; set; } = string.Empty;
   // public int PhoneNumber { get; set; }
    public List<EnrollmentDto> Enrollments { get; set; } = new();
}



