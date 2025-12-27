using Kobra.Rendering;
using Kobra.Graphics;

namespace Kobra.Scene
{
    public class KScene
    {
        public List<Mesh> Meshes { get; private set; }
        public List<Light> Lights { get; private set; }

        public KScene()
        {
            Meshes = new List<Mesh>();
            Lights = new List<Light>();
        }
        public void AddMesh(Mesh mesh)
        {
            if (!Meshes.Contains(mesh))
            {
                Meshes.Add(mesh);
            }
        }
        public void RemoveMesh(Mesh mesh)
        {
            if (Meshes.Contains(mesh))
            {
                mesh.VAO.Dispose();
                Meshes.Remove(mesh);
            }
        }

        public void AddLight(Light light)
        {
            if (!Lights.Contains(light))
            {
                Lights.Add(light);
            }
        }
    }
}