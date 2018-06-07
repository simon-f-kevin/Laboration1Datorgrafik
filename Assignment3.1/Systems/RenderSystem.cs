using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment3._1
{
    public class RenderSystem : IUpdateableSystem, IDrawableSystem
    {

        private Matrix World;
        private GraphicsDevice Graphics;
        public bool Visible { get; set; }

        public RenderSystem(GraphicsDevice graphicsDevice, Matrix world)
        {
            Visible = true;
            Graphics = graphicsDevice;
            World = world;
        }


        public void Update(GameTime gameTime)
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var rotationY = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.00005f;
            var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

            lightComponent.LightDir = Vector3.Transform(lightComponent.LightDir, rotation);
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                lightComponent.LightDir = new Vector3(lightComponent.LightDir.X, lightComponent.LightDir.Y, lightComponent.LightDir.Z - 0.002f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                lightComponent.LightDir = new Vector3(lightComponent.LightDir.X, lightComponent.LightDir.Y, lightComponent.LightDir.Z + 0.002f);
            }
        }
        public void Draw()
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            CreateLightViewProjectionMatrix(lightComponent, cameraComp);

            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap();
            DrawWithShadowMap();
        }

        private void CreateLightViewProjectionMatrix(LightComponent lightComponent, CameraComponent cameraComp)
        {

            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightComponent.LightDir,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraComp.CameraFrustum.GetCorners();

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
                                                   lightPosition - lightComponent.LightDir,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            lightComponent.LightViewProjection = lightView * lightProjection;
        }
        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        void CreateShadowMap()
        {
            var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            if (light == null)
            {
                return;
            }
            Graphics.SetRenderTarget(light.ShadowRenderTarget);
            Graphics.Clear(Color.White);

            // Draw any occluders in our case that is just the dude model

            // Set the models world matrix so it will rotate
            var models = ComponentManager.Instance.getDictionary<ModelComponent>();
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffect>();
            if (cameraComp == null || light == null)
            {
                return;
            }
            foreach (ModelComponent modelComp in models.Values)
            {
                EntityComponent shadowMappingEffec;
                if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out shadowMappingEffec))
                {
                    ShadowMappingEffect shadowMappingEffect = (ShadowMappingEffect)shadowMappingEffec;
                    shadowMappingEffect.CameraComponent = cameraComp;
                    shadowMappingEffect.LightComponet = light;
                    shadowMappingEffect.shadowRenderTarget = null;
                    DrawModel(modelComp, true, shadowMappingEffect);
                }

            }
            // Draw the dude model
            // Set render target back to the back buffer
            Graphics.SetRenderTarget(null);
        }


        /// <summary>
        /// Renders the scene using the shadow map to darken the shadow areas
        /// </summary>
        void DrawWithShadowMap()
        {
            Graphics.Clear(Color.CornflowerBlue);
            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.Default;
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            Graphics.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = TextureFilter.Linear,
                ComparisonFunction = CompareFunction.LessEqual,
                FilterMode = TextureFilterMode.Comparison
            };


            var models = ComponentManager.Instance.getDictionary<ModelComponent>();
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffect>();
            if (cameraComp == null || light == null)
            {
                return;
            }
            foreach (ModelComponent modelComp in models.Values)
            {
                EntityComponent shadowMappingEffec;
                if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out shadowMappingEffec))
                {
                    ShadowMappingEffect shadowMappingEffect = (ShadowMappingEffect)shadowMappingEffec;
                    shadowMappingEffect.CameraComponent = cameraComp;
                    shadowMappingEffect.LightComponet = light;
                    shadowMappingEffect.shadowRenderTarget = light.ShadowRenderTarget;
                    DrawModel(modelComp, false, shadowMappingEffect);
                }
            }
        }

        /// <summary>
        /// Helper function to draw a model
        /// </summary>
        /// <param name="model">The model to draw</param>
        /// <param name="technique">The technique to use</param>
        void DrawModel(ModelComponent modelComp, bool createShadowMap, ShadowMappingEffect _ShadowMappingEffect)
        {
            var model = modelComp.Model;
            string techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            // Loop over meshs in the modelgraphicsDevice.RasterizerState
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (var meshPart in mesh.MeshParts)
                {
                    _ShadowMappingEffect.AddEffect(meshPart.Effect, modelComp.Texture2D);
                    _ShadowMappingEffect.Techniquename = techniqueName;
                    _ShadowMappingEffect.world = transforms[mesh.ParentBone.Index] * modelComp.ObjectWorld * World;
                    _ShadowMappingEffect.createShadowMap = createShadowMap;
                    _ShadowMappingEffect.Apply();

                    Graphics.SetVertexBuffer(meshPart.VertexBuffer);
                    Graphics.Indices = meshPart.IndexBuffer;
                    Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);

                }
            }
        }
    }
}
