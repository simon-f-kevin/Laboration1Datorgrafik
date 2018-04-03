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
        public ushort FOV { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public CameraComponent(int id)
        {
            EntityID = id;
        }
    }
}
