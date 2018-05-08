using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Robot
{
    class Horn : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _position = new Vector3(0, 0.5f, 0);
        private Vector3 _jointPos = new Vector3(0, 1.5f, 0);

        public Horn(GraphicsDevice graphics, Vector3 size, Texture2D texture)
            : base(graphics, size, texture)
        {
            _rotation = new Vector3(_rotation.X, _rotation.Y + 0.5f, _rotation.Z);
        }

        public override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.D2))
            //    _rotation = new Vector3(_rotation.X, _rotation.Y + 0.01f, _rotation.Z);

            //if (Keyboard.GetState().IsKeyDown(Keys.D1))
            //    _rotation = new Vector3(_rotation.X, _rotation.Y - 0.01f, _rotation.Z);

            WorldMatrix = Matrix.Identity *
                Matrix.CreateTranslation(_position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_jointPos);

            foreach (IGameObject go in _children)
                go.Update(gameTime);
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = WorldMatrix * world;
            effect.CurrentTechnique.Passes[0].Apply();
            effect.Texture = Texture;
            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount);

            foreach (IGameObject go in _children)
                go.Draw(effect, WorldMatrix * world);
        }
    }
}
