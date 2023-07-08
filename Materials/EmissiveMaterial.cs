using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class EmissiveMaterial : Material
{
    private Color _color;
    private float _strength;

    public EmissiveMaterial(Color color, float strength)
    {
        _color = color;
        _strength = strength;
    }

    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        ray.color *= _color;
        ray.gatheredColor += (ray.color * _strength);
    }

    public void UpdateNextRay(ref Ray ray, ref RayHit hit)
    {
        hit.rayShouldContinue = false;
    }
}