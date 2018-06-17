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
            var keyboardState = Keyboard.GetState();
            var cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;

            float pitch = 0;
            float turn = 0;

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pitch -= cameraComponent.CameraTurnSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pitch += cameraComponent.CameraTurnSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                turn += cameraComponent.CameraTurnSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                turn -= cameraComponent.CameraTurnSpeed;
            }

            Vector3 cameraRight = Vector3.Cross(Vector3.Up, cameraComponent.CameraForward);
            Vector3 flatFront = Vector3.Cross(cameraRight, Vector3.Up);

            Matrix pitchMatrix = Matrix.CreateFromAxisAngle(cameraRight, pitch);
            Matrix turnMatrix = Matrix.CreateFromAxisAngle(Vector3.Up, turn);

            Vector3 tiltedFront = Vector3.TransformNormal(cameraComponent.CameraForward, pitchMatrix * turnMatrix);

            if (Vector3.Dot(tiltedFront, flatFront) > 0.001f)
            {
                cameraComponent.CameraForward = Vector3.Normalize(tiltedFront);
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                cameraComponent.CameraPosition += cameraComponent.CameraForward * cameraComponent.CameraMoveSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                cameraComponent.CameraPosition -= cameraComponent.CameraForward * cameraComponent.CameraMoveSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                cameraComponent.CameraPosition += cameraRight * cameraComponent.CameraMoveSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                cameraComponent.CameraPosition -= cameraRight * cameraComponent.CameraMoveSpeed;
            }

            cameraComponent.CameraForward.Normalize();
            cameraComponent.UpdateView();
            cameraComponent.UpdateFrustrum();
        }
    }
}
