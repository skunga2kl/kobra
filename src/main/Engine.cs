using Kobra.Graphics;
using Kobra.Rendering;
using Kobra.Scene;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;
using Kobra.Graphics.UI;

namespace Kobra.Main;

public class Engine
{
    private readonly IWindow _window;
    private GL _gl;
    private Renderer _renderer;
    private KShader _shader;
    private KScene _scene;
    private Random _random = new Random();
    private ImGuiController _controller;
    private Inspector _inspector;

    private Mesh _cube;
    private Mesh _floor;
    private Mesh _selectedMesh;
    private Camera _camera;
    private DirectionalLight _sun;

    private IInputContext _input;
    private IKeyboard _keyboard;
    private IMouse _mouse;

    private Vector2D<float> _lastMousePos;
    private bool _firstMove = true;
    private bool _showEditor = true;
    private Dictionary<Key, bool> _keyState = new();

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
        _mouse = _input.Mice.FirstOrDefault();
        if (_input != null)
        {
            if (_keyboard is null)
            {
                Console.WriteLine("no keyboard found");
            }

            if (_mouse != null)
            {
                _mouse.Cursor.CursorMode = CursorMode.Disabled;
                _mouse.MouseMove += OnMouseMove;
            }
        }

        _gl.Enable(EnableCap.DepthTest);
        _renderer = new Renderer(_gl);
        _camera ??= new Camera();
        _scene = new KScene();
        _inspector = new Inspector(); 

        _shader = new KShader(_gl, "shader/basicvert.vert", "shader/basicfrag.frag");

        var vertices = new Vertices();
        _cube = new Mesh(_gl, vertices.cubeVertices, 6);
        _scene.AddMesh(_cube);

        _selectedMesh = _cube;

        _sun = new DirectionalLight
        {
            Direction = new(-1, -1, -1),
            Color = new Vector3D<float>(1f, 1f, 1f),
            Intensity = 0.8f
        };

        _scene.AddLight(_sun);

        _floor = new Mesh(_gl, vertices.floorVertices, 6);
        _floor.Transform.Position = new Vector3D<float>(0f, -0.5f, 0f);
        _floor.Name = "Floor";  

        _scene.AddMesh(_floor);

        _controller = new ImGuiController(_gl, _window, _input);
    }

    private void OnRender(double deltaTime)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _controller.Update((float)deltaTime);

        if (_showEditor)
        {
            _inspector.Draw(_scene, _camera);
            _hierarchy.Draw(_scene);
        }

        var aspect = _window.Size.X / (float)_window.Size.Y;
        var projection = _camera.GetProjectionMatrix(aspect);
        var view = _camera.GetViewMatrix();

        float speed = 3f * (float)deltaTime;

        if (_keyboard != null)
        {
            if (_keyboard.IsKeyPressed(Key.W))
                _camera.Transform.Position += _camera.Forward * speed;

            if (_keyboard.IsKeyPressed(Key.S))
                _camera.Transform.Position -= _camera.Forward * speed;

            if (_keyboard.IsKeyPressed(Key.A))
                _camera.Transform.Position += _camera.Right * speed;

            if (_keyboard.IsKeyPressed(Key.D))
                _camera.Transform.Position -= _camera.Right * speed;

            if (_keyboard.IsKeyPressed(Key.F1))
            {
                var cube = Helpers.CreateCubeMesh(_gl);

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
                cube.Name = $"Cube {_scene.Meshes.Count}";

                _scene.AddMesh(cube);
            }

            if (IsKeyPressed(Key.F2))
            {
                var count = _scene.Meshes.Count;
                Console.WriteLine($"scene has {count} meshes");
            }

            if (IsKeyPressed(Key.Tab))
            {
                _showEditor = !_showEditor;
                _mouse.Cursor.CursorMode =
                    _showEditor ? CursorMode.Normal : CursorMode.Disabled;
            }
        }

        _renderer.Clear();
        _shader.Use();

        _shader.SetVec3("u_LightDirection", _sun.Direction);
        _shader.SetVec3("u_LightColor", _sun.Color);
        _shader.SetFloat("u_Intensity", _sun.Intensity);
        _shader.SetVec3("u_ViewPos", _camera.Transform.Position);

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

        _controller.Render();
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        const float sensitivity = 0.3f;

        if (_showEditor) return;

        if (_firstMove)
        {
            _lastMousePos = new Vector2D<float>(position.X, position.Y);
            _firstMove = false;
            return;
        }

        var deltaX = (position.X - _lastMousePos.X) * sensitivity;
        var deltaY = (position.Y - _lastMousePos.Y) * sensitivity;

        _lastMousePos = new Vector2D<float>(position.X, position.Y);

        _camera.Transform.Rotation.Y -= deltaX;
        _camera.Transform.Rotation.X -= deltaY;

        _camera.Transform.Rotation.X = Math.Clamp(_camera.Transform.Rotation.X, -89f, 89f);
    }

    private bool IsKeyPressed(Key key)
    {
        bool isPressed = _keyboard?.IsKeyPressed(key) ?? false;
        bool wasPressed = _keyState.ContainsKey(key) && _keyState[key];

        _keyState[key] = isPressed;

        return isPressed && !wasPressed;
    }

    private void OnClose() { }
}