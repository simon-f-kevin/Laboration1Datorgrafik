using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3._1
{
    public class LightSystem : IUpdateableSystem, IDrawableSystem
    {
        public LightSystem()
        {
            Enabled = true;
            Visible = true;
        }

        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        public void Draw()
        {
            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            CreateLightViewProjectionMatrix(lightComponent, cameraComp);
        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }


        public void Update(GameTime gameTime)
        {

            var lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.FirstOrDefault() as LightComponent;
            var rotationY = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.00005f;
            var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

            lightComponent.LightDir = Vector3.Transform(lightComponent.LightDir, rotation);
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                lightComponent.LightDir = new Vector3(lightComponent.LightDir.X, lightComponent.LightDir.Y, lightComponent.LightDir.Z - 0.002f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                lightComponent.LightDir = new Vector3(lightComponent.LightDir.X, lightComponent.LightDir.Y, lightComponent.LightDir.Z + 0.002f);
            }

        }


        private void CreateLightViewProjectionMatrix(LightComponent lightComponent, CameraComponent cameraComp)
        {

            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightComponent.LightDir,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraComp.CameraFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightComponent.LightDir,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            lightComponent.LightViewProjection = lightView * lightProjection;
        }
    }
}
