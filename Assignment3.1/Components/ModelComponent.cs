using Game_Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment3._1.Components
{
    
    public class ModelComponent: EntityComponent
    { 
        public Model Model { get; set; }
        public Texture2D Texture2D { get; set; }
        public Vector3 Position { get; set; }
        public float rotateModel { get; set; }
        public Matrix ObjectWorld { get; set; }
        public bool ShadowMapRendering { get; set; }
        public bool UpdateControls { get; set; }
        public float Scale { get; set; }
        

        public ModelComponent(int entityID) : base(entityID)
        {
        }
    }
}
