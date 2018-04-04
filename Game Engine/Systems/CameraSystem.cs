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
                    cameraComp.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, cameraComp.AspectRatio, cameraComp.NearPlane, cameraComp.FarPlane);
                }

                ModelComponent model = ComponentManager.Instance.GetComponentsById<ModelComponent>(cameraComp.EntityID);
                TransformComponent transform = ComponentManager.Instance.GetComponentsById<TransformComponent>(cameraComp.EntityID);
                Vector3 cameraPosition = new Vector3(0, 0, 20);
                cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateFromQuaternion(model.Quaternion));
                cameraPosition += new Vector3(transform.PosX, transform.PosY, transform.PosZ);

                Vector3 cameraUp = Vector3.Up;
                cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromQuaternion(model.Quaternion));

                cameraComp.View = Matrix.CreateLookAt(cameraPosition, new Vector3(transform.PosX, transform.PosY, transform.PosZ), cameraUp);
                cameraComp.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, cameraComp.AspectRatio, cameraComp.NearPlane, cameraComp.FarPlane);
                Console.WriteLine(cameraPosition);
            }
        }
    }
}
