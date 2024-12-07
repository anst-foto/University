using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Services;

public abstract class TeacherServiceBase : ITeacherService
{
    private readonly DataBaseContext _context;
    
    public IEnumerable<Teacher> Teachers => _context.Teachers;
    public IEnumerable<Faculty> Faculties => _context.Faculties;
    public IEnumerable<Subject> Subjects => _context.Subjects;

    protected TeacherServiceBase(DataBaseContext context)
    {
        _context = context;
        _context.Teachers
            .Include(t => t.Faculty)
            .Include(t => t.Subjects)
            .ToList();
    }

    public virtual Teacher? GetTeacher(Guid id) => _context.Teachers.SingleOrDefault(t => t.Id == id);

    public virtual IEnumerable<Teacher>? GetTeachers(string name) => _context.Teachers
        .Where(t => t.LastName.ToLower().Contains(name.ToLower()) ||
                    t.FirstName.ToLower().Contains(name.ToLower()));
    

    public virtual void AddTeacher(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        _context.SaveChanges();
    }

    public virtual void DeleteTeacher(Guid id)
    {
        var teacher = _context.Teachers.Single(t => t.Id == id);
        _context.Teachers.Remove(teacher);
        _context.SaveChanges();
    }

    public virtual void UpdateTeacher(Teacher teacher)
    {
        var teacherToUpdate = GetTeacher(teacher.Id);
        
        if (teacherToUpdate is null) return;
        
        teacherToUpdate.LastName = teacher.LastName;
        teacherToUpdate.FirstName = teacher.FirstName;
        teacherToUpdate.Faculty = teacher.Faculty;
        teacherToUpdate.Subjects = teacher.Subjects.ToList();
        
        _context.SaveChanges();
    }
}