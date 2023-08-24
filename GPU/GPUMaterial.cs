using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Structs;

namespace Raytracer.GPU;

public struct GPUMaterial
{
    public int type;
    public Color color;
    public float roughness;
    public float strength;

    public static GPUMaterialResult GetAddedGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material)
    {
        if (material.type == 0)
        {
            return UnlitGatheredLight(rayColor, hit, material);
        }

        if (material.type == 1)
        {
            return DiffuseGatheredLight(rayColor, hit, material);
        }

        if (material.type == 2)
        {
            return EmissiveGatheredLight(rayColor, hit, material);
        }

        return new GPUMaterialResult(){gatheredColor = new Color(0, 0, 0), shouldContinue = false};
    }

    private static GPUMaterialResult UnlitGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material)
    {
        return new GPUMaterialResult(){gatheredColor = material.color, shouldContinue = false, setFinalColor = true};
    }

    private static GPUMaterialResult DiffuseGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material)
    {
        return new GPUMaterialResult(){gatheredColor = material.color, shouldContinue = true, continueOrigin = hit.hitLocation, continueDirection = hit.hitNormal};
    }

    private static GPUMaterialResult EmissiveGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material)
    {
        return new GPUMaterialResult(){gatheredColor = (rayColor * material.color * material.strength), shouldContinue = false, setFinalColor = true};
    }
}

