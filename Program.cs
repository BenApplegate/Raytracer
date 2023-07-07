using System.Numerics;
using Raytracer;
using Raytracer.Core;
using Raytracer.Objects;

class Program
{
    public static void Main(string[] args)
    {
        Logger.Info("Raytracer Initializing");

        Scene scene = new Scene("BasicScene");
        scene.AddCamera(new Camera());
        scene.AddCamera(new Camera(new Vector3(0, 0, 10), new Vector3(0, 180, 0), 90, 1080, 720));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 5), 1));
        scene.RenderCamera(0);
        scene.RenderCamera(1);
        scene.SaveAllCameras();
    }
}