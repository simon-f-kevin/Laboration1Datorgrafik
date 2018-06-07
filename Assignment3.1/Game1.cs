using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using System.Linq;
using Assignment3._1.Components;

namespace Assignment3._1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix World = Matrix.Identity;

        const int shadowMapWidthHeight = 2048;

        const int windowWidth = 800;
        const int windowHeight = 480;

        private Model houseModel;
        private Model blobModel;
        private Model blockModel;
        private Model groundModel;
        private Texture2D houseTexture;

        private CameraSystem cameraSystem;
        private LightSystem lightSystem;
        private ShadowSystem shadowSystem;
        private RenderSystem renderSystem;


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
            //renderSystem = new RenderSystem(GraphicsDevice);
            cameraSystem = new CameraSystem();
            lightSystem = new LightSystem();
            shadowSystem = new ShadowSystem(graphics.GraphicsDevice, World);
            renderSystem = new RenderSystem(graphics.GraphicsDevice, World);
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

            int high = 125;
            int low = -high;
            int mid = 0;

            CreateHouse(1, new Vector3(50, mid, 20));
            CreateBlob(2, new Vector3(70, 20, 50));
            CreateGround(3, new Vector3(50, -10, 20));


            CreateCamera(13);
            CreateLighting(14);
            CreateAmbientLight(15);
            CreateShadowrender(16);
            CreateFog(17);

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

            //lightSystem.Update(gameTime);
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
            //lightSystem.Draw();
            //shadowSystem.Draw();
            renderSystem.Draw();

            base.Draw(gameTime);
        }


        private void CreateHouse(int houseId, Vector3 position)
        {
            CreateShadedModel(houseId, position, houseModel, houseTexture);
        }
        private void CreateGround(int groundId, Vector3 position)
        {
            CreateShadedModel(groundId, position, groundModel, CreateTexture(Color.Yellow));
        }
        private void CreateBlob(int blobId, Vector3 position)
        {
            CreateShadedModel(blobId, position, blobModel, CreateTexture(Color.PaleVioletRed));
        }

        private void CreateShadedModel(int entityId, Vector3 position, Model model, Texture2D texture)
        {

            var transformComponent = new TransformComponent(entityId);
            transformComponent.Position = position;
            transformComponent.Scale = 1f;

            var modelComponent = new ModelComponent(entityId);
            modelComponent.Model = model;
            modelComponent.Texture2D = texture;
            modelComponent.ObjectWorld = World 
                * Matrix.CreateScale(new Vector3(transformComponent.Scale)) 
                * Matrix.CreateTranslation(transformComponent.Position);

            var shadowMappingEffect = new ShadowMappingEffect(entityId);
            shadowMappingEffect.effect = Content.Load<Effect>("Shadow");

            ComponentManager.Instance.AddComponent(modelComponent);
            ComponentManager.Instance.AddComponent(shadowMappingEffect);
            ComponentManager.Instance.AddComponent(transformComponent);
        }

        private Texture2D CreateTexture(Color colour)
        {
            return CreateTexture(graphics.GraphicsDevice, 1, 1, c => colour);
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
            var cameraComp = new CameraComponent(cameraId);
            cameraComp.CameraPosition = new Vector3(0, 70, 100);
            cameraComp.CameraForward = new Vector3(0, -0.4472136f, -0.8944272f);
            cameraComp.CameraFrustum = new BoundingFrustum(Matrix.Identity);
            cameraComp.AspectRatio = (float)windowWidth / (float)windowHeight;
            cameraComp.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                             cameraComp.AspectRatio,
                                                             1.0f, 1000.0f);
            ComponentManager.Instance.AddComponent(cameraComp);
        }

        private void CreateLighting(int lightID)
        {
            LightComponent lightComp = new LightComponent(lightID);

            lightComp.LightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);
            lightComp.DiffusColor = Color.White.ToVector4();
            lightComp.DiffuseIntensity = 0.5f;
            lightComp.DiffuseLightDirection = lightComp.LightDir;
            lightComp.AmbientColor = Color.White.ToVector4();
            lightComp.AmbientIntensity = 0.2f;

            ComponentManager.Instance.AddComponent(lightComp);
        }

        private void CreateAmbientLight(int ambID)
        {
            AmbientComponent ambientComp = new AmbientComponent(ambID);
            //ambientComp.AmbientColor = Color.White.ToVector4();
            //ambientComp.AmbientIntensity = 0.2f;
            ComponentManager.Instance.AddComponent(ambientComp);
        }

        public void CreateFog(int fogid)
        {
            FogComponent fogComp = new FogComponent(fogid);
            fogComp.FogColor = Color.CornflowerBlue.ToVector4();
            fogComp.FogEnabled = true;
            fogComp.FogStart = 200f;
            fogComp.FogEnd = 300f;
            ComponentManager.Instance.AddComponent(fogComp);
        }
        public void CreateShadowrender(int shadowid)
        {
            ShadowRenderTargetComponent shadowRenderComp = new ShadowRenderTargetComponent(shadowid);
            shadowRenderComp.ShadowRenderTarget = new RenderTarget2D(graphics.GraphicsDevice,
                                shadowMapWidthHeight,
                                shadowMapWidthHeight,
                                false,
                                SurfaceFormat.Single,
                                DepthFormat.Depth24);
            ComponentManager.Instance.AddComponent(shadowRenderComp);
        }


    }
}
