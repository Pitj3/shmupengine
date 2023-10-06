using Engine.Core;
using Silk.NET.GLFW;
using Silk.NET.Maths;

namespace Engine.Graphics
{
    public class Window
    {
        internal unsafe WindowHandle* _handle;
        internal static Glfw _glfw;

        public unsafe static Window Create(string name, uint width, uint height)
        {
            _glfw = Glfw.GetApi();

            _glfw.Init();

            _glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            _glfw.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            _glfw.WindowHint(WindowHintInt.ContextVersionMinor, 5);
            _glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            _glfw.WindowHint(WindowHintBool.OpenGLForwardCompat, true);

            Window w = new()
            {
                _handle = _glfw.CreateWindow((int)width, (int)height, name, null, null)
            };

            _glfw.MakeContextCurrent(w._handle);

            /// TODO Make this a option
            // Set VSync
            _glfw.SwapInterval(1);

            _glfw.SetKeyCallback(w._handle, (handle, key, scancode, action, mods) =>
            {
                switch(action)
                {
                    case InputAction.Press:
                    {
                        Input.SetKeyPressed(key);
                        break;
                    }

                    case InputAction.Release:
                    {
                        Input.SetKeyReleased(key);
                        break;
                    }

                    case InputAction.Repeat:
                    {
                        Input.SetKeyPressed(key);
                        break;
                    }

                    default: break;
                }
            });

            _glfw.SetMouseButtonCallback(w._handle, (handle, button, action, mods) =>
            {
                switch (action)
                {
                    case InputAction.Press:
                        {
                            Input.SetMousePressed(button);
                            break;
                        }

                    case InputAction.Release:
                        {
                            Input.SetMouseReleased(button);
                            break;
                        }

                    case InputAction.Repeat:
                        {
                            Input.SetMousePressed(button);
                            break;
                        }

                    default: break;
                }
            });

            _glfw.SetCursorPosCallback(w._handle, (handle, xpos, ypos) =>
            {
                Input.MousePosition = new Vector2D<int>((int) xpos, (int) ypos);
            });

            return w;
        }

        public void SwapBuffers()
        {
            unsafe
            {
                _glfw.SwapBuffers(_handle);
            }
        }

        public void PollEvents()
        {
            unsafe
            {
                _glfw.PollEvents();
            }
        }

        public bool ShouldClose()
        {
            unsafe
            {
                return _glfw.WindowShouldClose(_handle);
            }
        }
    }
}
