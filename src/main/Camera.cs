using Silk.NET.Maths;

namespace Kobra.Scene
{
    public class Camera
    {
        public Vector3D<float> position = new(0f, 0f, 3f);

        public float fov = MathF.PI / 4f;
        public float near = 0.1f;
        public float far = 100f;

        public Matrix4X4<float> GetViewMatrix()
        {
            return Matrix4X4.CreateTranslation(-position);
        }

        public Matrix4X4<float> GetProjectionMatrix(float aspectRatio)
        {
            return Matrix4X4.CreatePerspectiveFieldOfView(
                fov,
                aspectRatio,
                near,
                far
            );
        }
    }
}
