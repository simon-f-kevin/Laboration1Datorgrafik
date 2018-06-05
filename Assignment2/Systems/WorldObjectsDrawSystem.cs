using Assignment2.Models;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Systems
{

    public class WorldObjectsDrawSystem : IDrawableSystem
    {
        public List<House> Objects { get; set; }

        private Dictionary<int, EntityComponent> cameras;

        public WorldObjectsDrawSystem()
        {
            
        }

        public void Draw()
        {
            cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComponent in cameras.Values)
            {
                foreach (var obj in Objects)
                {
                    obj.Draw(cameraComponent.View, cameraComponent.Projection);
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
