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
        Model model;

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
            heightmapSystem = new HeightmapSystem();

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
            model = Content.Load<Model>("Chopper");
            //Texture2D hm = Content.Load<Texture2D>("US_Canyon");

            CreateChopper();

            // TODO: use this.Content to load your game content here
        }

        public void CreateChopper()
        {
            int entityId = 1;
            ModelComponent mc = new ModelComponent(entityId, model);
            TransformComponent tc = new TransformComponent(entityId, new Vector3(50, 50, 50), new Vector3(0, 0, -500));
            CameraComponent cc = new CameraComponent(entityId, GraphicsDevice);
            VelocityComponent vc = new VelocityComponent(entityId, new Vector3(0.01f, 0.01f, 0.01f), new Vector3(0.008f, 0.008f, 0.008f));
            

            ComponentManager.Instance.AddComponent(mc);
            ComponentManager.Instance.AddComponent(tc);
            ComponentManager.Instance.AddComponent(cc);
            ComponentManager.Instance.AddComponent(vc);
        }

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
            SystemManager.Instance.Draw(spriteBatch);
            //spriteBatch.End();
        }
    }
}
