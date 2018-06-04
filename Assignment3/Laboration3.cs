using Assignment2.Models;
using Assignment3.Systems;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Assignment3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Laboration3 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        //Ground ground;
        //House house;

        //models
        private Model houseModel;
        private Model blobModel;
        private Model blockModel;
        private Model groundModel;
        private Texture2D houseTexture;

        //systems
        private ModelRenderSystem modelSystem;
        private CameraSystem cameraSystem;

        public Laboration3()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                GraphicsProfile = GraphicsProfile.HiDef
            };
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
            basicEffect = new BasicEffect(GraphicsDevice);
            
            modelSystem = new ModelRenderSystem();
            cameraSystem = new CameraSystem(GraphicsDevice);
            //transformSystem = new TransformSystem();

            SystemManager.Instance.addToUpdateableQueue(cameraSystem);
            SystemManager.Instance.addToDrawableQueue(modelSystem);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            int cameraId = 1;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            houseModel = Content.Load<Model>("farmhouse_obj");
            houseTexture = Content.Load<Texture2D>("farmhouse-texture");
            blobModel = Content.Load<Model>("Blob");
            blockModel = Content.Load<Model>("block2");
            groundModel = Content.Load<Model>("ground");

            CreateCamera(cameraId);
            CreateBlob(2, new Vector3(-30, 5, 30));
            CreateTerrain(3, new Vector3(0,0,0));
            //house = new House(houseModel, new Vector3(0, 0, -5000), houseTexture);
            CreateHouseModel(4, new Vector3(0,0,0));
            CreateBlock(5, new Vector3(-40, 5, -30));
            

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
            CameraComponent cameraComponent = (CameraComponent)ComponentManager.Instance.getDictionary<CameraComponent>().Values.First();
            // TODO: Add your drawing code here
            SystemManager.Instance.Draw();
            //ground.Draw(basicEffect, cameraComponent.World);
            //house.Draw(cameraComponent.World, cameraComponent.Projection);
            base.Draw(gameTime);
        }

        private Texture2D CreateTexture(GraphicsDevice device, int width, int height, System.Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] colorArray = new Color[width * height];
            for (int pixel = 0; pixel < colorArray.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                colorArray[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(colorArray);

            return texture;
        }

        private void CreateCamera(int cameraId)
        {
            var view = Matrix.CreateLookAt(new Vector3(-75, 15, 0), (Vector3.Zero + Vector3.Forward * 20), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            CameraComponent cameraComponent = new CameraComponent(cameraId, view, projection, false);
            cameraComponent.Position = new Vector3(0,0,0);
            basicEffect.Projection = cameraComponent.Projection;
            basicEffect.View = cameraComponent.View;
            TransformComponent transformComponent = new TransformComponent(cameraId, Vector3.One, cameraComponent.Position);
            ComponentManager.Instance.AddComponent(cameraComponent);
            ComponentManager.Instance.AddComponent(transformComponent);
        }

        private void CreateHouseModel(int houseId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(houseId, houseModel, houseTexture);
            TransformComponent transformComponent = new TransformComponent(houseId, new Vector3(5, 5, 5), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlob(int blobId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blobId, blobModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.BlueViolet));
            TransformComponent transformComponent = new TransformComponent(blobId, new Vector3(2, 2, 2), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlock(int blockId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blockId, blockModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.HotPink));
            TransformComponent transformComponent = new TransformComponent(blockId, new Vector3(50, 50, 50), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateTerrain(int terrainId, Vector3 position)
        {
            //ground = new Ground(GraphicsDevice, new Vector3(5000, 1, 5000), CreateTexture(GraphicsDevice, 1, 1, c => Color.LightYellow));
            ModelComponent modelComponent = new ModelComponent(terrainId, groundModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.Yellow));
            TransformComponent transformComponent = new TransformComponent(terrainId, new Vector3(50, 50, 50), position); //hej

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }
    }
}
