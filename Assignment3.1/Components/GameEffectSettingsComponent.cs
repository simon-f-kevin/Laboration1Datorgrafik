using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;

namespace Game_Engine.Components
{
    public class GameEffectSettingsComponent : EffectSettingsComponent
    {
        private LightSettingsComponent lightComponent;
        private CameraComponent cameraComponent;
        public Effect Effect { get; set; }

        public GameEffectSettingsComponent(int EntityID) : base(EntityID)
        {
            cameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            lightComponent = ComponentManager.Instance.getDictionary<LightSettingsComponent>().Values.FirstOrDefault() as LightSettingsComponent;
        }

        public override void Apply(Effect effectIN, Texture2D texture2D, Matrix worldMatrix, string techniqueName)
        {
            Effect.Parameters["Texture"].SetValue(effectIN.Parameters["Texture"].GetValueTexture2D());
            if (Effect.Parameters["Texture"] == null || Effect.Parameters["Texture"].GetValueTexture2D() == null)
            {
                Effect.Parameters["Texture"].SetValue(texture2D);
            }
            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
            Effect.Parameters["World"].SetValue(worldMatrix);
            Effect.Parameters["View"].SetValue(cameraComponent.View);
            Effect.Parameters["Projection"].SetValue(cameraComponent.Projection);
            Effect.Parameters["LightDirection"].SetValue(lightComponent.LightDirection);
            Effect.Parameters["LightViewProj"].SetValue(lightComponent.LightViewProjection);
            Effect.Parameters["ShadowStrenght"].SetValue(1f);
            Effect.Parameters["DepthBias"].SetValue(0.001f);
            Effect.Parameters["ShadowMap"].SetValue(lightComponent.RenderTarget);
            Effect.Parameters["AmbientColor"].SetValue(lightComponent.AmbientColor);
            Effect.Parameters["AmbientIntensity"].SetValue(lightComponent.AmbientIntensity);
            Effect.Parameters["ViewVector"].SetValue(Vector3.One);
            Effect.Parameters["DiffuseLightDirection"].SetValue(lightComponent.DiffuseLightDirection);
            Effect.Parameters["DiffuseColor"].SetValue(lightComponent.DiffusColor);
            Effect.Parameters["DiffuseIntensity"].SetValue(lightComponent.DiffuseIntensity);
            Effect.Parameters["CameraPosition"].SetValue(cameraComponent.CameraPosition);
            Effect.Parameters["FogStart"].SetValue(lightComponent.FogStart);
            Effect.Parameters["FogEnd"].SetValue(lightComponent.FogEnd);
            Effect.Parameters["FogColor"].SetValue(lightComponent.FogColor);
            Effect.Parameters["FogEnabled"].SetValue(lightComponent.FogEnabled);
            Effect.Parameters["Shininess"].SetValue(0.9f);
            Effect.Parameters["SpecularColor"].SetValue(Color.MediumVioletRed.ToVector4());
            Effect.Parameters["SpecularIntensity"].SetValue(0.1f);
            foreach(var effectPass in Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
            }
        }
    }
}
