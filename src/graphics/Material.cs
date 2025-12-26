using Kobra.Rendering;
using Silk.NET.Maths;

namespace Kobra.Graphics;

public sealed class Material
{
    public Vector3D<float> Color { get; set; } = new(1f, 1f, 1f);

    public void Apply(KShader shader)
    {
        shader.SetVec3("u_Color", Color);
    }
}