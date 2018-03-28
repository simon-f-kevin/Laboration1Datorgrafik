using Game_Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class PositionComponent : EntityComponent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float lastX { get; set; }
        public float lastY { get; set; }
        

        public PositionComponent(int id)
        {
            EntityID = id;
        }

        public void setValue(float X, float Y)
        {
            lastX = this.X;
            lastY = this.Y;
            this.X = X;
            this.Y = Y;
        }
    }
}
