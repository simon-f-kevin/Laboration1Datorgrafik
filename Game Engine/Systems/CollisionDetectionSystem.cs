using Game_Engine.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game_Engine.Mediator;
using Game_Engine.Managers;
using Game_Engine.Components;

namespace Game_Engine.Systems
{
    public class CollisionDetectionSystem : IUpdateableSystem
    {
        private Dictionary<int, EntityComponent> _rectangles;
        private Dictionary<int, EntityComponent> _positions;
        private Dictionary<int, EntityComponent> _collisions;
        private Dictionary<int, EntityComponent> _velocities;
        private Dictionary<int, EntityComponent> _textures;
        private IMediator _mediator;
        private GameTime _gameTime;
        private EntityComponent _outValue;

        public CollisionDetectionSystem(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public void Update(GameTime gameTime)
        {
            _rectangles = ComponentManager.Instance.getDictionary<RectangleComponent>();
            _positions = ComponentManager.Instance.getDictionary<PositionComponent>();
            _collisions = ComponentManager.Instance.getDictionary<CollisionComponent>();
            _velocities = ComponentManager.Instance.getDictionary<VelocityComponent>();
            _textures = ComponentManager.Instance.getDictionary<TextureComponent>();
            _gameTime = gameTime;
            PositionUpdate();
            CollisionDetection();
        }

        private void CollisionDetection()
        {
            CollisionComponent cc;

            foreach(RectangleComponent rectangle in _rectangles.Values)
            {
                cc = ComponentManager.Instance.getComponentFromDict<CollisionComponent>(rectangle.EntityID, _collisions);
                if(cc != null)
                {
                    foreach(RectangleComponent nextRectangle in _rectangles.Values)
                    {
                        if(rectangle.EntityID < nextRectangle.EntityID &&
                           rectangle.BoundingRectangle.Intersects(nextRectangle.BoundingRectangle))
                        {
                            _mediator.sendMessage(new CollisionMediatorMessage(rectangle.EntityID, nextRectangle.EntityID));
                        }
                    }
                }
            }
        }

        private void PositionUpdate()
        {
            PositionComponent pos;
            foreach(RectangleComponent rectangle in _rectangles.Values)
            {
                pos = ComponentManager.Instance.getComponentFromDict<PositionComponent>(rectangle.EntityID, _positions);
                if(pos == null)
                {
                    continue;
                }
                Rectangle bounding = rectangle.BoundingRectangle;
                bounding.X = (int)pos.X;
                bounding.Y = (int)pos.Y;
                rectangle.BoundingRectangle = bounding;
            }
        }
    }
}
