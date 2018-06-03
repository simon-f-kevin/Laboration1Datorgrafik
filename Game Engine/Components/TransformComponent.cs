using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class TransformComponent : EntityComponent
    {
        public Vector3 Scale {get; set;}
        public Vector3 Position;
        public Vector3 RotationVector; 
        public Matrix Rotation {get; set;}
        public TransformComponent(int id, Vector3 scale, Vector3 position) : base(id)
        {
            this.Scale = scale;
            this.Position = position;
            Rotation = Matrix.Identity;
        }
    }
}
