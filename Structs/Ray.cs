using System.Numerics;
using Raytracer.Core;

namespace Raytracer.Structs;

public struct Ray
{
    public Vector3 origin;
    public Vector3 direction;
    public int canvasX;
    public int canvasY;
    public Color color;
    public Color gatheredColor;
}