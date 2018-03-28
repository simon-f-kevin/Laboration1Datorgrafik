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
        public float Acceleration { get; set; }
        public float MaxVelX { get; set; }
        public float MaxVelY { get; set; }

        public VelocityComponent(int id)
        {
            EntityID = id;
        }

        public void setValues(Vector2 velocity, Vector2 maxVel, float acceleration)
        {
            VelX = velocity.X;
            VelY = velocity.Y;
            MaxVelX = maxVel.X;
            MaxVelY = maxVel.Y;
            Acceleration = acceleration;
        }
    }
}
