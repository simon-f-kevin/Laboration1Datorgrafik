using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public abstract class EffectSettingsComponent : EntityComponent
    {
        protected EffectSettingsComponent(int entityID) : base(entityID)
        {

        }
        public abstract void Apply(Effect effectIN, Texture2D texture2D, Matrix worldMatrix, string techniqueName);

    }
}
