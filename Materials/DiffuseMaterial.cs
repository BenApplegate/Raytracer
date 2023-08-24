using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class DiffuseMaterial : Material
{
    private Color _albedo;

    public DiffuseMaterial(Color albedo)
    {
        _albedo = albedo;
    }

    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        //Calculate random next direction
        Vector3 nextDirection = Utility.RandomDirection();
        if (Vector3.Dot(nextDirection, hit.hitNormal) < 0)
        {
            nextDirection *= -1;
        }

        ray.color *= _albedo * Vector3.Dot(hit.hitNormal, nextDirection);
        
        //update ray to point in new bounce direction
        ray.origin = hit.hitLocation;
        ray.direction = nextDirection;
    }

    public void ProcessAlbedo(ref Ray ray, ref RayHit hit)
    {
        ray.gatheredColor = _albedo;
    }

    public void ProcessNormal(ref Ray ray, ref RayHit hit)
    {
        float r = hit.hitNormal.X * .5f + .5f;
        float g = hit.hitNormal.Y * .5f + .5f;
        float b = hit.hitNormal.Z * .5f + .5f;
        ray.gatheredColor = new Color(r, g, b);
    }

    public GPUMaterial GetGPUMaterial()
    {
        return new GPUMaterial() { type = 1, color = _albedo };
    }
}