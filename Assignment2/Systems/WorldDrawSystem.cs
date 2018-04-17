using Assignment2.Models;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Systems
{
    public class WorldDrawSystem : IDrawableSystem
    {
        private WorldTerrain worldTerrain;
        private GraphicsDevice graphicsDevice;

        public WorldDrawSystem(WorldTerrain worldTerrain, GraphicsDevice graphicsDevice)
        {
            this.worldTerrain = worldTerrain;
            this.graphicsDevice = graphicsDevice;
        }

        public void Draw()
        {
            Dictionary<int, EntityComponent> cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComponent in cameras.Values)
            {
                worldTerrain.BasicEffect.View = cameraComponent.view;
                worldTerrain.BasicEffect.Projection = cameraComponent.projection;
                worldTerrain.Draw();
            }
           
        }
    }
}
