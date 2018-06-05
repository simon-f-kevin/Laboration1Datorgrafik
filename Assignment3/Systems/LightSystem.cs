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

namespace Assignment3.Systems
{
    public class LightSystem : IUpdateableSystem, IDrawableSystem
    {
        private LightComponent lightComponent;

        public void Update(GameTime gameTime)
        {
            lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;
            var rotationY = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.0005f;
            var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

            lightComponent.LightDirection = Vector3.Transform(lightComponent.LightDirection, rotation);
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                lightComponent.LightDirection = new Vector3(lightComponent.LightDirection.X, lightComponent.LightDirection.Y, lightComponent.LightDirection.Z - 0.2f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                lightComponent.LightDirection = new Vector3(lightComponent.LightDirection.X, lightComponent.LightDirection.Y, lightComponent.LightDirection.Z + 0.2f);
            }
        }
        public void Draw()
        {
            CreateLightMatrix();
        }

        public void Draw(SpriteBatch spriteBatch) { }

        private void CreateLightMatrix()
        {
            var cameraComp = ComponentManager.Instance.getDictionary<CameraComponent>().Values.First() as CameraComponent;
            lightComponent = ComponentManager.Instance.getDictionary<LightComponent>().Values.First() as LightComponent;

            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightComponent.LightDirection,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraComp.BoundingFrustum.GetCorners();

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
                                                   lightPosition - lightComponent.LightDirection,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            lightComponent.LightViewProjection = lightView * lightProjection;
        }
    }
}
