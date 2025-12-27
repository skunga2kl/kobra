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
            float yawRad = Transform.Rotation.Y * (MathF.PI / 180f);
            float pitchRad = Transform.Rotation.X * (MathF.PI / 180f);

            return new Vector3D<float>(
                MathF.Cos(pitchRad) * MathF.Sin(yawRad),
                MathF.Sin(pitchRad),
                MathF.Cos(pitchRad) * MathF.Cos(yawRad)
            );
        }
    }

    public Vector3D<float> Right
    {
        get
        {
            var forward = Forward;
            var up = new Vector3D<float>(0, 1, 0);
            return new Vector3D<float>(
                up.Y * forward.Z - up.Z * forward.Y,
                up.Z * forward.X - up.X * forward.Z,
                up.X * forward.Y - up.Y * forward.X
            );
        }
    }

    public Vector3D<float> Up
    {
        get
        {
            var forward = Forward;
            var right = Right;
            return new Vector3D<float>(
                forward.Y * right.Z - forward.Z * right.Y,
                forward.Z * right.X - forward.X * right.Z,
                forward.X * right.Y - forward.Y * right.X
            );
        }
    }

    public Matrix4X4<float> GetViewMatrix()
    {
        var position = Transform.Position;
        var target = position + Forward;
        var worldUp = new Vector3D<float>(0, 1, 0);

        var zAxis = Vector3D.Normalize(position - target);
        var xAxis = Vector3D.Normalize(Vector3D.Cross(worldUp, zAxis)); 
        var yAxis = Vector3D.Cross(zAxis, xAxis); 

        return new Matrix4X4<float>(
            xAxis.X, yAxis.X, zAxis.X, 0,
            xAxis.Y, yAxis.Y, zAxis.Y, 0,
            xAxis.Z, yAxis.Z, zAxis.Z, 0,
            -Vector3D.Dot(xAxis, position), -Vector3D.Dot(yAxis, position), -Vector3D.Dot(zAxis, position), 1
        );
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