using Kobra.Graphics;
using Kobra.Rendering;
using Kobra.Scene;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Kobra.Main;

public class Engine
{
    private readonly IWindow _window;
    private GL _gl;
    private Renderer _renderer;
    private KShader _shader;
    private KScene _scene;
    private Random _random = new Random();

    private Mesh _cube;
    private DirectionalLight _light = new DirectionalLight(
        new Vector3D<float>(-1f, -1f, -1f),
        new Vector3D<float>(1.0f, 1.0f, 1.0f),
        1.0f
    );
    private Camera _camera;

    private IInputContext _input;
    private IKeyboard _keyboard;

    public Engine()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.Title = "Kobra";
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.ForwardCompatible, new APIVersion(4, 5));

        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Closing += OnClose;
    }

    public void Run() => _window.Run();

    private void OnLoad()
    {
        _gl = GL.GetApi(_window);

        _input = _window.CreateInput();
        _keyboard = _input.Keyboards.FirstOrDefault();
        if (_input != null)
        {
            if (_keyboard is null)
            {
                System.Console.WriteLine("no keyboard found");
            }
        }

        _gl.Enable(EnableCap.DepthTest);
        _renderer = new Renderer(_gl);
        _camera ??= new Camera();
        _scene = new KScene();

        _shader = new KShader(_gl, "shader/basicvert.vert", "shader/basicfrag.frag");
        var vertices = new Vertices();
        _cube = new Mesh(_gl, vertices.cubeVertices, 6);
        _scene.AddMesh(_cube);
    }

    private void OnRender(double deltaTime)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var aspect = _window.Size.X / (float)_window.Size.Y;
        var projection = _camera.GetProjectionMatrix(aspect);
        var view = _camera.GetViewMatrix(); 

        float speed = 3f * (float)deltaTime;

        if (_keyboard != null)
        {
            if (_keyboard.IsKeyPressed(Key.W))
                _camera.Transform.Position -= _camera.Forward * speed;

            if (_keyboard.IsKeyPressed(Key.S))
                _camera.Transform.Position += _camera.Forward * speed;

            if (_keyboard.IsKeyPressed(Key.A))
                _camera.Transform.Position -= _camera.Right * speed;

            if (_keyboard.IsKeyPressed(Key.D))
                _camera.Transform.Position += _camera.Right * speed;

            if (_keyboard.IsKeyPressed(Key.F1))
            {
                var cube = Helpers.CreateMesh(_gl); 

                float x = (float)(_random.NextDouble() * 10);
                float y = (float)(_random.NextDouble() * 10);
                float z = (float)(_random.NextDouble() * 10);

                cube.Transform.Position = new Vector3D<float>(x, y, z);

                cube.Transform.Rotation = new Vector3D<float>(
                    (float)(_random.NextDouble() * 360),
                    (float)(_random.NextDouble() * 360),
                    (float)(_random.NextDouble() * 360)
                );

                cube.Material.Color = new Vector3D<float>(
                    (float)_random.NextDouble(),
                    (float)_random.NextDouble(),
                    (float)_random.NextDouble()
                );

                _scene.AddMesh(cube);
            }

            if (_keyboard.IsKeyPressed(Key.F2))
            {
                var count = _scene.Meshes.Count;
                Console.WriteLine($"scene has {count} meshes");
            }
        }

        _renderer.Clear();
        _shader.Use();

        _shader.SetVec3("u_LightDirection", _light.Direction);
        _shader.SetVec3("u_LightColor", _light.Color);
        _shader.SetFloat("u_Intensity", _light.Intensity);
        _shader.SetVec3("u_ViewPos", _camera.Transform.Position);

        _scene.AddMesh(_cube);

        foreach (var mesh in _scene.Meshes)
        {
            _shader.SetVec3("u_ObjectColor", mesh.Material.Color);

            var model = mesh.Transform.GetModelMatrix();
            var mvp = model * view * projection;

            _shader.SetMatrix4("u_Model", model);  
            _shader.SetMatrix4("u_MVP", mvp);

            mesh.Material.Apply(_shader);
            _renderer.Draw(_shader, mesh);
        }

        _cube.Transform.Rotation.Y += 1f * (float)deltaTime;
        _cube.Material.Color = new Vector3D<float>(
            (float)(Math.Sin(_window.Time) * 0.5 + 0.5),
            (float)(Math.Cos(_window.Time) * 0.5 + 0.5),
            0.5f
        );
    }

    private void OnClose() { }
}
