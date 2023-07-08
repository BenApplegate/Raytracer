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

        // SetupCameraRing(10, 32, ref scene);
        //
        // scene.AddRenderable(new Sphere(new Vector3(0, 0, 0), 1, new UnlitMaterial(new Color(0f, 0, 1f))));
        // scene.AddRenderable(new Sphere(new Vector3(0, 0, -2), .25f, new UnlitMaterial(new Color(0f, 1, 0))));
        // scene.AddRenderable(new Sphere(new Vector3(0, 0, 3), .5f, new UnlitMaterial(new Color(1f, 0, 0f))));
        // scene.AddRenderable(new Sphere(new Vector3(5, 1, 0), 2f, new UnlitMaterial(new Color(1f, 1, 0f))));
        // scene.AddRenderable(new Sphere(new Vector3(-3, -2, 0), 1f, new UnlitMaterial(new Color(0f, 1, 1f))));
        
        scene.AddCamera(new Camera(new Vector3(0, 0, -10), Vector3.Zero, 80, 1920, 1080));
        
        scene.AddRenderable(new Sphere(new Vector3(0, -101, 0), 100, new DiffuseMaterial(new Color(0, .2f, 1))));
        scene.AddRenderable(new Sphere(new Vector3(-2, 2, 0), .5f, new EmissiveMaterial(new Color(1, 1, 1), 1)));
        scene.AddRenderable(new Sphere(new Vector3(2, 2, 0), .5f, new EmissiveMaterial(new Color(1, 1, 1), 1)));
        
        scene.RenderAllCameras(1);
        
        scene.SaveAllCameras();
    }

    public static void SetupCameraRing(float radius, int count, ref Scene scene)
    {
        float degreesToRadians = MathF.PI / 180f;
        for (int i = 0; i < count; i++)
        {
            float zLocation = radius * float.Cos(((float)i / 32) * 360 * degreesToRadians);
            float xLocation = radius * float.Sin(((float)i / 32) * 360 * degreesToRadians);

            float yRotation = 180 + (((float)i/32) * 360); //((float)(180 + i) / 32) * 360;
            scene.AddCamera(new Camera(new Vector3(xLocation, 0, zLocation), new Vector3(0, yRotation, 0), 70, 1080, 720));
        }
    }
}