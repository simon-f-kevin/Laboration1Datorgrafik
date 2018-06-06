﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{

    public class ModelComponent2 : EntityComponent
    {
        public Texture2D Texture { get; set; }
        public Effect Effect { get; set; }
        public Matrix objectWorld { get; set; }
        public Model model { get; set; }
        public ModelComponent2(int id, Model model) : base(id)
        {
            objectWorld = Matrix.Identity;
            this.model = model;
        }

        public ModelComponent2(int id, Model model, Texture2D texture) : base(id)
        {
            objectWorld = Matrix.Identity;
            this.model = model;
            this.Texture = texture;
        }
    }
}
