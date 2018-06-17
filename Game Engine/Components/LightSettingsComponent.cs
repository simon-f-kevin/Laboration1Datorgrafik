using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class LightSettingsComponent : EntityComponent
    {
        public Vector3 LightDirection { get; set; }
        public Matrix LightViewProjection { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 DiffusColor { get; set; }
        public float DiffuseIntensity { get; set; }
        public Vector4 AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }
        public RenderTarget2D RenderTarget { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }
        public LightSettingsComponent(int entityID) : base(entityID)
        {

        }

        public void UpdateLightViewProjection(Vector3[] frustumCornerPositions)
        {
            Matrix lightRotationMatrix = Matrix.CreateLookAt(Vector3.Zero, -this.LightDirection, Vector3.Up);
            for (int i = 0; i < frustumCornerPositions.Length; i++)
            {
                frustumCornerPositions[i] = Vector3.Transform(frustumCornerPositions[i], lightRotationMatrix);
            }

            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCornerPositions);
            Vector3 lightBoxSize = lightBox.Max - lightBox.Min;
            Vector3 lightBoxPosition = Vector3.Transform(lightBox.Min + (lightBoxSize / 2), Matrix.Invert(lightRotationMatrix));

            Matrix lightViewMatrix = Matrix.CreateLookAt(lightBoxPosition, lightBoxPosition - this.LightDirection, Vector3.Up);
            Matrix lightProjectionMatrix = Matrix.CreateOrthographic(lightBoxSize.X, lightBoxSize.Y, -lightBoxSize.Z, lightBoxSize.Z);
            this.LightViewProjection = lightViewMatrix * lightProjectionMatrix;
        }
    }
}
