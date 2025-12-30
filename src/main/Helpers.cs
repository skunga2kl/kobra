using Kobra.Rendering;
using Kobra.Graphics;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using System.Numerics;

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

        public static bool WorldToScreen(
            Vector3D<float> worldPos,
            Matrix4X4<float> view,
            Matrix4X4<float> projection,
            Vector2 windowSize,
            out Vector2 screenPos)
        {
            var world = new Vector4D<float>(worldPos, 1f);

            var clip = Vector4D.Transform(world, view * projection);

            if (clip.W <= 0f)
            {
                screenPos = default;
                return false;
            }

            var ndc = new Vector3D<float>(
                clip.X / clip.W,
                clip.Y / clip.W,
                clip.Z / clip.W
            );

            screenPos = new Vector2(
                (ndc.X + 1f) / 2f * windowSize.X,
                (1f - ndc.Y) / 2f * windowSize.Y
            );

            return true;
        }
    }
}