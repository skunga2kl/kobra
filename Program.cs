using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using System;

namespace Kobra;

internal static class Program
{
    private static IWindow _window;
    private static GL _gl;

    static void Main(string[] args)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 960);
        options.Title = "Kobra";
        options.API = new GraphicsAPI(
            ContextAPI.OpenGL,
            ContextProfile.Core,
            ContextFlags.ForwardCompatible,
            new APIVersion(4, 5)
        );

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Closing += OnClose;

        _window.Run();
    }

    private static void OnLoad()
    {
        _gl = GL.GetApi(_window);

        Console.WriteLine($"opengl: {_gl.GetStringS(StringName.Version)}");
        Console.WriteLine($"glsl: {_gl.GetStringS(StringName.Renderer)}");    

        _gl.ClearColor(0.0f, 0.0f, 0.15f, 1.0f);
    }

    private static void OnRender(double delta)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    private static void OnClose()
    {
        _gl?.Dispose();
    }
}