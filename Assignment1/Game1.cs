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
            SystemManager.Instance.addToDrawableQueue(modelSystem);


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
            CreateChopper();
            // TODO: use this.Content to load your game content here
        }

        private void CreateChopper()
        {
            int id = 1;
            ModelComponent modelComponent = new ModelComponent(id);
            modelComponent.Model = model;
            modelComponent.ObjectWorld = Matrix.Identity;
            modelComponent.Rotation = Matrix.Identity;
            ComponentManager.Instance.addComponent(modelComponent);

            TransformComponent transformComponent = new TransformComponent(id);
            transformComponent.PosX = 0;
            transformComponent.PosY = 0;
            transformComponent.PosZ = 0;
            transformComponent.RotationX = 0;
            transformComponent.RotationY = 0;
            transformComponent.RotationZ = 0;
            transformComponent.ScalingX = 3;
            transformComponent.ScalingY = 3;
            transformComponent.ScalingZ = 3;
            ComponentManager.Instance.addComponent(transformComponent);

            VelocityComponent velocity = new VelocityComponent(id);
            velocity.VelX = 0.1f;
            velocity.VelY = 0.1f;
            velocity.VelZ = 0.1f;
            ComponentManager.Instance.addComponent(velocity);

            CameraComponent camera = new CameraComponent(id);
            camera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            camera.NearPlane = 0.1f;
            camera.FarPlane = 1000f;
            camera.FOV = 100;
            ComponentManager.Instance.addComponent(camera);



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

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;
            SystemManager.Instance.Update(gameTime);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var i = model.Meshes.Count;
            // TODO: Add your drawing code here
            SystemManager.Instance.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
