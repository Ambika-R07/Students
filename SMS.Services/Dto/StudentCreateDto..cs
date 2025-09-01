
namespace SMS.Infrastructure.Dto
{
    public class StudentCreateDto
    {
       
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = "O";
        public string Email { get; set; } = string.Empty;

        public List<int> CourseIds { get; set; } = new();
    }
}
