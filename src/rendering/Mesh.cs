using Kobra.Scene;
using Silk.NET.OpenGL;

namespace Kobra.Rendering
{
    public class Mesh
    {
        public KVertexArray VAO { get; }
        public uint VertexCount { get; }
        public Transform Transform = new Transform();

        public Mesh(GL gl, float[] vertices, int floatsPerVertex)
        {
            VertexCount = (uint)(vertices.Length / floatsPerVertex);
            VAO = new KVertexArray(gl, vertices);
        }
    }
}