using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Robot
{
    public class RobotArm : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 _rotation = Vector3.Zero;
        public Vector3 _position;
        private float movementSpeed = 0.5f;
        private float[,] heightMap;

        public RobotArm(GraphicsDevice graphics, Vector3 pos)
            : base(graphics, 4, 2, 4)
        {
            _children.Add(new LowerArm(graphics));
            _position = pos;
        }
        public void GetHeightMap(float[,] heightMap)
        {
            this.heightMap = heightMap;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                //_rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);
                _position.X -= movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                //_rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);
                _position.X += movementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                //_rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);
                _position.Z += movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                //_rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);
                _position.Z -= movementSpeed;
            }

            SetYPosition();


            World = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_position);

            foreach (IGameObject go in _children)
                go.Update(gameTime);
        }

        private void SetYPosition()
        {
            //if (_position.X < 0) _position.X = 0;
            //if (_position.X > 1080) _position.X = 1080;
            //if (_position.Z < 0) _position.Z = 0;
            //if (_position.Z > 1080) _position.Z = 1080;
            var yPos = heightMap[MathHelper.Clamp((int)_position.X,0, 1080), MathHelper.Clamp((int)_position.Y,0, 1080)]; //heightMap[(int)_position.X, (int)_position.Z];
            _position.Y = yPos + 10;
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = World * world;
            effect.View = world;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

            foreach (IGameObject go in _children)
                go.Draw(effect, World * world);
        }
    }
}
