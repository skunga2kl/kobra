using Silk.NET.Maths;

namespace Kobra.Graphics
{
    public class DirectionalLight
    {
        public Vector3D<float> Direction;
        public Vector3D<float> Color;
        public float Intensity;
        public DirectionalLight(Vector3D<float> direction, Vector3D<float> color, float intensity)
        {
            Direction = direction;
            Color = color;
            Intensity = intensity;
        }
    }
}