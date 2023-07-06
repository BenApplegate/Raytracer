using System.Numerics;
using Raytracer.Core;

namespace Raytracer.Interfaces;

public interface Renderable
{
    public RayHit Render(Ray ray);
}