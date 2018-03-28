using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class RectangleComponent : EntityComponent
    {
        public Rectangle BoundingRectangle { get; set; }
        //public BoundingSphere BoundingSphere { get; set; }
        public RectangleComponent(int id)
        {
            EntityID = id;
        }
        public void setValue(Rectangle bounds)
        {
            BoundingRectangle = bounds;
            //BoundingSphere = new BoundingSphere(new Vector3(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2, 0), bounds.Width / 2);
        }
    }
}
