﻿using Assignment2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Laboration2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D mapTexture;
        Texture2D mapTextureImage;
        Model playerModel;
        Model treeModel;
        Model houseModel;

        World world;
        WorldTerrain worldTerrain;

        public Laboration2()
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
            mapTexture = Content.Load<Texture2D>("US_Canyon");
            mapTextureImage = Content.Load<Texture2D>("grass");

            world = new World(this.GraphicsDevice, mapTexture, mapTextureImage);
            worldTerrain = new WorldTerrain(GraphicsDevice, mapTexture, new Texture2D[4] { mapTextureImage, mapTextureImage, mapTextureImage, mapTextureImage });
            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //world.BasicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up); 
            //world.BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f); 
            //world.BasicEffect.World = Matrix.CreateTranslation(new Vector3(0, -100, 256)); 

            //foreach (EffectPass pass in world.BasicEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    world.Draw();
            //}
            worldTerrain.BasicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            worldTerrain.BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f); 
            worldTerrain.Draw();

            base.Draw(gameTime);
        }
    }
}
