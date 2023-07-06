using System.Numerics;
using Raytracer.Core;
using Raytracer.Interfaces;

namespace Raytracer.Objects;

public class Sphere : Renderable
{
    private Vector3 location;
    private float radius;

    public Sphere(Vector3 location, float radius)
    {
        this.location = location;
        this.radius = radius;
    }
    public RayHit Render(Ray ray)
    {
        Vector3 rayOrigin = ray.origin - location;


        return new RayHit() { didHit = false, hitLocation = Vector3.Zero, distance = 0, hitNormal = Vector3.Zero };
    }
    
}