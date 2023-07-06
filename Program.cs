using Raytracer;
using Raytracer.Core;

class Program
{
    public static void Main(string[] args)
    {
        Logger.Info("Raytracer Initializing");

        Image img = new Image(1920, 1080);
        img.SaveToFile("test.png");
    }
}