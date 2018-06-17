using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{

    public class CameraComponent : EntityComponent
    {
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraForward { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public BoundingFrustum BoundingFrustrum { get; set; }
        public float AspectRatio { get; set; }
        public float CameraTurnSpeed { get; set; }
        public float CameraMoveSpeed { get; set; }
        public CameraComponent(int entityID) : base(entityID)
        {
            
        }

        public void UpdateView()
        {
            this.View = Matrix.CreateLookAt(CameraPosition, this.CameraPosition + this.CameraForward, Vector3.Up);
        }

        public void UpdateFrustrum()
        {
            this.BoundingFrustrum.Matrix = this.View * this.Projection;
        }
    }
}
