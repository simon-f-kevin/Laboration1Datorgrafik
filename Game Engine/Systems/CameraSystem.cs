using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Systems
{
    public class CameraSystem : IUpdateableSystem
    {
        private GraphicsDevice graphicsDevice;

        public CameraSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullClockwiseFace;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
                rasterizerState.FillMode = graphicsDevice.RasterizerState.FillMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                rasterizerState.CullMode = graphicsDevice.RasterizerState.CullMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.Solid;
                rasterizerState.CullMode = graphicsDevice.RasterizerState.CullMode;
                graphicsDevice.RasterizerState = rasterizerState;
            }
        }
    }
}
