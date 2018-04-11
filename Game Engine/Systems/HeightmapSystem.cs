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
    public class HeightmapSystem : IDrawableSystem
    {
        GraphicsDevice device;
        BasicEffect effect;
        public HeightmapSystem(GraphicsDevice device)
        {
            this.device = device;
            effect = new BasicEffect(device);
            /* //This part is required to see the primitives
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.WireFrame;
            device.RasterizerState = rs;
            */
        }
        public void Draw()
        {
            var heightmapComponents = ComponentManager.Instance.getDictionary<HeightmapComponent>();
            foreach(HeightmapComponent heightmapComponent in heightmapComponents.Values)
            {
                CameraComponent cameraComponent = ComponentManager.Instance.GetComponentsById<CameraComponent>(heightmapComponent.EntityID);

                Matrix worldMatrix = Matrix.CreateTranslation(-heightmapComponent.terrainWidth / 2.0f, 0, heightmapComponent.terrainHeight / 2.0f);
                effect.View = cameraComponent.view;
                effect.Projection = cameraComponent.projection;
                effect.World = worldMatrix;

                
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, heightmapComponent.vertices, 0, heightmapComponent.vertices.Length,
                    heightmapComponent.indices, 0, heightmapComponent.indices.Length / 3, VertexPositionColor.VertexDeclaration);
                }
                GC.Collect();
            }
        }
    }
}
