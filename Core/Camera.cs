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
}