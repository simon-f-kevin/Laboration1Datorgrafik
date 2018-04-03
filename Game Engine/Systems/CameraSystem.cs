using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Systems
{
    public class CameraSystem : IUpdateableSystem
    {
        
        private GraphicsDevice _graphicsDevice;

        private Dictionary<int, EntityComponent> _cameras;

        public CameraSystem(GraphicsDevice gd)
        {
            _graphicsDevice = gd;
        }

        public void Update(GameTime gameTime)
        {
            _cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComp in _cameras.Values)
            {
                if(cameraComp.View == cameraComp.Projection)
                {
                    cameraComp.View = Matrix.CreateLookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.Up);
                    cameraComp.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _graphicsDevice.Viewport.AspectRatio, cameraComp.NearPlane, cameraComp.FarPlane);
                }
            }
        }
    }
}
