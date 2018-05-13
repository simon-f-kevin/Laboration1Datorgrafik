using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Engine.Robot
{
    public abstract class CuboidMesh : IGameObject
    {
        public GraphicsDevice GraphicsDevice;
        public VertexBuffer VertexBuffer;
        public Texture2D Texture { get; set; }
        public Matrix WorldMatrix = Matrix.Identity;
        private float _sizeX = 1f;
        private float _sizeY = 1f;
        private float _sizeZ = 1f;

        public CuboidMesh(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;

            Initialize();
        }

        public CuboidMesh(GraphicsDevice graphics, Vector3 size, Texture2D texture)
        {
            GraphicsDevice = graphics;
            Texture = texture;
            _sizeX = size.X;
            _sizeY = size.Y;
            _sizeZ = size.Z;

            Initialize();
        }

        private void Initialize()
        {

            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();

            float dX = _sizeX / 2;
            float dY = _sizeY / 2;
            float dZ = _sizeZ / 2;

            Vector3 FRONT_TOP_LEFT = new Vector3(-dX, dY, dZ);
            Vector3 FRONT_TOP_RIGHT = new Vector3(dX, dY, dZ);
            Vector3 FRONT_BOTTOM_LEFT = new Vector3(-dX, -dY, dZ);
            Vector3 FRONT_BOTTOM_RIGHT = new Vector3(dX, -dY, dZ);
            Vector3 BACK_TOP_LEFT = new Vector3(-dX, dY, -dZ);
            Vector3 BACK_TOP_RIGHT = new Vector3(dX, dY, -dZ);
            Vector3 BACK_BOTTOM_LEFT = new Vector3(-dX, -dY, -dZ);
            Vector3 BACK_BOTTOM_RIGHT = new Vector3(dX, -dY, -dZ);

            // Front face Texture
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Up, new Vector2(0, 0)));

            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Up, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));
            //Bottom face
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Down, new Vector2(1, 0)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(0, 0)));

            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 0)));


            // Front face
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 0)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Forward, new Vector2(0, 0)));

            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 0)));

            // Right face
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 0)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Right, new Vector2(0, 0)));

            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Right, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 0)));


            // Left face
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 0)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Left, new Vector2(0, 0)));

            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Left, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 0)));

            // Back face
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(0, 0)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Backward, new Vector2(1, 0)));

            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Backward, new Vector2(1, 1)));
            verticesList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(0, 0)));

            VertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, 36, BufferUsage.WriteOnly);
            VertexBuffer.SetData(verticesList.ToArray());

        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(BasicEffect effect, Matrix world) { }

    }
}
