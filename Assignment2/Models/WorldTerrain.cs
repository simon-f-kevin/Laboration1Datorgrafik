using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public struct VertexTextures
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TetxureWeights;

        public static int Size = (3 + 3 + 4 + 4) * sizeof(float);

        public static VertexElement[] VertexElements = new[]
        {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float)*3, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float)*6, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                new VertexElement(sizeof(float)*10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2)
            };
    }

    public class WorldTerrain
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int minHeight;
        public int maxHeight;

        public BasicEffect BasicEffect;
        public Matrix HeightmapWorldMatrix;
        public float[,] heightmapData;

        private Texture2D HeightMapTexture;
        private Texture2D texture;
        private VertexPositionNormalTexture[] Vertices;

        //private VertexTextures[] vertices;
        private int[] Indices;
        private VertexDeclaration vertexDeclaration;
        private GraphicsDevice graphicsDevice;
        //private Vector3 worldPosition;

        public WorldTerrain(GraphicsDevice device, Texture2D heightMap, Texture2D texture, Vector3 worldPosition)
        {
            graphicsDevice = device;
            HeightMapTexture = heightMap;
            this.texture = texture;
            //this.worldPosition = worldPosition;
            Width = HeightMapTexture.Width;
            Height = HeightMapTexture.Height;
            SetHeights();
            SetVertices();
            SetIndices();
            InitNormal();
            BasicEffect = new BasicEffect(graphicsDevice);
        }

        public float[,] GetHeightmapData()
        {
            return heightmapData;
        }

        private void SetHeights()
        {
            Color[] greyValues = new Color[Width * Height];
            HeightMapTexture.GetData(greyValues);
            heightmapData = new float[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    heightmapData[x, y] = greyValues[x + y * Width].G / 3.1f;
                    minHeight = (int)Math.Min(heightmapData[x, y], minHeight);
                    maxHeight = (int)Math.Max(heightmapData[x, y], maxHeight);
                }
            }
        }
        #region old set vertices
        //private void SetVertices()
        //{
        //    vertices = new VertexTextures[Height * Width];
        //    float step = (maxHeight - minHeight) / 3;

        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            vertices[x + y * Width].Position = new Vector3(x, heightmapData[x, y], -y);
        //            vertices[x + y * Width].TextureCoordinate.X = x;
        //            vertices[x + y * Width].TextureCoordinate.Y = y;

        //            vertices[x + y * Width].TetxureWeights = Vector4.Zero;

        //            //normalize each weight between 0 and 1
        //            vertices[x + y * Width].TetxureWeights.X =
        //                MathHelper.Clamp(1.0f - Math.Abs(heightmapData[x, y]) / step, 0, 1);
        //            vertices[x + y * Width].TetxureWeights.Y =
        //                MathHelper.Clamp(1.0f - Math.Abs(heightmapData[x, y] - step) / step, 0, 1);
        //            vertices[x + y * Width].TetxureWeights.Z =
        //                MathHelper.Clamp(1.0f - Math.Abs(heightmapData[x, y] - 2 * step) / step, 0, 1);
        //            vertices[x + y * Width].TetxureWeights.W =
        //                MathHelper.Clamp(1.0f - Math.Abs(heightmapData[x, y] - 3 * step) / step, 0, 1);

        //            //add to toal
        //            float total = vertices[x + y * Width].TetxureWeights.X;
        //            total += vertices[x + y * Width].TetxureWeights.Y;
        //            total += vertices[x + y * Width].TetxureWeights.Z;
        //            total += vertices[x + y * Width].TetxureWeights.W;

        //            //divide by total
        //            vertices[x + y * Width].TetxureWeights.X /= total;
        //            vertices[x + y * Width].TetxureWeights.Y /= total;
        //            vertices[x + y * Width].TetxureWeights.Z /= total;
        //            vertices[x + y * Width].TetxureWeights.W /= total;
        //        }
        //    }
        //    vertexDeclaration = new VertexDeclaration(VertexTextures.VertexElements);
        //}
        #endregion

        private void SetVertices()
        {
            Vertices = new VertexPositionNormalTexture[Width * Height];
            float step = (maxHeight - minHeight) / 3;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vertices[x + y * Width].Position = new Vector3(x, heightmapData[x, y], -y);
                    Vertices[x + y * Width].TextureCoordinate.X = (float)x / Width;
                    Vertices[x + y * Width].TextureCoordinate.Y = (float)y / Height;

                }
            }
            
        }
        private void SetIndices()
        {
            // amount of triangles
            Indices = new int[(Width - 1) * (Height - 1) * 6];
            int number = 0;
            // collect data for corners
            for (int y = 0; y < Height - 1; y++)
                for (int x = 0; x < Width - 1; x++)
                {
                    // create double triangles
                    Indices[number++] = x + (y + 1) * Width;      // up left
                    Indices[number++] = ((x + 1) + y * Width);        // down right
                    Indices[number++] = (x + y * Width);            // down left

                    Indices[number++] = x + (y + 1) * Width;      // up left
                    Indices[number++] = ((x + 1) + (y + 1) * Width);  // up right
                    Indices[number++] = ((x + 1) + y * Width);        // down right
                }
        }

        //sets the vertices to the identity matrix
        private void InitNormal()
        {
            for(int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Vector3.Zero;
            }
            for (int i = 0; i < Indices.Length / 3; i++)
            {
                int index0 = Indices[i * 3];
                int index1 = Indices[i * 3 + 1];
                int index2 = Indices[i * 3 + 2];

                Vector3 side0 = Vertices[index0].Position - Vertices[index2].Position;
                Vector3 side1 = Vertices[index0].Position - Vertices[index1].Position;
                Vector3 normal = Vector3.Cross(side0, side1);

                Vertices[index0].Normal += normal;
                Vertices[index1].Normal += normal;
                Vertices[index2].Normal += normal;
            }

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i].Normal.Normalize();
        }

        public void Draw()
        {
            BasicEffect.World = HeightmapWorldMatrix;//Matrix.CreateTranslation(new Vector3(-0, -0, 1080));
            BasicEffect.Texture = texture;
            BasicEffect.TextureEnabled = true;

            //*/
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            //*/

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length, Indices, 0, Indices.Length / 3);
            }
        }
    }
}
