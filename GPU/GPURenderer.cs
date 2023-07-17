using System.Diagnostics;
using ILGPU;
using ILGPU.Runtime;
using Raytracer.Core;
using Raytracer.Interfaces;
using Raytracer.Objects;
using Raytracer.Structs;

namespace Raytracer.GPU;

public static class GPURenderer
{
    private static Context _context;
    private static Device _device;
    private static Accelerator _accelerator;
    private static MemoryBuffer1D<Ray, Stride1D.Dense> _rayBuffer;
    private static MemoryBuffer1D<GPUResult, Stride1D.Dense> _resultBuffer;
    private static MemoryBuffer1D<GPURenderable, Stride1D.Dense> _renderableBuffer;
    private static MemoryBuffer1D<GPUMaterial, Stride1D.Dense> _materialBuffer;
    private static Action<Index1D, ArrayView<Ray>, ArrayView<GPURenderable>, ArrayView<GPUMaterial>, ArrayView<GPUResult>> _loadedKernel;

    public static void Initialize(Scene scene)
    {
        //Create GPU context
        _context = Context.CreateDefault();
        _device = _context.GetPreferredDevice(false);
        Logger.Important($"Initialized Rendering Device: {_device}");
        _accelerator = _device.CreateAccelerator(_context);

        _rayBuffer = _accelerator.Allocate1D(scene.GetGPURays());
        _resultBuffer = _accelerator.Allocate1D<GPUResult>(_rayBuffer.Length);

        //load all scene data
        int currentMaterial = 0;
        List<GPURenderable> renderables = new List<GPURenderable>();
        Dictionary<GPUMaterial, int> materials = new Dictionary<GPUMaterial, int>();
        foreach (Renderable obj in scene.GetRenderables())
        {
            var gpuObject = obj.GetGPUData();
            if (!materials.ContainsKey(gpuObject.Item2))
            {
                materials[gpuObject.Item2] = currentMaterial;
                currentMaterial++;
            }

            gpuObject.Item1.materialIndex = materials[gpuObject.Item2];
            renderables.Add(gpuObject.Item1);
        }
        
        //Convert renderables and materials to buffers
        _renderableBuffer = _accelerator.Allocate1D(renderables.ToArray());
        GPUMaterial[] finalMaterials = new GPUMaterial[currentMaterial];
        foreach (var m in materials)
        {
            finalMaterials[m.Value] = m.Key;
        }

        _materialBuffer = _accelerator.Allocate1D(finalMaterials);
        
        //Load Kernel
        _loadedKernel = _accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<Ray>, ArrayView<GPURenderable>,
            ArrayView<GPUMaterial>, ArrayView<GPUResult>>(Kernel);
    }

    public static GPUResult[] Render()
    {
        Stopwatch watch = Stopwatch.StartNew();
        _loadedKernel((int) _rayBuffer.Length, _rayBuffer.View, _renderableBuffer.View, _materialBuffer.View,
            _resultBuffer.View);
        
        _accelerator.Synchronize();
        watch.Stop();
        
        Logger.Important($"GPU render took {watch.Elapsed}");
        
        var result = _resultBuffer.GetAsArray1D();
        return result;

    }

    private static void Kernel(Index1D index, ArrayView<Ray> rays, ArrayView<GPURenderable> renderables,
        ArrayView<GPUMaterial> materials, ArrayView<GPUResult> output)
    {
        Ray ray = rays[index];
        GPUResult rayResult;
        
        //Find closest hit among renderables
        GPUHit closesetHit = new GPUHit() { didHit = false, distance = float.PositiveInfinity};
        GPURenderable closestRenderable = new GPURenderable();

        for (int i = 0; i < renderables.IntLength; i++)
        {
            GPURenderable renderable = renderables[i];
            GPUHit hit = GPUHit.GetHit(ray, renderable);
            if(!hit.didHit) continue;
            if (hit.distance < closesetHit.distance)
            {
                closesetHit = hit;
                closestRenderable = renderable;
            }
        }
        
        //We now have the closest renderable object
        Color rayColor = new Color(1, 1, 1);
        Color gatheredColor = new Color(0, 0, 0);

        if (closesetHit.didHit)
        {
            gatheredColor += GPUMaterial.GetAddedGatheredLight(rayColor, ray, materials[closestRenderable.materialIndex]);
        }

        output[index] = new GPUResult()
        {
            color = gatheredColor,
            x = ray.canvasX,
            y = ray.canvasY
        };
    }

    public static void Dispose()
    {
        _materialBuffer.Dispose();
        _renderableBuffer.Dispose();
        _resultBuffer.Dispose();
        _rayBuffer.Dispose();
        _accelerator.Dispose();
        _context.Dispose();
    }
}