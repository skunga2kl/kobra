using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Kobra.Rendering
{
    public class KShader
    {
        private readonly GL _gl;
        public readonly uint Handle;

        public KShader(GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;

            if (!File.Exists(vertexPath))
            {
                throw new FileNotFoundException($"Vertex shader not found: {vertexPath}");
            }

            if (!File.Exists(fragmentPath))
            {
                throw new FileNotFoundException($"Fragment shader not found: {fragmentPath}");
            }

            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);

            uint vertex = CompileShader(ShaderType.VertexShader, vertexSource);
            uint fragment = CompileShader(ShaderType.FragmentShader, fragmentSource);

            Handle = _gl.CreateProgram();
            _gl.AttachShader(Handle, vertex);
            _gl.AttachShader(Handle, fragment);
            _gl.LinkProgram(Handle);

            _gl.GetProgram(Handle, ProgramPropertyARB.LinkStatus, out int status);
            if (status == 0)
            {
                string info = _gl.GetProgramInfoLog(Handle);
                throw new Exception($"Error linking shader program: {info}");
            }

            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public uint CompileShader(ShaderType type, string source)
        {
            uint shader = _gl.CreateShader(type);

            _gl.ShaderSource(shader, source);
            _gl.CompileShader(shader);

            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
            if (status == 0)
            {
                string info = _gl.GetShaderInfoLog(shader);
                throw new Exception($"Error compiling {type} shader: {info}");
            }

            return shader;
        }

        public unsafe void SetMatrix4(string name, Matrix4X4<float> matrix)
        {
            int location = _gl.GetUniformLocation(Handle, name);
            _gl.UniformMatrix4(location, 1, false, (float*)&matrix);
        }

        public void SetVec3(string name, Vector3D<float> value)
        {
            int location = _gl.GetUniformLocation(Handle, name);
            _gl.Uniform3(location, value.X, value.Y, value.Z);
        }

        public void SetFloat(string name, float value)
        {
            int location = _gl.GetUniformLocation(Handle, name);
            _gl.Uniform1(location, value);
        }

        public void Use()
        {
            _gl.UseProgram(Handle);
        }
    }
}
