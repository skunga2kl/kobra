using Silk.NET.Maths;

namespace Kobra.Scene
{
    public class Transform
    {
        public Vector3D<float> Position = Vector3D<float>.Zero;
        public Vector3D<float> Rotation = Vector3D<float>.Zero;
        public Vector3D<float> Scale = Vector3D<float>.One;

        public Matrix4X4<float> GetModelMatrix()
        {
            var translation = Matrix4X4.CreateTranslation(Position);
            var rotation = 
                Matrix4X4.CreateRotationX(Rotation.X) *
                Matrix4X4.CreateRotationY(Rotation.Y) *
                Matrix4X4.CreateRotationZ(Rotation.Z);

            var scale = Matrix4X4.CreateScale(this.Scale);

            return scale * rotation * translation;
        }
    }
}
