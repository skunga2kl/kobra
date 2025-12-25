using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Kobra.Rendering;

namespace Kobra.Main;

public class Engine
{
    private readonly IWindow _window;
    private GL _gl;
    private Renderer _renderer;
    private KShader _shader;
    private Mesh _cube;

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

        _gl.Enable(EnableCap.DepthTest);
        _renderer = new Renderer(_gl);

        _shader = new KShader(_gl, "shader/basicvert.vert", "shader/basicfrag.frag");
        var vertices = new Vertices();
        _cube = new Mesh(_gl, vertices.cubeVertices, 6);
    }

    private void OnRender(double deltaTime)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var projection = Matrix4X4.CreatePerspectiveFieldOfView(
            MathF.PI / 4f,
            (float)_window.Size.X / _window.Size.Y,
            0.1f,
            100f
        );

        var view = Matrix4X4.CreateTranslation(0f, 0f, -3f);
        var model = Matrix4X4.CreateRotationY((float)_window.Time);
        var mvp = model * view * projection;

        _shader.Use();
        _shader.SetMatrix4("u_MVP", mvp);

        _renderer.Clear();
        _renderer.Draw(_shader, _cube);
    }

    private void OnClose() { }
}
