﻿using Game_Engine.Components;
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
        public LightComponent(int entityID) : base(entityID)
        {
        }

        public Vector3 LightDir { get; set; }
        public Matrix LightViewProjection { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 DiffusColor { get; set; }
        public float DiffuseIntensity { get; set; }
  
    }
}
