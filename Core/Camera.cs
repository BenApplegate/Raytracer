using System.Drawing;
using System.Net;
using System.Numerics;
using Raytracer.Structs;

namespace Raytracer.Core;

public class Camera
{
    private Vector3 _location;
    private Vector3 _rotation;
    private float _hFov;
    private float _vFov;
    private int _canvasXResolution;
    private int _canvasYResolution;
    private Image _canvas;
    private Bitmap? _startingImage = null;
    private int? _startingImageSampleCount = null;

    private float Lerp(float a, float b, float t)
    {
        return (a * (1 - t)) + (b * t);
    }
    
    public Camera(Vector3 location, Vector3 rotation, float fov, int xRes, int yRes)
    {
        Logger.Info($"Initializing new camera at {location}");
        _location = location;
        _rotation = rotation;
        _hFov = fov;
        _vFov = ((float)yRes / xRes) * _hFov;
        _canvasXResolution = xRes;
        _canvasYResolution = yRes;
        _canvas = new Image(xRes, yRes);
    }

    public Camera()
    {
        _location = Vector3.Zero;
        Logger.Info($"Initializing new camera at {_location}");
        _rotation = Vector3.Zero;
        _hFov = 70;
        _vFov = ((float)1080 / 1920) * _hFov;
        _canvasXResolution = 1920;
        _canvasYResolution = 1080;
        _canvas = new Image(1920, 1080);
    }

    public void SaveImage(string filename, int samples)
    {
        _startingImage?.Dispose();
        
        File.WriteAllText($"{filename}.state", (samples + (_startingImageSampleCount ?? 0)).ToString());
        
        _canvas.SaveToFile(filename);
    }

    public void AddStartingImage(string filename)
    {
        if (!File.Exists(filename))
        {
            Logger.Warn("Could not find provided starter image");
            return;
        }

        if (!File.Exists($"{filename}.state"))
        {
            Logger.Warn("Could not find renderer state file, cannot continue previous render");
            return;
        }
        
        Bitmap img = new Bitmap(filename);
        if (_canvasXResolution != img.Width || _canvasYResolution != img.Height)
        {
            Logger.Warn("Provided starter image has wrong resolution, cannot be used");
            return;
        }

        string state = File.ReadAllText($"{filename}.state");
        int existingSamples = int.Parse(state);
        Logger.Info($"Resuming image with {existingSamples} samples");

        _startingImage = img;
        _startingImageSampleCount = existingSamples;
    }

    public void HandleProcessedRays(List<Ray> rays, int samples)
    {
        foreach (Ray ray in rays)
        {
            Color c = ray.gatheredColor;
            if (_startingImage is not null)
            {
                var pColor = _startingImage.GetPixel(ray.canvasX, ray.canvasY);
                c *= samples;
                c += new Color(pColor.R / 255f, pColor.G / 255f, pColor.B / 255f) * _startingImageSampleCount!.Value;
                c /= samples + _startingImageSampleCount!.Value;
            }
            _canvas.SetPixel(ray.canvasX, ray.canvasY, c);
        }
    }
    
    public List<Ray> GetCameraRays()
    {
        List<Ray> rays = new List<Ray>();
        
        for (int y = 0; y < _canvasYResolution; y++)
        {
            for (int x = 0; x < _canvasXResolution; x++)
            {
                float degreesToRad = MathF.PI / 180f;

                float aspectRatio = (float)_canvasXResolution / _canvasYResolution;

                float xPos = aspectRatio * (x - (_canvasXResolution/2f))/_canvasXResolution;
                float yPos = (y - (_canvasYResolution/2f))/_canvasYResolution;
                yPos *= -1;
                float zPos = 1/MathF.Tan(_hFov * degreesToRad / 2);

                Vector3 direction = new Vector3(xPos, yPos, zPos);
                direction /= direction.Length();

                Matrix4x4 rotation = Matrix4x4.CreateFromYawPitchRoll(_rotation.Y * degreesToRad,
                    _rotation.X * degreesToRad, _rotation.Z * degreesToRad);

                direction = Vector3.Transform(direction, rotation);

                rays.Add(new Ray()
                {
                    origin = _location,
                    direction = direction,
                    canvasX = x,
                    canvasY = y,
                    color = new Color(1, 1, 1),
                    gatheredColor = new Color(0, 0, 0),
                    bounces = 0
                });
            }
        }

        return rays;
    }
}