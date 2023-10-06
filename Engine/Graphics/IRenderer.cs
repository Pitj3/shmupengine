using Engine.Core;
using System.Drawing;

namespace Engine.Graphics
{
    public interface IRenderCommand
    {
    }

    public abstract class IRenderer : IDisposable
    {
        internal abstract void Init();
        internal abstract void Shutdown();

        public abstract void SetBackgroundColor(Color color);

        internal abstract void BeginRender(GameTime time);
        internal abstract void EndRender();

        public abstract void Submit(IRenderCommand command);

        internal abstract void RenderFullscreenQuad(uint texID);

        public abstract void Dispose();
    }
}
