using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace Assignment3._1.Components
{
   
    public class CameraComponent : EntityComponent
    {
        public Vector3 CameraPosition   { get; set; }
        public Vector3 CameraForward    { get; set; }
        public Matrix View              { get; set; }
        public Matrix Projection        { get; set; }
        public BoundingFrustum CameraFrustum { get; set; }
        public float AspectRatio { get; set; }

        public CameraComponent(int entityID) : base(entityID)
        {
        }
    }
}
