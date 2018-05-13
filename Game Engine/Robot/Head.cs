using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Robot
{
    class Head : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 Rotation = Vector3.Zero;
        private Vector3 _position = new Vector3(0, 2.5f, 0);
        private Vector3 _jointPos = new Vector3(0, 0.5f, 0);

        public Head(GraphicsDevice graphics, Vector3 size, Texture2D texture)
            : base(graphics, size, texture)
        {
            _children.Add(new Horn(graphics, new Vector3(0.5f, 2f, 0.5f), texture));
        }

       

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Rotation = new Vector3(Rotation.X + 0.2f, Rotation.Y, Rotation.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Rotation = new Vector3(Rotation.X - 0.2f, Rotation.Y, Rotation.Z);
            }

            WorldMatrix = Matrix.Identity *
                Matrix.CreateTranslation(_position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
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
