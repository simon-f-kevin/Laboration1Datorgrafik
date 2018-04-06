using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class VelocityComponent : EntityComponent
    {
        public Vector3 MovementSpeed { get; set; }

        public Vector3 RotationSpeed { get; set; }

        public VelocityComponent(int id, Vector3 move, Vector3 rot) : base(id)
        {
            MovementSpeed = move;
            RotationSpeed = rot;
        }
    }
}
