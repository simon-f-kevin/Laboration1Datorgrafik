using Game_Engine.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    public class TextureRenderSystem : IDrawableSystem
    {
        private Dictionary<int, EntityComponent> _textures;
        private Dictionary<int, EntityComponent> _positions;

        public TextureRenderSystem()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _textures = ComponentManager.Instance.getDictionary<TextureComponent>();
            _positions = ComponentManager.Instance.getDictionary<PositionComponent>();

            PositionComponent position;
            foreach(TextureComponent texture in _textures.Values)
            {
                position = ComponentManager.Instance.getComponentFromDict<PositionComponent>(texture.EntityID, _positions);
                if(position == null)
                {
                    continue;
                }
               spriteBatch.Draw(texture: texture.sprite,
                    destinationRectangle: new Rectangle((int)position.X, (int)position.Y, texture.width, texture.height),
                    color: Color.White);
            }
        }
    }
}
