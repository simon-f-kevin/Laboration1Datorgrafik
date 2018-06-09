using System.Linq;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Assignment3._1
{
    public class CameraSystem : IUpdateableSystem
    {
        public CameraSystem()
        {

        }

        public void Update(GameTime gameTime)
        {
            var currentKeyboardState = Keyboard.GetState();
            var currentGamePadState = GamePad.GetState(0);
            var CameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check for input to rotate the camera.
            float pitch = -currentGamePadState.ThumbSticks.Right.Y * time * 0.001f;
            float turn = -currentGamePadState.ThumbSticks.Right.X * time * 0.001f;

            if (currentKeyboardState.IsKeyDown(Keys.Up))
                pitch += time * 0.001f;

            if (currentKeyboardState.IsKeyDown(Keys.Down))
                pitch -= time * 0.001f;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
                turn += time * 0.001f;

            if (currentKeyboardState.IsKeyDown(Keys.Right))
                turn -= time * 0.001f;

            Vector3 cameraRight = Vector3.Cross(Vector3.Up, CameraComp.CameraForward);
            Vector3 flatFront = Vector3.Cross(cameraRight, Vector3.Up);

            Matrix pitchMatrix = Matrix.CreateFromAxisAngle(cameraRight, pitch);
            Matrix turnMatrix = Matrix.CreateFromAxisAngle(Vector3.Up, turn);

            Vector3 tiltedFront = Vector3.TransformNormal(CameraComp.CameraForward, pitchMatrix *
                                                            turnMatrix);

            // Check angle so we cant flip over
            if (Vector3.Dot(tiltedFront, flatFront) > 0.001f)
            {
                CameraComp.CameraForward = Vector3.Normalize(tiltedFront);
            }

            // Check for input to move the camera around.
            if (currentKeyboardState.IsKeyDown(Keys.W))
                CameraComp.CameraPosition += CameraComp.CameraForward * time * 0.1f;

            if (currentKeyboardState.IsKeyDown(Keys.S))
                CameraComp.CameraPosition -= CameraComp.CameraForward * time * 0.1f;

            if (currentKeyboardState.IsKeyDown(Keys.A))
                CameraComp.CameraPosition += cameraRight * time * 0.1f;

            if (currentKeyboardState.IsKeyDown(Keys.D))
                CameraComp.CameraPosition -= cameraRight * time * 0.1f;

            CameraComp.CameraPosition += CameraComp.CameraForward *
                                currentGamePadState.ThumbSticks.Left.Y * time * 0.1f;

            CameraComp.CameraPosition -= cameraRight *
                                currentGamePadState.ThumbSticks.Left.X * time * 0.1f;

            if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.R))
            {
                CameraComp.CameraPosition = new Vector3(0, 50, 50);
                CameraComp.CameraForward = new Vector3(0, 0, -1);
            }

            CameraComp.CameraForward.Normalize();

            // Create the new view matrix
            CameraComp.View = Matrix.CreateLookAt(CameraComp.CameraPosition,
                                        CameraComp.CameraPosition + CameraComp.CameraForward,
                                        Vector3.Up);

            // Set the new frustum value
            CameraComp.CameraFrustum.Matrix = CameraComp.View * CameraComp.Projection;
        }
    }
}
