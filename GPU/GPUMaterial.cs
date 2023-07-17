using Raytracer.Core;
using Raytracer.Structs;

namespace Raytracer.GPU;

public struct GPUMaterial
{
    public int type;
    public Color color;

    public static Color GetAddedGatheredLight(Color rayColor, Ray ray, GPUMaterial material)
    {
        if (material.type == 0)
        {
            return UnlitGatheredLight(rayColor, ray, material);
        }

        return new Color(0, 0, 0);
    }

    private static Color UnlitGatheredLight(Color rayColor, Ray ray, GPUMaterial material)
    {
        return material.color;
    }
}

