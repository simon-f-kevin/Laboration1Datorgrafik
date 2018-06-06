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
        private Effect effect;

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
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            CreateLightViewProjectionMatrix(lightComponent, cameraComponent);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var modelComponents =
                ComponentManager.Instance.getDictionary<ModelComponent>();
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SetRenderTarget(shadowRenderTarget);
            //graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            DrawScene("ShadowMap", modelComponents.Values);
            //graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawScene("ShadowedScene", modelComponents.Values);
            graphicsDevice.SetRenderTarget(null);
            //DrawShadows(spriteBatch);
            //CreateShadowMap();
            //DrawWithShadowMap();
        }

        private void DrawScene(string technique, Dictionary<int, EntityComponent>.ValueCollection modelComponents)
        {
            var modelComp = (ModelComponent) modelComponents.First();
            var cameraComponent =
                ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var lightComponent =
                ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            effect = modelComp.Effect;
            foreach (ModelComponent model in modelComponents)
            {
                effect.CurrentTechnique = effect.Techniques[technique];
                effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * cameraComponent.View * cameraComponent.Projection);
                effect.Parameters["xTex"].SetValue(modelComp.Texture);
                effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                effect.Parameters["xLightPosition"].SetValue(lightComponent.LightPosition);
                effect.Parameters["xLightPower"].SetValue(lightComponent.LightIntensity);
                effect.Parameters["xAmbientLight"].SetValue(lightComponent.AmbientIntensity);
                effect.Parameters["xLightWorldViewProjection"].SetValue(Matrix.Identity * lightComponent.LightViewProjection);
                effect.Parameters["xShadowMap"].SetValue((Texture2D)shadowRenderTarget);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.SetVertexBuffer(model.model.Meshes[0].MeshParts[0].VertexBuffer);
                    graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 16);
                }
                DrawModel(model, technique);
            }
            
        }

        #region stuff
        private void DrawShadows(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(shadowRenderTarget);
            var modelComponents = ComponentManager.Instance.getDictionary<ModelComponent>().Values;
            foreach (ModelComponent modelComponent in modelComponents)
            {
                TransformComponent transformComponent =
                    ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComponent.EntityID);
                DrawShadowMap(modelComponent, transformComponent);
            }
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.AnisotropicClamp);
            spriteBatch.Draw((Texture2D)shadowRenderTarget, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        private void DrawShadowMap(ModelComponent modelComponent, TransformComponent transformComponent)
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            foreach (ModelMesh mesh in modelComponent.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    modelComponent.Effect.Parameters["WorldViewProj"].SetValue(modelComponent.objectWorld * lightComponent.LightViewProjection);
                    modelComponent.Effect.CurrentTechnique.Passes[0].Apply();

                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }
        }
        #endregion  

        private void CreateLightViewProjectionMatrix(LightComponent lightComponent, CameraComponent cameraComponent)
        {
            #region light code
            //Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
            //                                           -lightComponent.LightDirection,
            //                                           Vector3.Up);

            //// Get the corners of the frustum
            //Vector3[] frustumCorners = cameraComponent.BoundingFrustum.GetCorners();

            //// Transform the positions of the corners into the direction of the light
            //for (int i = 0; i < frustumCorners.Length; i++)
            //{
            //    frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            //}

            //// Find the smallest box around the points
            //BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            //Vector3 boxSize = lightBox.Max - lightBox.Min;
            //Vector3 halfBoxSize = boxSize * 0.5f;

            //// The position of the light should be in the center of the back
            //// pannel of the box. 
            //Vector3 lightPosition = lightBox.Min + halfBoxSize;
            //lightPosition.Z = lightBox.Min.Z;

            //// We need the position back in world coordinates so we transform 
            //// the light position by the inverse of the lights rotation
            //lightPosition = Vector3.Transform(lightPosition,
            //                                  Matrix.Invert(lightRotation));
            #endregion

            var lightPosition = lightComponent.LightPosition;
            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition + lightComponent.LightDirection,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            Matrix lightProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45F), 1f, 1f, 10f);

            lightComponent.LightViewProjection =  lightView * lightProjection;
        }

        private void CreateShadowMap()
        {
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;

            worldRenderMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(rotationthingy));

            foreach(ModelComponent model in models)
            {
                if((string)model.model.Tag == "ground")
                {
                    worldRenderMatrix = Matrix.Identity;
                    DrawGround(model, "CreateShadowMap");
                }
                else
                {
                    TransformComponent tc = ComponentManager.Instance.GetComponentsById<TransformComponent>(model.EntityID);
                    worldRenderMatrix = Matrix.CreateWorld(tc.Position, Vector3.Forward, Vector3.Up);
                    DrawModel(model, "CreateShadowMap");
                }
            }
            graphicsDevice.SetRenderTarget(null);
        }



        private void DrawWithShadowMap()
        {
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;

            graphicsDevice.Clear(Color.CornflowerBlue);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            //worldRenderMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(rotationthingy));
            
            foreach (ModelComponent model in models)
            {
                TransformComponent tc = ComponentManager.Instance.GetComponentsById<TransformComponent>(model.EntityID);
                worldRenderMatrix = Matrix.CreateWorld(tc.Position, Vector3.Forward, Vector3.Up);
                DrawModel(model, "DrawWithShadowMap");
            }
        }

        private void DrawModel(ModelComponent modelComp, string techniqueName)
        {
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComp.EntityID) as TransformComponent;
            Matrix[] modelTransforms = new Matrix[modelComp.model.Bones.Count];
            modelComp.model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    #region draw model
                    //modelComp.Effect.CurrentTechnique = modelComp.Effect.Techniques[techniqueName];
                    //if (techniqueName.Contains("DrawWithShadowMap"))
                    //{
                    //    modelComp.Effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                    //}
                    //modelComp.Effect.Parameters["Texture"].SetValue(modelComp.Texture);
                    //modelComp.Effect.Parameters["World"].SetValue(worldRenderMatrix); // Matrix.CreateTranslation(transformComponent.Position)
                    //modelComp.Effect.Parameters["View"].SetValue(cameraComponent.View);
                    //modelComp.Effect.Parameters["Projection"].SetValue(cameraComponent.Projection);
                    //modelComp.Effect.Parameters["LightDirection"].SetValue(lightComponent.LightDirection);
                    //modelComp.Effect.Parameters["LightViewProj"].SetValue(lightComponent.LightViewProjection);
                    //modelComp.Effect.Parameters["AmbientColor"].SetValue(lightComponent.AmbientColor);
                    //modelComp.Effect.Parameters["AmbientIntensity"].SetValue(lightComponent.AmbientIntensity);
                    //modelComp.Effect.Parameters["DiffuseLightDirection"].SetValue(lightComponent.DiffuseLightDirection);
                    //modelComp.Effect.Parameters["DiffuseColor"].SetValue(lightComponent.DiffuseColor);
                    //modelComp.Effect.Parameters["DiffuseIntensity"].SetValue(lightComponent.DiffuseIntensity);
                    //modelComp.Effect.Parameters["CameraPosition"].SetValue(cameraComponent.Position);

                    //modelComp.Effect.Parameters["ShadowStrenght"].SetValue(0.9f);
                    //modelComp.Effect.Parameters["DepthBias"].SetValue(0.1f);
                    //modelComp.Effect.Parameters["ViewVector"].SetValue(Vector3.One);
                    //modelComp.Effect.Parameters["Shininess"].SetValue(0.9f);
                    //modelComp.Effect.Parameters["SpecularColor"].SetValue(Color.CornflowerBlue.ToVector4());
                    //modelComp.Effect.Parameters["SpecularIntensity"].SetValue(0.1f);

                    //modelComp.Effect.Parameters["FogStart"].SetValue(150f);
                    //modelComp.Effect.Parameters["FogEnd"].SetValue(350f);
                    //modelComp.Effect.Parameters["FogColor"].SetValue(Color.HotPink.ToVector4());
                    //modelComp.Effect.Parameters["FogEnabled"].SetValue(true);

                    //foreach (var pass in modelComp.Effect.CurrentTechnique.Passes)
                    //{
                    //    pass.Apply();
                    //}
                    #endregion

                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * modelComp.objectWorld;
                    var currentEffect = modelComp.Effect;
                    currentEffect.CurrentTechnique = currentEffect.Techniques[techniqueName];
                    currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * cameraComponent.View * cameraComponent.Projection);
                    currentEffect.Parameters["xTex"].SetValue(modelComp.Texture);
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xLightPosition"].SetValue(lightComponent.LightPosition);
                    currentEffect.Parameters["xLightPower"].SetValue(lightComponent.LightIntensity);
                    currentEffect.Parameters["xAmbientLight"].SetValue(lightComponent.AmbientIntensity);
                    currentEffect.Parameters["xLightWorldViewProjection"].SetValue(worldMatrix * lightComponent.LightViewProjection);
                    currentEffect.Parameters["xShadowMap"].SetValue(shadowRenderTarget);

                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
                
            }
        }
        private void DrawGround(ModelComponent modelComp, string techniqueName)
        {
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComp.EntityID) as TransformComponent;
            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {

                    modelComp.Effect.CurrentTechnique = modelComp.Effect.Techniques[techniqueName];
                    modelComp.Effect.Parameters["ShadowMap"].SetValue((RenderTarget2D)null);
                    modelComp.Effect.Parameters["Texture"].SetValue(modelComp.Texture);
                    modelComp.Effect.Parameters["World"].SetValue(worldRenderMatrix); // Matrix.CreateTranslation(transformComponent.Position)
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

                    modelComp.Effect.Parameters["FogStart"].SetValue(150f);
                    modelComp.Effect.Parameters["FogEnd"].SetValue(350f);
                    modelComp.Effect.Parameters["FogColor"].SetValue(Color.HotPink.ToVector4());
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
