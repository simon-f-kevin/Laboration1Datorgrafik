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

        public int terrainWidth = 4;
        public int terrainHeight = 3;
        public float[,] heightData;


        public HeightmapComponent(int id) : base(id)
        {

        }
    }
}
