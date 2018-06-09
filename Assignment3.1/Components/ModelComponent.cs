using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class ModelComponent: EntityComponent
    {
        public Model Model { get; set; }
        public Texture2D Texture2D { get; set; }
        public Matrix ObjectWorld { get; set; }
        public ModelComponent(int entityID) : base(entityID)
        {

        }
    }
}
