using Application.Dto;
using Application.Interfaces.Services;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
// [Route("api/[controller]")]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private readonly IStudentServices _studentServices;

    public StudentController(IStudentServices studentServices)
    {
        _studentServices = studentServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentServices.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _studentServices.GetStudentByIdAsync(id);

        if (student == null)
        {
            return NotFound("Student not found");
        }
        
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRequestDto student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var newStudent = await _studentServices.AddStudentAsync(student);
        
        if (newStudent == null)
        {
            return BadRequest("Student not created");
        }
        
        return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentRequestDto student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var studentToUpdate = await _studentServices.GetStudentByIdAsync(id);

        if (studentToUpdate == null)
        {
            return NotFound("Student not found");
        }
        
        var updatedStudent = await _studentServices.UpdateStudentAsync(id, student);

        if (updatedStudent == null)
        {
            return NotFound("Student not found");
        }
        
        return Ok(updatedStudent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var deleted = await _studentServices.DeleteStudentAsync(id);

        if (deleted == null)
        {
            return NotFound("Student not found");
        }

        return Ok("Student deleted successfully");
    }
    
}