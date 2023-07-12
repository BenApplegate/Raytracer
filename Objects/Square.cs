using System.Numerics;
using Raytracer.Interfaces;

namespace Raytracer.Objects;

public static class Square
{
    public static Mesh Down(Vector3 origin, Material material, float scaleX, float scaleZ)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(-scaleX, 0, scaleZ) + origin, new Vector3(scaleX, 0, scaleZ) + origin, new Vector3(-scaleX, 0, -scaleZ) + origin);
        mesh.AddTriangle(new Vector3(scaleX, 0, scaleZ) + origin, new Vector3(scaleX, 0, -scaleZ) + origin, new Vector3(-scaleX, 0, -scaleZ) + origin);
        return mesh;
    }
    
    public static Mesh Up(Vector3 origin, Material material, float scaleX, float scaleZ)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(-scaleX, 0, -scaleZ) + origin, new Vector3(scaleX, 0, -scaleZ) + origin, new Vector3(scaleX, 0, scaleZ) + origin);
        mesh.AddTriangle(new Vector3(-scaleX, 0, -scaleZ) + origin, new Vector3(scaleX, 0, scaleZ) + origin, new Vector3(-scaleX, 0, scaleZ) + origin);
        return mesh;
    }
    
    public static Mesh Right(Vector3 origin, Material material, float scaleZ, float scaleY)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(0, -scaleY, -scaleZ) + origin, new Vector3(0, -scaleY, scaleZ) + origin, new Vector3(0, scaleY, scaleZ) + origin);
        mesh.AddTriangle(new Vector3(0, -scaleY, -scaleZ) + origin, new Vector3(0, scaleY, scaleZ) + origin, new Vector3(0, scaleY, -scaleZ) + origin);
        return mesh;
    }
    
    public static Mesh Left(Vector3 origin, Material material, float scaleZ, float scaleY)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(0, -scaleY, scaleZ) + origin, new Vector3(0, -scaleY, -scaleZ) + origin, new Vector3(0, scaleY, scaleZ) + origin);
        mesh.AddTriangle(new Vector3(0, -scaleY, -scaleZ) + origin, new Vector3(0, scaleY, -scaleZ) + origin, new Vector3(0, scaleY, scaleZ) + origin);
        return mesh;
    }
    
    public static Mesh Back(Vector3 origin, Material material, float scaleX, float scaleY)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(-scaleX, -scaleY, 0) + origin, new Vector3(scaleX, -scaleY, 0) + origin, new Vector3(scaleX, scaleY, 0) + origin);
        mesh.AddTriangle(new Vector3(-scaleX, -scaleY, 0) + origin, new Vector3(scaleX, scaleY, 0) + origin, new Vector3(-scaleX, scaleY, 0) + origin);
        return mesh;
    }
    
    public static Mesh Forward(Vector3 origin, Material material, float scaleX, float scaleY)
    {
        Mesh mesh = new Mesh(material);
        mesh.AddTriangle(new Vector3(scaleX, -scaleY, 0) + origin, new Vector3(-scaleX, -scaleY, 0) + origin, new Vector3(-scaleX, scaleY, 0) + origin);
        mesh.AddTriangle(new Vector3(scaleX, -scaleY, 0) + origin, new Vector3(-scaleX, scaleY, 0) + origin, new Vector3(scaleX, scaleY, 0) + origin);
        return mesh;
    }
}