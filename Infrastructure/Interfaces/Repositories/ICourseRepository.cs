using Core.Entities;

namespace Infrastructure.Interfaces.Repositories;

public interface ICourseRepository
{

    public Task<IEnumerable<Course>> GetAllAsync();
    public Task<Course?> GetByIdAsync (int id);
    public Task<Course> AddAsync (Course course);
    public Task<Course?> UpdateAsync (Course course);
    public Task<bool?> DeleteAsync (int id);
    public bool CheckIfCourseTitleIsUnique(Course course);

}