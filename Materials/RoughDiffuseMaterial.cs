using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class RoughDiffuseMaterial : Material
{
    private Color _albedo;
    private float _roughness;

    public RoughDiffuseMaterial(Color albedo, float roughness)
    {
        _albedo = albedo;
        _roughness = roughness;
    }

    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        //Calculate random next direction
        Vector3 nextDirection = Utility.RandomDirection();
        if (Vector3.Dot(nextDirection, hit.hitNormal) < 0)
        {
            nextDirection *= -1;
        }

        nextDirection = (_roughness * nextDirection) +
                        ((1 - _roughness) * Vector3.Reflect(ray.direction, hit.hitNormal));

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
        throw new NotImplementedException();
    }
}