using Application.Dto;
using Core.Entities;

namespace Application.Interfaces.Services;

public interface IStudentServices
{
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
    Task<StudentResponseDto?> GetStudentByIdAsync(int id);
    Task<StudentResponseDto?> AddStudentAsync(StudentRequestDto student);
    Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentRequestDto studentDto);
    Task<bool?> DeleteStudentAsync(int id);
}