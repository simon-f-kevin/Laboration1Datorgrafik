using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace Assignment3._1.Components
{
    public class LightComponent : EntityComponent
    {
        public Vector3 LightDir { get; set; }
        public Matrix LightViewProjection { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 DiffusColor { get; set; }
        public float DiffuseIntensity { get; set; }

        public LightComponent(int entityID) : base(entityID)
        {
        }
    }
}
