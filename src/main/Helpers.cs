using Kobra.Rendering;
using Kobra.Graphics;
using Silk.NET.OpenGL;

namespace Kobra.Main
{
    public static class Helpers
    {
        public static Mesh CreateMesh(GL gl)
        {
            gl = new GL(gl.Context);

            var vertices = new Vertices();
            var mesh = new Mesh(gl, vertices.cubeVertices, 6);
            mesh.Material = new Material();
            return mesh;
        }
    }
}