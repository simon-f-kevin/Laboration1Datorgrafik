using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Robot
{
    public class RobotCameraSystem : IUpdateableSystem
    {
        private RobotArm robotArm;
        public RobotCameraSystem(RobotArm robotArm)
        {
            this.robotArm = robotArm;
        }
        public void Update(GameTime gameTime)
        {
            var cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach (CameraComponent cameraComp in cameras.Values)
            {
                if (cameraComp.Follow)
                {
                    //ModelComponent model = ComponentManager.Instance.GetComponentsById<ModelComponent>(cameraComp.EntityID);
                    

                    

                    Vector3 cameraPosition = robotArm.World.Translation + robotArm.World.Backward * 20f;//model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Backward * 20f);
                    Vector3 cameraLookAt = robotArm.World.Translation + robotArm.World.Forward * 20f; //model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Forward * 20f);

                    cameraComp.view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                    Console.WriteLine("campos " + cameraPosition);
                    Console.WriteLine("cam lookat " + cameraLookAt);
                }
            }
        }
    }
}
