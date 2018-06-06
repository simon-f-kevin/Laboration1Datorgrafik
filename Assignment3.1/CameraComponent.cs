using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
   
    public class CameraComponent : EntityComponent
    {
        public CameraComponent(int entityID) : base(entityID)
        {
        }

        public Vector3 CameraPosition   { get; set; }
        public Vector3 CameraForward    { get; set; }
        public Matrix View              { get; set; }
        public Matrix Projection        { get; set; }
        public BoundingFrustum CameraFrustum { get; set; }
        public float AspectRatio { get; set; }

    }
}
