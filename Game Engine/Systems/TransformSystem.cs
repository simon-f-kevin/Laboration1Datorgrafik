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
            var transformComponents = ComponentManager.Instance.getDictionary<TransformComponent>();
            foreach(TransformComponent transformComponent in transformComponents.Values)
            {
                VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentsById<VelocityComponent>(transformComponent.EntityID);
                ModelComponent modelComponent = ComponentManager.Instance.GetComponentsById<ModelComponent>(transformComponent.EntityID);
                float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                var speedConstant = velocityComponent.MovementSpeed.X;
                // ------
                // Movement
                // ------

                // Move right
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(velocityComponent.MovementSpeed.X, 0, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.MovementSpeed.X, 0, 0);

                // Move left
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(-velocityComponent.MovementSpeed.X, 0, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(-velocityComponent.MovementSpeed.X, 0, 0);

                // Move forward
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(0, 0, -velocityComponent.MovementSpeed.Z) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(0, 0, -velocityComponent.MovementSpeed.Z);

                // Move backward
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(0, 0, velocityComponent.MovementSpeed.Z) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(0, 0, velocityComponent.MovementSpeed.Z);

                //Move up
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(0, velocityComponent.MovementSpeed.Y, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(0, velocityComponent.MovementSpeed.Y, 0);
                
                //Move down
                if(Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(0, -velocityComponent.MovementSpeed.Y, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(0, -velocityComponent.MovementSpeed.Y, 0);

                // --------
                // Rotation
                // --------

                Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(), (-elapsedGameTime * 0.01f));
                // Left (Positive Y)
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(-modelComponent.model.Bones[0].Transform.Translation) * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new Vector3(0, velocityComponent.RotationSpeed.Y ,0), (-elapsedGameTime * 0.9f))) * Matrix.CreateTranslation(modelComponent.model.Bones[0].Transform.Translation);
                    

                // Right (Negative Y)
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(-modelComponent.model.Bones[0].Transform.Translation) * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new Vector3(0, -velocityComponent.RotationSpeed.Y, 0), (-elapsedGameTime * 0.9f))) * Matrix.CreateTranslation(modelComponent.model.Bones[0].Transform.Translation);

                // Backward ()
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(-modelComponent.model.Bones[0].Transform.Translation) * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new Vector3(-velocityComponent.RotationSpeed.X, 0, 0), (-elapsedGameTime * 0.9f))) * Matrix.CreateTranslation(modelComponent.model.Bones[0].Transform.Translation);

                // Forward (Negative Z)
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    modelComponent.model.Bones[0].Transform *= Matrix.CreateTranslation(-modelComponent.model.Bones[0].Transform.Translation) * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new Vector3(velocityComponent.RotationSpeed.X, 0, 0), (-elapsedGameTime * 0.9f))) * Matrix.CreateTranslation(modelComponent.model.Bones[0].Transform.Translation);
                

                // Reset to original (zero) position and rotation
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    modelComponent.model.Bones[0].Transform = Matrix.Identity;

                //Rotate blades of chopper
                transformComponent.rotationVector.Y += 0.9f;
                transformComponent.rotationVector.X += 0.9f;

                modelComponent.model.Bones["Main_Rotor"].Transform = Matrix.CreateRotationY(transformComponent.rotationVector.Y) * Matrix.CreateTranslation(modelComponent.model.Bones["Main_Rotor"].Transform.Translation);
                modelComponent.model.Bones["Back_Rotor"].Transform = Matrix.CreateRotationZ((float)Math.PI / 2f) * Matrix.CreateRotationX(transformComponent.rotationVector.X) * Matrix.CreateTranslation(modelComponent.model.Bones["Back_Rotor"].Transform.Translation);
            }
        }
    }
}
