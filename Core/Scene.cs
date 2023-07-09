using System.Diagnostics;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Core;

public class Scene
{
    private List<Camera> _cameras;
    private List<Renderable> _renderables;
    private string _name;
    private Material? _environment;

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

    public void SetEnvironment(Material material)
    {
        _environment = material;
    }

    public void AddRenderable(Renderable obj)
    {
        Logger.Info("Adding Renderable to scene");
        _renderables.Add(obj);
    }

    public void RenderAllCamerasProgressive(int maxLightBounces, int sampleCount = 1, int threadCount = 1, string filename = "", int denoiseEvery = 10)
    {
        //Create a task that runs in new thread just waiting for input
        Task inputTask = Task.Run(() =>
        {
            Console.ReadKey(true);
            Logger.Important("Stopping Render");
        });

        int count = 0;
        
        Logger.Important("Generating Albedo Pass");
        RenderAllCameras(0, 1, 12, 1);
        SaveAllCameras(1, "ALBEDO_");
        
        Logger.Important("Generating Normal Pass");
        RenderAllCameras(0, 1, 12, 2);
        SaveAllCameras(1, "NORMAL_");

        //Run on a loop until the task has completed (User presses input
        while (!inputTask.IsCompleted)
        {
            count++;
            Logger.Important($"Starting next progressive samples\t{denoiseEvery-count} until denoise pass");
            for(int i = 0; i < _cameras.Count; i++)
            {
                //Set saved image for camera to render along side
                _cameras[i].AddStartingImage(filename+$"{_name}_cam{i}.png");
            }
            
            RenderAllCameras(maxLightBounces, sampleCount, threadCount);
            
            Logger.Important("Progressive samples finished rendering, saving files");
            
            SaveAllCameras(sampleCount, filename);

            if (count == denoiseEvery && denoiseEvery >= 1)
            {
                Logger.Important("Running Denoiser");

                for (int i = 0; i < _cameras.Count; i++)
                {
                    var proc = Process.Start("Denoiser", $"-i \"{filename + $"{_name}_cam{i}.png"}\" -o \"{filename + $"DN_{_name}_cam{i}.png"}\"" +
                                                         $" -a \"{filename + $"ALBEDO_{_name}_cam{i}.png"}\" -n \"{filename + $"NORMAL_{_name}_cam{i}.png"}\" -clean_aux 1 -hdr 0");
                    proc.WaitForExit();
                }

                count = 0;
            }
            
        }
    }
    
    public void RenderAllCameras(int maxLightBounces, int sampleCount = 100, int threadCount = 1, int renderPass=0)
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            RenderCamera(i, maxLightBounces, sampleCount, threadCount, renderPass);
        }
    }

    public void RenderCamera(int camIndex, int maxLightBounces, int sampleCount = 100, int threadCount = 1,
        int renderPass = 0)
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

        //Create list to store all tasks
        List<Task> tasks = new List<Task>();
        
        //Iterate over every ray from the camera and run raytracing algorithm
        var rays = _cameras[camIndex].GetCameraRays();
        for(int i = 0; i < rays.Count; i++)
        {
            int rayIndex = i;
            if ((i + 1) % 10000 == 0)
            {
                Logger.Info($"Now running ray {i+1}/{rays.Count}");
            }

            if (tasks.Count >= threadCount)
            {
                //Wait for existing tasks to finish
                foreach (Task t in tasks)
                {
                    t.Wait();
                }
                
                //Clear now old tasks from list
                tasks.Clear();
            }
            
            //Add each ray to thread pool
            var task = Task.Run(() => TraceRay(maxLightBounces, sampleCount, rays, rayIndex, renderPass));

            tasks.Add(task);
            //TraceRay(maxLightBounces, sampleCount, ref rays, i);
        }
        //Wait for all tasks to finish
        foreach (Task t in tasks)
        {
            t.Wait();
        }
        
        Logger.Info($"Finished Rendering Camera");
        //Send rays back to camera to save
        _cameras[camIndex].HandleProcessedRays(rays, sampleCount);
    }

    private void TraceRay(int maxLightBounces, int sampleCount, List<Ray> rays, int rayIndex, int pass = 0)
    {
        List<Ray> samples = new List<Ray>();
        for (int sample = 0; sample < sampleCount; sample++)
        {
            
            //Add this new ray to our samples
            samples.Add(rays[rayIndex]);
            Ray ray = samples[sample];
            
            //repeat tracing for every allowed light bounce
            for (int bounce = 0; bounce <= maxLightBounces; bounce++)
            {
                List<RayHit> hits = new List<RayHit>();

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
                    if (pass == 0)
                    {
                        //Process lighting info for hit
                        closestHit.material?.ProcessLighting(ref ray, ref closestHit);

                        closestHit.material?.UpdateNextRay(ref ray, ref closestHit);
                    }
                    else if (pass == 1)
                    {
                        //handle albedo pass
                        closestHit.material?.ProcessAlbedo(ref ray, ref closestHit);
                        closestHit.rayShouldContinue = false;
                    }
                    else if (pass == 2)
                    {
                        closestHit.material?.ProcessNormal(ref ray, ref closestHit);
                        closestHit.rayShouldContinue = false;
                    }
                }
                else
                {
                    //Handle environment light
                    if (_environment is not null)
                    {
                        if (pass == 0)
                        {
                            _environment.ProcessLighting(ref ray, ref closestHit);
                            _environment.UpdateNextRay(ref ray, ref closestHit);
                        }
                        else if (pass == 1)
                        {
                            _environment.ProcessAlbedo(ref ray, ref closestHit);
                            closestHit.rayShouldContinue = false;
                        }
                        else if (pass == 2)
                        {
                            _environment.ProcessNormal(ref ray, ref closestHit);
                            closestHit.rayShouldContinue = false;
                        }
                    }
                    else
                    {
                        //If there is no set environment, stop bounces
                        break;
                    }
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
        rays[rayIndex] = finalRay;
    }

    public void SaveAllCameras(int samples, string location = "")
    {
        for(int i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].SaveImage(location+$"{_name}_cam{i}.png", samples);
        }
    }
}