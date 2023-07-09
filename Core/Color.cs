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
        return System.Drawing.Color.FromArgb((int)(float.Clamp(r, 0, 1) * 255), (int)(float.Clamp(float.Clamp(g, 0, 1), 0, 1) * 255), (int)(float.Clamp(b, 0, 1) * 255));
    }

    public static Color operator*(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b);
    }

    public static Color operator +(Color a, Color b)
    {
        return new Color(a.r + b.r, a.g + b.g, a.b + b.b);
    }

    public static Color operator *(Color a, float b)
    {
        return new Color(a.r * b, a.g * b, a.b * b);
    }
    
    public static Color operator /(Color a, float b)
    {
        return new Color(a.r / b, a.g / b, a.b / b);
    }

    public Color Normalize()
    {
        float magnitude = r * r + b * b + g * g;
        magnitude = MathF.Sqrt(magnitude);
        return this / magnitude;
    }
}