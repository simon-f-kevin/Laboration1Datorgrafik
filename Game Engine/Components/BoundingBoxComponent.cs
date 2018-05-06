using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class BoundingBoxComponent : EntityComponent
    {
        public enum CollisionType
        {
            Default, Ground
        }

        public BoundingBox BoundingBox { get; set; }
        public CollisionType Type { get; set; }

        public BoundingBoxComponent(int entityID) : base(entityID)
        {
        }
    }
}
