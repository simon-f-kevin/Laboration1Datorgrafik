using Assignment2.Models;
using Assignment3.Systems;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        //models
        private Model houseModel;
        private Model blobModel;
        private Model blockModel;
        private Model groundModel;
        private Texture2D houseTexture;
        private Model chopperModel;

        //update
        private TransformComponent blobTransformComponent;

        //systems
        private CameraSystem cameraSystem;
        private LightSystem lightSystem;
        private ShadowSystem shadowSystem;

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
            
            //modelSystem = new ModelRenderSystem();
            cameraSystem = new CameraSystem(GraphicsDevice);
            lightSystem = new LightSystem();
            shadowSystem = new ShadowSystem(GraphicsDevice);

            SystemManager.Instance.addToUpdateableQueue(cameraSystem, lightSystem);
            SystemManager.Instance.addToDrawableQueue(lightSystem, shadowSystem);
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
            chopperModel = Content.Load<Model>("Chopper");

            CreateCamera(cameraId);
            CreateLightSource(14);
            CreateBlob(2, new Vector3(-30, 5, 30));
            CreateTerrain(3, new Vector3(0,0,0));
            CreateTerrain(6, new Vector3(0, 0, 125));
            CreateTerrain(7, new Vector3(124, 0, 0));
            CreateTerrain(8, new Vector3(-124, 0, 0));
            CreateTerrain(9, new Vector3(0, 0, -125));
            CreateTerrain(10, new Vector3(124, 0, 125));
            CreateTerrain(11, new Vector3(-124, 0, 125));
            CreateTerrain(12, new Vector3(124, 0, -125));
            CreateTerrain(13, new Vector3(-124, 0, -125));
            CreateHouseModel(4, new Vector3(0,0,0));
            CreateBlock(5, new Vector3(-40, 1, -30));
            CreateChopper(14, new Vector3(-20, 10, -20));
            
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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                blobTransformComponent.Position = new Vector3(blobTransformComponent.Position.X + 1, blobTransformComponent.Position.Y, blobTransformComponent.Position.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                blobTransformComponent.Position = new Vector3(blobTransformComponent.Position.X - 1, blobTransformComponent.Position.Y, blobTransformComponent.Position.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                blobTransformComponent.Position = new Vector3(blobTransformComponent.Position.X, blobTransformComponent.Position.Y, blobTransformComponent.Position.Z + 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                blobTransformComponent.Position = new Vector3(blobTransformComponent.Position.X, blobTransformComponent.Position.Y, blobTransformComponent.Position.Z - 1);
            }
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
            SystemManager.Instance.Draw(spriteBatch);
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
            var view = Matrix.CreateLookAt(new Vector3(-62, 15, 0), (Vector3.Zero + Vector3.Forward * 20), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            CameraComponent2 cameraComponent = new CameraComponent2(cameraId, view, projection, false);
            cameraComponent.Position = new Vector3(0,0,0);
            cameraComponent.BoundingFrustum = new BoundingFrustum(Matrix.Identity);
            TransformComponent transformComponent = new TransformComponent(cameraId, Vector3.One, cameraComponent.Position);
            ComponentManager.Instance.AddComponent(cameraComponent);
            ComponentManager.Instance.AddComponent(transformComponent);
        }


        private void CreateLightSource(int id)
        {
            LightComponent2 lightComponent = new LightComponent2(id)
            {
                LightDirection = new Vector3(-1, 2, 2), //new Vector3(-0.3333333f, 0.6666667f, 0.6666667f), //
                DiffuseLightDirection = new Vector3(-1, 2, 2), //new Vector3(-0.3333333f, 0.6666667f, 0.6666667f),
                DiffuseColor = Color.HotPink.ToVector4(),
                DiffuseIntensity = 0.8f,
                AmbientColor = Color.IndianRed.ToVector4(),
                AmbientIntensity = 0.5f,
            };

            ComponentManager.Instance.AddComponent(lightComponent);
        }

        private void CreateHouseModel(int houseId, Vector3 position)
        {
            ModelComponent2 modelComponent = new ModelComponent2(houseId, houseModel, houseTexture);
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(houseId, new Vector3(5, 5, 5), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlob(int blobId, Vector3 position)
        {
            ModelComponent2 modelComponent = new ModelComponent2(blobId, blobModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.BlueViolet));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            blobTransformComponent = new TransformComponent(blobId, new Vector3(2, 2, 2), position);

            ComponentManager.Instance.AddComponent(blobTransformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlock(int blockId, Vector3 position)
        {
            ModelComponent2 modelComponent = new ModelComponent2(blockId, blockModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.HotPink));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(blockId, new Vector3(50, 50, 50), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateTerrain(int terrainId, Vector3 position)
        {
            ModelComponent2 modelComponent = new ModelComponent2(terrainId, groundModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.Yellow));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(terrainId, new Vector3(50, 50, 50), position); 

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);

        }

        private void CreateChopper(int chopperId, Vector3 pos)
        {
            ModelComponent2 modelComponent = new ModelComponent2(chopperId, chopperModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.LightGray));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(chopperId, Vector3.One, pos);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }
    }
}
