using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class AmbientComponent : EntityComponent
    {
        public AmbientComponent(int entityID) : base(entityID)
        {
        }

        //public Vector4 AmbientColor { get; set; }
        //public float AmbientIntensity { get; set; }

    }
}
