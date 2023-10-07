using Engine.Core;
using Engine.Graphics;

namespace Engine.Entities.Components
{
    public class AnimatedSpriteRenderer : IComponent
    {
        public SpriteAnimation Sprite { get; set; }
        public ShaderProgram Material { get; set; } = DefaultShaders.AnimatedSpriteShader;


        public override void Render(GameTime time)
        {
            base.Render(time);

            if (Sprite == null)
                return;

            RenderCommandAnimatedSprite command = new RenderCommandAnimatedSprite(Sprite)
            {
                Location = Parent.Transform.Position,
                Scale = Parent.Transform.Scale,
                Shader = Material
            };

            Application.Instance.Renderer.Submit(command);
        }
    }
}
