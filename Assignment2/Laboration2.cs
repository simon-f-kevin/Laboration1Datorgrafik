using Assignment2.Models;
using Assignment2.Systems;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelDemo;
using System;
using System.Collections.Generic;

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
        Texture2D houseTexture;
        Texture2D treeTexture;
        Model playerModel;
        Model treeModel;
        Model houseModel;

        WorldTerrain worldTerrain;
        CameraSystem cameraSystem;
        WorldDrawSystem worldDrawSystem;
        WorldObjectsDrawSystem worldObjectsDrawSystem;

        RobotArm _arm;
        BasicEffect _effect;

        List<Vector3> modelPositions;

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
            cameraSystem = new CameraSystem(this.GraphicsDevice);

            SystemManager.Instance.addToUpdateableQueue(cameraSystem);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            _arm = new RobotArm(GraphicsDevice);
            _effect = new BasicEffect(GraphicsDevice);
            _effect.VertexColorEnabled = true;

            _effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 16 / 9f, 0.01f, 1000f);
            _effect.View = Matrix.CreateLookAt(new Vector3(10f, 10f, 10f), new Vector3(0, 0, 0), Vector3.Up);


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapTexture = Content.Load<Texture2D>("US_Canyon");
            mapTextureImage = Content.Load<Texture2D>("grass");
            houseModel = Content.Load<Model>("farmhouse_obj");
            houseTexture = Content.Load<Texture2D>("farmhouse-texture");
            treeModel = Content.Load<Model>("lowpolytree");
            treeTexture = Content.Load<Texture2D>("Tree");

            worldTerrain = new WorldTerrain(GraphicsDevice, mapTexture,
                new Texture2D[4] { mapTextureImage, mapTextureImage, mapTextureImage, mapTextureImage }, new Vector3(0,0,0));

            List<House> houses = CreateHouses(houseModel, 100);
            List<Tree> trees = CreateTrees(houseModel, 1);

            //systems
            worldDrawSystem = new WorldDrawSystem(worldTerrain, GraphicsDevice);
            worldObjectsDrawSystem = new WorldObjectsDrawSystem();
            worldObjectsDrawSystem.Houses = houses;
            //worldObjectsDrawSystem.Trees = trees;

            SystemManager.Instance.addToDrawableQueue(worldDrawSystem, worldObjectsDrawSystem);

            //Create engine components
            int id = 1;
            var view = Matrix.CreateLookAt(new Vector3(100, 0, 10), new Vector3(0, 0, 0), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            CameraComponent cameraComponent = new CameraComponent(id,view,  projection, false);
            ComponentManager.Instance.AddComponent(cameraComponent);
        }

        private List<Tree> CreateTrees(Model treeModel, int nModels)
        {
            List<Tree> trees = new List<Tree>();
            var heightmapData = worldTerrain.GetHeightmapData();
            modelPositions = GeneratePositions(heightmapData, nModels);
            for (int i = 0; i < nModels; i++)
            {
                var house = new Tree(treeModel, modelPositions[i], treeTexture);
                trees.Add(house);
            }
            return trees;
        }

        private List<House> CreateHouses(Model houseModel, int nModels)
        {
            List<House> houses = new List<House>();
            var heightmapData = worldTerrain.GetHeightmapData();
            modelPositions = GeneratePositions(heightmapData, nModels);
            for(int i = 0; i < nModels; i++)
            {
                var house = new House(houseModel, modelPositions[i], houseTexture);
                houses.Add(house);
            }
            return houses;
        }

        private List<Vector3> GeneratePositions(float[,] heightmapData, int nPositions)
        {
            List<Vector3> positions = new List<Vector3>();

            Random rnd = new Random();
            int x = 0;
            int z = 0;
            for (int i = 0; i < nPositions; i++)
            {
                x += 10;
                z += 10;
                positions.Add(new Vector3(x, heightmapData[Math.Abs(x), Math.Abs(z)], z-150));
                Console.WriteLine(x.ToString() + " " + z.ToString());
            }

             return positions;
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
            _arm.Update(gameTime);

            SystemManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            SystemManager.Instance.Draw();
            _arm.Draw(_effect, Matrix.Identity);

            base.Draw(gameTime);
        }
    }
}
