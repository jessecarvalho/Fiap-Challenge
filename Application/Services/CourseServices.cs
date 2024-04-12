using Application.DTOs;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;

namespace Application.Services;
public class CourseServices : ICourseServices
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public CourseServices(ICourseRepository courseRepository, IMapper mapper, IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CourseResponseDto>>(courses);
    }

    public async Task<CourseResponseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
        {
            throw new CourseNotFoundException();
        }
        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto> AddCourseAsync(CourseRequestDto courseDto)
    {
        
        var course = _mapper.Map<Course>(courseDto);
        
        var isTitleNotUnique = _courseRepository.CheckIfCourseTitleIsUnique(course);

        if (isTitleNotUnique)
        {
            throw new CourseNotUniqueException();
        }
        
        var newCourse = await _courseRepository.AddAsync(course);
        
        return _mapper.Map<CourseResponseDto>(newCourse);
        
    }

    public async Task<CourseResponseDto?> UpdateCourseAsync(int id, CourseRequestDto courseRequestDto)
    {
        var courseExists = await _courseRepository.GetByIdAsync(id);

        if (courseExists == null)
        {
            throw new CourseNotFoundException();
        }

        var course = _mapper.Map<Course>(courseRequestDto);
        
        var isTitleNotUnique = _courseRepository.CheckIfCourseTitleIsUnique(course);

        if (isTitleNotUnique)
        {
            throw new CourseNotUniqueException();
        }

        course.Id = id;

        var updatedCourse = await _courseRepository.UpdateAsync(course);

        return _mapper.Map<CourseResponseDto>(updatedCourse);

    }

    public async Task<bool?> DeleteCourseAsync(int id)
    {
        var courseToDelete = await _courseRepository.GetByIdAsync(id);

        if (courseToDelete == null)
        {
            throw new CourseNotFoundException();
        }
        
        return await _courseRepository.DeleteAsync(id);
    }

    public async Task<CourseResponseDto> AddStudentToCourseAsync(int courseId, int studentId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course == null)
        {
            throw new CourseNotFoundException();
        }

        var student = await _studentRepository.GetByIdAsync(studentId);

        if (student == null)
        {
            throw new StudentNotFoundException();
        }

        var result = await _courseRepository.AddStudentToCourseAsync(courseId, studentId);

        return _mapper.Map<CourseResponseDto>(result);
    }

    public async Task<bool> RemoveStudentFromCourseAsync(int courseId, int studentId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course == null)
        {
            throw new CourseNotFoundException();
        }

        var student = await _studentRepository.GetByIdAsync(studentId);

        if (student == null)
        {
            throw new StudentNotFoundException();
        }

        return await _courseRepository.RemoveStudentFromCourseAsync(courseId, studentId);
        
    }

}