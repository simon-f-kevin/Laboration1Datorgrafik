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
    public class TransformSystem : IUpdateableSystem
    {
        private Dictionary<int, EntityComponent> _transformations;

        public void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var kbState = Keyboard.GetState();
            _transformations = ComponentManager.Instance.getDictionary<TransformComponent>();
            
            foreach(TransformComponent tc in _transformations.Values)
            {
                VelocityComponent speed = ComponentManager.Instance.GetComponentsById<VelocityComponent>(tc.EntityID);
                ModelComponent model = ComponentManager.Instance.GetComponentsById<ModelComponent>(tc.EntityID);
                //Position
                if (kbState.IsKeyDown(Keys.Up))
                {
                    tc.PosZ -= speed.VelZ * elapsedGameTime;
                }
                if (kbState.IsKeyDown(Keys.Down))
                {
                    tc.PosZ += speed.VelZ * elapsedGameTime;
                }
                if (kbState.IsKeyDown(Keys.Right))
                {
                    tc.PosX += speed.VelX * elapsedGameTime; 
                }
                if (kbState.IsKeyDown(Keys.Left))
                {
                    tc.PosX -= speed.VelX * elapsedGameTime;
                }
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    tc.PosY -= speed.VelY * elapsedGameTime;
                }
                if (kbState.IsKeyDown(Keys.LeftControl))
                {
                    tc.PosY += speed.VelY * elapsedGameTime;
                }
                if (kbState.IsKeyDown(Keys.A))
                {
                    //tc.RotationX += (ushort)(2 * elapsedGameTime);
                    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1f, 0), (-elapsedGameTime * 0.01f));
                    rot.Normalize();
                    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                    model.Quaternion = rot;
                }
                if (kbState.IsKeyDown(Keys.D))
                {
                    //tc.RotationX += (ushort)(2 * elapsedGameTime);
                    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1f, 0), (-elapsedGameTime * 0.01f));
                    rot.Normalize();
                    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                    model.Quaternion = rot;
                }
                //rotation

                Quaternion rotorRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1f, 0), (-elapsedGameTime * 0.01f));
                rotorRot.Normalize();
                //model.Rotation *= Matrix.CreateFromQuaternion(rotorRot);

                model.Model.Bones["Main_Rotor"].Transform = Matrix.CreateFromQuaternion(rotorRot) * Matrix.CreateTranslation(model.Model.Bones["Main_Rotor"].Transform.Translation);
                model.Model.Bones["Back_Rotor"].Transform = Matrix.CreateRotationZ((float)Math.PI / 2f) * Matrix.CreateRotationX(tc.RotationY) * Matrix.CreateTranslation(model.Model.Bones["Back_Rotor"].Transform.Translation);
                //
                //model.Model.Bones["Body"].Transform = Matrix.CreateRotationY(0);// * Matrix.CreateTranslation(model.Model.Bones["Body"].Transform.Translation);
            }
        }
    }
}
