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

    public void RenderAllCameras(int maxLightBounces, int sampleCount = 100)
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            RenderCamera(i, maxLightBounces, sampleCount);
        }
    }
    
    public void RenderCamera(int camIndex, int maxLightBounces, int sampleCount = 100)
    {
        if (sampleCount < 1)
        {
            Logger.Error("Provided sample count less than 1, cannot render");
            return;
        }

        if (maxLightBounces < 0)
        {
            Logger.Error("Provided max light bounces less than 0, cannot render");
            return;
        }
        
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

            List<Ray> samples = new List<Ray>();
            for (int sample = 0; sample < sampleCount; sample++)
            {
                //repeat tracing for every allowed light bounce
                for (int bounce = 0; bounce <= maxLightBounces; bounce++)
                {
                    List<RayHit> hits = new List<RayHit>();
                    
                    //Add this new ray to our samples
                    samples.Add(rays[i]);
                    Ray ray = samples[sample];
            
                    //Call render object for each renderable object
                    foreach (Renderable obj in _renderables)
                    {
                        hits.Add(obj.Render(ref ray));
                    }

                    //Find the closest found hit to get correct location
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
            
                    if (hitSomething)
                    {
                        //Process lighting info for hit
                        closestHit.material?.ProcessLighting(ref ray, ref closestHit);
                
                        closestHit.material?.UpdateNextRay(ref ray, ref closestHit);
                    }

                    //Update bounce count
                    if (closestHit.rayShouldContinue)
                    {
                        ray.bounces++;
                    }
            
                    //Save ray info back into this sample
                    samples[sample] = ray;
                }
            }
            
            //We now need to average all of our samples together to get the final pixel color
            Color finalColor = new Color(0, 0, 0);
            foreach (Ray sample in samples)
            {
                finalColor += sample.gatheredColor;
            }

            finalColor /= (float)samples.Count;

            Ray finalRay = samples[0];
            finalRay.gatheredColor = finalColor;
            
            
            //Save our finalized ray back to the image's rays;
            rays[i] = finalRay;

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