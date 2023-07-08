using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Materials;

public class AmbientEnvironmentMaterial : Material
{
    private Color _color;

    public AmbientEnvironmentMaterial(Color color)
    {
        _color = color;
    }


    public void ProcessLighting(ref Ray ray, ref RayHit hit)
    {
        ray.gatheredColor += ray.color * _color;
    }

    public void UpdateNextRay(ref Ray ray, ref RayHit hit)
    {
        hit.rayShouldContinue = false;
    }
}