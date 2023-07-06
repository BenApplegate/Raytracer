using System.Numerics;

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

    public void SaveImage(string filename)
    {
        _canvas.SaveToFile(filename);
    }

    public List<Ray> GetCameraRays()
    {
        List<Ray> rays = new List<Ray>();
        
        for (int y = 0; y < _canvasYResolution; y++)
        {
            for (int x = 0; x < _canvasXResolution; x++)
            {
                float degreesToRad = MathF.PI / 180f;
                
                float hOffset = Lerp(-1 * (_hFov) / 2, _hFov / 2, (float)x / (_canvasXResolution - 1));
                float vOffset = Lerp(-1 * (_vFov / 2), _vFov / 2, (float)y / (_canvasYResolution - 1));

                float zPos = MathF.Cos((vOffset + _rotation.X) * degreesToRad) *
                             MathF.Cos((hOffset + _rotation.Y) * degreesToRad);
                
                float xPos = MathF.Cos((vOffset + _rotation.X) * degreesToRad) *
                             MathF.Sin((hOffset + _rotation.Y) * degreesToRad);

                float yPos = MathF.Sin((vOffset + _rotation.X) * degreesToRad);
                
                rays.Add(new Ray(_location, new Vector3(xPos, yPos, zPos)));
            }
        }

        return rays;
    }
}