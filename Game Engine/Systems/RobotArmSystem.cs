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
    public class RobotArmSystem : IUpdateableSystem, IDrawableSystem
    {
        public void Update(GameTime gameTime)
        {
            var arms = ComponentManager.Instance.getDictionary<RobotArmComponent>();
            foreach(RobotArmComponent rArm in arms.Values)
            {
                var lArm = ComponentManager.Instance.GetComponentsById<LowerArmComponent>(rArm.EntityID);
                var CMC = ComponentManager.Instance.GetComponentsById<CuboidMeshComponent>(rArm.EntityID);

                if (lArm == null || CMC != null) continue;

                //Robot Arm
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    rArm._rotation = new Vector3(rArm._rotation.X + 0.01f, rArm._rotation.Y, rArm._rotation.Z);

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    rArm._rotation = new Vector3(rArm._rotation.X - 0.01f, rArm._rotation.Y, rArm._rotation.Z);

                CMC.World = Matrix.Identity *
                    Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rArm._rotation.X, rArm._rotation.Y, rArm._rotation.Z)) *
                    Matrix.CreateTranslation(rArm._position);

                //LowerArm
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    lArm._rotation = new Vector3(lArm._rotation.X, lArm._rotation.Y, lArm._rotation.Z + 0.01f);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    lArm._rotation = new Vector3(lArm._rotation.X, lArm._rotation.Y, lArm._rotation.Z - 0.01f);

                CMC.World = Matrix.Identity *
                    Matrix.CreateTranslation(lArm._position) *
                    Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(lArm._rotation.X, lArm._rotation.Y, lArm._rotation.Z)) *
                    Matrix.CreateTranslation(lArm._jointPos);
            }
        }

        public void Draw()
        {
            var arms = ComponentManager.Instance.getDictionary<RobotArmComponent>();

            //This should probably be changed
            var world = Matrix.Identity;

            foreach (RobotArmComponent rArm in arms.Values)
            {
                var lArm = ComponentManager.Instance.GetComponentsById<LowerArmComponent>(rArm.EntityID);
                var CMC = ComponentManager.Instance.GetComponentsById<CuboidMeshComponent>(rArm.EntityID);

                if (lArm == null || CMC != null) continue;

                //Robot Arm
                CMC.effect.World = CMC.World * world;
                CMC.effect.CurrentTechnique.Passes[0].Apply();

                CMC.GraphicsDevice.SetVertexBuffer(CMC.VertexBuffer);
                CMC.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

                world *= CMC.World;

                //Lower Arm
                CMC.effect.World = CMC.World * world;
                CMC.effect.CurrentTechnique.Passes[0].Apply();

                CMC.GraphicsDevice.SetVertexBuffer(CMC.VertexBuffer);
                CMC.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);
            }


        }
    }
}
