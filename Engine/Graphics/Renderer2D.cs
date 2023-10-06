using Engine.Core;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Drawing;

namespace Engine.Graphics
{
    public class RenderCommandSprite : IRenderCommand
    {
        public Sprite Sprite { get; set; }
        public Vector2D<float> Location { get; set; }
        public Vector2D<float> Scale { get; set; } = Vector2D<float>.One;

        public RenderCommandSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
    }

    public class RenderCommandAnimatedSprite : IRenderCommand
    {
        public SpriteAnimation Sprite { get; set; }
        public Vector2D<float> Location { get; set; }
        public Vector2D<float> Scale { get; set; } = Vector2D<float>.One;

        public RenderCommandAnimatedSprite(SpriteAnimation sprite) 
        {
            Sprite = sprite;
        }
    }

    public class Renderer2D : IRenderer
    {
        private readonly float[] _quadVertices =
        {
            1f, 0f, 0f, 1f, 0f,  // top right
            1f, 1f, 0f, 1f, 1f,  // bottom right
            0f, 1f, 0f, 0f, 1f,  // bottom left
            0f, 0f, 0f, 0f, 0f   // top left 
        };

        private readonly uint[] _quadIndices = {
            0, 1, 3,
            1, 2, 3
        };

        private VertexArray _quadVAO;
        private ShaderProgram _spriteShader;
        private ShaderProgram _animatedSpriteShader;
        private ShaderProgram _screenShader;

        private Color _backgroundColor;

        private float _currentFrameTime;
        private GameTime _time;

        internal override void BeginRender(GameTime time)
        {
            Graphics.GL.ClearColor(_backgroundColor);
            Graphics.GL.Clear(ClearBufferMask.ColorBufferBit);

            _time = time;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        internal override void EndRender()
        {
            _currentFrameTime += _time.Delta;
        }

        internal override void Init()
        {
            Graphics.GL.Enable(GLEnum.Texture2D);

            Graphics.GL.Enable(GLEnum.Blend);
            Graphics.GL.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);

            BufferLayout layout = new();
            layout.Push<Vector3D<float>>("Position");
            layout.Push<Vector2D<float>>("TexCoords");

            VertexBuffer vb = new VertexBuffer(_quadVertices);
            IndexBuffer ib = new IndexBuffer(_quadIndices);

            _quadVAO = new VertexArray(new List<VertexBuffer>() { vb }, ib, layout);

            ShaderModule spriteVertexModule = new ShaderModule(ShaderType.VertexShader, File.ReadAllText("data/sprite_vert.glsl"));
            ShaderModule spriteFragmentModule = new ShaderModule(ShaderType.FragmentShader, File.ReadAllText("data/sprite_frag.glsl"));

            _spriteShader = new ShaderProgram(spriteVertexModule, spriteFragmentModule);

            ShaderModule animatedSpriteVertexModule = new ShaderModule(ShaderType.VertexShader, File.ReadAllText("data/animated_sprite_vert.glsl"));
            ShaderModule animatedSpriteFragmentModule = new ShaderModule(ShaderType.FragmentShader, File.ReadAllText("data/animated_sprite_frag.glsl"));

            _animatedSpriteShader = new ShaderProgram(animatedSpriteVertexModule, animatedSpriteFragmentModule);

            ShaderModule screenVertexModule = new ShaderModule(ShaderType.VertexShader, File.ReadAllText("data/screen_vert.glsl"));
            ShaderModule screenFragmentModule = new ShaderModule(ShaderType.FragmentShader, File.ReadAllText("data/screen_frag.glsl"));
            _screenShader = new ShaderProgram(screenVertexModule, screenFragmentModule);
        }

        public override void SetBackgroundColor(Color color)
        {
            _backgroundColor = color;
        }

        internal override void Shutdown()
        {

        }

