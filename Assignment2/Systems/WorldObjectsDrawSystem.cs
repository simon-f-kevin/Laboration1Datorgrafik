using Assignment2.Models;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Systems
{

    public class WorldObjectsDrawSystem : IDrawableSystem
    {
        public List<House> Houses { get; set; }
        public List<Tree> Trees { get; set; }

        private Dictionary<int, EntityComponent> cameras;

        public WorldObjectsDrawSystem()
        {
            
        }

        public void Draw()
        {
            cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComponent in cameras.Values)
            {
                foreach (var house in Houses)
                {
                    house.Draw(cameraComponent.view, cameraComponent.projection);
                }
                foreach (var tree in Trees)
                {
                    tree.Draw(cameraComponent.view, cameraComponent.projection);
                }
            }
            
        }
    }
}
