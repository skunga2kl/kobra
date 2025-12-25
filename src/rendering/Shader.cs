using Silk.NET.OpenGL;

namespace Kobra.Rendering
{
    public class KShader
    {
        private readonly GL _gl;
        public readonly uint Handle;

        public KShader(GL gl, string vertexSource, string fragmentSource)
        {
            _gl = gl;

            uint vertex = CompileShader(ShaderType.VertexShader, vertexSource);
            uint fragment = CompileShader(ShaderType.FragmentShader, fragmentSource);

            Handle = _gl.CreateProgram();
            _gl.AttachShader(Handle, vertex);
            _gl.AttachShader(Handle, fragment);
            _gl.LinkProgram(Handle);

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

        public void Use()
        {
            _gl.UseProgram(Handle);
        }
    }
}
