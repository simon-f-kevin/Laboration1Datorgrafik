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



        public CameraComponent(int id, GraphicsDevice graphicsDevice) : base(id)
        {
            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, graphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
        }
        public CameraComponent(int id, GraphicsDevice graphicsDevice, Matrix View, Matrix Projection) : base(id)
        {
            world = Matrix.Identity;
            this.view = View;
            this.projection = Projection;
        }
    }
}
