using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Assignment3._1
{
    public class RenderSystem : IDrawableSystem
    {
        private Matrix worldMatrix;
        private GraphicsDevice graphicsDevice;

        public RenderSystem(GraphicsDevice graphicsDevice, Matrix worldMatrix)
        {
            this.graphicsDevice = graphicsDevice;
            this.worldMatrix = worldMatrix;
        }

        public void Draw()
        {
            var lightSettingsComponent = ComponentManager.Instance.getDictionary<LightSettingsComponent>().Values.FirstOrDefault() as LightSettingsComponent;
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;

            if (cameraComponent == null || lightSettingsComponent == null)
            {
                return;
            }

            CreateLightViewProjection(lightSettingsComponent, cameraComponent);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            CreateShadowMap(lightSettingsComponent, cameraComponent);
            DrawWithShadowMap(lightSettingsComponent, cameraComponent);
        }

        private void CreateLightViewProjection(LightSettingsComponent lightSettingsComponent, CameraComponent cameraComponent)
        {
            Matrix lightRotationMatrix = Matrix.CreateLookAt(Vector3.Zero, -lightSettingsComponent.LightDirection, Vector3.Up);
            Vector3[] frustumCornerPositions = cameraComponent.BoundingFrustrum.GetCorners();
            for (int i = 0; i < frustumCornerPositions.Length; i++)
            {
                frustumCornerPositions[i] = Vector3.Transform(frustumCornerPositions[i], lightRotationMatrix);
            }

            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCornerPositions);
            Vector3 lightBoxSize = lightBox.Max - lightBox.Min;
            Vector3 lightBoxPosition = lightBox.Min + (lightBoxSize/2);
            lightBoxPosition = Vector3.Transform(lightBoxPosition, Matrix.Invert(lightRotationMatrix));

            Matrix lightViewMatrix = Matrix.CreateLookAt(lightBoxPosition,
                                                   lightBoxPosition - lightSettingsComponent.LightDirection,
                                                   Vector3.Up);
            Matrix lightProjectionMatrix = Matrix.CreateOrthographic(lightBoxSize.X, lightBoxSize.Y, -lightBoxSize.Z, lightBoxSize.Z);
            lightSettingsComponent.LightViewProjection = lightViewMatrix * lightProjectionMatrix;
        }

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
