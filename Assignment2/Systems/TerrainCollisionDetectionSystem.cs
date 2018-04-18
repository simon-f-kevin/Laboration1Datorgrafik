using Assignment2.Models;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Systems
{
    public class TerrainCollisionDetectionSystem : IUpdateableSystem
    {


        public void Update(GameTime gameTime)
        {
            //DetectGroundCollision(model);
        }

        private void DetectGroundCollision(AbstractModel model)
        {
            var position = model.model.Bones[0].Transform.Translation;

        }
    }
}
