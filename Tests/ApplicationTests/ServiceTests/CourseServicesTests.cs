using System.ComponentModel.DataAnnotations;
using Application.Dto;
using Application.Mappings;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;
using Moq;

namespace Tests.ApplicationTests.ServiceTests;

public class CourseServicesTests
{

    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CourseServices _courseServices;
    private readonly Course _course;
    private readonly List<Course> _courses;
    private readonly CourseRequestDto _courseRequestDto;
    private readonly CourseResponseDto _courseResponseDto;
    private readonly CourseRequestDtoValidator _validator;

    public CourseServicesTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApplicationProfile>();
        });

        var mapper = mapperConfig.CreateMapper();

        _validator = new CourseRequestDtoValidator();
        _course = new Course { Id = 1, Title = "Math", Description = "Math Best Course", Year = CourseYearEnum.First };
        _courses = new List<Course>
        {
            new Course { Id = 1, Title = "Math", Description = "Math Best Course", Year = CourseYearEnum.First },
            new Course
                { Id = 2, Title = "English", Description = "English Best Course", Year = CourseYearEnum.Fifth, }
        };
        _courseRequestDto = new CourseRequestDto{ Id = 1, Title = "Math", Description = "Math Best Course", Year = CourseYearEnum.First };
        _courseResponseDto = new CourseResponseDto{ Id = 1, Title = "Math", Description = "Math Best Course", Year = CourseYearEnum.First };
        
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _courseServices = new CourseServices(_courseRepositoryMock.Object, mapper);
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(_course);
        _courseRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_courses);
        _courseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Course>())).ReturnsAsync(_course);
        _courseRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Course>())).ReturnsAsync(_course);
        _courseRepositoryMock.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);
    }

    [Fact]
    public async Task GetAllCoursesAsync_WhenCalled_ReturnsAllCourses()
    {
        // Act
        var result = await _courseServices.GetAllCoursesAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

    }

    [Fact]
    public async Task GetCourseByIdAsync_WhenCalled_ReturnsCourse()
    {
        // Act
        var result = await _courseServices.GetCourseByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddCourse_WhenCalledWithValidCourse_ReturnsNewCourse()
    {
        // Arrange
        var validationResult = await _validator.ValidateAsync(_courseRequestDto);
        
        // Act
        var result = validationResult.IsValid 
            ? await _courseServices.AddCourseAsync(_courseRequestDto) 
            : null;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Title, _courseRequestDto.Title);
    }

    [Fact]
    public async Task AddCourse_WhenCalledWithInvalidCourse_ReturnsNull()
    {
        // Arrange
        _courseRequestDto.Description = null;
        var validationResult = await _validator.ValidateAsync(_courseRequestDto);
        
        // Act
        var result = validationResult.IsValid 
            ? await _courseServices.AddCourseAsync(_courseRequestDto) 
            : null;
        
        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Null(result);
    }

    [Fact]
    public async Task AddCourse_WhenCalledWithCourseWithNotUniqueTitle_ReturnsException()
    {
        
        // Arrange
        _courseRepositoryMock.Setup(x => x.CheckIfCourseTitleIsUnique(It.IsAny<Course>())).Returns(true);
        var courseRequestDtoWithSameName = new CourseRequestDto{ Id = 2, Title = "Math", Description = "Math Best Course", Year = CourseYearEnum.First };
        
        // Assert
        await Assert.ThrowsAsync<CourseNotUniqueException>(async () => await _courseServices.AddCourseAsync(courseRequestDtoWithSameName));

    }
    
    [Fact]
    public async Task UpdateCourse_WhenCalledWithValidCourse_ReturnsCourse()
    {
        // Arrange
        var validationResult = await _validator.ValidateAsync(_courseRequestDto);
        
        // Act
        var result = validationResult.IsValid
            ? await _courseServices.UpdateCourseAsync(1, _courseRequestDto)
            : null;
        
        // Assert
        Assert.True(validationResult.IsValid);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateCourse_WhenCalledWithInvalidCourse_ReturnsNull()
    {
        // Arrange
        _courseRequestDto.Description = null;
        var validationResult = await _validator.ValidateAsync(_courseRequestDto);
        
        // Act
        var result = validationResult.IsValid
            ? await _courseServices.UpdateCourseAsync(1, _courseRequestDto)
            : null;
        
        Assert.False(validationResult.IsValid);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCourse_WhenCalledWithValidId_ReturnsTrue()
    {
        
        // Act
        var result = await _courseServices.DeleteCourseAsync(1);
        
        // Assert
        Assert.True(result);

    }

    [Fact]
    public async Task DeleteCourse_WhenCalledWithInvalidId_ThrowsError()
    {
        // Assert
        await Assert.ThrowsAsync<CourseNotFoundException>(async () => await _courseServices.DeleteCourseAsync(200));
    }
    
    
}