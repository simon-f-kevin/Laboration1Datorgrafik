using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Assignment3._1.Systems
{
    class HandleInputSystem : IUpdateableSystem
    {
        public HandleInputSystem()
        {
        }

        public bool Enabled { get; set; }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var currentKeyboardState = Keyboard.GetState();
            var currentGamePadState = GamePad.GetState(PlayerIndex.One);
            var models = ComponentManager.Instance.getDictionary<ModelComponent>().Values;
            foreach (ModelComponent model in models)
            {
                //if (!model.UpdateControlls)
                //{
                //    continue;
                //}
                //// Rotate the dude model
                //model.ro += currentGamePadState.Triggers.Right * time * 0.2f;
                //model.rotateModel -= currentGamePadState.Triggers.Left * time * 0.2f;

                //if (currentKeyboardState.IsKeyDown(Keys.Q))
                //    model.rotateModel += time * 0.2f;
                //if (currentKeyboardState.IsKeyDown(Keys.E))
                //    model.rotateModel -= time * 0.2f;
                //var rotation = Quaternion.CreateFromAxisAngle(model.ObjectWorld.Right, 0) *
                //           Quaternion.CreateFromAxisAngle(model.ObjectWorld.Up, model.rotateModel);
                //model.ObjectWorld = EnvironmentService.Instance().World
                //        * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(model.Position);
                var transformComp = ComponentManager.Instance.GetComponentsById<TransformComponent>(model.EntityID);
                model.ObjectWorld = Matrix.Identity * Matrix.CreateScale(new Vector3(1f)) * Matrix.CreateTranslation(transformComp.Position);
                //model.ObjectWorld = Matrix.CreateRotationY(MathHelper.ToRadians(model.rotateModel)) * EnvironmentService.Instance().World;
            }
        }
    }
}
