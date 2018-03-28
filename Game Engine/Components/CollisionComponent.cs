namespace Game_Engine.Components
{
    public class CollisionComponent : EntityComponent
    {
        public int CollisionType { get; set; }

        public CollisionComponent()
        {
            CollisionType = 0;
        }

        public CollisionComponent(int id) 
        {
            CollisionType = 0;
            EntityID = id;
        }
    }
}
