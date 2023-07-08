namespace Raytracer.Structs;

public struct ThreadState
{
    public int maxLightBounces;
    public int sampleCount;
    public List<Ray> rays;
    public int rayIndex;
}