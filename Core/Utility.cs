using System.Numerics;

namespace Raytracer.Core;

public static class Utility
{
    public static Random _globalRandom = Random.Shared;
    [ThreadStatic] public static Random? _random;

    //Returns a random normalized direction vector
    public static Vector3 RandomDirection()
    {
        if (_random is null) _random = new Random(_globalRandom.Next());
        float theta = (float)_random.NextDouble() * 2 * MathF.PI;
        float phi = (float)_random.NextDouble() * MathF.PI;

        float z = MathF.Sin(phi) * MathF.Cos(theta);
        float x = MathF.Sin(phi) * MathF.Sin(theta);
        float y = MathF.Cos(phi);

        return new Vector3(x, y, z);
    }

    public static void ResetRandom(int seed)
    {
        _random = new Random(seed);
    }

    public static float ComponentSum(this Vector3 v)
    {
        return v.X + v.Y + v.Z;
    }
}