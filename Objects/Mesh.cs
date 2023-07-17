using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Objects;

public class Mesh : Renderable
{
    private Material _material;
    private List<Triangle> _triangles;

    public Mesh(Material material)
    {
        _material = material;
        _triangles = new List<Triangle>();
    }

    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        _triangles.Add(new Triangle(p1, p2, p3, _material));
    }

    public RayHit Render(ref Ray ray)
    {
        List<RayHit> hits = new List<RayHit>();

        //Call render object for each renderable object
        foreach (Renderable obj in _triangles)
        {
            hits.Add(obj.Render(ref ray));
        }

        //Find the closest found hit to get correct location
        bool hitSomething = false;
        RayHit closestHit = new RayHit();
        float closestDistance = float.PositiveInfinity;
        foreach (var hit in hits)
        {
            if (hit.didHit)
            {
                hitSomething = true;
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                }
            }
        }

        if (!hitSomething)
        {
            return new RayHit()
            {
                didHit = false
            };
        }

        return closestHit;
    }

    public (GPURenderable, GPUMaterial) GetGPUData()
    {
        throw new NotImplementedException();
    }
}