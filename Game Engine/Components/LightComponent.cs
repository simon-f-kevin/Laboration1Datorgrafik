using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class LightComponent : EntityComponent
    {
        public Vector3 LightDirection { get; set; }
        public Matrix LightViewProjection { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 DiffuseColor { get; set; }
        public float DiffuseIntensity { get; set; }
        public Vector4 AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }
        public float LightIntensity { get; set; }
        public Vector3 LightPosition { get; set; }

        public LightComponent(int entityID) : base(entityID)
        {
        }
    }
}
