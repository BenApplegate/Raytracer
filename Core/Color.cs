namespace Raytracer.Core;

public class Color
{
    public float r;
    public float g;
    public float b;

    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public System.Drawing.Color ToSystemColor()
    {
        return System.Drawing.Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    public static Color operator*(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b);
    }
}