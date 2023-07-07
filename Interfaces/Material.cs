using Raytracer.Core;
using Raytracer.Structs;

namespace Raytracer.Interfaces;

public interface Material
{
    public void ProcessLighting(ref Ray ray, ref RayHit hit);
}