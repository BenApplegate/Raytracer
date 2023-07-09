using System.Numerics;
using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class DiffuseMaterial : Material
{
    private Color _albedo;
    private Vector3 _nextRandomDirection;

    public DiffuseMaterial(Color albedo)
    {
        _albedo = albedo;
    }

    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        //Update next ray to be in a random direction in the hemisphere around the normal
        _nextRandomDirection = Utility.RandomDirection();
        
        //Check if random ray is in correct hemisphere, otherwise flip
        if (Vector3.Dot(_nextRandomDirection, hit.hitNormal) < 0)
        {
            _nextRandomDirection *= -1;
        }
        ray.color *= _albedo * Vector3.Dot(hit.hitNormal, _nextRandomDirection);
    }

    public void UpdateNextRay(ref Ray ray, ref RayHit hit)
    {
        
        
        //update ray origin to be the hit location, and the ray direction to be our random direction
        ray.origin = hit.hitLocation;
        ray.direction = _nextRandomDirection;
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
}