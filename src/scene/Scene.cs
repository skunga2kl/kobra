using Kobra.Rendering;

namespace Kobra.Scene
{
    public class KScene
    {
        public List<Mesh> Meshes { get; private set; }
        public KScene()
        {
            Meshes = new List<Mesh>();
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