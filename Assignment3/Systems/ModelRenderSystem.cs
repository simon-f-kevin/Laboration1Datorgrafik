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
    public class ModelRenderSystem : IDrawableSystem
    {
        public void Draw()
        {
            var ModelComponents = ComponentManager.Instance.getDictionary<ModelComponent>();

            foreach (ModelComponent modelComponent in ModelComponents.Values)
            {
                var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComponent.EntityID);
                var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;

                var viewVector = Vector3.Transform(transformComponent.Position - cameraComponent.Position, Matrix.CreateRotationY(0));
                viewVector.Normalize();

                //modelComponent.model.BoneTransformations[0] = model.World;

                foreach (var modelMesh in modelComponent.model.Meshes)
                {

                    foreach (ModelMeshPart part in modelMesh.MeshParts)
                    {
                        part.Effect = modelComponent.Effect;
                        part.Effect.Parameters["DiffuseLightDirection"].SetValue(transformComponent.Position + Vector3.Up);

                        part.Effect.Parameters["World"].SetValue(Matrix.CreateTranslation(transformComponent.Position));
                        part.Effect.Parameters["View"].SetValue(cameraComponent.View);
                        part.Effect.Parameters["Projection"].SetValue(cameraComponent.Projection);
                        part.Effect.Parameters["ViewVector"].SetValue(viewVector);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraComponent.Position);

                        var worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(modelMesh.ParentBone.Transform * Matrix.Identity));

                        part.Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

                        if (modelComponent.Texture != null)
                        {
                            part.Effect.Parameters["ModelTexture"].SetValue(modelComponent.Texture);

                        }
                    }
                    modelMesh.Draw();
                }
                #region old render code
                //foreach (ModelMesh modelMesh in modelComponent.model.Meshes)
                //{
                //    foreach (BasicEffect effect in modelMesh.Effects)
                //    {
                //        effect.World = Matrix.CreateTranslation(transformComponent.Position);
                //        effect.View = cameraComponent.View;
                //        effect.Projection = cameraComponent.Projection;

                //        effect.FogEnabled = true;
                //        effect.FogColor = Color.CornflowerBlue.ToVector3();
                //        effect.FogStart = 50;
                //        effect.FogEnd = 160;

                //        effect.DiffuseColor = Color.HotPink.ToVector3();
                //        effect.AmbientLightColor = Color.IndianRed.ToVector3();
                //        effect.SpecularColor = Color.CornflowerBlue.ToVector3();

                //        effect.EnableDefaultLighting();
                //        effect.LightingEnabled = true;
                //        effect.Texture = modelComponent.Texture;
                //        effect.TextureEnabled = true;
                //        modelMesh.Draw();
                //    }
                //}
                #endregion  
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
