using Core.Enums;

namespace Application.DTOs;

public record CourseResponseDto
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseYearEnum Year { get; set; }
    public List<StudentResponseWithoutCourseDto>? Students { get; set; }
}