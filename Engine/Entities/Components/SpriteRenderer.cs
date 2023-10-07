using Engine.Core;
using Engine.Graphics;

namespace Engine.Entities.Components
{
    public class SpriteRenderer : IComponent
    {
        public Sprite Sprite { get; set; }

        public ShaderProgram Material { get; set; } = DefaultShaders.SpriteShader;

        public override void Render(GameTime time)
        {
            base.Render(time);

            if (Sprite == null)
                return;

            RenderCommandSprite command = new RenderCommandSprite(Sprite)
            {
                Location = Parent.Transform.Position,
                Scale = Parent.Transform.Scale,
                Shader = Material
            };

            Application.Instance.Renderer.Submit(command);
        }
    }
}
