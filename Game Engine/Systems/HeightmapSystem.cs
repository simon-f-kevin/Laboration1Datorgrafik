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
        private void SetUpVertices(HeightmapComponent hc)
        {
            hc.vertices = new VertexPositionColor[hc.terrainWidth * hc.terrainHeight];
            for (int x = 0; x < hc.terrainWidth; x++)
            {
                for (int y = 0; y < hc.terrainHeight; y++)
                {
                    hc.vertices[x + y * hc.terrainWidth].Position = new Vector3(x, hc.heightData[x, y], -y);
                    hc.vertices[x + y * hc.terrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices(HeightmapComponent hc)
        {
            hc.indices = new int[(hc.terrainWidth - 1) * (hc.terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < hc.terrainHeight - 1; y++)
            {
                for (int x = 0; x < hc.terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * hc.terrainWidth;
                    int lowerRight = (x + 1) + y * hc.terrainWidth;
                    int topLeft = x + (y + 1) * hc.terrainWidth;
                    int topRight = (x + 1) + (y + 1) * hc.terrainWidth;

                    hc.indices[counter++] = topLeft;
                    hc.indices[counter++] = lowerRight;
                    hc.indices[counter++] = lowerLeft;

                    hc.indices[counter++] = topLeft;
                    hc.indices[counter++] = topRight;
                    hc.indices[counter++] = lowerRight;
                }
            }
        }

        private void LoadHeightData(Texture2D heightMap, HeightmapComponent hc)
        {
            hc.terrainWidth = heightMap.Width;
            hc.terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[hc.terrainWidth * hc.terrainHeight];
            heightMap.GetData(heightMapColors);

            hc.heightData = new float[hc.terrainWidth, hc.terrainHeight];
            for (int x = 0; x < hc.terrainWidth; x++)
                for (int y = 0; y < hc.terrainHeight; y++)
                    hc.heightData[x, y] = heightMapColors[x + y * hc.terrainWidth].R / 5.0f;
        }

        public void CreateEverything(Texture2D img,HeightmapComponent hc)
        {
            LoadHeightData(img,hc);

            SetUpVertices(hc);
            SetUpIndices(hc);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var _hc = ComponentManager.Instance.getDictionary<HeightmapComponent>();

            foreach (HeightmapComponent hc in _hc.Values)
            {


                hc.device.Clear(Color.Black);

                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.None;
                rs.FillMode = FillMode.WireFrame;
                hc.device.RasterizerState = rs;


                Matrix worldMatrix = Matrix.CreateTranslation(-hc.terrainWidth / 2.0f, 0, hc.terrainHeight / 2.0f);
                hc.effect.View = hc.viewMatrix;
                hc.effect.Projection = hc.projectionMatrix;
                hc.effect.World = worldMatrix;

                foreach (EffectPass pass in hc.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    hc.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, hc.vertices, 0, hc.vertices.Length, hc.indices, 0, hc.indices.Length / 3, VertexPositionColor.VertexDeclaration);
                }
            }
        }
    }
}
