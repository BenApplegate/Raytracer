using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Structs;

namespace Raytracer.Interfaces;

public interface Material
{
    public void ProcessLighting(ref Ray ray, ref RayHit hit);

    //public void UpdateNextRay(ref Ray ray, ref RayHit hit);

    public void ProcessAlbedo(ref Ray ray, ref RayHit hit);

    public void ProcessNormal(ref Ray ray, ref RayHit hit);

    public GPUMaterial GetGPUMaterial();
}