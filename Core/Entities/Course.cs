using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Entities;

public record Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    [Required]
    [EnumDataType(typeof(CourseYearEnum))]
    public CourseYearEnum Year { get; set; }
    
    public List<Student>? Students { get; set; }
    
    public Course()
    {
        Students = new List<Student>();
    }
    
    public Course(int id, string title, string description, CourseYearEnum year)
    {
        Id = id;
        Title = title;
        Description = description;
        Year = year;
        Students = new List<Student>();
    }
    
}