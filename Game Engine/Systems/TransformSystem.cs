using System;
using System.Collections.Generic;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Systems
{
    public class TransformSystem : IUpdateableSystem
    {
        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var transformComponents = ComponentManager.Instance.getDictionary<TransformComponent>();
            foreach(TransformComponent transformComponent in transformComponents.Values)
            {
                VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentsById<VelocityComponent>(transformComponent.EntityID);
                float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // -----
                // Scale
                // -----

                // Enlarge (uniformly)
                if (Keyboard.GetState().IsKeyDown(Keys.Add))
                    transformComponent.scale *= 1.1f;

                // Shrink (uniformly)
                if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                    transformComponent.scale *= 0.9f;

                // --------
                // Tanslate
                // --------

                // Left (Negative X)
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    transformComponent.position.X -= velocityComponent.speed.X * elapsedGameTime;

                // Right (Positive X)
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    transformComponent.position.X += velocityComponent.speed.X * elapsedGameTime;

                // Backward (Positive Z)
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    transformComponent.position.Z += velocityComponent.speed.Z * elapsedGameTime;

                // Forward (Negative Z)
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    transformComponent.position.Z -= velocityComponent.speed.Z * elapsedGameTime;

                // Up (Positive Y)
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    transformComponent.position.Y += velocityComponent.speed.Y * elapsedGameTime;

                // Down (Negative Y)
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    transformComponent.position.Y -= velocityComponent.speed.Y * elapsedGameTime;

                // ------
                // Rotate
                // ------

                Vector3 axis = new Vector3(0, 0, 0);
                float angle = -elapsedGameTime * 0.01f;

                // Clockwise around positive Y-axis
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    axis = new Vector3(0, 1f, 0);

                // Clockwise around negative Y-axis
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    axis = new Vector3(0, -1f, 0);

                // Clockwise around positive X-axis
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    axis = new Vector3(1f, 0, 0);

                // Clockwise around negative X-axis
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    axis = new Vector3(-1f, 0, 0);

                // Clockwise around positive Z-axis
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                    axis = new Vector3(0, 0, 1f);

                // Clockwise around negative Z-axis
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    axis = new Vector3(0, 0, -1f);

                Quaternion rot = Quaternion.CreateFromAxisAngle(axis, angle);
                rot.Normalize();
                transformComponent.rotation *= Matrix.CreateFromQuaternion(rot);

                // Reset to original (zero) rotation
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    transformComponent.rotation = Matrix.Identity;
            }
        }
    }
}
