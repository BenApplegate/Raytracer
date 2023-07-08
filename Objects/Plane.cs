using System.Numerics;
using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Objects;

public class Plane : Renderable
{
    private Vector3 _normal;
    private Vector3 _origin;
    private Material _material;

    public Plane(Vector3 normal, Vector3 origin, Material material)
    {
        _origin = origin;
        _material = material;
        _normal = normal / normal.Length();
    }
    public RayHit Render(ref Ray ray)
    {
        Vector3 offsetOrigin = ray.origin - _origin;

        if (Vector3.Dot(ray.direction, _normal) >= 0)
        {
            return new RayHit()
            {
                didHit = false
            };
        }

        Vector3 normalDot = offsetOrigin * _normal * -1;
        Vector3 rayDot = ray.direction * _normal;
        float t = normalDot.ComponentSum() / rayDot.ComponentSum();

        if (t < 0)
        {
            // Logger.Warn("Skipped due to negative t");
            return new RayHit()
            {
                didHit = false
            };
        }

        Vector3 hitPos = ray.origin + ray.direction * t;

        // Logger.Warn("Hit");
        
        return new RayHit()
        {
            didHit = true,
            hitLocation = hitPos,
            hitNormal = _normal,
            distance = t,
            material = _material,
            rayShouldContinue = true
        };
    }
}