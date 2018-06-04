using Game_Engine.Robot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    public class Ground : CuboidMesh
    {
        public Vector3 Position = Vector3.Zero;

        public Ground(GraphicsDevice graphics, Vector3 size, Texture2D texture) : base(graphics, size, texture)
        {
            
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = WorldMatrix * world;
            effect.Texture = Texture;
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = false;
            effect.EnableDefaultLighting();
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount);
        }
    }
}
