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
        ray.color *= _color;
        ray.gatheredColor += _color;
    }
}