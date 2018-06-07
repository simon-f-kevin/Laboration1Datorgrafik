using Game_Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Innehåller två köer med system. En för renderbara och en för uppdateringsbara system
 * 
 * innehåler metoder för att lägga till system av båda typerna
 * innehåller metoder för att ta bort system av båda typerna
 * 
 * innehåller en draw och en update metod
 */

namespace Game_Engine.Managers
{
    public class SystemManager
    {
        private static SystemManager _instance;
        private Queue<IUpdateableSystem> UpdateableSystems { get; set; }
        private Queue<IDrawableSystem> DrawableSystems { get; set; }

        private SystemManager()
        {
            UpdateableSystems = new Queue<IUpdateableSystem>();
            DrawableSystems = new Queue<IDrawableSystem>();
        }

        public static SystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SystemManager();
                }
                return _instance;
            }
        }
      
        public void addToUpdateableQueue(params IUpdateableSystem[] list)
        {
            for(int i = 0; i < list.Length; i++)
            {
                UpdateableSystems.Enqueue(list[i]);
            }
        }
        public void addToDrawableQueue(params IDrawableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                DrawableSystems.Enqueue(list[i]);
            }
        }
        public void clearDrawableQueue()
        {
            DrawableSystems.Clear();
        }
        public void ClearUpdateableQueue()
        {
            UpdateableSystems.Clear();
        }
        public void RemoveFromUpdateableQueue(params IUpdateableSystem[] list)
        {
            for(int i = 0; i < list.Length; i++)
            {
                UpdateableSystems.ToList().Remove(list[i]);
            }
        }
        public void removeFromDrawableQueue(params IDrawableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                DrawableSystems.ToList().Remove(list[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            int size = UpdateableSystems.Count;
            for(int i = 0; i < size; i++)
            {
                IUpdateableSystem system = UpdateableSystems.Dequeue();
                system.Update(gameTime);
                UpdateableSystems.Enqueue(system);
            }
        }

        public void Draw()
        {
            int size = DrawableSystems.Count;
            for(int i = 0; i < size; i++)
            {
                IDrawableSystem system = DrawableSystems.Dequeue();
                system.Draw();
                DrawableSystems.Enqueue(system);
            }
        }
    }
}
