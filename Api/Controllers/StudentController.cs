using Application.Dto;
using Application.Interfaces.Services;
using Core.Entities;
using Core.Exceptions;
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
        try
        {
            var student = await _studentServices.GetStudentByIdAsync(id);
            return Ok(student);
        }
        catch (StudentNotFoundException notFoundException)
        {
            return NotFound("Student not found");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRequestDto student)
    {
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

        try
        {
            var updatedStudent = await _studentServices.UpdateStudentAsync(id, student);
            return Ok(updatedStudent);
        }
        catch (StudentNotFoundException studentNotFoundException)
        {
            return NotFound("Student not found");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            var deleted = await _studentServices.DeleteStudentAsync(id);
            return Ok("Student deleted successfully");
        }
        catch (StudentNotFoundException studentNotFoundException)
        {
            return NotFound("Student not found");
        }
        
    }
    
}