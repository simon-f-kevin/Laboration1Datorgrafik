using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class HeightmapComponent : EntityComponent
    {
        public VertexPositionColor[] Vertices { get; set; }

        public GraphicsDevice graphicsDevice { get; set; }

        public int[] Indices { get; set; }

        public Texture2D HeightMapTextureImage { get; set; }
        public Texture2D HeightmapTexture { get; set; }
        public Color[] Colors { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public Vector2 MeshImageSize;
        public Vector2 MeshDimensions;

        public int TerrainWidth { get; set; }
        public int TerratinHeight { get; set; }
        public float[,] HeightData { get; set; }

        public List<HeightmapMeshComponent> HeightMapMeshes { get; set; }


        public HeightmapComponent(int id, Texture2D[] heightMap, int width, int height, GraphicsDevice graphicsDevice) : base(id)
        {
            this.HeightmapTexture = heightMap[0];
            this.HeightMapTextureImage = heightMap[1];
            this.TerrainWidth = width;
            this.TerratinHeight = height;
            this.graphicsDevice = graphicsDevice;
            HeightMapMeshes = new List<HeightmapMeshComponent>();
            MeshDimensions = new Vector2(10,10);
            MeshImageSize.X = (float)Math.Floor(HeightmapTexture.Width / MeshDimensions.X);
            MeshImageSize.Y = (float)Math.Floor(HeightmapTexture.Height / MeshDimensions.Y);
            for(int x = 0; x < MeshDimensions.X; x++)
            {
                for(int y = 0; y < MeshDimensions.Y; y++)
                {
                    HeightmapMeshComponent meshComp = new HeightmapMeshComponent(id, MeshImageSize, x, y, HeightmapTexture);
                    meshComp.LoadHeightData();

                    meshComp.ObjectWorld = Matrix.Identity;
                    meshComp.Position = new Vector3(x * MeshImageSize.X, 0, -y * MeshImageSize.Y);
                    meshComp.IndexBuffer = new IndexBuffer(graphicsDevice, typeof(int), meshComp.Indices.Length, BufferUsage.None);
                    meshComp.IndexBuffer.SetData(meshComp.Indices);
                    meshComp.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), meshComp.Vertices.Length, BufferUsage.None);
                    meshComp.VertexBuffer.SetData(meshComp.Vertices);

                    HeightMapMeshes.Add(meshComp);
                }
            }
            SetupHeightData();
        }

        private void SetupHeightData()
        {
            Colors = new Color[HeightmapTexture.Width * HeightmapTexture.Height];
            HeightmapTexture.GetData(Colors);
            HeightData = new float[HeightmapTexture.Width, HeightmapTexture.Height];
            for(int i = 0; i < HeightmapTexture.Width; i++)
            {
                for(int j = 0; j < HeightmapTexture.Height; j++)
                {
                    HeightData[i, j] = Colors[i + j * HeightmapTexture.Width].R / 2;
                }
            }
        }
    }
}
