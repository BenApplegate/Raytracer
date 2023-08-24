using System.Numerics;
using Raytracer.Core;

namespace Raytracer.GPU;

public struct GPUMaterialResult
{
    public Color gatheredColor;
    public Vector3 continueDirection;
    public Vector3 continueOrigin;
    public bool shouldContinue;
    public bool setFinalColor;
}