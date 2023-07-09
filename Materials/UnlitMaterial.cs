using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class UnlitMaterial : Material
{
    private Color _color;

    public UnlitMaterial(Color color)
    {
        _color = color;
    }
    
    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        if(ray.bounces != 0) return;
        
        ray.color *= _color;
        ray.gatheredColor += _color;
    }

    public void UpdateNextRay(ref Ray ray, ref RayHit hit)
    {
        hit.rayShouldContinue = false;
    }

    public void ProcessAlbedo(ref Ray ray, ref RayHit hit)
    {
        ray.gatheredColor = _color;
    }

    public void ProcessNormal(ref Ray ray, ref RayHit hit)
    {
        float r = hit.hitNormal.X * .5f + .5f;
        float g = hit.hitNormal.Y * .5f + .5f;
        float b = hit.hitNormal.Z * .5f + .5f;
        ray.gatheredColor = new Color(r, g, b);
    }
}