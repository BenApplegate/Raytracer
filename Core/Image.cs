using System.Drawing;

namespace Raytracer.Core;

public class Image
{
    private Bitmap _bitmap;

    public Image(int width, int height)
    {
        _bitmap = new Bitmap(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if ((x/15 + y/15) % 2 == 0)
                {
                    SetPixel(x, y, 1, 0, 1);
                }
                else
                {
                    SetPixel(x, y, 0, 0, 0);
                }
            }
        }
        
        Logger.Info("Initialized Image");
    }

    public void SaveToFile(string location)
    {
        Logger.Info("Saving Image");
        _bitmap.Save(location);
        Logger.Info($"Saved Image to \"{location}\"");
    }

    public void SetPixelRGB(int x, int y, uint r, uint g, uint b)
    {
        if (r > 255 || g > 255 || b > 255)
        {
            Logger.Warn("Attempt to set pixel brighter than 255, value has been clamped");
        }

        r = uint.Clamp(r, 0, 255);
        g = uint.Clamp(g, 0, 255);
        b = uint.Clamp(b, 0, 255);
        _bitmap.SetPixel(x, y, Color.FromArgb((int) r, (int) g, (int) b));
    }

    public void SetPixel(int x, int y, float r, float g, float b)
    {
        SetPixelRGB(x, y, (uint)(r * 255), (uint)(g * 255), (uint)(b * 255));
    }
}