using Raytracer.Core;
using Raytracer.GPU;
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
        hit.rayShouldContinue = false;
    }

    public void ProcessAlbedo(ref Ray ray, ref RayHit hit)
    {
        ray.gatheredColor = _color.Normalize();
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