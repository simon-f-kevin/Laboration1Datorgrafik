using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace Assignment3._1.Components
{
    public class AmbientComponent : EntityComponent
    {
        public Vector4 AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }

        public AmbientComponent(int entityID) : base(entityID)
        {
        }
    }
}
