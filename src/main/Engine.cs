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
    private KVertexArray _triangle;

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
        _renderer = new Renderer(_gl);

        string vertexShader = @"
            #version 450 core
            layout(location = 0) in vec3 aPosition;
            void main() { gl_Position = vec4(aPosition, 1.0); }
        ";
        string fragmentShader = @"
            #version 450 core
            out vec4 FragColor;
            void main() { FragColor = vec4(0.9, 0.2, 0.3, 1.0); }
        ";

        _shader = new KShader(_gl, vertexShader, fragmentShader);

        float[] triangleVerts =
        {
             0.0f,  0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f
        };
        _triangle = new KVertexArray(_gl, triangleVerts);
    }

    private void OnRender(double deltaTime)
    {
        _renderer.Clear();
        _renderer.Draw(_shader, _triangle);
    }

    private void OnClose() { }
}
