using System.Numerics;
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

    public static GPUMaterialResult GetAddedGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material, GPURandomResult random, Ray ray)
    {
        if (material.type == 0)
        {
            return UnlitGatheredLight(rayColor, hit, material, random, ray);
        }

        if (material.type == 1)
        {
            return DiffuseGatheredLight(rayColor, hit, material, random, ray);
        }

        if (material.type == 2)
        {
            return EmissiveGatheredLight(rayColor, hit, material, random, ray);
        }

        return new GPUMaterialResult(){gatheredColor = new Color(0, 0, 0), shouldContinue = false};
    }

    private static GPUMaterialResult UnlitGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material, GPURandomResult random, Ray ray)
    {
        return new GPUMaterialResult(){gatheredColor = material.color, shouldContinue = false, setFinalColor = true};
    }

    private static GPUMaterialResult DiffuseGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material, GPURandomResult random, Ray ray)
    {
        GPUMaterialResult result = new GPUMaterialResult();
        
        Vector3 nextDirection = random.randomDirection;
        
        
        if (Vector3.Dot(nextDirection, hit.hitNormal) < 0.0f)
        {
            nextDirection *= -1.0f;
        }
        
        nextDirection = (material.roughness * nextDirection) +
                        ((1 - material.roughness) * Vector3.Reflect(ray.direction, hit.hitNormal));

        rayColor *= material.color * Vector3.Dot(nextDirection, hit.hitNormal);
        
        //update ray to point in new bounce direction
        result.gatheredColor = rayColor;
        result.continueOrigin = hit.hitLocation;
        result.continueDirection = nextDirection;
        result.shouldContinue = true;
        result.setFinalColor = false;
        return result;
    }

    private static GPUMaterialResult EmissiveGatheredLight(Color rayColor, GPUHit hit, GPUMaterial material, GPURandomResult random, Ray ray)
    {
        return new GPUMaterialResult(){gatheredColor = (rayColor * material.color * material.strength), shouldContinue = false, setFinalColor = true};
    }
}

