using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class RobotArmComponent : EntityComponent
    {
        public Vector3 _rotation = Vector3.Zero;
        public Vector3 _position = Vector3.Zero;

        public RobotArmComponent(int entityID) : base(entityID)
        {
        }
    }
}
