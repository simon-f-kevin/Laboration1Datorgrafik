using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class HeightmapMeshComponent : EntityComponent
    {
        private int X;
        private int Y;
        private float HighestPoint;
        private float LowestPoint;
        private Texture2D heightmapTexture;
        private float[,] HeightData;


        public Matrix ObjectWorld { get;  set; }
        public Vector3 Position { get;  set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexBuffer VertexBuffer { get;  set; }
        public int[] Indices { get;  set; }
        public VertexPositionColor[] Vertices { get;  set; }

        public Vector2 MeshSize;
        private Color[] HeightmapColors;

        public BoundingBoxComponent BoundingBoxComponent;

        public HeightmapMeshComponent(int entityID, Vector2 meshImageSize, int x, int y, Texture2D heightmapTexture) : base(entityID)
        {
            this.MeshSize = meshImageSize;
            this.X = x;
            this.Y = y;
            this.heightmapTexture = heightmapTexture;
        }

        public void LoadHeightData()
        {
            HeightmapColors = new Color[heightmapTexture.Width * heightmapTexture.Height];
            heightmapTexture.GetData(HeightmapColors);
            HighestPoint = 0;
            LowestPoint = 0;
            HeightData = new float[(int)MeshSize.X, (int)MeshSize.Y];
            for (int i = 0; i < (int)MeshSize.X; i++)
            {
                for (int j = 0; j < (int)MeshSize.Y; j++)
                {
                    HeightData[i, j] = HeightmapColors[((X * (int)MeshSize.X) + i) + ((Y * (int)MeshSize.Y) + j) * heightmapTexture.Width].R / 2;
                    if (HeightData[i, j] > HighestPoint)
                    {
                        HighestPoint = HeightData[i, j];
                    }
                    if (HeightData[i, j] < LowestPoint)
                    {
                        LowestPoint = HeightData[i, j];
                    }
                }
            }
            SetupBoundingBox();
            SetupIndices();
            SetupVertices();
        }

        private void SetupBoundingBox()
        {
            Vector3 min = new Vector3(X * MeshSize.X, -10, -((Y * MeshSize.Y) + MeshSize.Y));
            Vector3 max = new Vector3(X * MeshSize.X + MeshSize.X, HighestPoint, -Y * MeshSize.Y);
            BoundingBoxComponent = new BoundingBoxComponent(this.EntityID);
            BoundingBoxComponent.BoundingBox = new BoundingBox(min, max);
            BoundingBoxComponent.Type = BoundingBoxComponent.CollisionType.Ground;
        }

        private void SetupIndices()
        {
            var _indices = new int[((int)MeshSize.X - 1) * ((int)MeshSize.Y - 1) * 6];
            int counter = 0;
            for (int y = 0; y < MeshSize.Y - 1; y++)
            {
                for (int x = 0; x < MeshSize.X - 1; x++)
                {
                    int lowerLeft = (int)x + y * (int)MeshSize.X;
                    int lowerRight = (x + 1) + y * (int)MeshSize.X;
                    int topLeft = x + (y + 1) * (int)MeshSize.X;
                    int topRight = (x + 1) + (y + 1) * (int)MeshSize.X;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = lowerLeft;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = topRight;
                    _indices[counter++] = lowerRight;
                }
            }
            Indices = _indices;
        }

        private void SetupVertices()
        {
            var _vertices = new VertexPositionColor[(int)MeshSize.X * (int)MeshSize.Y];
            int x = 0;
            int y = 0;
            for (x = 0; x < MeshSize.X; x++)
            {
                for (y = 0; y < MeshSize.Y; y++)
                {
                    _vertices[x + y * (int)MeshSize.X].Position = new Vector3((X * (int)MeshSize.X) + x, HeightData[x, y], -(Y * (int)MeshSize.X) - y);
                    _vertices[x + y * (int)MeshSize.X].Color = HeightmapColors[((X * (int)MeshSize.X) + x) + ((Y * (int)MeshSize.Y) + y) * heightmapTexture.Width];
                }
            }
            Vertices = _vertices;
        }

    }
}
