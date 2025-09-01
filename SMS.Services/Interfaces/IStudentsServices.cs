using SMS.Infrastructure.Dto;

namespace SMS.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDto> CreateAsync(StudentCreateDto dto);
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<StudentResponseDto> UpdateAsync(int id, StudentUpdateDto dto);
        Task<StudentResponseDto> PatchAsync(int id, StudentPatchDto dto);
        Task<StudentResponseDto> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);

    }
}
