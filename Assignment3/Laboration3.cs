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

        //models
        private Model houseModel;
        private Texture2D houseTexture;

        //systems
        private HeightmapSystem heightmapSystem;
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
            heightmapSystem = new HeightmapSystem(GraphicsDevice, basicEffect);
            

            
            SystemManager.Instance.addToDrawableQueue(heightmapSystem);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            houseModel = Content.Load<Model>("farmhouse_obj");
            houseTexture = Content.Load<Texture2D>("farmhouse-texture");

            HeightmapComponent heightmapComponent = new HeightmapComponent(1, new[]{ CreateTexture(GraphicsDevice, 200, 200, color => Color.White),
                CreateTexture(GraphicsDevice, 200, 200, color => Color.Green)}, 0,0,GraphicsDevice);
            ComponentManager.Instance.AddComponent(heightmapComponent);

            var view = Matrix.CreateLookAt( Vector3.Zero, (Vector3.Zero + Vector3.Backward * 20) , Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            CameraComponent cameraComponent = new CameraComponent(1, view, projection, true);
            ComponentManager.Instance.AddComponent(cameraComponent);

            basicEffect.Projection = cameraComponent.Projection;
            basicEffect.View = cameraComponent.View;

            cameraSystem = new CameraSystem(GraphicsDevice);
            SystemManager.Instance.addToUpdateableQueue(cameraSystem);
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

            // TODO: Add your drawing code here
            SystemManager.Instance.Draw();
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
    }
}
