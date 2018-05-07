using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Robot
{
    public class Limb : CuboidMesh
    {
        private Vector3 Rotation = Vector3.Zero;
        public Vector3 Position = Vector3.Zero;
        private Vector3 JointPos = new Vector3(0, 1.5f, 0);
        private bool isLeg = false;

        public Limb(GraphicsDevice graphics, Vector3 size, Vector3 position) : base(graphics, size)
        {
            JointPos = position;
        }
        public Limb(GraphicsDevice graphics, Vector3 size, Vector3 position, bool isLeg) : base(graphics, size)
        {
            JointPos = position;
            if (isLeg == true) this.isLeg = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isLeg && (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right)))
                Rotation = new Vector3(Rotation.X, Rotation.Y - 0.2f, Rotation.Z);

            WorldMatrix = Matrix.Identity *
                Matrix.CreateTranslation(Position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
                Matrix.CreateTranslation(JointPos);

        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = WorldMatrix * world;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

        }

    }
}
