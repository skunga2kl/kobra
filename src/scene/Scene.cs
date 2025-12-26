using Kobra.Rendering;
using Kobra.Graphics;

namespace Kobra.Scene
{
    public class KScene
    {
        public List<Mesh> Meshes { get; private set; }
        public List<DirectionalLight> DirectionalLights { get; private set; }

        public KScene()
        {
            Meshes = new List<Mesh>();
            DirectionalLights = new List<DirectionalLight>();
        }
        public void AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
        }
        public void RemoveMesh(Mesh mesh)
        {
            if (Meshes.Contains(mesh))
            {
                mesh.VAO.Dispose();
                Meshes.Remove(mesh);
            }
        }
    }
}