using System.Diagnostics;

namespace Raytracer;

public static class Logger
{
    private static bool _suppressInfo = false;

    public static void SuppressInfo(bool suppress)
    {
        _suppressInfo = suppress;
    }
    
    public static void Info(string message)
    {
        if(_suppressInfo) return;
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method is not null)
        {
            Console.WriteLine($"INFO {{{method.ReflectedType?.Name}:{method.Name}}} {{{DateTime.Now}}}: {message}");
        }
        else
        {
            Console.WriteLine($"INFO {{UnknownClass:UnknownMethod}} {{{DateTime.Now}}}: {message}");
        }
    }

    public static void Warn(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method is not null)
        {
            Console.WriteLine($"WARN {{{method.ReflectedType?.Name}:{method.Name}}} {{{DateTime.Now}}}: {message}");
        }
        else
        {
            Console.WriteLine($"WARN {{UnknownClass:UnknownMethod}} {{{DateTime.Now}}}: {message}");
        }

        Console.ForegroundColor = oldColor;
    }
    
    public static void Important(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method is not null)
        {
            Console.WriteLine($"IMPORTANT {{{method.ReflectedType?.Name}:{method.Name}}} {{{DateTime.Now}}}: {message}");
        }
        else
        {
            Console.WriteLine($"IMPORTANT {{UnknownClass:UnknownMethod}} {{{DateTime.Now}}}: {message}");
        }

        Console.ForegroundColor = oldColor;
    }

    public static void Error(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method is not null)
        {
            Console.Error.WriteLine($"ERROR {{{method.ReflectedType?.Name}:{method.Name}}} {{{DateTime.Now}}}: {message}");
        }
        else
        {
            Console.Error.WriteLine($"ERROR {{UnknownClass:UnknownMethod}} {{{DateTime.Now}}}: {message}");
        }

        Console.ForegroundColor = oldColor;
    }
}