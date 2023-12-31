﻿using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Structs;

namespace Raytracer.Objects;

public class Sphere : Renderable
{
    private Vector3 _location;
    private float _radius;
    private Material _material;

    public Sphere(Vector3 location, float radius, Material material)
    {
        this._location = location;
        this._radius = radius;
        this._material = material;
    }
    
    //Thanks to The Art of Code youtube channel for
    //Ray-Sphere Intersection math
    //https://www.youtube.com/watch?v=HFPlKQGChpE
    public RayHit Render(ref Ray ray)
    {
        Vector3 rayOrigin = ray.origin - _location;

        //Use dot product to find t value of point on line closest to center
        float tCenter = -Vector3.Dot(rayOrigin, ray.direction);
        float centerDistance = (rayOrigin + ray.direction * tCenter).Length();
        float underSqrt = _radius * _radius - centerDistance * centerDistance;

        if (underSqrt < 0)
        {
            //Ray falls outside of sphere
            return new RayHit() { didHit = false, hitLocation = Vector3.Zero, distance = 0, hitNormal = Vector3.Zero };
        }

        float tOffset = MathF.Sqrt(underSqrt);
        float firstIntersection = tCenter - tOffset;

        if (firstIntersection < 0)
        {
            //Ray intersects behind camera, discard
            return new RayHit() { didHit = false, hitLocation = Vector3.Zero, distance = 0, hitNormal = Vector3.Zero };
        }
        
        //We have found a hit, now we must calculate actual positions
        Vector3 hitPos = ray.origin + ray.direction * firstIntersection;
        float distance = (ray.direction * firstIntersection).Length();
        
        //Calculate normal by finding location of hit around unit sphere
        Vector3 normalHitPos = rayOrigin + ray.direction * firstIntersection;
        Vector3 normalVector = normalHitPos / normalHitPos.Length();

        RayHit hit = new RayHit() { didHit = true, hitLocation = hitPos, distance = distance, hitNormal = normalVector, material = _material, rayShouldContinue = true};
        
        return hit;
    }

    public (GPURenderable, GPUMaterial) GetGPUData()
    {
        GPURenderable renderable = new GPURenderable
        {
            origin = _location,
            radius = _radius,
            objectType = 0
        };

        return (renderable, _material.GetGPUMaterial());
    }
}