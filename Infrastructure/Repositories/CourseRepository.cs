using System.Diagnostics;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{

    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses.Include(s => s.Students).ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.Include(s => s.Students).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Course> AddAsync (Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course?> UpdateAsync (Course course)
    {
        var courseInDb = await _context.Courses.FindAsync(course.Id);

        if (courseInDb == null)
        {
            return null;
        }

        courseInDb.Title = course.Title;
        courseInDb.Description = course.Description;
        courseInDb.Year = course.Year;
        courseInDb.Students = course.Students;

        await _context.SaveChangesAsync();

        return courseInDb;
    }

    public async Task<bool?> DeleteAsync (int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return null;
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<Course> AddStudentToCourseAsync(int courseId, int studentId)
    {
        var course = await _context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null)
        {
            throw new Exception("Course not found");
        }

        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        course.Students?.Add(student);
        await _context.SaveChangesAsync();

        return course;
    }

    public async Task<bool> RemoveStudentFromCourseAsync(int courseId, int studentId)
    {
        var course = await _context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new Exception("Course not found");
        }

        var student = await _context.Students.FindAsync(studentId);

        if (student == null)
        {
            throw new Exception("Student not found");
        }

        course.Students?.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }
    
    public bool CheckIfCourseTitleIsUnique(Course course)
    {
        var courseTitle = course.Title;

        return _context.Courses.Any(x => x.Title == courseTitle);
    }
    
}