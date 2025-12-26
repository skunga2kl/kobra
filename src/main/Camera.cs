using Silk.NET.Maths;

namespace Kobra.Scene;

public class Camera
{
    public Transform Transform = new();

    public float Fov = MathF.PI / 4f;
    public float Near = 0.1f;
    public float Far = 100f;

    public Vector3D<float> Forward
    {
        get
        {
            float yaw = Transform.Rotation.Y;
            return new Vector3D<float>(
                MathF.Sin(yaw),
                0,
                MathF.Cos(yaw)
            );
        }
    }

    public Vector3D<float> Right
    {
        get
        {
            var f = Forward;
            return new Vector3D<float>(f.Z, 0f, -f.X);
        }
    }

    public Matrix4X4<float> GetViewMatrix()
    {
        var position = Transform.Position;
        var rotation = Transform.Rotation;

        var translation = Matrix4X4.CreateTranslation(-position);

        var rotationMatrix =
            Matrix4X4.CreateRotationZ(-rotation.Z) *
            Matrix4X4.CreateRotationY(-rotation.Y) *
            Matrix4X4.CreateRotationX(-rotation.X);

        return rotationMatrix * translation;
    }

    public Matrix4X4<float> GetProjectionMatrix(float aspectRatio)
    {
        return Matrix4X4.CreatePerspectiveFieldOfView(
            Fov,
            aspectRatio,
            Near,
            Far
        );
    }
}
