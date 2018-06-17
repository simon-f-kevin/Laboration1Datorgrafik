using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{
    public class TransformComponent : EntityComponent
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public TransformComponent(int entityID) : base(entityID)
        {

        }
    }
}
