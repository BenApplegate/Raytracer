using System.Numerics;

namespace Raytracer.GPU;

public struct GPURenderable
{
    public Vector3 origin;
    public float radius;
    public int materialIndex;
    public int objectType;
}