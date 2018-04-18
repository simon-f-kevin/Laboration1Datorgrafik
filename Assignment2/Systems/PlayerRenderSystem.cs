using Assignment2.Models;
using Game_Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Systems
{
    public class PlayerRenderSystem : IDrawableSystem
    {
        Player player;
        public PlayerRenderSystem(Player player)
        {
            this.player = player;
        }

        public void Draw()
        {
            
        }
    }
}
