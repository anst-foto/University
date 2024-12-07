using Microsoft.Extensions.Caching.Memory;
using University.Models;

namespace University.Services;

public class TeacherServiceWithMemoryCache : TeacherServiceBase, ITeacherService
{
    private readonly IMemoryCache _memoryCache;

    public TeacherServiceWithMemoryCache(DataBaseContext context) : base(context)
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }
    
    public override Teacher? GetTeacher(Guid id)
    {
        var result = _memoryCache.TryGetValue(id, out Teacher? teacher);

        if (result) return teacher;
        
        
        teacher = base.GetTeacher(id);
        _memoryCache.Set(id, teacher);
            
        return teacher;
    }
    
    public override IEnumerable<Teacher>? GetTeachers(string name) => base.GetTeachers(name);

    public override void AddTeacher(Teacher teacher)
    {
        base.AddTeacher(teacher);
        
        _memoryCache.Set(teacher.Id, teacher);
    }

    public override void DeleteTeacher(Guid id)
    {
        base.DeleteTeacher(id);
        
        _memoryCache.Remove(id);
    }
    
    public override void UpdateTeacher(Teacher teacher)
    {
        base.UpdateTeacher(teacher);
        
        var result = _memoryCache.TryGetValue(teacher.Id, out Teacher? _);
        if (result)
        {
            _memoryCache.Remove(teacher.Id);
        }
        _memoryCache.Set(teacher.Id, teacher);
    }
}