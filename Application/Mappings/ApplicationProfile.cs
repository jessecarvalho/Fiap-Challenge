using Application.Dto;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<CourseRequestDto, Course>();
        CreateMap<Course, CourseResponseDto>();
        CreateMap<StudentRequestDto, Student>();
        CreateMap<Student, StudentResponseDto>();
    }
}