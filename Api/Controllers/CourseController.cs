using Application.Dto;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/course")]
public class CourseController : ControllerBase
{

    private readonly ICourseServices _courseServices;
    private readonly IMapper _mapper;

    public CourseController(ICourseServices courseServices, IMapper mapper)
    {
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
        var course = await _courseServices.GetCourseByIdAsync(id);
        
        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequestDto courseDto)
    {
        var newCourse = await _courseServices.AddCourseAsync(courseDto);
        return CreatedAtAction(nameof(GetCoursesById), new { id = newCourse.Id }, newCourse);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseRequestDto courseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedCourse = await _courseServices.UpdateCourseAsync(id, courseDto);
        if (updatedCourse == null)
        {
            return NotFound();
        }

        return Ok(updatedCourse);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var deleted = await _courseServices.DeleteCourseAsync(id);
        if (deleted == null)
        {
            return NotFound();
        }

        return Ok();
    }
    
}