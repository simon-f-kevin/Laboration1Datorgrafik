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
        public Vector3 speed { get; set; }

        public VelocityComponent(int id) : base(id)
        {
            speed = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
