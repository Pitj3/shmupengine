using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Reflection.Metadata;

namespace Engine.Graphics
{
    public class RenderCommandSprite : IRenderCommand
    {
        public Sprite? Sprite { get; set; }
        public Vector2D<float> Location { get; set; }

        public RenderCommandSprite(Sprite? sprite)
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

        private VertexArray? _quadVAO;
        private ShaderProgram? _spriteShader;

        internal override void BeginRender()
        {
            Graphics.GL.Clear(ClearBufferMask.ColorBufferBit);
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
        }

        public override void SetBackgroundColor(Color color)
        {
            Graphics.GL.ClearColor(color);
        }

        internal override void Shutdown()
        {

        }

        public override void Submit(IRenderCommand command)
        {
            if (Core.Application.Instance == null) return;

            if (command is RenderCommandSprite spriteCommand)
            {
                if (spriteCommand.Sprite == null) return;
                
                Sprite sprite = spriteCommand.Sprite;
                Vector2D<float> location = spriteCommand.Location;

                _spriteShader?.Bind();

                _spriteShader?.Set(0, "uTexture");
                _spriteShader?.Set(location, "uLocation");

                Matrix4X4<float> projection = Matrix4X4.CreateOrthographic(Core.Application.Instance.Width, Core.Application.Instance.Height, -10.0f, 10.0f);
                _spriteShader?.Set(projection, "projection");

                Matrix4X4<float> view = Matrix4X4<float>.Identity;
                _spriteShader?.Set(view, "view");

                _spriteShader?.Set(sprite.Width, "width");
                _spriteShader?.Set(sprite.Height, "height");

                Graphics.GL.ActiveTexture(GLEnum.Texture0);
                Graphics.GL.BindTexture(GLEnum.Texture2D, sprite.ID);

                _quadVAO?.Bind();

                unsafe
                {
                    Graphics.GL.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, null);
                }

                VertexArray.Unbind();

                ShaderProgram.Unbind();
            }
        }
    }
}
