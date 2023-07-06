namespace Raytracer.Core;

public class Scene
{
    private List<Camera> _cameras;
    private string _name;

    public Scene(string name)
    {
        Logger.Info("Initializing new scene");
        _name = name;
        _cameras = new List<Camera>();
    }

    public void AddCamera(Camera cam)
    {
        Logger.Info("Adding Camera to scene");
        _cameras.Add(cam);
    }

    public void SaveAllCameras(string location = "")
    {
        for(int i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].SaveImage(location+$"{_name}_cam{i}.png");
        }
    }
}