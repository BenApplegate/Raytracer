using System.Numerics;
using Raytracer;
using Raytracer.Core;

class Program
{
    public static void Main(string[] args)
    {
        Logger.Info("Raytracer Initializing");

        Scene scene = new Scene("BasicScene");
        scene.AddCamera(new Camera());
        scene.AddCamera(new Camera(new Vector3(0, 0, -5), new Vector3(0, 0, 0), 90, 1080, 720));
        scene.SaveAllCameras();
    }
}