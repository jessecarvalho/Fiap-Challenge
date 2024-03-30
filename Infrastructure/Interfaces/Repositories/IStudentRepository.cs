using Core.Entities;

namespace Infrastructure.Interfaces.Repositories;

public interface IStudentRepository
{
    
    public Task<IEnumerable<Student>> GetAllAsync();
    public Task<Student?> GetByIdAsync(int id);
    public Task<Student> AddAsync(Student student);
    public Task<Student?> UpdateAsync(Student student);
    public Task<bool?> DeleteAsync(int id);

}