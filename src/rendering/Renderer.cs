using Silk.NET.OpenGL;
using System.Net;

namespace Kobra.Rendering
{
    public class Renderer
    {
        private readonly GL _gl;

        public Renderer(GL gl)
        {
            _gl = gl;
            _gl.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);
        }

        public void Clear() => _gl.Clear(ClearBufferMask.ColorBufferBit);

        public void Draw(KShader shader, Mesh mesh)
        {
            shader.Use();
            mesh.VAO.Bind();
            _gl.DrawArrays(PrimitiveType.Triangles, 0, mesh.VertexCount);
        }
    }
}