using System.Diagnostics;
using System.Numerics;
using Raytracer;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Objects;
using Plane = Raytracer.Objects.Plane;

class Program
{
    public static void Main(string[] args)
    {

        Logger.Info("Raytracer Initializing");

        Scene scene = new Scene("BasicScene");

        int samples = 600;
        
        //UnlitTestScene(scene, samples);

        //BasicDiffuseScene(scene, samples);

        //ComplexDiffuseScene(scene, samples);

        PlaneTestScene(scene, samples);

        scene.SaveAllCameras(samples);
    }

    private static void PlaneTestScene(Scene scene, int samples)
    {
        Camera cam = new Camera(new Vector3(0, 0, -5), new Vector3(0, 0, 0), 80, 1280, 720);
        scene.AddCamera(cam);
        cam.AddStartingImage("BasicScene_cam0.png");
        scene.SetEnvironment(new SkyEnvironmentMaterial());
        
        scene.AddRenderable(new Plane(new Vector3(-.5f, 1, 0), new Vector3(0, -1.5f, 0), new DiffuseMaterial(new Color(1f, 1f, 1f))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 0), 1, new DiffuseMaterial(new Color(1, .3f, 1))));
        
        scene.RenderAllCameras(4, samples, 12);
    }
    
    private static void BasicDiffuseScene(Scene scene, int samples)
    {
        //TEST BASIC DIFFUSE
        scene.AddCamera(new Camera(new Vector3(0, 0, -10), Vector3.Zero, 80, 1920, 1080));

        scene.AddRenderable(new Sphere(new Vector3(0, -101, 0), 100, new DiffuseMaterial(new Color(0, .2f, 1))));
        scene.AddRenderable(new Sphere(new Vector3(-2, 1, 0), .5f, new EmissiveMaterial(new Color(1, 1, 1), 40)));
        scene.AddRenderable(new Sphere(new Vector3(2, 1, 0), .5f, new EmissiveMaterial(new Color(1, 1, 1), 20)));

        scene.RenderAllCameras(1, samples, 16);
    }

    private static void UnlitTestScene(Scene scene, int samples)
    {
        SetupCameraRing(10, 32, ref scene);

        scene.AddRenderable(new Sphere(new Vector3(0, 0, 0), 1, new UnlitMaterial(new Color(0f, 0, 1f))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, -2), .25f, new UnlitMaterial(new Color(0f, 1, 0))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 3), .5f, new UnlitMaterial(new Color(1f, 0, 0f))));
        scene.AddRenderable(new Sphere(new Vector3(5, 1, 0), 2f, new UnlitMaterial(new Color(1f, 1, 0f))));
        scene.AddRenderable(new Sphere(new Vector3(-3, -2, 0), 1f, new UnlitMaterial(new Color(0f, 1, 1f))));

        scene.RenderAllCameras(1, samples, 16);
    }

    private static void ComplexDiffuseScene(Scene scene, int samples)
    {
        //TEST COMPLEX DIFFUSE
        scene.AddRenderable(new Sphere(new Vector3(0, -20, 0), 23, new DiffuseMaterial(new Color(1, 1, 1))));
        scene.AddRenderable(new Sphere(new Vector3(0, 5, 0), 1.5f, new DiffuseMaterial(new Color(1, .1f, .1f))));
        scene.AddRenderable(new Sphere(new Vector3(3, 4.5f, 1), 1, new DiffuseMaterial(new Color(.1f, .1f, 1))));
        scene.AddRenderable(new Sphere(new Vector3(-4, 4, -.5f), .75f, new DiffuseMaterial(new Color(.1f, 1, .1f))));
        //scene.AddRenderable(new Sphere(new Vector3(-20, 20, 30), 10, new EmissiveMaterial(new Color(1, 1, 1), 35)));
        scene.SetEnvironment(new SkyEnvironmentMaterial(sunStrength: 125f, dayStrength: .3f));

        Camera cam1 = new Camera(new Vector3(-1, 7, -10), new Vector3(20, 0, 0), 80, 1920, 1080);
        Camera cam2 = new Camera(new Vector3(-1, 7, -10), new Vector3(20, 0, 0), 80, 1920, 1080);
        Camera cam3 = new Camera(new Vector3(-1, 7, -10), new Vector3(20, 0, 0), 80, 1920, 1080);

        scene.AddCamera(cam1);
        scene.AddCamera(cam2);
        scene.AddCamera(cam3);

        cam1.AddStartingImage("BasicScene_cam0.png");
        cam2.AddStartingImage("BasicScene_cam1.png");
        cam3.AddStartingImage("BasicScene_cam2.png");

        var stopwatch = Stopwatch.StartNew();
        
        // scene.RenderCamera(0, 1, samples, 16);
        // stopwatch.Stop();
        // Logger.Warn($"Render took {stopwatch.Elapsed}");
        //
        // stopwatch = Stopwatch.StartNew();
        // scene.RenderCamera(1, 2, samples, 16);
        // stopwatch.Stop();
        // Logger.Warn($"Render took {stopwatch.Elapsed}");

        stopwatch = Stopwatch.StartNew();
        scene.RenderCamera(2, 3, samples, 16);
        stopwatch.Stop();
        Logger.Warn($"Render took {stopwatch.Elapsed}");
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