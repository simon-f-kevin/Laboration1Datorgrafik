using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class UpperArmComponent : EntityComponent
    {
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _position = new Vector3(0, 1f, 0);
        private Vector3 _jointPos = new Vector3(0, 1.5f, 0);
        public UpperArmComponent(int entityID) : base(entityID)
        {
            
        }
    }
}
