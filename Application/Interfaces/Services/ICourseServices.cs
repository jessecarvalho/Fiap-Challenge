using Application.Dto;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface ICourseServices
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();
        Task<CourseResponseDto?> GetCourseByIdAsync(int id);
        Task<CourseResponseDto> AddCourseAsync(CourseRequestDto course);
        Task<CourseResponseDto?> UpdateCourseAsync(int id, CourseRequestDto course);
        Task<bool?> DeleteCourseAsync(int id);
    }
}