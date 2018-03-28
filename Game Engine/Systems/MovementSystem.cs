using Game_Engine.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game_Engine.Components;
using Game_Engine.Managers;

namespace Game_Engine.Systems
{
    public class MovementSystem : IUpdateableSystem
    {
        private Dictionary<int, EntityComponent> _positions;
        private Dictionary<int, EntityComponent> _velocitites;
        private EntityComponent outValue;
        private int ScreenHeight { get;  }
        private int ScreenWidth { get;  }

        public MovementSystem()
        {
            ScreenHeight = GamePropertyManager.Instance.getGraphics().Viewport.Height;
            ScreenWidth = GamePropertyManager.Instance.getGraphics().Viewport.Width;
        }

        public void Update(GameTime gameTime)
        {
            _positions = ComponentManager.Instance.getDictionary<PositionComponent>();
            _velocitites = ComponentManager.Instance.getDictionary<VelocityComponent>();
            float dT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PositionComponent position;

            foreach(VelocityComponent velocity in _velocitites.Values)
            {
                position = ComponentManager.Instance.getComponentFromDict<PositionComponent>(velocity.EntityID, _positions);

                if(velocity.Acceleration > 1)
                {
                    velocity.VelX += (velocity.VelX * velocity.Acceleration) * dT;
                    velocity.VelY += (velocity.VelY * velocity.Acceleration) * dT;
                }
                if (velocity.VelX > velocity.MaxVelX)
                {
                    velocity.VelX = velocity.MaxVelX;
                }
                if (velocity.VelY > velocity.MaxVelY)
                {
                    velocity.VelY = velocity.MaxVelY;
                }

                position.X += velocity.VelX * dT;
                position.Y += velocity.VelX * dT;

                if(position.X > ScreenWidth)
                {
                    position.X = ScreenWidth;
                }
                if (position.Y > ScreenHeight)
                {
                    position.Y = ScreenWidth;
                }
            }
        }
    }
}
