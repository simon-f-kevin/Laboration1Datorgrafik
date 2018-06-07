using Game_Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    
    public class ModelComponent: EntityComponent
    {
        public ModelComponent(int entityID) : base(entityID)
        {
        }

        public Model Model { get; set; }
        public Texture2D Texture2D { get; set; }
        public Matrix ObjectWorld { get; set; }
    }
}
