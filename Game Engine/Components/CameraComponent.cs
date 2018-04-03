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
        public Tuple<ushort, ushort> AspectRatio { get; set; }
        public ushort NearPlane { get; set; }
        public ushort FarPlane { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public CameraComponent(int id)
        {
            EntityID = id;
        }
    }
}
