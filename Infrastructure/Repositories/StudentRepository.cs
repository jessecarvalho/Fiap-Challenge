using Core.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);

        return student;
    }

    public async Task<Student> AddAsync (Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync ();
        return student;
    }

    public async Task<Student?> UpdateAsync(Student student)
    {
        var studentInDb = await _context.Students.FindAsync(student.Id);

        if (studentInDb == null)
        {
            return null;
        }

        studentInDb.Name = student.Name;
        studentInDb.Email = student.Email;
        studentInDb.UserName = student.UserName;
        studentInDb.Password = student.Password;

        await _context.SaveChangesAsync();
        return studentInDb;

    }

    public async Task<bool?> DeleteAsync (int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return null;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
    
}