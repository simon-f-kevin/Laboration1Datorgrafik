using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class CameraComponent2 : EntityComponent
    {
        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Vector3 Position;

        public bool Follow { get; set; }

        public BoundingFrustum BoundingFrustum { get; set; }

        public CameraComponent2(int id, Matrix View, Matrix Projection, bool Follow) : base(id)
        {
            World = Matrix.Identity;
            this.View = View;
            this.Projection = Projection;
            this.Follow = Follow;
            BoundingFrustum = new BoundingFrustum(Matrix.Identity);
        }
    }
}
