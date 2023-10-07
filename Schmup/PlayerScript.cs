using Engine.Core;
using Engine.Entities;
using Engine.Entities.Components;
using Silk.NET.GLFW;
using Silk.NET.Maths;

namespace Schmup
{
    public class PlayerScript : IComponent
    {
        private readonly float _playerSpeed = 300.0f;
        private SpriteRenderer _renderer;

        public override void Init()
        {
            base.Init();

            _renderer = Parent.GetComponent<SpriteRenderer>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            Vector2D<float> direction = new Vector2D<float>();
            if (Input.IsKeyDown(Keys.W))
            {
                direction.Y = 1;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                direction.Y = -1;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                direction.X = -1;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                direction.X = 1;
            }

            Vector2D.Normalize(direction);

            Parent.Transform.Position += direction * _playerSpeed * time.Delta;

            Parent.Transform.Position.X = Math.Clamp(Parent.Transform.Position.X, 0, Application.Instance.Width - _renderer.Sprite.Width);
            Parent.Transform.Position.Y = Math.Clamp(Parent.Transform.Position.Y, 0, Application.Instance.Height - _renderer.Sprite.Height);
        }
    }
}
