using Silk.NET.OpenGL;
using System.Runtime.CompilerServices;

namespace Kobra.Rendering
{
    public class KVertexArray
    {   
        private readonly GL _gl;
        public readonly uint Handle;
        private readonly uint _vbo;

        public unsafe KVertexArray(GL gl, float[] vertices)
        {
            _gl = gl;

            Handle = _gl.GenVertexArray();
            _gl.BindVertexArray(Handle);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

            unsafe
            {
                fixed (float* v = vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer,
                                   (nuint)(vertices.Length * sizeof(float)),
                                   v,
                                   BufferUsageARB.StaticDraw);
                }
            }

            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);
            _gl.EnableVertexAttribArray(0);
        }

        public void Bind()
        {
            _gl.BindVertexArray(Handle);
        }
    }
}
