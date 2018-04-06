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
        public void Draw()
        {
            var ModelComponents = ComponentManager.Instance.getDictionary<ModelComponent>();
            foreach(ModelComponent modelComponent in ModelComponents.Values)
            {
                var transformComponent = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComponent.EntityID);
                var cameraComponent = ComponentManager.Instance.GetComponentsById<CameraComponent>(modelComponent.EntityID);
                foreach (ModelMesh modelMesh in modelComponent.model.Meshes)
                {
                    //System.Console.WriteLine(modelMesh.Name);
                    foreach (BasicEffect effect in modelMesh.Effects)
                    {
                        modelComponent.objectWorld = Matrix.CreateScale(transformComponent.scale) * transformComponent.rotation * Matrix.CreateTranslation(transformComponent.position);
                        effect.World = modelMesh.ParentBone.Transform * modelComponent.objectWorld * cameraComponent.world;
                        effect.View = cameraComponent.view;
                        effect.Projection = cameraComponent.projection;

                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;

                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            modelMesh.Draw();
                        }
                    }
                }
            }
        }
    }
}
