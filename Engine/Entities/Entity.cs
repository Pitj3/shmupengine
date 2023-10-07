using Silk.NET.Maths;

namespace Engine.Entities
{
    public class Transform
    {
        public Vector2D<float> Position;
        public Vector2D<float> Scale { get; set; } = Vector2D<float>.One;
        public float Rotation { get; set; } = 0;
    }

    public class Entity
    {
        internal readonly List<IComponent> _components = new List<IComponent>();

        public Transform Transform { get; set; }

        public Entity() 
        {
            Transform = new Transform();
        }

        public List<IComponent> GetComponents()
        {
            List<IComponent> components = new List<IComponent>();
            components.AddRange(_components);

            return components;
        }

        public T AddComponent<T>() where T : IComponent, new()
        {
            T component = new T
            {
                Parent = this
            };

            _components.Add(component);

            return (T)component;
        }

        public T GetComponent<T>() where T : IComponent, new()
        {
            foreach(IComponent component in _components)
            {
                if (component is T t)
                    return t;
            }

            throw new NullReferenceException("Could not find component");

        }

        internal void RemoveComponent(IComponent component)
        {
            _components.Remove(component);
        }

        public static Entity Spawn(Vector2D<float> position, float rotation)
        {
            Entity e = new Entity();
            e.Transform.Position = position;
            e.Transform.Rotation = rotation;

            Scene.Current.AddEntity(e);

            return e;
        }
    }

    public static class EntityExtensions
    {
        public static void Destroy(this Entity entity)
        {
            Scene.Current.RemoveEntity(entity);
        }
    }
}
