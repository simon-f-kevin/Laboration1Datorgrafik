using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{

    public class ModelComponent : EntityComponent
    {
        public Matrix objectWorld { get; set; }
        public Model model { get; set; }
        public ModelComponent(int id, Model model) : base(id)
        {
            objectWorld = Matrix.Identity;
            this.model = model;
        }
    }
}
