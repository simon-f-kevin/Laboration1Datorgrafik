using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
   
    public class CameraComponent : EntityComponent
    {
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraForward { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public BoundingFrustum CameraFrustum { get; set; }
        public float AspectRatio { get; set; }
        public float CameraTurnSpeed { get; set; }
        public float CameraMoveSpeed { get; set; }
        public CameraComponent(int entityID) : base(entityID)
        {
            
        }

        public void UpdateView()
        {
            View = Matrix.CreateLookAt(CameraPosition, this.CameraPosition + this.CameraForward, Vector3.Up);
        }

        public void UpdateFrustrum()
        {
            CameraFrustum.Matrix = this.View * this.Projection;
        }
    }
}
