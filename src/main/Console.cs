using Kobra.Main;
using Kobra.Scene;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Kobra.Console
{
    public class KConsole
    {
        private readonly KScene _scene;
        private readonly GL _gl;

        private bool _running = true;

        public KConsole(KScene scene)
        {
            _scene = scene;
            new Thread(ConsoleLoop) { IsBackground = true }.Start();
        }

        private void ConsoleLoop()
        {
            while (_running)
            {
                System.Console.Write("> ");
                var input = System.Console.ReadLine();
                if (input == null) continue;

                try
                {
                    Parse(input);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void Parse(string input)
        {
            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0) return;

            var command = args[0].ToLower();

            switch (command)
            {
               case "addmesh":
                    AddMesh(args);
                    break;
                case "removemesh":
                    RemoveMesh(args);
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    System.Console.WriteLine("Unknown command.");
                    break;
            }
        }
        private void AddMesh(string[] args)
        {
            float x = 0, y = 0, z = 0;
            if (args.Length >= 4)
            {
                x = float.Parse(args[1]);
                y = float.Parse(args[2]);
                z = float.Parse(args[3]);
            }

            var cube = Helpers.CreateMesh(_gl);
            cube.Transform.Position = new Vector3D<float>(x, y, z);
            _scene.AddMesh(cube);
            System.Console.WriteLine($"Added mesh at position ({x}, {y}, {z})");
        }

        private void RemoveMesh(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: removemesh <index>");
                return;
            }

            int index = int.Parse(args[1]);
            if (index < 0 || index >= _scene.Meshes.Count)
            {
                System.Console.WriteLine("Invalid mesh index.");
                return;
            }

            var mesh = _scene.Meshes[index];
            _scene.RemoveMesh(mesh);
            System.Console.WriteLine($"Removed mesh at index {index}");
        }

        private void Exit()
        {
            System.Console.WriteLine("Closing engine");
            Environment.Exit(0);
        }
    }
}