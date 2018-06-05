using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment3._1
{
    public class RenderSystem
    {
        GraphicsDevice graphicsDevice;
        RenderTarget2D shadowRenderTarget;
        Matrix worldRenderMatrix;

        float rotationthingy = 0.0f;

        public RenderSystem(GraphicsDevice graphics)
        {
            graphicsDevice = graphics;

            shadowRenderTarget = new RenderTarget2D(graphicsDevice,
                                                    2048,
                                                    2048,
                                                    false,
                                                    SurfaceFormat.Single,
                                                    DepthFormat.Depth24);
        }

        internal void Update(GameTime gameTime)
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var rotationY = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.00005f;
            var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

            lightComponent.LightDirection = Vector3.Transform(lightComponent.LightDirection, rotation);
        }

        internal void Draw()
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            CreateLightViewProjectionMatrix(lightComponent, cameraComponent);


            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap();
            DrawWithShadowMap();
        }

        private void CreateLightViewProjectionMatrix(LightComponent lightComponent, CameraComponent cameraComponent)
        {
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightComponent.LightDirection,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraComponent.BoundingFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightComponent.LightDirection,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            lightComponent.LightViewProjection =  lightView * lightProjection;
        }

        private void CreateShadowMap()
        {
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;
            graphicsDevice.SetRenderTarget(shadowRenderTarget);
            graphicsDevice.Clear(Color.White);

            worldRenderMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(rotationthingy));

            foreach(ModelComponent model in models)
            {
                if((string)model.model.Tag == "ground")
                {
                    worldRenderMatrix = Matrix.Identity;
                    DrawModel(model, "CreateShadowMap");
                }
                DrawModel(model, "CreateShadowMap");
            }
            graphicsDevice.SetRenderTarget(null);
        }

        

        private void DrawWithShadowMap()
        {
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;

            graphicsDevice.Clear(Color.CornflowerBlue);

            graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            worldRenderMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(rotationthingy));
            foreach (ModelComponent model in models)
            {
                DrawModel(model, "DrawWithShadowMap");
            }
        }

        private void DrawModel(ModelComponent model, string techniqueName)
        {
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            foreach (ModelMesh mesh in model.model.Meshes)
            {
                // Loop over effects in the mesh
                foreach (Effect effect in mesh.Effects)
                {
                    // Set the currest values for the effect
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(worldRenderMatrix);
                    effect.Parameters["View"].SetValue(cameraComp.View);
                    effect.Parameters["Projection"].SetValue(cameraComp.Projection);
                    effect.Parameters["LightDirection"].SetValue(lightComponent.LightDirection);
                    effect.Parameters["LightViewProj"].SetValue(lightComponent.LightViewProjection);

                    if (techniqueName.Equals("DrawWithShadowMap"))
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                }
                // Draw the mesh
                mesh.Draw();
            }
        }
    }
}
