using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    public interface IUpdateableSystem
    {
        void Update(GameTime gameTime);
    }
}
