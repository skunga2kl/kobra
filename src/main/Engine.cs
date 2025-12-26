using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Input;
using Kobra.Rendering;
using Kobra.Scene;

namespace Kobra.Main;

public class Engine
{
    private readonly IWindow _window;
    private GL _gl;
    private Renderer _renderer;
    private KShader _shader;
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
            Console.WriteLine("no keyboard found");
        }

        _gl.Enable(EnableCap.DepthTest);
        _renderer = new Renderer(_gl);
        _camera ??= new Camera();

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
        var model = _cube.Transform.GetModelMatrix();
        var mvp = model * view * projection;

        _cube.Transform.Rotation.Y += (float)(deltaTime * 0.5);
        _cube.Transform.Rotation.X += (float)(deltaTime * -0.5);

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


        _shader.Use();
        _shader.SetMatrix4("u_MVP", mvp);

        
        _cube.Material.Apply(_shader);
        _cube.Material.Color = new Vector3D<float>(0f, 1f, 0f);

        _renderer.Clear();
        _renderer.Draw(_shader, _cube);
    }

    private void OnClose() { }
}
