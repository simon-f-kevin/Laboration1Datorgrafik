using Microsoft.Xna.Framework.Graphics;
using Game_Engine.Components;

namespace Assignment3._1.Components
{
    public class ShadowRenderTargetComponent : EntityComponent
    {
        public RenderTarget2D ShadowRenderTarget { get; set; }


        public ShadowRenderTargetComponent(int entityID) : base(entityID)
        {
        }
    }
}
