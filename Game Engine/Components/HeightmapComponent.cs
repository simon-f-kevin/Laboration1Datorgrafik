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
        public VertexPositionColor[] vertices { get; set; }

        public int[] indices { get; set; }

        public Texture2D heightMap { get; set; }

        public int terrainWidth { get; set; }
        public int terrainHeight { get; set; }
        public float[,] heightData { get; set; }


        public HeightmapComponent(int id, Texture2D heightMap, int width, int height) : base(id)
        {
            this.heightMap = heightMap;
            this.terrainWidth = width;
            this.terrainHeight = height;
        }
    }
}
