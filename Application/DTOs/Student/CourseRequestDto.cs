using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Application.Dto;

public record CourseRequestDto
{
    public int Id { get; init; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    [Required]
    public CourseYearEnum Year { get; set; }
    
    public List<StudentRequestDto>? Students { get; set; }
}