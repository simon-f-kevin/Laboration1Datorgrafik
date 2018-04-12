using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class CameraComponent : EntityComponent
    {
        public Matrix world { get; set; }
        public Matrix view { get; set; }
        public Matrix projection { get; set; }

        public bool Follow { get; set; }

        public CameraComponent(int id, Matrix View, Matrix Projection, bool Follow) : base(id)
        {
            world = Matrix.Identity;
            this.view = View;
            this.projection = Projection;
            this.Follow = Follow;
        }
    }
}
