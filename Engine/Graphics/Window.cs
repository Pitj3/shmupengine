using Silk.NET.GLFW;

namespace Engine.Graphics
{
    public class Window
    {
        internal unsafe WindowHandle* _handle;
        internal static Glfw? _glfw;

        public unsafe static Window Create(string name, uint width, uint height)
        {
            _glfw = Glfw.GetApi();

            _glfw.Init();

            _glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            _glfw.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            _glfw.WindowHint(WindowHintInt.ContextVersionMinor, 5);
            _glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
            _glfw.WindowHint(WindowHintBool.OpenGLForwardCompat, true);

            Window w = new()
            {
                _handle = _glfw.CreateWindow((int)width, (int)height, name, null, null)
            };

            _glfw.MakeContextCurrent(w._handle);

            /// TODO Make this a option
            // Set VSync
            _glfw.SwapInterval(1);

            return w;
        }

        public void SwapBuffers()
        {
            unsafe
            {
                _glfw?.SwapBuffers(_handle);
            }
        }

        public void PollEvents()
        {
            unsafe
            {
                _glfw?.PollEvents();
            }
        }

        public bool ShouldClose()
        {
            unsafe
            {
                return _glfw?.WindowShouldClose(_handle) ?? false;
            }
        }
    }
}
