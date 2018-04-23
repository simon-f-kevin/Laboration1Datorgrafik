using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Robot
{
    public interface IGameObject
    {
        void Draw(BasicEffect effect, Matrix world);
        void Update(GameTime gameTime);
    }
}
