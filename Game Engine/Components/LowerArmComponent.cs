using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    class LowerArmComponent : EntityComponent
    {
        public Vector3 _rotation = Vector3.Zero;
        public Vector3 _position = new Vector3(0, 1.5f, 0);
        public Vector3 _jointPos = new Vector3(0, 0.5f, 0);

        protected LowerArmComponent(int entityID) : base(entityID)
        {
        }
    }
}
