using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Objects;

public class Triangle : Renderable
{
    private Vector3 _p1, _p2, _p3;
    private Vector3 _normal;
    private Material _material;

    public Triangle(Vector3 p1, Vector3 p2, Vector3 p3, Material material)
    {
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
        _material = material;

        Vector3 p12 = p2 - p1;
        Vector3 p13 = p3 - p1;
        _normal = -Vector3.Cross(p12, p13);
        _normal /= _normal.Length();
    }


    public RayHit Render(ref Ray ray)
    {
        Vector3 offsetOrigin = ray.origin - _p1;

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

        if (t < -0.001)
        {
            // Logger.Warn("Skipped due to negative t");
            return new RayHit()
            {
                didHit = false
            };
        }

        Vector3 hitPos = ray.origin + ray.direction * t;

        Vector3 p12 = _p2 - _p1;
        Vector3 p13 = _p3 - _p1;
        Vector3 p1T = hitPos - _p1;

        Vector3 cross1 = Vector3.Cross(p12, p1T);
        Vector3 cross2 = Vector3.Cross(p13, p1T);
        if(cross1.Length() > 0.0001) cross1 /= cross1.Length();
        if(cross2.Length() > 0.0001) cross2 /= cross2.Length();
        
        if(Vector3.Dot(cross1, cross2) > 0.999)
        {
            return new RayHit()
            {
                didHit = false
            };
        }

        Vector3 p21 = _p1 - _p2;
        Vector3 p23 = _p3 - _p2;
        Vector3 p2T = hitPos - _p2;

        cross1 = Vector3.Cross(p21, p2T);
        cross2 = Vector3.Cross(p23, p2T);
        if(cross1.Length() > 0.0001) cross1 /= cross1.Length();
        if(cross2.Length() > 0.0001) cross2 /= cross2.Length();
        
        if (Vector3.Dot(cross1, cross2) > 0.999)
        {
            return new RayHit()
            {
                didHit = false
            };
        }
        
        
        Vector3 p31 = _p1 - _p3;
        Vector3 p32 = _p2 - _p3;
        Vector3 p3T = hitPos - _p3;

        cross1 = Vector3.Cross(p31, p3T);
        cross2 = Vector3.Cross(p32, p2T);
        if(cross1.Length() > 0.0001) cross1 /= cross1.Length();
        if(cross2.Length() > 0.0001) cross2 /= cross2.Length();
        
        if (Vector3.Dot(cross1, cross2) > 0.999)
        {
            return new RayHit()
            {
                didHit = false
            };
        }
        
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

    public (GPURenderable, GPUMaterial) GetGPUData()
    {
        throw new NotImplementedException();
    }
}