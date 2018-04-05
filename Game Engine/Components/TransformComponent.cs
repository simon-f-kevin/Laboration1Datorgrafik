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
        //Position
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        //Rotation
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        //Scaling
        public short ScalingX { get; set; }
        public short ScalingY { get; set; }
        public short ScalingZ { get; set; }

        public TransformComponent(int id)
        {
            EntityID = id;
        }
    }
}
