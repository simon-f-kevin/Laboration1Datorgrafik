using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class HeighmapComponent : EntityComponent
    {
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public short[][] HeightData { get; set; }
        public HeighmapComponent(int id)
        {
            EntityID = id;
        }
    }
}
