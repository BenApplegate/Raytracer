using System.Numerics;
using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class SkyEnvironmentMaterial : Material
{
    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        Color nightColor = new Color(0, 0, .1f);
        float nightStrength = 0;

        Color highDayColor = new Color(.01f, 85f / 255, 1);
        Color lowDayColor = new Color(.05f, 110f / 255, 1);
        float dayStrength = .25f;

        Color sunColor = new Color(1, 1, 1);
        Vector3 sunDirection = new Vector3(-.2f, .7f, 1f);
        sunDirection = sunDirection / sunDirection.Length();
        float sunStrength = 100;
        float sunSize = .02f;

        float t = Vector3.Dot(ray.direction, new Vector3(0, 1, 0));
        
        if (t < 0)
        {
            if (ray.bounces == 0)
            {
                ray.gatheredColor += nightColor;
                return;
            }
            ray.gatheredColor += ray.color * nightColor * nightStrength;
            return;
        }

        float sunMatch = Vector3.Dot(ray.direction, sunDirection);
        if (1 - sunMatch < sunSize)
        {
            ray.gatheredColor += ray.color * sunColor * sunStrength;
            return;
        }

        Color skyColor = (lowDayColor * t) + (highDayColor * (1 - t));
        
        if (ray.bounces == 0)
        {
            //This ray directly hit the sky and should show the full color
            ray.gatheredColor += skyColor;
            return;
        }
        
        ray.gatheredColor += ray.color * skyColor * dayStrength;

    }

    public void UpdateNextRay(ref Ray ray, ref RayHit hit)
    {
        hit.rayShouldContinue = false;
    }
}