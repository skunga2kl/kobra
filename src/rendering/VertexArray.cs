using Silk.NET.OpenGL;

namespace Kobra.Rendering
{
    public class KVertexArray : IDisposable
    {   
        private readonly GL _gl;
        public readonly uint Handle;
        private readonly uint _vbo;
        private bool _disposed = false;

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

            _gl.VertexAttribPointer(
                0,
                3,
                VertexAttribPointerType.Float,
                false,
                (uint)(6 * sizeof(float)),
                (void*)0
            );
            _gl.EnableVertexAttribArray(0);

            _gl.VertexAttribPointer(
                1,
                3,
                VertexAttribPointerType.Float,
                false,
                (uint)(6 * sizeof(float)),
                (void*)(3 * sizeof(float))
            );
            _gl.EnableVertexAttribArray(1);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_vbo);
            _gl.DeleteVertexArray(Handle);
            GC.SuppressFinalize(this);

            _disposed = true;
        }

        public void Bind()
        {
            _gl.BindVertexArray(Handle);
        }
    }
}
