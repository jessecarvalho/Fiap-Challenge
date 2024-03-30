using Core.Enums;

namespace Application.Dto;

public record CourseResponseDto
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseYearEnum Year { get; set; }
    public List<StudentResponseDto>? Students { get; set; }
}