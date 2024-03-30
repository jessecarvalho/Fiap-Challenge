namespace Application.Dto;

public record StudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<CourseResponseDto>? Courses { get; set; }
    
}