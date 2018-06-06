using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Engine.Components;
using Game_Engine.Managers;
//using Game_Engine.Systems;
using System.Linq;
using Assignment3._1.Components;
using Assignment3._1.Systems;
using CameraComponent = Assignment3._1.Components.CameraComponent;
using LightComponent = Assignment3._1.Components.LightComponent;
//using CameraComponent = Game_Engine.Components.CameraComponent;
using ModelComponent = Game_Engine.Components.ModelComponent;

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
        private HandleInputSystem handleInputSystem;
        private LightSystem lightSystem;
        private ShadowSystem shadowSystem;
        //private RenderSystem renderSystem;


        private TransformComponent blobTransformComponent;

        public Game1()
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
            //renderSystem = new RenderSystem(GraphicsDevice);
            cameraSystem = new CameraSystem();
            handleInputSystem = new HandleInputSystem();
            lightSystem = new LightSystem();
            shadowSystem = new ShadowSystem(GraphicsDevice);
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
            //CreateBlob(2, new Vector3(-30, 10, 50));
            //CreateBlock(3, new Vector3(50, mid, -40));
            //Ground                                         
            CreateGround(4, new Vector3(mid, mid, mid));
            CreateGround(5, new Vector3(mid, mid, high));
            //CreateGround(6, new Vector3(mid, mid, low));
            CreateGround(7, new Vector3(high, mid, mid));
            CreateGround(8, new Vector3(high, mid, high));
            //CreateGround(9, new Vector3(high, mid, low));
            //CreateGround(10, new Vector3(low, mid, mid));
            //CreateGround(11, new Vector3(low, mid, high));
            //CreateGround(12, new Vector3(low, mid, low));

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

            //renderSystem.Update(gameTime);
            cameraSystem.Update(gameTime);
            // TODO: Add your update logic here
            lightSystem.Update(gameTime);
            cameraSystem.Update(gameTime);
            handleInputSystem.Update(gameTime);
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
            //renderSystem.Draw(spriteBatch);
            lightSystem.Draw();
            shadowSystem.Draw();
            base.Draw(gameTime);
        }


        private void CreateHouse(int houseId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(houseId, houseModel, houseTexture);
            modelComponent.RenderShadowMap = true;
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            modelComponent.ObjectWorld = Matrix.Identity;
            TransformComponent transformComponent = new TransformComponent(houseId, new Vector3(5, 5, 5), position);

            ShadowMappingEffectComponent shadowMap = new ShadowMappingEffectComponent(houseId);
            shadowMap.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(shadowMap);
            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlob(int blobId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blobId, blobModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.BlueViolet));
            modelComponent.RenderShadowMap = true;
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            modelComponent.ObjectWorld = Matrix.Identity;
            blobTransformComponent = new TransformComponent(blobId, new Vector3(2, 2, 2), position);
            ShadowMappingEffectComponent shadowMap = new ShadowMappingEffectComponent(blobId);
            shadowMap.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(shadowMap);
            ComponentManager.Instance.AddComponent(blobTransformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateBlock(int blockId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(blockId, blockModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.HotPink));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            modelComponent.ObjectWorld = Matrix.Identity;
            TransformComponent transformComponent = new TransformComponent(blockId, new Vector3(50, 50, 50), position);
            ShadowMappingEffectComponent shadowMap = new ShadowMappingEffectComponent(blockId);
            shadowMap.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(shadowMap);
            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);
        }

        private void CreateGround(int groundId, Vector3 position)
        {
            ModelComponent modelComponent = new ModelComponent(groundId, groundModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.Yellow));
            modelComponent.RenderShadowMap = true;
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            modelComponent.ObjectWorld = Matrix.Identity;
            TransformComponent transformComponent = new TransformComponent(groundId, new Vector3(50, 50, 50), position);
            ShadowMappingEffectComponent shadowMap = new ShadowMappingEffectComponent(groundId);
            shadowMap.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(shadowMap);
            ComponentManager.Instance.AddComponent(transformComponent);
            ComponentManager.Instance.AddComponent(modelComponent);

        }

        private void CreateChopper(int chopperId, Vector3 pos)
        {
            ModelComponent modelComponent = new ModelComponent(chopperId, chopperModel, CreateTexture(GraphicsDevice, 1, 1, c => Color.LightGray));
            modelComponent.Effect = Content.Load<Effect>("Shadow");
            modelComponent.RenderShadowMap = true;
            modelComponent.ObjectWorld = Matrix.Identity;
            TransformComponent transformComponent = new TransformComponent(chopperId, Vector3.One, pos);
            ShadowMappingEffectComponent shadowMap = new ShadowMappingEffectComponent(chopperId);
            shadowMap.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(shadowMap);
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
            CameraComponent cameraComp = new CameraComponent(cameraId);
            cameraComp.CameraPosition = new Vector3(0, 70, 100);
            cameraComp.CameraForward = new Vector3(0, -0.4472136f, -0.8944272f);
            cameraComp.CameraFrustum = new BoundingFrustum(Matrix.Identity);
            cameraComp.AspectRatio = (float)800 / (float)400;
            cameraComp.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                cameraComp.AspectRatio,
                1.0f, 1000.0f);
            ComponentManager.Instance.AddComponent(cameraComp);
        }

        private void CreateLighting(int lightID)
        {
            var lightComp = new LightComponent(lightID);
            lightComp.LightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);
            lightComp.DiffusColor = Color.White.ToVector4();
            lightComp.DiffuseIntensity = 0.5f;
            lightComp.DiffuseLightDirection = lightComp.LightDir;
            ComponentManager.Instance.AddComponent(lightComp);
            var ambientComp = new AmbientComponent(lightID);
            ambientComp.AmbientColor = Color.White.ToVector4();
            ambientComp.AmbientIntensity = 0.2f;
            ComponentManager.Instance.AddComponent(ambientComp);

            var shadowRenderComp = new ShadowRenderTargetComponent(lightID);
            shadowRenderComp.ShadowRenderTarget = new RenderTarget2D(GraphicsDevice,
                2048,
                2048,
                false,
                SurfaceFormat.Single,
                DepthFormat.Depth24);
            ComponentManager.Instance.AddComponent(shadowRenderComp);
        }
    }
}
