using Kobra.Rendering;
using Kobra.Graphics;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace Kobra.Main
{
    public static class Helpers
    {
        public static Mesh CreateCubeMesh(GL gl)
        {
            gl = new GL(gl.Context);

            var vertices = new Vertices();
            var mesh = new Mesh(gl, vertices.cubeVertices, 6);
            mesh.Material = new Material();
            return mesh;
        }

        public static Mesh CreateFloorMesh(GL gl, Vertices verts)
        {
            var floor = new Mesh(gl, verts.floorVertices, 6);
            floor.Material.Color = new Vector3D<float>(0.5f, 0.5f, 0.5f);
            return floor;
        }
    }
}