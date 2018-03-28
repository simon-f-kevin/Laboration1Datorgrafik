using Game_Engine.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class TextureComponent : EntityComponent
    {
        public Texture2D sprite { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public TextureComponent(int id)
        {
            EntityID = id;
        }
        public void setValue(Texture2D sprite)
        {
            this.sprite = sprite;
            this.width = sprite.Width;
            this.height = sprite.Height;
        }
        public void setValue(Texture2D sprite, int height, int width)
        {
            this.sprite = sprite;
            this.height = height;
            this.width = width;
        }
    }
}
