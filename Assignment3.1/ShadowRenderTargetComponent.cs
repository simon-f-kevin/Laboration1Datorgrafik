using Game_Engine.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class ShadowRenderTargetComponent : EntityComponent
    {
        public RenderTarget2D ShadowRenderTarget { get; set; }
        public ShadowRenderTargetComponent(int entityID) : base(entityID)
        {

        }

    }
}
