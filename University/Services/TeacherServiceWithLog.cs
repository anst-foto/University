using System.Text.Json;
using University.Models;

namespace University.Services;

public class TeacherServiceWithLog : TeacherServiceWithMemoryCache, ITeacherService
{
    
    public TeacherServiceWithLog(DataBaseContext context) : base(context)
    {
    }
    
    public override Teacher? GetTeacher(Guid id)
    {
        var teacher = base.GetTeacher(id);
        Logger.Log("GetTeacher called");
        return teacher;
    }

    public override IEnumerable<Teacher>? GetTeachers(string name)
    {
        var teachers = base.GetTeachers(name);
        Logger.Log("GetTeachers called");
        return teachers;
    }

    public override void AddTeacher(Teacher teacher)
    {
        base.AddTeacher(teacher);
        
        Logger.Log("Added teacher");
    }

    public override void DeleteTeacher(Guid id)
    {
        base.DeleteTeacher(id);
        
        Logger.Log("Deleted teacher");
    }
    
    public override void UpdateTeacher(Teacher teacher)
    {
        base.UpdateTeacher(teacher);
        
        Logger.Log("Updated teacher");
    }
}