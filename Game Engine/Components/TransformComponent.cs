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
        public Vector3 scale {get; set;}
        public Vector3 position;
        public Matrix rotation {get; set;}
        public TransformComponent(int id, Vector3 scale, Vector3 position) : base(id)
        {
            this.scale = scale;
            this.position = position;
            rotation = Matrix.Identity;
        }
    }
}
