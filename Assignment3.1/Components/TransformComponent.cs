using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3._1.Components
{
    public class TransformComponent : EntityComponent
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public TransformComponent(int entityID) : base(entityID)
        {

        }
    }
}