        public override void Submit(IRenderCommand command)
        {
            if (Core.Application.Instance == null) return;

            if (command is RenderCommandSprite spriteCommand)
            {
                Sprite sprite = spriteCommand.Sprite;
                Vector2D<float> location = spriteCommand.Location;
                Vector2D<float> scale = spriteCommand.Scale;

                _spriteShader.Bind();

                _spriteShader.Set(0, "uTexture");
                _spriteShader.Set(location, "uLocation");

                Matrix4X4<float> projection = Matrix4X4.CreateOrthographicOffCenter(0.0f, Core.Application.Instance.Width, Core.Application.Instance.Height, 0, -10, 10);
                _spriteShader.Set(projection, "projection");

                Matrix4X4<float> view = Matrix4X4<float>.Identity;
                _spriteShader.Set(view, "view");

                _spriteShader.Set<uint>(sprite.Width * (uint)scale.X, "width");
                _spriteShader.Set<uint>(sprite.Height * (uint)scale.Y, "height");

                Graphics.GL.ActiveTexture(GLEnum.Texture0);
                Graphics.GL.BindTexture(GLEnum.Texture2D, sprite.ID);

                _quadVAO.Bind();

                unsafe
                {
                    Graphics.GL.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, null);
                }

                VertexArray.Unbind();

                ShaderProgram.Unbind();
            }

            if (command is RenderCommandAnimatedSprite animatedSpriteCommand)
            {
                SpriteAnimation animation = animatedSpriteCommand.Sprite;
                Sprite sprite = animation._sheet;
                Vector2D<float> location = animatedSpriteCommand.Location;
                Vector2D<float> scale = animatedSpriteCommand.Scale;

                animation._currentFrameTime += _time.Delta;

                if(animation._currentFrameTime > animation.FrameTime)
                {
                    animation._currentFrameTime = 0;
                    animation.Frame++;

                    if(animation.Frame >= animation._frames)
                    {
                        animation.Frame = 0;
                    }
                }

                uint frame = animation.Frame;

                _animatedSpriteShader.Bind();

                _animatedSpriteShader.Set(0, "uTexture");
                _animatedSpriteShader.Set(location, "uLocation");

                Matrix4X4<float> projection = Matrix4X4.CreateOrthographicOffCenter(0.0f, Core.Application.Instance.Width, Core.Application.Instance.Height, 0, -10, 10);
                _animatedSpriteShader.Set(projection, "projection");

                Matrix4X4<float> view = Matrix4X4<float>.Identity;
                _animatedSpriteShader.Set(view, "view");

                _animatedSpriteShader.Set<uint>(animation.FrameWidth * (uint)scale.X, "width");
                _animatedSpriteShader.Set<uint>(animation.FrameHeight * (uint)scale.Y, "height");
                _animatedSpriteShader.Set<uint>(animation._xAmount, "xframes");
                _animatedSpriteShader.Set<uint>(animation._yAmount, "yframes");

                _animatedSpriteShader.Set<Vector2D<float>>(animation.GetOffset(frame), "offset");


                Graphics.GL.ActiveTexture(GLEnum.Texture0);
                Graphics.GL.BindTexture(GLEnum.Texture2D, sprite.ID);

                _quadVAO.Bind();

                unsafe
                {
                    Graphics.GL.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, null);
                }

                VertexArray.Unbind();

                ShaderProgram.Unbind();
            }

        }

        internal override void RenderFullscreenQuad(uint texID)
        {
            Graphics.GL.ClearColor(_backgroundColor);
            Graphics.GL.Clear(ClearBufferMask.ColorBufferBit);

            _screenShader.Bind();

            _screenShader.Set(0, "uTexture");

            Matrix4X4<float> projection = Matrix4X4.CreateOrthographicOffCenter(0.0f, 1.0f, 1.0f, 0.0f, -10.0f, 10.0f);
            _screenShader.Set(projection, "projection");

            Matrix4X4<float> view = Matrix4X4<float>.Identity;
            _screenShader.Set(view, "view");

            Graphics.GL.ActiveTexture(GLEnum.Texture0);
            Graphics.GL.BindTexture(GLEnum.Texture2D, texID);

            _quadVAO.Bind();

            unsafe
            {
                Graphics.GL.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, null);
            }

            VertexArray.Unbind();

            ShaderProgram.Unbind();
        }
    }
}
