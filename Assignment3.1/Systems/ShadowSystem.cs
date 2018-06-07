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

namespace Assignment3._1
{
    class ShadowSystem : IDrawableSystem
    {
        Matrix World;
        GraphicsDevice Graphics;
        public ShadowSystem(GraphicsDevice graphicsDevice, Matrix world)
        {
            Visible = true;
            Graphics = graphicsDevice;
            World = world;
        }

        public bool Visible { get; set; }

        public void Draw()
        {
            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap();


            DrawWithShadowMap();
        }
        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        void CreateShadowMap()
        {
            var shadowRender = ComponentManager.Instance.getDictionary<ShadowRenderTargetComponent>().Values.FirstOrDefault() as ShadowRenderTargetComponent;
            if (shadowRender == null)
            {
                return;
            }
            Graphics.SetRenderTarget(shadowRender.ShadowRenderTarget);


            Graphics.Clear(Color.White);

            // Draw any occluders in our case that is just the dude model

            // Set the models world matrix so it will rotate
            var models = ComponentManager.Instance.getDictionary<ModelComponent>();
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffect>();
            var fogComp = ComponentManager.Instance.getDictionary<FogComponent>().Values.FirstOrDefault() as FogComponent;
            var ambientComp = ComponentManager.Instance.getDictionary<AmbientComponent>().Values.FirstOrDefault() as AmbientComponent;
            if (cameraComp == null || shadowRender == null || light == null || fogComp == null || ambientComp == null)
            {
                return;
            }
            foreach (ModelComponent modelComp in models.Values)
            {
                EntityComponent shadowMappingEffec;
                if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out shadowMappingEffec))
                {
                    ShadowMappingEffect shadowMappingEffect = (ShadowMappingEffect) shadowMappingEffec;
                    shadowMappingEffect.AmbientComponent = ambientComp;
                    shadowMappingEffect.CameraComponent = cameraComp;
                    shadowMappingEffect.FogComponent = fogComp;
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
            var shadowRender = ComponentManager.Instance.getDictionary<ShadowRenderTargetComponent>().Values.FirstOrDefault() as ShadowRenderTargetComponent;
            var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffect>();
            var fogComp = ComponentManager.Instance.getDictionary<FogComponent>().Values.FirstOrDefault() as FogComponent;
            var ambientComp = ComponentManager.Instance.getDictionary<AmbientComponent>().Values.FirstOrDefault() as AmbientComponent;
            if (cameraComp == null || shadowRender == null || light == null || fogComp == null || ambientComp == null)
            {
                return;
            }
            foreach (ModelComponent modelComp in models.Values)
            {
                EntityComponent shadowMappingEffec;
                if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out shadowMappingEffec))
                {
                    ShadowMappingEffect shadowMappingEffect = (ShadowMappingEffect) shadowMappingEffec;
                    shadowMappingEffect.AmbientComponent = ambientComp;
                    shadowMappingEffect.CameraComponent = cameraComp;
                    shadowMappingEffect.FogComponent = fogComp;
                    shadowMappingEffect.LightComponet = light;
                    shadowMappingEffect.shadowRenderTarget = shadowRender.ShadowRenderTarget;
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