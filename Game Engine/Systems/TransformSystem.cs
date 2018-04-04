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
        private Dictionary<int, EntityComponent> _speeds;
        private Dictionary<int, EntityComponent> _models;

        public void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var kbState = Keyboard.GetState();
            _transformations = ComponentManager.Instance.getDictionary<TransformComponent>();
            _speeds = ComponentManager.Instance.getDictionary<VelocityComponent>();
            
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
                    tc.RotationX += (ushort)(2 * elapsedGameTime);
                    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1f, 0), (-elapsedGameTime * 0.01f));
                    rot.Normalize();
                    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                    model.Quaternion = rot;
                }
                if (kbState.IsKeyDown(Keys.D))
                {
                    tc.RotationX += (ushort)(2 * elapsedGameTime);
                    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1f, 0), (-elapsedGameTime * 0.01f));
                    rot.Normalize();
                    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                    model.Quaternion = rot;
                }
                //rotation
                //if(model.ModelType == ModelType.TopRotor)
                //{
                //    tc.RotationX += (ushort)(2 * elapsedGameTime);
                //    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(1f, 0, 0), (-elapsedGameTime * 0.01f));
                //    rot.Normalize();
                //    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                //}
                //if(model.ModelType == ModelType.BackRotor)
                //{
                //    tc.RotationY += (ushort)(2 * elapsedGameTime);
                //    Quaternion rot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1f, 0), (-elapsedGameTime * 0.01f));
                //    rot.Normalize();
                //    model.Rotation *= Matrix.CreateFromQuaternion(rot);
                //}
            }
        }
    }
}
