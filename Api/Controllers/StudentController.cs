using Application.Dto;
using Application.Interfaces.Services;
using Application.Validators;
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
    private readonly StudentRequestDtoValidator _studentRequestDtoValidator;

    public StudentController(IStudentServices studentServices, StudentRequestDtoValidator studentRequestDtoValidator)
    {
        _studentServices = studentServices;
        _studentRequestDtoValidator = studentRequestDtoValidator;
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
        var validationResult = await _studentRequestDtoValidator.ValidateAsync(student);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
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
        var validationResult = await _studentRequestDtoValidator.ValidateAsync(student);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
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