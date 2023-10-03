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

        internal abstract void BeginRender();
        internal abstract void EndRender();

        public abstract void Submit(IRenderCommand command);

        public abstract void Dispose();
    }
}
