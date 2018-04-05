using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public enum ModelType
    {
        TopRotor,
        BackRotor,
        Body
    }
    public class ModelComponent : EntityComponent
    {

        public ModelType ModelType { get; set; }

        public Model Model { get; set; }

        public Matrix ObjectWorld { get; set; }

        public Matrix Rotation { get; set; }

        public Quaternion Quaternion { get; set; }

        public ModelBone MainRotorBone { get; set; }

        public Matrix MainRotorBaseTransform { get; set; }



        public ModelComponent(int id)
        {
            EntityID = id;
        }
    }
}
