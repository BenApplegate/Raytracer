using System.Numerics;
using Raytracer.Structs;

namespace Raytracer.GPU;

public struct GPUHit
{
    public bool didHit;
    public Vector3 hitLocation;
    public float distance;
    public Vector3 hitNormal;
    
    public static GPUHit GetHit(Ray ray, GPURenderable renderable)
    {
        if (renderable.objectType == 0)
        {
            return RaySphere(ray, renderable);
        }
        
        GPUHit defaultReturn = new GPUHit
        {
            didHit = false
        };
        return defaultReturn;
    }

    //Thanks to The Art of Code youtube channel for
    //Ray-Sphere Intersection math
    //https://www.youtube.com/watch?v=HFPlKQGChpE
    private static GPUHit RaySphere(Ray ray, GPURenderable renderable)
    {
        Vector3 rayOrigin = ray.origin - renderable.origin;

        //Use dot product to find t value of point on line closest to center
        float tCenter = -Vector3.Dot(rayOrigin, ray.direction);
        float centerDistance = (rayOrigin + ray.direction * tCenter).Length();
        float underSqrt = renderable.radius * renderable.radius - centerDistance * centerDistance;

        if (underSqrt < 0)
        {
            //Ray falls outside of sphere
            return new GPUHit() { didHit = false};
        }
        
        float tOffset = MathF.Sqrt(underSqrt);
        float firstIntersection = tCenter - tOffset;

        if (firstIntersection < 0)
        {
            //Ray intersects behind camera, discard
            return new GPUHit() { didHit = false};
        }
        
        //We have found a hit, now we must calculate actual positions
        Vector3 hitPos = ray.origin + ray.direction * firstIntersection;
        float distance = (ray.direction * firstIntersection).Length();
        
        //Calculate normal by finding location of hit around unit sphere
        Vector3 normalHitPos = rayOrigin + ray.direction * firstIntersection;
        Vector3 normalVector = normalHitPos / normalHitPos.Length();

        GPUHit hit = new GPUHit() { didHit = true, hitLocation = hitPos, distance = distance, hitNormal = normalVector};
        
        return hit;
    }
}