using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Player
    {
        public VertexBuffer VertexBuffer;
        public Matrix WorldMatrix;

        public Vector3 Scale;
        public Vector3 Position;
        public Vector3 Rotation;

        public GraphicsDevice graphicsDevice;
        public float MovementSpeed;
        public float[,] HeightMap;
        public List<Player> Children;

        public Player(Vector3 pos, Vector3 rot, Vector3 scale, GraphicsDevice device)
        {
            graphicsDevice = device;
            Init();
            Position = pos;
            Rotation = rot;
            Scale = scale;
            MovementSpeed = 0.5f;
            Children = new List<Player>();
        }

        #region init model
        private void Init()
        {
            VertexPositionColor[] nonIndexedCube = new VertexPositionColor[36];

            float dX = Scale.X / 2;
            float dY = Scale.Y / 2;
            float dZ = Scale.Z / 2;

            Vector3 topLeftFront = new Vector3(-dX, dY, dZ);
            Vector3 bottomLeftFront = new Vector3(-dX, -dY, dZ);
            Vector3 topRightFront = new Vector3(dX, dY, dZ);
            Vector3 bottomRightFront = new Vector3(dX, -dY, dZ);
            Vector3 topLeftBack = new Vector3(-dX, dY, -dZ);
            Vector3 topRightBack = new Vector3(dX, dY, -dZ);
            Vector3 bottomLeftBack = new Vector3(-dX, -dY, -dZ);
            Vector3 bottomRightBack = new Vector3(dX, -dY, -dZ);

            // Front face
            nonIndexedCube[0] =
                new VertexPositionColor(topLeftFront, Color.Red);
            nonIndexedCube[1] =
                new VertexPositionColor(bottomLeftFront, Color.Red);
            nonIndexedCube[2] =
                new VertexPositionColor(topRightFront, Color.Red);
            nonIndexedCube[3] =
                new VertexPositionColor(bottomLeftFront, Color.Red);
            nonIndexedCube[4] =
                new VertexPositionColor(bottomRightFront, Color.Red);
            nonIndexedCube[5] =
                new VertexPositionColor(topRightFront, Color.Red);

            // Back face 
            nonIndexedCube[6] =
                new VertexPositionColor(topLeftBack, Color.Orange);
            nonIndexedCube[7] =
                new VertexPositionColor(topRightBack, Color.Orange);
            nonIndexedCube[8] =
                new VertexPositionColor(bottomLeftBack, Color.Orange);
            nonIndexedCube[9] =
                new VertexPositionColor(bottomLeftBack, Color.Orange);
            nonIndexedCube[10] =
                new VertexPositionColor(topRightBack, Color.Orange);
            nonIndexedCube[11] =
                new VertexPositionColor(bottomRightBack, Color.Orange);

            // Top face
            nonIndexedCube[12] =
                new VertexPositionColor(topLeftFront, Color.Yellow);
            nonIndexedCube[13] =
                new VertexPositionColor(topRightBack, Color.Yellow);
            nonIndexedCube[14] =
                new VertexPositionColor(topLeftBack, Color.Yellow);
            nonIndexedCube[15] =
                new VertexPositionColor(topLeftFront, Color.Yellow);
            nonIndexedCube[16] =
                new VertexPositionColor(topRightFront, Color.Yellow);
            nonIndexedCube[17] =
                new VertexPositionColor(topRightBack, Color.Yellow);

            // Bottom face 
            nonIndexedCube[18] =
                new VertexPositionColor(bottomLeftFront, Color.Purple);
            nonIndexedCube[19] =
                new VertexPositionColor(bottomLeftBack, Color.Purple);
            nonIndexedCube[20] =
                new VertexPositionColor(bottomRightBack, Color.Purple);
            nonIndexedCube[21] =
                new VertexPositionColor(bottomLeftFront, Color.Purple);
            nonIndexedCube[22] =
                new VertexPositionColor(bottomRightBack, Color.Purple);
            nonIndexedCube[23] =
                new VertexPositionColor(bottomRightFront, Color.Purple);

            // Left face
            nonIndexedCube[24] =
                new VertexPositionColor(topLeftFront, Color.Blue);
            nonIndexedCube[25] =
                new VertexPositionColor(bottomLeftBack, Color.Blue);
            nonIndexedCube[26] =
                new VertexPositionColor(bottomLeftFront, Color.Blue);
            nonIndexedCube[27] =
                new VertexPositionColor(topLeftBack, Color.Blue);
            nonIndexedCube[28] =
                new VertexPositionColor(bottomLeftBack, Color.Blue);
            nonIndexedCube[29] =
                new VertexPositionColor(topLeftFront, Color.Blue);

            // Right face 
            nonIndexedCube[30] =
                new VertexPositionColor(topRightFront, Color.Green);
            nonIndexedCube[31] =
                new VertexPositionColor(bottomRightFront, Color.Green);
            nonIndexedCube[32] =
                new VertexPositionColor(bottomRightBack, Color.Green);
            nonIndexedCube[33] =
                new VertexPositionColor(topRightBack, Color.Green);
            nonIndexedCube[34] =
                new VertexPositionColor(topRightFront, Color.Green);
            nonIndexedCube[35] =
                new VertexPositionColor(bottomRightBack, Color.Green);
            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColor.VertexDeclaration, 36, BufferUsage.WriteOnly);
            VertexBuffer.SetData(nonIndexedCube);

        }
        #endregion
        public void AddChildren(Player childPlayer)
        {
            Children.Add(childPlayer);
        }

        public void SetHeightMap(float [,] heightMap)
        {
            this.HeightMap = heightMap;
        }

        public virtual void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X -= MovementSpeed;
                Rotation.X += Matrix.CreateRotationX(0.45f).Translation.X;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X += MovementSpeed;
                Rotation.X -= Matrix.CreateRotationX(0.45f).Translation.X;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Position.Z += MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position.Z -= MovementSpeed;
            }

            var yPos = HeightMap[MathHelper.Clamp((int)Position.X, 0, 1080), MathHelper.Clamp((int)Position.Y, 0, 1080)];
            Position.Y = yPos + 10;

            WorldMatrix = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
                Matrix.CreateTranslation(Position);

            foreach (Player child in Children)
                child.Update();

        }

        public virtual void Draw(BasicEffect effect, Matrix worldMatrix)
        {
            effect.World = worldMatrix * WorldMatrix;
            effect.View = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

            foreach (Player child in Children)
            {
                child.Draw(effect, worldMatrix * WorldMatrix);
            }
        }
    }
}
