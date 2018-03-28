using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Managers
{
    public class GameContentProvider
    {
        private static GameContentProvider _instance;
        private Dictionary<string, Texture2D> _TextureDict;

        private GameContentProvider()
        {
            _TextureDict = new Dictionary<string, Texture2D>();
        }
        public static GameContentProvider getInstance()
        {
            if (_instance == null)
            {
                _instance = new GameContentProvider();
            }
            return _instance;
        }
        public void add2DTexture(String textureName, Texture2D texture)
        {
            _TextureDict.Add(textureName, texture);
        }
        public Texture2D get2DTexture(String textureName)
        {
            Texture2D texture;
            if (_TextureDict.TryGetValue(textureName, out texture))
            {
                return texture;
            }
            else return null;
        }
    }
}
