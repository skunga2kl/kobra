using Kobra.Scene;
using Kobra.Graphics;
using Silk.NET.OpenGL;

namespace Kobra.Rendering
{
    public class Mesh
    {
        public KVertexArray VAO { get; }
        public uint VertexCount { get; }
        public Transform Transform = new();
        public Material Material = new();
        public String Name = "Mesh";

        public Mesh(GL gl, float[] vertices, int floatsPerVertex)
        {
            VertexCount = (uint)(vertices.Length / floatsPerVertex);
            VAO = new KVertexArray(gl, vertices);
        }
    }
}