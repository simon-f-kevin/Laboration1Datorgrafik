using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Robot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Systems
{
    public class CameraSystem : IUpdateableSystem
    {
        private GraphicsDevice graphicsDevice;
        private RobotBody robot;

        public CameraSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            
        }

        public void SetModelToFollow(IGameObject player)
        {
            robot = (RobotBody)player;
        }

        public void Update(GameTime gameTime)
        {
            var cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComp in cameras.Values)
            {
                if (cameraComp.Follow)
                {
                    //Console.WriteLine(cameraPosition);

                    Vector3 cameraPosition = robot.WorldMatrix.Translation + Vector3.Backward * 20f;//model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Backward * 20f);
                    Vector3 cameraLookAt = robot.WorldMatrix.Translation + Vector3.Forward * 20f;//model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Forward * 20f);

                    cameraComp.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                    var clipProjection = Matrix.CreatePerspectiveFieldOfView(1.1f * MathHelper.PiOver2, graphicsDevice.Viewport.AspectRatio,
                        0.5f * 0.1f, 1.3f * 1000f);
                    cameraComp.BoundingFrustum.Matrix = cameraComp.View * clipProjection;
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        cameraComp.Position += Vector3.Up;
                        cameraComp.View = Matrix.CreateLookAt(cameraComp.Position, Vector3.Forward, Vector3.Up);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        cameraComp.Position += Vector3.Down;
                        cameraComp.View = Matrix.CreateLookAt(cameraComp.Position, Vector3.Forward, Vector3.Up);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        cameraComp.Position += Vector3.Left;
                        cameraComp.View = Matrix.CreateLookAt(cameraComp.Position, Vector3.Forward, Vector3.Up);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        cameraComp.Position += Vector3.Right;
                        cameraComp.View = Matrix.CreateLookAt(cameraComp.Position, Vector3.Forward, Vector3.Up);
                    }
                }
            }
            
        }
    }
}
