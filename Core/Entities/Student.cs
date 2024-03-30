using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public record class Student
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    [StringLength(255)]
    public string UserName { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Password { get; set; }
    public List<Course>? Courses { get; set; }

    public Student()
    {
        Courses = new List<Course>();
    }
    
    public Student(int id, string name, string email, string username, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        UserName = username;
        Password = password;
        Courses = new List<Course>();
    }
    
}