using System.Numerics;
using ILGPU;
using ILGPU.Algorithms;

namespace Raytracer.GPU;

public struct GPURandom
{

   //XORshift random function provided by ChatGPT 
    private static uint nextInt(uint state)
    {
        // Ensure that the state is never zero (zero is an absorbing state)
        if (state == 0) state = 1;
        
        state ^= (state << 13);
        state ^= (state >> 17);
        state ^= (state << 5);
        return state;
    }

    public static GPURandomResult GetRandom(uint state)
    {
        GPURandomResult random;
        random.randomInt = (int) nextInt(state);
        float theta = nextInt(state + 1) / (uint.MaxValue + 1.0f);
        float phi = nextInt(state + 2) / (uint.MaxValue + 1.0f);

        theta *= 2 * MathF.PI;
        phi *= MathF.PI;
        
        float z = MathF.Sin(phi) * MathF.Cos(theta);
        float x = MathF.Sin(phi) * MathF.Sin(theta);
        float y = MathF.Cos(phi);

        random.randomDirection = new Vector3(x, y, z);
        random.nextState = nextInt(state + 3);

        return random;
    }

}

public struct GPURandomResult
{
    public int randomInt;
    public Vector3 randomDirection;
    public uint nextState;
}