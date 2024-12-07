using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using University.Models;

namespace University.Services;

public class TeacherService
{
    private readonly DataBaseContext _context;
    private readonly IMemoryCache _memoryCache;
    
    public IEnumerable<Teacher> Teachers => _context.Teachers;
    public IEnumerable<Faculty> Faculties => _context.Faculties;
    public IEnumerable<Subject> Subjects => _context.Subjects;

    public TeacherService(DataBaseContext context)
    {
        _context = context;
        
        _context.Teachers
            .Include(t => t.Faculty)
            .Include(t => t.Subjects)
            .ToList();

        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public void AddTeacher(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        _context.SaveChanges();
        
        _memoryCache.Set(teacher.Id, teacher);
    }

    public void DeleteTeacher(Guid id)
    {
        var teacher = _context.Teachers.Single(t => t.Id == id);
        _context.Teachers.Remove(teacher);
        _context.SaveChanges();
        
        _memoryCache.Remove(id);
    }

    public Teacher? GetTeacher(Guid id)
    {
        var result = _memoryCache.TryGetValue(id, out Teacher? teacher);

        if (result) return teacher;
        
        
        teacher = _context.Teachers.SingleOrDefault(t => t.Id == id);
        _memoryCache.Set(id, teacher);
            
        return teacher;
    }

    public IEnumerable<Teacher>? GetTeacher(string name) => _context.Teachers
        .Where(t => t.LastName.ToLower().Contains(name.ToLower()) ||
                t.FirstName.ToLower().Contains(name.ToLower()));

    public void UpdateTeacher(Teacher teacher)
    {
        var result = _memoryCache.TryGetValue(teacher.Id, out Teacher? _);
        if (result)
        {
            _memoryCache.Remove(teacher.Id);
            _memoryCache.Set(teacher.Id, teacher);
        }
        
        var teacherToUpdate = GetTeacher(teacher.Id);
        
        if (teacherToUpdate is null) return;
        
        teacherToUpdate.LastName = teacher.LastName;
        teacherToUpdate.FirstName = teacher.FirstName;
        teacherToUpdate.Faculty = teacher.Faculty;
        teacherToUpdate.Subjects = teacher.Subjects.ToList();
        
        _context.SaveChanges();
        
        _memoryCache.Set(teacher.Id, teacherToUpdate);
    }
}