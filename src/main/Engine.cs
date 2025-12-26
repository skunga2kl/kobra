using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Input;
using Kobra.Rendering;
using Kobra.Scene;
using Kobra.Console;

namespace Kobra.Main;

public class Engine
{
    private readonly IWindow _window;
    private GL _gl;
    private Renderer _renderer;
    private KShader _shader;
    private KScene _scene;
    private KConsole _console;

    private Mesh _cube;
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
        if (_keyboard is null)
        {
            System.Console.WriteLine("no keyboard found");
        }

        _gl.Enable(EnableCap.DepthTest);
        _renderer = new Renderer(_gl);
        _camera ??= new Camera();
        _scene = new KScene();
        _console = new KConsole(_scene);

        _shader = new KShader(_gl, "shader/basicvert.vert", "shader/basicfrag.frag");
        var vertices = new Vertices();
        _cube = new Mesh(_gl, vertices.cubeVertices, 6);
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
        }

        _renderer.Clear();
        _shader.Use();

        _scene.AddMesh(_cube);

        foreach (var mesh in _scene.Meshes)
        {
            var model = mesh.Transform.GetModelMatrix();
            var mvp = model * view * projection;
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
