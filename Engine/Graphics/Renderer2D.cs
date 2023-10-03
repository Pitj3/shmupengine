using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Drawing;

namespace Engine.Graphics
{
    public class RenderCommandSprite : IRenderCommand
    {
        public Sprite Sprite { get; set; }
        public Vector2D<float> Location { get; set; }

        public RenderCommandSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
    }

    public class Renderer2D : IRenderer
    {
        private Glfw? _glfw;
        private GL? _gl;

        internal override void BeginRender()
        {
            if (_gl == null)
                return;

            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        internal override void EndRender()
        {
        }

        internal override void Init()
        {
            _glfw = Glfw.GetApi();
            _gl = GL.GetApi(_glfw.GetProcAddress);
        }

        public override void SetBackgroundColor(Color color)
        {
            _gl?.ClearColor(color);
        }

        internal override void Shutdown()
        {

        }

        public override void Submit(IRenderCommand command)
        {
            if (_gl == null)
                return;

            if (command is RenderCommandSprite spriteCommand)
            {
                Sprite sprite = spriteCommand.Sprite;
                Vector2D<float> location = spriteCommand.Location;

                // Bind sprite shader
                // Bind texture to shader

                //DrawQuad(location.X, location.Y, sprite.Width, sprite.Height);

                // Finish shader program
            }
        }
    }
}
