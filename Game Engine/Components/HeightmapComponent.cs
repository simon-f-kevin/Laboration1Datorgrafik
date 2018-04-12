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

        public int[] Indices { get; set; }

        public Texture2D HeightMap { get; set; }

        public int TerrainWidth { get; set; }
        public int TerratinHeight { get; set; }
        public float[,] HeightData { get; set; }


        public HeightmapComponent(int id, Texture2D heightMap, int width, int height) : base(id)
        {
            this.HeightMap = heightMap;
            this.TerrainWidth = width;
            this.TerratinHeight = height;
        }
    }
}
