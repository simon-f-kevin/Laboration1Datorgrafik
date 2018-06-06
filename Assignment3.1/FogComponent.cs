using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class FogComponent : EntityComponent
    {
        public FogComponent(int entityID) : base(entityID)
        {
        }

        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }

    }
}
