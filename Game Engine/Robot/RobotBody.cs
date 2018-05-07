using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Robot
{
    public class RobotBody : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 Rotation = Vector3.Zero;
        public Vector3 Position = Vector3.Zero;
        private float MovementSpeed = 0.5f;
        private float[,] HeightMap;
        private Texture2D Texture;

        private float Left = 269;
        private float Right = 89;

        public RobotBody(GraphicsDevice graphics, Vector3 size, Texture2D texture)
            : base(graphics, size)
        {
            Texture = texture;
            _children.Add(new Head(graphics, new Vector3(1f,2f,1f)));
            _children.Add(new Limb(graphics, new Vector3(1, 2, 1), new Vector3(1.5f, 0, 0)));
            _children.Add(new Limb(graphics, new Vector3(1, 2, 1), new Vector3(-1.5f, 0, 0)));
            _children.Add(new Limb(graphics, new Vector3(1, 2, 1), new Vector3(0.7f, -2, 0), true));
            _children.Add(new Limb(graphics, new Vector3(1, 2, 1), new Vector3(-0.7f, -2, 0), true));
        }
        public void GetHeightMap(float[,] heightMap)
        {
            this.HeightMap = heightMap;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position = new Vector3(Position.X - 0.5f, Position.Y, Position.Z);
                Rotation = new Vector3(Left, Rotation.Y, Rotation.Z);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position = new Vector3(Position.X + 0.5f, Position.Y, Position.Z);
                Rotation = new Vector3(Right, Rotation.Y, Rotation.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Position = new Vector3(Position.X, Position.Y, Position.Z + 0.5f);
                Rotation = new Vector3(179, Rotation.Y, Rotation.Z);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position = new Vector3(Position.X, Position.Y, Position.Z - 0.5f);
                Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
            }



            var y = HeightMap[Math.Abs((int)Position.X), Math.Abs((int)Position.Z)];
            Position.Y = y;

            WorldMatrix = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
                Matrix.CreateTranslation(Position);

            foreach (IGameObject child in _children)
                child.Update(gameTime);

        }
        //public override void Update(GameTime gameTime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.Left))
        //    {
        //        _rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);
        //        _position.X -= movementSpeed;
        //    }

        //    if (Keyboard.GetState().IsKeyDown(Keys.Right))
        //    {
        //        _rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);
        //        _position.X += movementSpeed;
        //    }
        //    if (Keyboard.GetState().IsKeyDown(Keys.Up))
        //    {
        //        _rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);
        //        _position.Z += movementSpeed;
        //    }

        //    if (Keyboard.GetState().IsKeyDown(Keys.Down))
        //    {
        //        _rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);
        //        _position.Z -= movementSpeed;
        //    }

        //    SetYPosition();


        //    World = Matrix.Identity *
        //        Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
        //        Matrix.CreateTranslation(_position);

        //    foreach (IGameObject go in _children)
        //        go.Update(gameTime);
        //}


        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = WorldMatrix * world;
            effect.Texture = Texture;
            effect.TextureEnabled = false;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount);

            foreach (IGameObject go in _children)
                go.Draw(effect, WorldMatrix * world);
        }
    }
}
