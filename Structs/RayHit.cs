using System.Numerics;

namespace Raytracer.Core;

public struct RayHit
{
    public bool didHit;
    public Vector3 hitLocation;
    public float distance;
    public Vector3 hitNormal;
}