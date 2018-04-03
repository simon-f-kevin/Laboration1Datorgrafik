using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class VelocityComponent : EntityComponent
    {
        public float VelX { get; set; }
        public float VelY { get; set; }
        public float VelZ { get; set; }

        public VelocityComponent(int id)
        {
            EntityID = id;
        }

    }
}
