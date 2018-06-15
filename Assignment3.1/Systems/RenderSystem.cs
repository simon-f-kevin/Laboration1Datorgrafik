using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Assignment3._1
{
    public class RenderSystem : IUpdateableSystem, IDrawableSystem
    {
        private Matrix worldMatrix;
        private GraphicsDevice graphicsDevice;

        public RenderSystem(GraphicsDevice graphicsDevice, Matrix worldMatrix)
        {
            this.graphicsDevice = graphicsDevice;
            this.worldMatrix = worldMatrix;
        }

        public void Update(GameTime gameTime)
        {
            /*
            var lightSettingsComponent = ComponentManager.Instance.getDictionary<LightSettingsComponent>().Values.FirstOrDefault() as LightSettingsComponent;
            var rotationY = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.00005f;
            var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

            lightSettingsComponent.LightDirection = Vector3.Transform(lightSettingsComponent.LightDirection, rotation);
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                lightSettingsComponent.LightDirection = new Vector3(lightSettingsComponent.LightDirection.X, lightSettingsComponent.LightDirection.Y, lightSettingsComponent.LightDirection.Z - 0.002f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                lightSettingsComponent.LightDirection = new Vector3(lightSettingsComponent.LightDirection.X, lightSettingsComponent.LightDirection.Y, lightSettingsComponent.LightDirection.Z + 0.002f);
            }
            */
        }
        public void Draw()
        {
            var lightSettingsComponent = ComponentManager.Instance.getDictionary<LightSettingsComponent>().Values.FirstOrDefault() as LightSettingsComponent;
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;

            if (cameraComponent == null || lightSettingsComponent == null)
            {
                return;
            }

            CreateLightViewProjectionMatrix(lightSettingsComponent, cameraComponent);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap(lightSettingsComponent, cameraComponent);
            DrawWithShadowMap(lightSettingsComponent, cameraComponent);
        }

        private void CreateLightViewProjectionMatrix(LightSettingsComponent lightSettingsComponent, CameraComponent cameraComponent)
        {

            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero, -lightSettingsComponent.LightDirection, Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraComponent.BoundingFrustrum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;

            // The position of the light should be in the center of the back panel of the box. 
            Vector3 lightPosition = lightBox.Min + (boxSize/2);
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition, Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightViewMatrix = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightSettingsComponent.LightDirection,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjectionMatrix = Matrix.CreateOrthographic(boxSize.X, boxSize.Y, -boxSize.Z, boxSize.Z);

            lightSettingsComponent.LightViewProjection = lightViewMatrix * lightProjectionMatrix;
        }
        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        void CreateShadowMap(LightSettingsComponent lightSettingsComponent, CameraComponent cameraComponent)
        {
            graphicsDevice.SetRenderTarget(lightSettingsComponent.RenderTarget);
            graphicsDevice.Clear(Color.White);

            var modelComponents = ComponentManager.Instance.getDictionary<ModelComponent>().Values;
            foreach (ModelComponent modelComponent in modelComponents)
            {
                DrawModel(modelComponent, "CreateShadowMap");
            }
            graphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Renders the scene using the shadow map to darken the shadow areas
        /// </summary>
        void DrawWithShadowMap(LightSettingsComponent lightSettingsComponent, CameraComponent cameraComponent)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = TextureFilter.Linear,
                ComparisonFunction = CompareFunction.LessEqual,
                FilterMode = TextureFilterMode.Comparison
            };

            var modelComponents = ComponentManager.Instance.getDictionary<ModelComponent>().Values;
            foreach (ModelComponent modelComponent in modelComponents)
            {
                DrawModel(modelComponent, "DrawWithShadowMap");
            }
        }

        /// <summary>
        /// Helper function to draw a model
        /// </summary>
        /// <param name="model">The model to draw</param>
        /// <param name="technique">The technique to use</param>
        void DrawModel(ModelComponent modelComponent, string techniqueName)
        {
            var model = modelComponent.Model;
            var effectSettingsComponent = ComponentManager.Instance.GetComponentsById<EffectSettingsComponent>(modelComponent.EntityID);

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    var world = transforms[mesh.ParentBone.Index] * modelComponent.ObjectWorld * worldMatrix;
                    effectSettingsComponent.Apply(meshPart.Effect, modelComponent.Texture2D, world, techniqueName);

                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }
        }
    }
}
