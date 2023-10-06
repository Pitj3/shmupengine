using Engine.Assets;
using Silk.NET.Maths;
using StbImageSharp;

namespace Engine.Graphics
{
    public class Sprite : Asset
    {
        internal uint ID { get; set; }

        public uint Width { get; set; }
        public uint Height { get; set; }

        public Sprite() : base(string.Empty)
        {
        }

        public Sprite(string path) : base(path)
        {
            ID = Graphics.GL.GenTexture();
            Graphics.GL.ActiveTexture(Silk.NET.OpenGL.GLEnum.Texture0);
            Graphics.GL.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, ID);

            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

            Width = (uint)result.Width;
            Height = (uint)result.Height;

            unsafe
            {
                fixed (byte* ptr = result.Data)
                {
                    Graphics.GL.TexImage2D(Silk.NET.OpenGL.GLEnum.Texture2D, 0, Silk.NET.OpenGL.InternalFormat.Rgba,
                        Width, Height, 0, Silk.NET.OpenGL.GLEnum.Rgba, Silk.NET.OpenGL.GLEnum.UnsignedByte, ptr);
                }
            }

            Graphics.GL.TextureParameter(ID, Silk.NET.OpenGL.GLEnum.TextureWrapS, (int)Silk.NET.OpenGL.TextureWrapMode.Repeat);
            Graphics.GL.TextureParameter(ID, Silk.NET.OpenGL.GLEnum.TextureWrapT, (int)Silk.NET.OpenGL.TextureWrapMode.Repeat);
            Graphics.GL.TextureParameter(ID, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (int)Silk.NET.OpenGL.TextureMinFilter.Nearest);
            Graphics.GL.TextureParameter(ID, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (int)Silk.NET.OpenGL.TextureMagFilter.Nearest);

            Graphics.GL.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, 0);
        }
    }

    public class SpriteAnimation
    {
        internal readonly Sprite _sheet;
        internal readonly uint _frames;

        internal readonly uint _xAmount;
        internal readonly uint _yAmount;

        private readonly List<Vector2D<float>> _uvOffsets = new List<Vector2D<float>>();

        public uint FrameWidth { get { return _sheet.Width / _xAmount; } }
        public uint FrameHeight { get { return _sheet.Height / _yAmount; } }
        
        public uint Frame { get; set; }

        public float FrameTime { get; set; }

        internal float _currentFrameTime;

        public SpriteAnimation(Sprite sheet, uint xAmount, uint yAmount, uint xIndex, uint yIndex, uint frames, float frameTime)
        {
            _sheet = sheet;
            _frames = frames;
            _xAmount = xAmount;
            _yAmount = yAmount;
            FrameTime = frameTime;

            for(uint i  = 0; i < _frames; i++)
            {
                float yOffset = 0;
                if(yIndex != 0.0f)
                {
                    yOffset = (1.0f / yIndex);
                }
                _uvOffsets.Add(new Vector2D<float>((i + xIndex) * (1.0f / xAmount), yOffset));
            }
        }

        internal Vector2D<float> GetOffset(uint frame)
        {
            return _uvOffsets[(int)frame];
        }
    }
}
