using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Core;

public class Scene
{
    private List<Camera> _cameras;
    private List<Renderable> _renderables;
    private string _name;

    public Scene(string name)
    {
        Logger.Info("Initializing new scene");
        _name = name;
        _cameras = new List<Camera>();
        _renderables = new List<Renderable>();
    }

    public void AddCamera(Camera cam)
    {
        Logger.Info("Adding Camera to scene");
        _cameras.Add(cam);
    }

    public void AddRenderable(Renderable obj)
    {
        Logger.Info("Adding Renderable to scene");
        _renderables.Add(obj);
    }

    public void RenderAllCameras()
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            RenderCamera(i);
        }
    }
    
    public void RenderCamera(int camIndex)
    {
        if (camIndex < 0 || camIndex >= _cameras.Count)
        {
            Logger.Warn("Selected camera is outside of range. Cannot render");
            return;
        }
        
        //Iterate over every ray from the camera and run raytracing algorithm
        var rays = _cameras[camIndex].GetCameraRays();
        int hitCount = 0;
        for(int i = 0; i < rays.Count; i++)
        {


            if ((i + 1) % 10000 == 0)
            {
                Logger.Info($"Now running ray {i+1}/{rays.Count}");
            }
            
            List<RayHit> hits = new List<RayHit>();
            Ray ray = rays[i];

            foreach (Renderable obj in _renderables)
            {
                hits.Add(obj.Render(ref ray));
            }

            bool hitSomething = false;
            RayHit closestHit = new RayHit();
            float closestDistance = float.PositiveInfinity;
            foreach (var hit in hits)
            {
                if (hit.didHit)
                {
                    hitCount++;
                    hitSomething = true;
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        closestHit = hit;
                    }
                }
            }
            
            //Process lighting info for hit
            if (hitSomething)
            {
                closestHit.material?.ProcessLighting(ref ray, ref closestHit);
            }
            
            //Actually save updated ray info
            rays[i] = ray;
        }
        
        Logger.Info($"Found {hitCount} hits");
        //Send rays back to camera to save
        _cameras[camIndex].HandleProcessedRays(rays);
    }

    public void SaveAllCameras(string location = "")
    {
        for(int i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].SaveImage(location+$"{_name}_cam{i}.png");
        }
    }
}