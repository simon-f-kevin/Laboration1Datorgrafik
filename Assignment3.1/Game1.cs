using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using System.Linq;

namespace Assignment3._1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Model houseModel;
        private Model blobModel;
        private Model blockModel;
        private Model groundModel;
        private Model chopperModel;
        private Texture2D houseTexture;

        private CameraSystem cameraSystem;
        private RenderSystem renderSystem;

        private TransformComponent blobTransformComponent;

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
            renderSystem = new RenderSystem(GraphicsDevice);
            cameraSystem = new CameraSystem(GraphicsDevice);
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

            houseModel = Content.Load<Model>("farmhouse_obj");
            houseTexture = Content.Load<Texture2D>("farmhouse-texture");
            blobModel = Content.Load<Model>("Blob");
            blockModel = Content.Load<Model>("block2");
            groundModel = Content.Load<Model>("ground");
            //chopperModel = Content.Load<Model>("chopper");

            int high = 125;
            int low = -high;
            int mid = 0;

            CreateHouse(1, new Vector3(50, mid, 20));
            CreateBlob(2, new Vector3(-30, 10, 50));
            CreateBlock(3, new Vector3(50, mid, -40));
            //Ground                                         
            CreateGround(4, new Vector3(mid, mid, mid));
            CreateGround(5, new Vector3(mid, mid, high));
            CreateGround(6, new Vector3(mid, mid, low));
            CreateGround(7, new Vector3(high, mid, mid));
            CreateGround(8, new Vector3(high, mid, high));
            CreateGround(9, new Vector3(high, mid, low));
            CreateGround(10, new Vector3(low, mid, mid));
            CreateGround(11, new Vector3(low, mid, high));
            CreateGround(12, new Vector3(low, mid, low));

            CreateCamera(13);
            CreateLighting(14);

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

            renderSystem.Update(gameTime);
            cameraSystem.Update(gameTime);
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
            renderSystem.Draw();

            base.Draw(gameTime);
        }


        private void CreateHouse(int houseId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(houseId, houseModel, houseTexture);
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(houseId, new Vector3(5, 5, 5), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlob(int blobId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blobId, blobModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.BlueViolet));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            blobTransformComponent = new TransformComponent(blobId, new Vector3(2, 2, 2), position);

            ComponentManager.Instance.AddComponent(blobTransformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlock(int blockId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blockId, blockModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.HotPink));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(blockId, new Vector3(50, 50, 50), position);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateGround(int groundId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(groundId, groundModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.Yellow));
            modelComponent.model.Tag = "ground";
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(groundId, new Vector3(50, 50, 50), position);


            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);

        }

        private void CreateChopper(int chopperId, Vector3 pos)
        {
            ModelComponent modelComponent = new ModelComponent(chopperId, chopperModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.LightGray));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            TransformComponent transformComponent = new TransformComponent(chopperId, Vector3.One, pos);

            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
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
            Vector3 cameraPosition = new Vector3(0, 70, 100);
            Vector3 cameraForward = new Vector3(0, -0.4472136f, -0.8944272f);
            BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);
            float aspectRatio = (float)800 / (float)480;
            Matrix viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraForward, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 1000.0f);
            CameraComponent cameraComponent = new CameraComponent(cameraId, viewMatrix, projection, false);
            cameraComponent.Position = cameraPosition;
            cameraComponent.BoundingFrustum = cameraFrustum;
            TransformComponent transformComponent = new TransformComponent(cameraId, Vector3.One, cameraComponent.Position);

            ComponentManager.Instance.AddComponent(cameraComponent);
            ComponentManager.Instance.AddComponent(transformComponent);
        }

        private void CreateLighting(int lightID)
        {
            LightComponent lightComponent = new LightComponent(lightID)
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
    }
}
