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
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public short[][] HeightData { get; set; }
        public HeightmapComponent(int id)
        {
            EntityID = id;
        }

        public GraphicsDevice device { get; set; }
        public BasicEffect effect { get; set; }
        public VertexPositionColor[] vertices { get; set; }
        public Matrix viewMatrix { get; set; }
        public Matrix projectionMatrix { get; set; }
        public int[] indices { get; set; }

        public Texture2D heightMap { get; set; }

        public int terrainWidth = 4;
        public int terrainHeight = 3;
        public float[,] heightData;

    }
}
