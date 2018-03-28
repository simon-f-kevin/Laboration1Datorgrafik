using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Game_Engine.Mediator;
using Game_Engine.Managers;
using Microsoft.Xna.Framework.Input;
using Game_Engine.Components;

namespace Game_Engine.Systems
{
    public class InputSystem : IUpdateableSystem
    {
        Game game;
        KeyboardState prevKBState;
        MouseState prevMouseState;
        int id;

        public InputSystem(int playerOneId)
        {
            this.game = GamePropertyManager.Instance.GetGame();
            id = playerOneId;
            prevKBState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            VelocityComponent velComp = ComponentManager.Instance.GetComponentsById<VelocityComponent>(id);
            if (kbState.IsKeyDown(Keys.Left)){
                velComp.Acceleration = 100;
            }
            if (kbState.IsKeyDown(Keys.Right))
            {
                velComp.Acceleration = -100;
            }
            if (kbState.IsKeyDown(Keys.Enter))
            {

            }
        }
    }
}
