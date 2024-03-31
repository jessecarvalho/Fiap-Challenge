using Application.Dto;
using Application.Interfaces.Services;
using Application.Mappings;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Interfaces.Repositories;
using Moq;

namespace Tests.ApplicationTests.ServiceTests;

public class StudentServicesTests
{
    private Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly StudentServices _studentServices;
    private readonly Student _student;
    private readonly StudentRequestDtoValidator _validator;
    private readonly StudentRequestDto _studentRequestDto;
    private readonly StudentResponseDto _studentResponseDto;

    public StudentServicesTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApplicationProfile>();
        });

        var mapper = mapperConfig.CreateMapper();
        _validator = new StudentRequestDtoValidator();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _studentServices = new StudentServices(_studentRepositoryMock.Object, mapper);
        _student = new Student() { Id = 1, Name = "John", Email = "teste@teste.com", UserName = "teste", Password = "SenhaF#rte123" };
        _studentResponseDto = new StudentResponseDto() { Name = "John", Email = "teste@teste.com", UserName = "teste" };
        _studentRequestDto = new StudentRequestDto() { Name = "John", Email = "teste@teste.com", UserName = "teste", Password = "SenhaF#rte123" };
        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>())).ReturnsAsync(_student);
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(_student);
        _studentRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(_student);
        _studentRepositoryMock.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

    }

    [Fact]
    public async Task GetAllStudentsAsync_WhenCalled_ReturnsAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Email = "johndoe@email.com" },
            new Student { Id = 2, Name = "Jane Doe", Email = "janedoe@email.com" }
        };

        _studentRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(students);

        // Act
        var result = await _studentServices.GetAllStudentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetStudentByIdAsync_WhenCalledWithValidId_ReturnsStudent()
    {
        // Act
        var result = await _studentServices.GetStudentByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(_student.Id, result.Id);
    }

    [Fact]
    public async Task GetStudentByIdAsync_WhenCalledWithInvalidId_ThrowsError()
    {
        // Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(async () => await _studentServices.GetStudentByIdAsync(200));
    }

    [Fact]
    public async Task AddStudentAsync_WhenCalledWithValidStudent_ReturnsNewStudent()
    {
        // Arrange
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);

        // Act
        var result = validationResult.IsValid 
            ? await _studentServices.AddStudentAsync(_studentRequestDto) 
            : null;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_student.Id, result.Id);
    }

    [Fact]
    public async Task AddStudentAsync_WhenCalledWithInvalidStudent_ReturnsNull()
    {
        
        // Arrange
        _studentRequestDto.Email = null;
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);

        // Act
        var result = validationResult.IsValid 
            ? await _studentServices.AddStudentAsync(_studentRequestDto) 
            : null;

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Null(result);
    }

    [Fact]
    public async Task AddStudentAsync_WhenCalledWithWeakPassword_ReturnsNull()
    {
        // Arrange
        _studentRequestDto.Password = "123";
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);
        
        // Act
        var result = validationResult.IsValid
            ? await _studentServices.AddStudentAsync(_studentRequestDto)
            : null;
        
        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStudentAsync_WhenCalledWithValidStudent_ReturnsStudent()
    {
        
        // Arrange
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);
        
        // Act
        var result = validationResult.IsValid 
            ? await _studentServices.UpdateStudentAsync(1, _studentRequestDto) 
            : null;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(_student.Id, result.Id);
    }
    
    [Fact]
    public async Task UpdateStudentAsync_WhenCalledWithInvalidStudent_ReturnsNull()
    {
        // Arrange
        _studentRequestDto.Email = null;
       
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);
        
        // Act
        var result = validationResult.IsValid 
            ? await _studentServices.UpdateStudentAsync(1, _studentRequestDto) 
            : null;
        
        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStudentAsync_WhenCalledWithInvalidId_ThrowsError()
    {
        // Arrange
        _studentRequestDto.Email = null;
        var validationResult = await _validator.ValidateAsync(_studentRequestDto);

        // Act
        var result = validationResult.IsValid
            ? await _studentServices.UpdateStudentAsync(2, _studentRequestDto)
            : null;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_WhenCalledWithValidId_ReturnsTrue()
    {
        // Act
        var result = await _studentServices.DeleteStudentAsync(1);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_WhenCalledWithInvalidId_ThrowsError()
    {
        await Assert.ThrowsAsync<StudentNotFoundException>(async () => await _studentServices.DeleteStudentAsync(200));
    }
}