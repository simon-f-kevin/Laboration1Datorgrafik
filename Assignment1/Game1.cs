using System;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model chopperModel;
        Texture2D heightmapTexture;

        TransformSystem transformSystem;
        ModelSystem modelSystem;
        CameraSystem cameraSystem;
        HeightmapSystem heightmapSystem;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            transformSystem = new TransformSystem();
            cameraSystem = new CameraSystem(this.GraphicsDevice);
            modelSystem = new ModelSystem();
            heightmapSystem = new HeightmapSystem(GraphicsDevice);

            SystemManager.Instance.addToUpdateableQueue(transformSystem, cameraSystem);
            SystemManager.Instance.addToDrawableQueue(modelSystem, heightmapSystem);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            chopperModel = Content.Load<Model>("Chopper");
            heightmapTexture = Content.Load<Texture2D>("US_Canyon");

            CreateChopper(1);
            //CreateHeightMap(2);

            // TODO: use this.Content to load your game content here
        }

        public void CreateChopper(int entityId)
        {

            var view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            ModelComponent mc = new ModelComponent(entityId, chopperModel);
            TransformComponent tc = new TransformComponent(entityId, new Vector3(50, 50, 50), new Vector3(0, 0, -100));
            CameraComponent cc = new CameraComponent(entityId, view, projection);
            VelocityComponent vc = new VelocityComponent(entityId, new Vector3(0.01f, 0.01f, 0.01f), new Vector3(0.008f, 0.008f, 0.008f));
            

            ComponentManager.Instance.AddComponent(mc);
            ComponentManager.Instance.AddComponent(tc);
            ComponentManager.Instance.AddComponent(cc);
            ComponentManager.Instance.AddComponent(vc);
        }
        public void CreateHeightMap(int entityId)
        {
            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

            CameraComponent cc = new CameraComponent(entityId, viewMatrix, projectionMatrix);
            HeightmapComponent hm = new HeightmapComponent(entityId, heightmapTexture, 4, 3);
            LoadHeightData(hm);
            SetUpVertices(hm);
            SetUpIndices(hm);

            ComponentManager.Instance.AddComponent(hm);
            ComponentManager.Instance.AddComponent(cc);

        }
        #region Helper functions for the HeightmapComponent
        private void SetUpVertices(HeightmapComponent hm)
        {
            hm.vertices = new VertexPositionColor[hm.terrainWidth * hm.terrainHeight];
            for (int x = 0; x < hm.terrainWidth; x++)
            {
                for (int y = 0; y < hm.terrainHeight; y++)
                {
                    hm.vertices[x + y * hm.terrainWidth].Position = new Vector3(x, hm.heightData[x, y], -y);
                    hm.vertices[x + y * hm.terrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices(HeightmapComponent hm)
        {
            hm.indices = new int[(hm.terrainWidth - 1) * (hm.terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < hm.terrainHeight - 1; y++)
            {
                for (int x = 0; x < hm.terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * hm.terrainWidth;
                    int lowerRight = (x + 1) + y * hm.terrainWidth;
                    int topLeft = x + (y + 1) * hm.terrainWidth;
                    int topRight = (x + 1) + (y + 1) * hm.terrainWidth;

                    hm.indices[counter++] = topLeft;
                    hm.indices[counter++] = lowerRight;
                    hm.indices[counter++] = lowerLeft;

                    hm.indices[counter++] = topLeft;
                    hm.indices[counter++] = topRight;
                    hm.indices[counter++] = lowerRight;
                }
            }
        }

        private void LoadHeightData(HeightmapComponent hm)
        {
            hm.terrainWidth = hm.heightMap.Width;
            hm.terrainHeight = hm.heightMap.Height;

            Color[] heightMapColors = new Color[hm.terrainWidth * hm.terrainHeight];
            hm.heightMap.GetData(heightMapColors);

            hm.heightData = new float[hm.terrainWidth, hm.terrainHeight];
            for (int x = 0; x < hm.terrainWidth; x++)
                for (int y = 0; y < hm.terrainHeight; y++)
                    hm.heightData[x, y] = heightMapColors[x + y * hm.terrainWidth].R / 5.0f;
        }
        #endregion


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            /*
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;
*/          SystemManager.Instance.Update(gameTime);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //DepthStencilState dss = new DepthStencilState();
            //dss.DepthBufferEnable = true;
            //GraphicsDevice.DepthStencilState = dss;

            // TODO: Add your drawing code here
            SystemManager.Instance.Draw();
            //spriteBatch.End();
        }
    }
}
