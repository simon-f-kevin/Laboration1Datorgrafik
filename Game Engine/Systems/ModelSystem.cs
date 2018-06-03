using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Systems
{
    public class ModelSystem : IDrawableSystem
    {
        Matrix[] boneTransformations;
        public void Draw()
        {
            var ModelComponents = ComponentManager.Instance.getDictionary<ModelComponent>();
            
            foreach(ModelComponent modelComponent in ModelComponents.Values)
            {
                boneTransformations = new Matrix[modelComponent.model.Bones.Count];
                modelComponent.model.CopyAbsoluteBoneTransformsTo(boneTransformations);
                var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComponent.EntityID);
                var cameraComponent = ComponentManager.Instance.GetComponentsById<CameraComponent>(modelComponent.EntityID);
                foreach (ModelMesh modelMesh in modelComponent.model.Meshes)
                {
                    foreach (BasicEffect effect in modelMesh.Effects)
                    {
                        effect.World = boneTransformations[modelMesh.ParentBone.Index];
                        effect.View = cameraComponent.View;
                        effect.Projection = cameraComponent.Projection;

                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;

                        modelMesh.Draw();
                    }
                }
            }
        }
    }
}
