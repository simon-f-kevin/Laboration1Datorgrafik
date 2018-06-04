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
        Matrix[] boneTransformations;
        public void Draw()
        {
            var ModelComponents = ComponentManager.Instance.getDictionary<ModelComponent>();

            foreach (ModelComponent modelComponent in ModelComponents.Values)
            {
                boneTransformations = new Matrix[modelComponent.model.Bones.Count];
                modelComponent.model.CopyAbsoluteBoneTransformsTo(boneTransformations);
                var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComponent.EntityID);
                var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
                foreach (ModelMesh modelMesh in modelComponent.model.Meshes)
                {
                    foreach (BasicEffect effect in modelMesh.Effects)
                    {
                        effect.World = Matrix.CreateTranslation(transformComponent.Position); //boneTransformations[modelMesh.ParentBone.Index]; //
                        effect.View = cameraComponent.View;
                        effect.Projection = cameraComponent.Projection;

                        effect.FogEnabled = true;
                        effect.FogColor = Color.CornflowerBlue.ToVector3();
                        effect.FogStart = 50;
                        effect.FogEnd = 160;

                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;
                        effect.Texture = modelComponent.Texture;
                        effect.TextureEnabled = true;
                        modelMesh.Draw();
                    }
                }
            }
        }
    }
}
