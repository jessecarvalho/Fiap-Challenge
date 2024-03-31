using Application.Dto;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

using Infrastructure.Repositories;

public class StudentServices : IStudentServices
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentServices(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
        {
            throw new StudentNotFoundException();
        }
        return _mapper.Map<StudentResponseDto>(student);
    }

    public async Task<StudentResponseDto?> AddStudentAsync(StudentRequestDto studentRequestDto)
    {
        var student = _mapper.Map<Student>(studentRequestDto);

        student.Password = HashStudentPassword(student, studentRequestDto.Password);
        
        var newStudent = await _studentRepository.AddAsync(student);
        
        return _mapper.Map<StudentResponseDto>(newStudent);
    }

    public async Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentRequestDto studentRequestDto)
    {
        
        var studentExists = await _studentRepository.GetByIdAsync(id);

        if (studentExists == null)
        {
            throw new StudentNotFoundException();
        }

        var student = _mapper.Map<Student>(studentRequestDto);
        
        student.Password = HashStudentPassword(student, studentRequestDto.Password);

        student.Id = id;
        
        var updatedStudent = await _studentRepository.UpdateAsync(student);

        return _mapper.Map<StudentResponseDto>(updatedStudent);
        
    }

    public async Task<bool?> DeleteStudentAsync(int id)
    {
        var studentToDelete = await _studentRepository.GetByIdAsync(id);

        if (studentToDelete == null)
        {
            throw new StudentNotFoundException();
        }

        return await _studentRepository.DeleteAsync(id);
    }

    private string HashStudentPassword(Student student, string password)
    {
        var passwordHasher = new PasswordHasher<Student>();
        return passwordHasher.HashPassword(student, password);
    }
}