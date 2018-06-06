using System.Collections.Generic;
using System.Linq;
using Assignment3._1.Components;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CameraComponent = Assignment3._1.Components.CameraComponent;
using LightComponent = Assignment3._1.Components.LightComponent;
using ModelComponent = Assignment3._1.Components.ModelComponent;

namespace Assignment3._1.Systems
{
    public class ShadowSystem : IDrawableSystem
    {
        private GraphicsDevice graphicsDevice;

        public ShadowSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Visible = true;
        }

        public bool Visible { get; set; }

        public void Draw()
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap();
            DrawWithShadowMap();
        }
        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        void CreateShadowMap()
        {
            if (ComponentManager.Instance.getDictionary<ShadowRenderTargetComponent>().Values.First() is
                ShadowRenderTargetComponent shadowRender)
            {
                graphicsDevice.SetRenderTarget(shadowRender.ShadowRenderTarget);


                graphicsDevice.Clear(Color.White);

                // Draw any occluders in our case that is just the dude model

                // Set the models world matrix so it will rotate
                var models = ComponentManager.Instance.getDictionary<ModelComponent>();
                var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault();
                var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault();
                var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffectComponent>();
                var fogComp = ComponentManager.Instance.getDictionary<FogComponent>().Values.FirstOrDefault();
                var ambientComp = ComponentManager.Instance.getDictionary<AmbientComponent>().Values.FirstOrDefault();
                if (cameraComp == null || shadowRender == null || light == null || fogComp == null ||
                    ambientComp == null)
                {
                    return;
                }

                foreach (var modelComp in models.Values)
                {
                    EntityComponent temp;
                    if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out temp))
                    {
                        ShadowMappingEffectComponent shadowMappingEffect = temp as ShadowMappingEffectComponent;
                        shadowMappingEffect.AmbientComponent = ambientComp as AmbientComponent;
                        shadowMappingEffect.CameraComponent = cameraComp as CameraComponent;
                        shadowMappingEffect.FogComponent = fogComp as FogComponent;
                        shadowMappingEffect.LightComponet = light as LightComponent;
                        shadowMappingEffect.shadowRenderTarget = null;
                        DrawModel((ModelComponent) modelComp, true, shadowMappingEffect);
                    }
                }

                // Draw the dude model
                // Set render target back to the back buffer
                graphicsDevice.SetRenderTarget(null);
            }
        }


        /// <summary>
        /// Renders the scene using the shadow map to darken the shadow areas
        /// </summary>
        void DrawWithShadowMap()
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


            var models = ComponentManager.Instance.getDictionary<ModelComponent>();
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault();
            var shadowRender = ComponentManager.Instance.getDictionary<ShadowRenderTargetComponent>().Values.First() as ShadowRenderTargetComponent;
            var light = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault();
            var shadowMappingEffects = ComponentManager.Instance.getDictionary<ShadowMappingEffectComponent>();
            var fogComp = ComponentManager.Instance.getDictionary<FogComponent>().Values.FirstOrDefault();
            var ambientComp = ComponentManager.Instance.getDictionary<AmbientComponent>().Values.FirstOrDefault();
            if (cameraComp == null || shadowRender == null || light == null || fogComp == null || ambientComp == null)
            {
                return;
            }
            foreach (var modelComp in models.Values)
            {
                EntityComponent tComponent;
                if (shadowMappingEffects.TryGetValue(modelComp.EntityID, out tComponent))
                {
                    ShadowMappingEffectComponent shadowMappingEffect = tComponent as ShadowMappingEffectComponent;
                    shadowMappingEffect.AmbientComponent = ambientComp as AmbientComponent;
                    shadowMappingEffect.CameraComponent = cameraComp as CameraComponent;
                    shadowMappingEffect.FogComponent = fogComp as FogComponent;
                    shadowMappingEffect.LightComponet = light as LightComponent;
                    shadowMappingEffect.shadowRenderTarget = shadowRender.ShadowRenderTarget;
                    DrawModel((ModelComponent)modelComp, false, shadowMappingEffect);
                }

            }
        }

        /// <summary>
        /// Helper function to draw a model
        /// </summary>
        /// <param name="model">The model to draw</param>
        /// <param name="technique">The technique to use</param>
        void DrawModel(ModelComponent modelComp, bool createShadowMap, ShadowMappingEffectComponent _ShadowMappingEffect)
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
                    _ShadowMappingEffect.world =
                        transforms[mesh.ParentBone.Index] * modelComp.ObjectWorld * Matrix.Identity;
                    _ShadowMappingEffect.createShadowMap = createShadowMap;
                    _ShadowMappingEffect.Apply();

                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
