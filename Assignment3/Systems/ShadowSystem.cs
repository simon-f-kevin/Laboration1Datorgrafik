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

namespace Assignment3.Systems
{
    public class ShadowSystem : IDrawableSystem
    {
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D renderTarget;

        public ShadowSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            renderTarget = new RenderTarget2D(graphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24);
        }

        public void Draw()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap();
            DrawShadowMap();
            //DrawShadowMapToScreen(spriteBatch);
        }

        private void CreateShadowMap()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;

            foreach(ModelComponent modelComp in models)
            {
                TransformComponent transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComp.EntityID);
                DrawModel(modelComp, transformComponent, lightComponent, cameraComponent, "CreateShadowMap");
                graphicsDevice.SetRenderTarget(null);
            }
            
        }

        private void DrawShadowMap()
        {
            graphicsDevice.Clear(Color.CornflowerBlue);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //graphicsDevice.SamplerStates[0] = new SamplerState
            //{
            //    AddressU = TextureAddressMode.Clamp,
            //    AddressV = TextureAddressMode.Clamp,
            //    AddressW = TextureAddressMode.Clamp,
            //    Filter = TextureFilter.Linear,
            //    ComparisonFunction = CompareFunction.LessEqual,
            //    FilterMode = TextureFilterMode.Comparison
            //};
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;

            foreach (ModelComponent modelComp in models)
            {
                TransformComponent transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComp.EntityID);
                DrawModel(modelComp, transformComponent, lightComponent, cameraComponent, "DrawWithShadowMap");
            }
            
        }
        void DrawShadowMapToScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 128, 128), Color.White);
            spriteBatch.End();

            graphicsDevice.Textures[0] = null;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DrawModel(ModelComponent modelComp, TransformComponent transformComponent, LightComponent lightComponent, CameraComponent cameraComponent, string technique)
        {
            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    modelComp.Effect.CurrentTechnique = modelComp.Effect.Techniques[technique];
                    if (technique.Contains("DrawWithShadowMap"))
                    {
                        modelComp.Effect.Parameters["ShadowMap"].SetValue(renderTarget);
                    }
                    modelComp.Effect.Parameters["Texture"].SetValue(meshPart.Effect.Parameters["Texture"].GetValueTexture2D());
                    modelComp.Effect.Parameters["Texture"].SetValue(modelComp.Texture);
                    modelComp.Effect.Parameters["World"].SetValue(Matrix.CreateWorld(transformComponent.Position, Vector3.Forward, Vector3.Up)); // Matrix.CreateTranslation(transformComponent.Position)
                    modelComp.Effect.Parameters["View"].SetValue(cameraComponent.View);
                    modelComp.Effect.Parameters["Projection"].SetValue(cameraComponent.Projection);
                    modelComp.Effect.Parameters["LightDirection"].SetValue(lightComponent.LightDirection);
                    modelComp.Effect.Parameters["LightViewProj"].SetValue(lightComponent.LightViewProjection);
                    modelComp.Effect.Parameters["AmbientColor"].SetValue(lightComponent.AmbientColor);
                    modelComp.Effect.Parameters["AmbientIntensity"].SetValue(lightComponent.AmbientIntensity);
                    modelComp.Effect.Parameters["DiffuseLightDirection"].SetValue(lightComponent.DiffuseLightDirection);
                    modelComp.Effect.Parameters["DiffuseColor"].SetValue(lightComponent.DiffuseColor);
                    modelComp.Effect.Parameters["DiffuseIntensity"].SetValue(lightComponent.DiffuseIntensity);
                    modelComp.Effect.Parameters["CameraPosition"].SetValue(cameraComponent.Position);

                    modelComp.Effect.Parameters["ShadowStrenght"].SetValue(0.9f);
                    modelComp.Effect.Parameters["DepthBias"].SetValue(0.1f);
                    modelComp.Effect.Parameters["ViewVector"].SetValue(Vector3.One);
                    modelComp.Effect.Parameters["Shininess"].SetValue(0.9f);
                    modelComp.Effect.Parameters["SpecularColor"].SetValue(Color.CornflowerBlue.ToVector4());
                    modelComp.Effect.Parameters["SpecularIntensity"].SetValue(0.1f);

                    modelComp.Effect.Parameters["FogStart"].SetValue(100f);
                    modelComp.Effect.Parameters["FogEnd"].SetValue(150f);
                    modelComp.Effect.Parameters["FogColor"].SetValue(Color.CornflowerBlue.ToVector4());
                    modelComp.Effect.Parameters["FogEnabled"].SetValue(true);

                    foreach (var pass in modelComp.Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                    }

                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }
        }
    }
}
