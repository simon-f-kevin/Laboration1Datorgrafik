using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Managers
{
    public class GamePropertyManager
    {
        private static GamePropertyManager instance;
        private Game _game;
        public static GamePropertyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GamePropertyManager();
                }
                return instance;
            }
        }

        public Game GetGame()
        {
            return _game;
        }

        public void setGame(Game game)
        {
            this._game = game;
        }
        public GraphicsDevice getGraphics()
        {
            return _game.GraphicsDevice;
        }
    }
}