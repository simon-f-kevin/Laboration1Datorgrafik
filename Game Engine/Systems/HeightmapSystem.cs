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
            
        }
        public void Draw()
        {
            var heightmapComponents = ComponentManager.Instance.getDictionary<HeightmapComponent>();
            foreach(HeightmapComponent heightmapComponent in heightmapComponents.Values)
            {
                //CameraComponent cameraComponent = ComponentManager.Instance.GetComponentsById<CameraComponent>(heightmapComponent.EntityID);

                //Matrix worldMatrix = Matrix.CreateTranslation(-heightmapComponent.TerrainWidth / 2.0f, 0, heightmapComponent.TerratinHeight / 2.0f);
                //effect.View = cameraComponent.view;
                //effect.Projection = cameraComponent.projection;
                //effect.World = Matrix.CreateTranslation(new Vector3(-100, -35, 200));


                //foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                //{
                //    pass.Apply();
                //    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, heightmapComponent.Vertices, 0, heightmapComponent.Vertices.Length,
                //    heightmapComponent.Indices, 0, heightmapComponent.Indices.Length / 3, VertexPositionColor.VertexDeclaration);
                //}
                //GC.Collect();
                foreach(HeightmapMeshComponent mesh in heightmapComponent.HeightMapMeshes)
                {
                    CameraComponent _cameraComp = ComponentManager.Instance.GetComponentsById<CameraComponent>(heightmapComponent.EntityID);
                    if (_cameraComp.BoundingFrustum.Intersects(mesh.BoundingBoxComponent.BoundingBox))
                    {
                        device.SetVertexBuffer(mesh.VertexBuffer);
                        device.Indices = mesh.IndexBuffer;
                        effect.View = _cameraComp.View;
                        effect.World = mesh.ObjectWorld;
                        effect.Projection = _cameraComp.Projection;
                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = false;
                        effect.VertexColorEnabled = true;
                        effect.Texture = heightmapComponent.HeightMapTextureImage;
                        effect.TextureEnabled = false;

                        foreach (var pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mesh.Indices.Length / 3);

                        }
                    }
                }
                
            }
        }
    }
}
