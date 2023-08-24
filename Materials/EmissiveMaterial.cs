using Raytracer.Core;
using Raytracer.GPU;
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

    public GPUMaterial GetGPUMaterial()
    {
        return new GPUMaterial() { type = 2, color = _color, roughness = 0, strength = _strength };
    }
}