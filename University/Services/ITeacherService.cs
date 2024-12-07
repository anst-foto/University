using University.Models;

namespace University.Services;

public interface ITeacherService
{
    public IEnumerable<Teacher> Teachers { get; }
    public IEnumerable<Faculty> Faculties { get; }
    public IEnumerable<Subject> Subjects { get; }
    
    public Teacher? GetTeacher(Guid id);
    public IEnumerable<Teacher> GetTeachers(string name);

    public void AddTeacher(Teacher teacher);
    public void DeleteTeacher(Guid id);
    public void UpdateTeacher(Teacher teacher);
}