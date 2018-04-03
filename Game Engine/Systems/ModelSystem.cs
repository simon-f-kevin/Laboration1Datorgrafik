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
        
        private Dictionary<int, EntityComponent> _models;
        

        public void Draw(SpriteBatch spriteBatch)
        {
            _models = ComponentManager.Instance.getDictionary<ModelComponent>();

            foreach(ModelComponent modelComp in _models.Values)
            {
                TransformComponent tc = ComponentManager.Instance.GetComponentsById<TransformComponent>(modelComp.EntityID);
                CameraComponent cc = ComponentManager.Instance.GetComponentsById<CameraComponent>(modelComp.EntityID);
                foreach(ModelMesh mesh in modelComp.Model.Meshes)
                {
                    foreach(BasicEffect effect in mesh.Effects)
                    {
                        modelComp.ObjectWorld = Matrix.CreateScale(new Vector3(tc.ScalingX, tc.ScalingY, tc.ScalingZ))
                            * modelComp.Rotation
                            * Matrix.CreateTranslation(new Vector3(tc.PosX, tc.PosY, tc.PosZ));
                        effect.World = mesh.ParentBone.Transform * modelComp.ObjectWorld * Matrix.Identity;
                        effect.View = cc.View;
                        effect.Projection = cc.Projection;

                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;

                        foreach(EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            mesh.Draw();
                        }
                    }
                }
            }
        }
    }
}
