using Assignment2.Models;
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

namespace Assignment2.Systems
{
    public class PlayerCameraSystem : IUpdateableSystem
    {
        private Player player;
        public PlayerCameraSystem(Player player)
        {
            this.player = player;
        }
        public void Update(GameTime gameTime)
        {
            var cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach (CameraComponent cameraComp in cameras.Values)
            {
                if (cameraComp.Follow)
                {
                    Vector3 cameraPosition = player.WorldMatrix.Translation + player.WorldMatrix.Backward * 40f;//model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Backward * 20f);
                    Vector3 cameraLookAt = player.WorldMatrix.Translation;//model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Forward * 20f);

                    cameraComp.view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                    Console.WriteLine("campos " + cameraPosition);
                    //Console.WriteLine("cam lookat " + cameraLookAt);
                }
            }
        }
    }
}
