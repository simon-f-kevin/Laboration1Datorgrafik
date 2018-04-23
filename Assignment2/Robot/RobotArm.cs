using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ModelDemo
{
    class RobotArm : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _position = Vector3.Zero;

        public RobotArm(GraphicsDevice graphics)
            : base(graphics, 2, 1, 2)
        {
            _children.Add(new LowerArm(graphics));
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);
                _position.X -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);
                _position.X += 0.01f;
            }
                

            World = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_position);

            foreach (IGameObject go in _children)
                go.Update(gameTime);
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = World * world;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

            foreach (IGameObject go in _children)
                go.Draw(effect, World * world);
        }
    }
}
