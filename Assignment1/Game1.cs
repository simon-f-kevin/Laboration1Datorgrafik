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
            CreateHeightMap(1);

            // TODO: use this.Content to load your game content here
        }

        public void CreateChopper(int entityId)
        {

            var view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            var view2 = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            ModelComponent mc = new ModelComponent(entityId, chopperModel);
            TransformComponent tc = new TransformComponent(entityId, new Vector3(50, 50, 50), new Vector3(0, 0, -100));
            CameraComponent cc = new CameraComponent(entityId, view, projection, true);
            VelocityComponent vc = new VelocityComponent(entityId, new Vector3(0.51f, 0.51f, 0.51f), new Vector3(0.098f, 0.098f, 0.098f));
            
            ComponentManager.Instance.AddComponent(mc);
            ComponentManager.Instance.AddComponent(tc);
            ComponentManager.Instance.AddComponent(cc);
            ComponentManager.Instance.AddComponent(vc);
        }
        public void CreateHeightMap(int entityId)
        {
            //Original from tutorial
            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
            
            //Identical to chopper
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            //CameraComponent cc = new CameraComponent(entityId, view, projection);
            HeightmapComponent hm = new HeightmapComponent(entityId, heightmapTexture, 4, 3);
            LoadHeightData(hm);
            SetUpVertices(hm);
            SetUpIndices(hm);

            ComponentManager.Instance.AddComponent(hm);
            //ComponentManager.Instance.AddComponent(cc);

        }
        #region Helper functions for the HeightmapComponent
        private void SetUpVertices(HeightmapComponent hm)
        {
            hm.Vertices = new VertexPositionColor[hm.TerrainWidth * hm.TerratinHeight];
            for (int x = 0; x < hm.TerrainWidth; x++)
            {
                for (int y = 0; y < hm.TerratinHeight; y++)
                {
                    hm.Vertices[x + y * hm.TerrainWidth].Position = new Vector3(x, hm.HeightData[x, y], -y);
                    hm.Vertices[x + y * hm.TerrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices(HeightmapComponent hm)
        {
            hm.Indices = new int[(hm.TerrainWidth - 1) * (hm.TerratinHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < hm.TerratinHeight - 1; y++)
            {
                for (int x = 0; x < hm.TerrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * hm.TerrainWidth;
                    int lowerRight = (x + 1) + y * hm.TerrainWidth;
                    int topLeft = x + (y + 1) * hm.TerrainWidth;
                    int topRight = (x + 1) + (y + 1) * hm.TerrainWidth;

                    hm.Indices[counter++] = topLeft;
                    hm.Indices[counter++] = lowerRight;
                    hm.Indices[counter++] = lowerLeft;

                    hm.Indices[counter++] = topLeft;
                    hm.Indices[counter++] = topRight;
                    hm.Indices[counter++] = lowerRight;
                }
            }
        }

        private void LoadHeightData(HeightmapComponent hm)
        {
            hm.TerrainWidth = hm.HeightMap.Width;
            hm.TerratinHeight = hm.HeightMap.Height;

            Color[] heightMapColors = new Color[hm.TerrainWidth * hm.TerratinHeight];
            hm.HeightMap.GetData(heightMapColors);

            hm.HeightData = new float[hm.TerrainWidth, hm.TerratinHeight];
            for (int x = 0; x < hm.TerrainWidth; x++)
                for (int y = 0; y < hm.TerratinHeight; y++)
                    hm.HeightData[x, y] = heightMapColors[x + y * hm.TerrainWidth].R / 5.0f;
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
