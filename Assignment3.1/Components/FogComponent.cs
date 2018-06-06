using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace Assignment3._1.Components
{
    public class FogComponent : EntityComponent
    {
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }

        public FogComponent(int entityID) : base(entityID)
        {
        }
    }
}
