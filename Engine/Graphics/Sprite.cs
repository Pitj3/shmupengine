using StbImageSharp;

namespace Engine.Graphics
{
    public class Sprite
    {
        internal uint ID { get; private set; }

        public uint Width { get; set; }
        public uint Height { get; set; }

        public Sprite(string path)
        {
            ID = Graphics.GL.GenTexture();
            Graphics.GL.ActiveTexture(Silk.NET.OpenGL.GLEnum.Texture0);
            Graphics.GL.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, ID);

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
}
