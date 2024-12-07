using System.IO;

namespace University.Services;

public static class Logger
{
    public static void Log(string message)
    {
        using var file = new StreamWriter("log.txt", append: true);
        file.WriteLine($"[{DateTime.Now:g}] {message}");
    }
}