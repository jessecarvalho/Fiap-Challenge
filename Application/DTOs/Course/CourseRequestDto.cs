using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Application.DTOs;

public record CourseRequestDto
{
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    [Required]
    [EnumDataType(typeof(CourseYearEnum))]
    public CourseYearEnum Year { get; set; }
    
}