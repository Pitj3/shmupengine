using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Engine.Graphics
{
    public sealed class Graphics
    {
        private static GL? _gl;
        public static GL GL
        {
            get { return _gl ??= GL.GetApi(Glfw.GetApi().GetProcAddress); }
        }
    }
}
