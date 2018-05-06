using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Systems
{
    public class CameraSystem : IUpdateableSystem
    {
        private GraphicsDevice graphicsDevice;

        public CameraSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            
        }

        public void Update(GameTime gameTime)
        {
            var cameras = ComponentManager.Instance.getDictionary<CameraComponent>();
            foreach(CameraComponent cameraComp in cameras.Values)
            {
                if (cameraComp.Follow)
                {
                    ModelComponent model = ComponentManager.Instance.GetComponentsById<ModelComponent>(cameraComp.EntityID);
                    TransformComponent transform = ComponentManager.Instance.GetComponentsById<TransformComponent>(cameraComp.EntityID);
                    
                    //Console.WriteLine(cameraPosition);

                    Vector3 cameraPosition = model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Backward * 20f);
                    Vector3 cameraLookAt = model.model.Bones[0].Transform.Translation + (model.model.Bones[0].Transform.Forward * 20f);

                    cameraComp.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                    var clipProjection = Matrix.CreatePerspectiveFieldOfView(1.1f * MathHelper.PiOver2, graphicsDevice.Viewport.AspectRatio,
                        0.5f * 0.1f, 1.3f * 1000f);
                    cameraComp.BoundingFrustum.Matrix = cameraComp.View * clipProjection;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullClockwiseFace;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                rasterizerState.CullMode = graphicsDevice.RasterizerState.CullMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.Solid;
                rasterizerState.CullMode = graphicsDevice.RasterizerState.CullMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }
        }
    }
}
