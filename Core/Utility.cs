using System.Numerics;

namespace Raytracer.Core;

public static class Utility
{
    private static Random _random = new Random();

    //Returns a random normalized direction vector
    public static Vector3 RandomDirection()
    {
        float theta = (float)_random.NextDouble() * 2 * MathF.PI;
        float phi = (float)_random.NextDouble() * MathF.PI;

        float z = MathF.Sin(phi) * MathF.Cos(theta);
        float x = MathF.Sin(phi) * MathF.Sin(theta);
        float y = MathF.Cos(phi);

        return new Vector3(x, y, z);
    }
}