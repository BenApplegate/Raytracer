using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class SkyEnvironmentMaterial : Material
{
    
    Color nightColor = new Color(0.3f, .3f, .3f);
    float nightStrength = .1f;

    Color highDayColor = new Color(90f/255, 193f / 255, 237f/255);
    Color lowDayColor = new Color(.05f, 110f / 255, 1);
    float dayStrength = .25f;

    Color sunColor = new Color(1, 1, 0.95f);
    Vector3 sunDirection = new Vector3(-1.5f, 1.6f, 1f);
    
    float sunStrength = 60;
    float sunSize = .02f;

    public SkyEnvironmentMaterial(Color? nightColor = null, float? nightStrength = null,
        Color? highDayColor = null, Color? lowDayColor = null, float? dayStrength = null, Color? sunColor = null,
        Vector3? sunDirection = null, float? sunStrength = null, float? sunSize = null)
    {
        if (nightColor is not null) this.nightColor = nightColor.Value;
        if (nightStrength is not null) this.nightStrength = nightStrength.Value;
        if (highDayColor is not null) this.highDayColor = highDayColor.Value;
        if (lowDayColor is not null) this.lowDayColor = lowDayColor.Value;
        if (dayStrength is not null) this.dayStrength = dayStrength.Value;
        if (sunColor is not null) this.sunColor = sunColor.Value;
        if (sunDirection is not null) this.sunDirection = sunDirection.Value;
        if (sunStrength is not null) this.sunStrength = sunStrength.Value;
        if (sunSize is not null) this.sunSize = sunSize.Value;
        
        this.sunDirection /= this.sunDirection.Length();
    }
    
    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        hit.rayShouldContinue = false;

        float t = Vector3.Dot(ray.direction, new Vector3(0, 1, 0));
        
        if (t < 0)
        {
            if (ray.bounces == 0)
            {
                ray.gatheredColor = nightColor;
                return;
            }
            ray.gatheredColor += ray.color * nightColor * nightStrength;
            return;
        }

        float sunMatch = Vector3.Dot(ray.direction, sunDirection);
        if (1 - sunMatch < sunSize)
        {
            if (ray.bounces == 0)
            {
                ray.gatheredColor = sunColor;
                return;
            }
            ray.gatheredColor += ray.color * sunColor * sunStrength;
            return;
        }

        Color skyColor = (lowDayColor * t) + (highDayColor * (1 - t));
        
        if (ray.bounces == 0)
        {
            //This ray directly hit the sky and should show the full color
            ray.gatheredColor = skyColor;
            return;
        }
        
        ray.gatheredColor += ray.color * skyColor * dayStrength;

    }

    public void ProcessAlbedo(ref Ray ray, ref RayHit hit)
    {
        float t = Vector3.Dot(ray.direction, new Vector3(0, 1, 0));
        
        if (t < 0)
        {
            ray.gatheredColor = nightColor;
            return;
        }

        float sunMatch = Vector3.Dot(ray.direction, sunDirection);
        if (1 - sunMatch < sunSize)
        {
            ray.gatheredColor = sunColor;
            return;
        }

        Color skyColor = (lowDayColor * t) + (highDayColor * (1 - t));
        
        if (ray.bounces == 0)
        {
            //This ray directly hit the sky and should show the full color
            ray.gatheredColor = skyColor;
            return;
        }

        ray.gatheredColor += skyColor;
    }

    public void ProcessNormal(ref Ray ray, ref RayHit hit)
    {
        ray.gatheredColor = new Color(0, 0, 0);
    }

    public GPUMaterial GetGPUMaterial()
    {
        throw new NotImplementedException();
    }
}