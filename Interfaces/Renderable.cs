using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Structs;

namespace Raytracer.Interfaces;

public interface Renderable
{
    public RayHit Render(ref Ray ray);
    
    public (GPURenderable, GPUMaterial) GetGPUData();
}