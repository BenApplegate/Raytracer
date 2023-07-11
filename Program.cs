using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Raytracer;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Objects;
using Plane = Raytracer.Objects.Plane;

class Program
{
    public static void Main(string[] args)
    {
        var watch = Stopwatch.StartNew();
        Logger.Info("Raytracer Initializing");
        //Logger.SuppressInfo(true);

        Scene scene = new Scene("BasicScene");

        int samples = 1;
        
        // UnlitTestScene(scene, samples);

        // BasicDiffuseScene(scene, samples);

        //ComplexDiffuseSceneProgressive(scene, samples);

        //PlaneTestScene(scene, samples);

        // PlaneBackfaceScene(scene, samples);

        //ProjectionTestScene(scene, samples);

        TriangleTestScene(scene, samples);
        
        scene.SaveAllCameras(samples);
        
        watch.Stop();
        Logger.Info($"Render took {watch.Elapsed}");
    }

    private static void TriangleTestScene(Scene scene, int samples)
    {
        scene.AddCamera(new Camera(new Vector3(0, 0, -10), Vector3.Zero, 90, 1280, 720));
        
        scene.AddRenderable(new Triangle(new Vector3(-1, -1, 0), new Vector3(1, 0, 0), new Vector3(-1, 1, 0), new UnlitMaterial(new Color(1, 1, 1))));
        
        scene.RenderAllCameras(1, samples, 30);
    }

    private static void ProjectionTestScene(Scene scene, int samples)
    {
        scene.AddCamera(new Camera(new Vector3(0, 0, -15), Vector3.Zero, 90, 1280, 720));
        scene.AddCamera(new Camera(new Vector3(0, 15, 0), new Vector3(90, 0, 0), 90, 1280, 720));
        
        scene.AddRenderable(new Sphere(new Vector3(5, 0, 5), 1, new DiffuseMaterial(new Color(1, 0, 0))));
        scene.AddRenderable(new Sphere(new Vector3(-5, 0, 5), 1, new DiffuseMaterial(new Color(1, 0, 0))));
        scene.AddRenderable(new Sphere(new Vector3(-5, 0, -5), 1, new DiffuseMaterial(new Color(1, 0, 0))));
        scene.AddRenderable(new Sphere(new Vector3(5, 0, -5), 1, new DiffuseMaterial(new Color(1, 0, 0))));
        scene.AddRenderable(new Sphere(new Vector3(0, 0, 0), 1, new DiffuseMaterial(new Color(1, 0, 0))));
        scene.AddRenderable(new Plane(new Vector3(0, 1, 0), new Vector3(0, -1, 0),
            new DiffuseMaterial(new Color(.8f, .8f, .8f))));
        
        scene.SetEnvironment(new AmbientEnvironmentMaterial(new Color(2, 2, 2)));
        
        scene.RenderAllCameras(1, samples, 12);
    }

    private static void PlaneBackfaceScene(Scene scene, int samples)
    {
        Camera cam = new Camera(new Vector3(0, 5, -5), new Vector3(45, 0, 0), 90, 1280, 720);
        scene.AddCamera(cam);
        cam.AddStartingImage("BasicScene_cam0.png");
        
        scene.AddRenderable(new Plane(new Vector3(0, 1, 0), Vector3.Zero, new DiffuseMaterial(new Color(1, 1, 1))));
        scene.AddRenderable(new Plane(new Vector3(0, 0, 1), Vector3.Zero, new DiffuseMaterial(new Color(0, 0, 1))));
        
        scene.SetEnvironment(new AmbientEnvironmentMaterial(new Color(2, 2, 2)));
        
        scene.RenderAllCameras(10, samples, 12);
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

    private static void ComplexDiffuseSceneProgressive(Scene scene, int samples)
    {
        //TEST COMPLEX DIFFUSE
        scene.AddRenderable(new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0), new DiffuseMaterial(new Color(.85f, .85f, .85f))));
        scene.AddRenderable(new Sphere(new Vector3(0, 1.5f, 0), 1.5f, new DiffuseMaterial(new Color(0, .95f, .95f))));
        scene.AddRenderable(new Sphere(new Vector3(3, 1f, 1), 1, new DiffuseMaterial(new Color(.95f, 0, .95f))));
        scene.AddRenderable(new Sphere(new Vector3(-4, .75f, -.5f), .75f, new DiffuseMaterial(new Color(.95f, .95f, 0))));
        //scene.AddRenderable(new Sphere(new Vector3(-20, 20, 30), 10, new EmissiveMaterial(new Color(1, 1, 1), 35)));
        scene.SetEnvironment(new SkyEnvironmentMaterial());
        //scene.SetEnvironment(new AmbientEnvironmentMaterial(new Color(1, 1, 1)));
        
        Camera cam3 = new Camera(new Vector3(5, 3.5f, -10), new Vector3(10, -30, 0), 80, 1280, 720);

        scene.AddCamera(cam3);

        //scene.RenderAllCameras(5, samples, 32);
        //scene.SaveAllCameras(samples);
        
        scene.RenderAllCamerasProgressive(5, samples, 32, "", 5);
        
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