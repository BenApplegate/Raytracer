using System.Numerics;
using Raytracer;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Objects;

class Program
{
    public static void Main(string[] args)
    {
        Logger.Info("Raytracer Initializing");

        Scene scene = new Scene("BasicScene");

        SetupCameraRing(10, 32, ref scene);
        
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 0), 1, new UnlitMaterial(new Color(0f, 0, 1f))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, -1), .25f, new UnlitMaterial(new Color(0f, 1, 0))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 3), .5f, new UnlitMaterial(new Color(1f, 0, 0f))));
        scene.RenderCamera(0);
        scene.RenderCamera(1);
        scene.SaveAllCameras();
    }

    public static void SetupCameraRing(float radius, int count, ref Scene scene)
    {
        
    }
}