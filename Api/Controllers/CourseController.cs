using Application.Dto;
using Application.DTOs;
using Application.Interfaces.Services;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/course")]
public class CourseController : ControllerBase
{

    private readonly ICourseServices _courseServices;
    private readonly IMapper _mapper;
    private readonly CourseRequestDtoValidator _courseRequestDtoValidator;

    public CourseController(ICourseServices courseServices, IMapper mapper, CourseRequestDtoValidator courseRequestDtoValidator)
    {
        _courseRequestDtoValidator = courseRequestDtoValidator;
        _courseServices = courseServices;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseServices.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoursesById(int id)
    {
        try
        {
            var course = await _courseServices.GetCourseByIdAsync(id);
            return Ok(course);
        }
        catch (CourseNotFoundException notFoundException)
        {
            return NotFound("Course not found");
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequestDto courseDto)
    {
        var validationResult = await _courseRequestDtoValidator.ValidateAsync(courseDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var newCourse = await _courseServices.AddCourseAsync(courseDto);
        return CreatedAtAction(nameof(GetCoursesById), new { id = newCourse.Id }, newCourse);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseRequestDto courseDto)
    {
        var validationResult = await _courseRequestDtoValidator.ValidateAsync(courseDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var updatedCourse = await _courseServices.UpdateCourseAsync(id, courseDto);
            return Ok(updatedCourse);
        }
        catch (CourseNotFoundException notFoundException)
        {
            return NotFound("Course not found");
        }
        
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            var deleted = await _courseServices.DeleteCourseAsync(id);
            return Ok();
        }
        catch (CourseNotFoundException notFoundException)
        {
            return NotFound("Course not found");
        }
        
    }
    
    [HttpPost("{courseId}/students/{studentId}")]
    public async Task<IActionResult> AddStudentToCourse(int courseId, int studentId)
    {
        try
        {
            var result = await _courseServices.AddStudentToCourseAsync(courseId, studentId);

            return Ok(result);
        }
        catch (CourseNotFoundException)
        {
            return NotFound("Course was not found");
        }
        catch (StudentNotFoundException)
        {
            return NotFound("Student was not found");
        }
        
    }

    [HttpDelete("{courseId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId, int studentId)
    {
        var result = await _courseServices.RemoveStudentFromCourseAsync(courseId, studentId);

        if (!result)
        {
            return BadRequest("Failed to remove student from course");
        }

        return Ok("Student removed from course");
    }

}