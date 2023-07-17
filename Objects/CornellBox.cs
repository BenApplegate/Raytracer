using System.Numerics;
using Raytracer.Core;
using Raytracer.GPU;
using Raytracer.Interfaces;
using Raytracer.Materials;
using Raytracer.Structs;

namespace Raytracer.Objects;

public class CornellBox : Renderable
{
    private List<Renderable> _surfaces;

    public CornellBox(Vector3 origin, float xSize, float ySize, float zSize, float lightStrength)
    {
        float xSizeAdj = xSize;// + 1.001f;
        float ySizeAdj = ySize;// + 1.001f;
        float zSizeAdj = zSize;// + 1.001f;
        _surfaces = new List<Renderable>();
        _surfaces.Add(Square.Up(Vector3.Zero + origin, new DiffuseMaterial(new Color(.95f, .95f, .95f)), xSizeAdj, zSizeAdj));
        _surfaces.Add(Square.Down(new Vector3(0, ySize * 2, 0) + origin, new EmissiveMaterial(new Color(1, 1, 1), lightStrength), xSize - 2.5f, zSize - 2.5f));
        _surfaces.Add(Square.Down(new Vector3(0, ySize * 2, 0) + origin, new DiffuseMaterial(new Color(.95f, .95f, .95f)), xSizeAdj, zSizeAdj));
        _surfaces.Add(Square.Right(new Vector3(-xSize, ySize, 0) + origin, new DiffuseMaterial(new Color(0.95f, 0, 0)), zSizeAdj, ySizeAdj));
        _surfaces.Add(Square.Left(new Vector3(xSize, ySize, 0) + origin, new DiffuseMaterial(new Color(0, 0.95f, 0)), zSizeAdj, ySizeAdj));
        _surfaces.Add(Square.Back(new Vector3(0, ySize, zSize) + origin, new DiffuseMaterial(new Color(.95f, .95f, .95f)), xSizeAdj, ySizeAdj));
        _surfaces.Add(Square.Forward(new Vector3(0, ySize, -zSize) + origin, new DiffuseMaterial(new Color(0.95f, 0.95f, 0.95f)), xSizeAdj, ySizeAdj));
    }

    public RayHit Render(ref Ray ray)
    {
        List<RayHit> hits = new List<RayHit>();

        //Call render object for each renderable object
        foreach (Renderable obj in _surfaces)
        {
            hits.Add(obj.Render(ref ray));
        }

        //Find the closest found hit to get correct location
        bool hitSomething = false;
        RayHit closestHit = new RayHit();
        float closestDistance = float.PositiveInfinity;
        foreach (var hit in hits)
        {
            if (hit.didHit)
            {
                hitSomething = true;
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                }
            }
        }

        if (!hitSomething)
        {
            return new RayHit()
            {
                didHit = false
            };
        }

        return closestHit;
    }

    public (GPURenderable, GPUMaterial) GetGPUData()
    {
        throw new NotImplementedException();
    }
}