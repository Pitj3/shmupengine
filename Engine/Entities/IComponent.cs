using Engine.Core;

namespace Engine.Entities
{
    public class IComponent
    {
        public Entity Parent { get; internal set; }

        public virtual void Init() { }
        public virtual void Update(GameTime time) { }
        public virtual void Render(GameTime time) { }
    }

    public static class ComponentExtensions
    {
        public static void Destroy(this IComponent component)
        {
            component.Parent.RemoveComponent(component);
        }
    }
}
