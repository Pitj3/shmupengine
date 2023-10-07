namespace Engine.Entities
{
    public class Scene
    {
        internal List<Entity> _entities = new List<Entity>();
        public static Scene Current { get; private set; }

        public Scene() 
        {
            Current = this;
        }

        public List<Entity> GetEntities()
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(_entities);

            return entities;
        }

        internal void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        internal void RemoveEntity(Entity entity)
        {
            foreach(IComponent component in entity.GetComponents()) 
            {
                component.Destroy();
            }

            _entities.Remove(entity);
        }
    }
}
