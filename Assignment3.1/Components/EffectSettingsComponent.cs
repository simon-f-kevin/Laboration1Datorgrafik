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
    public class EffectSettingsComponent : EntityComponent
    {
        public EffectSettingsComponent(int EntityID) : base(EntityID)
        {
            CameraComponent = ComponentManager.Instance.getDictionary<CameraComponent>().Values.FirstOrDefault() as CameraComponent;
            LightComponent = ComponentManager.Instance.getDictionary<LightSettingsComponent>().Values.FirstOrDefault() as LightSettingsComponent;
        }
        private LightSettingsComponent LightComponent { get; set; }
        private CameraComponent CameraComponent { get; set; }
        public Effect effect { get; set; }
        public Matrix world{ get; set; }      
        public string Techniquename { get; set; }
        public void AddEffect(Effect effectIN, Texture2D texture2D)
        {
            effect.Parameters["Texture"].SetValue(effectIN.Parameters["Texture"].GetValueTexture2D());
            if(effect.Parameters["Texture"] == null || effect.Parameters["Texture"].GetValueTexture2D() == null)
            {
                effect.Parameters["Texture"].SetValue(texture2D);
            }
        }
        public void Apply()
        {
            effect.CurrentTechnique = effect.Techniques[Techniquename];
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(CameraComponent.View);
            effect.Parameters["Projection"].SetValue(CameraComponent.Projection);
            effect.Parameters["LightDirection"].SetValue(LightComponent.LightDirection);
            effect.Parameters["LightViewProj"].SetValue(LightComponent.LightViewProjection);
            effect.Parameters["ShadowStrenght"].SetValue(1f);
            effect.Parameters["DepthBias"].SetValue(0.001f);

            //if (!createShadowMap)
//            {
            effect.Parameters["ShadowMap"].SetValue(LightComponent.RenderTarget);
//            }

            effect.Parameters["AmbientColor"].SetValue(LightComponent.AmbientColor);
            effect.Parameters["AmbientIntensity"].SetValue(LightComponent.AmbientIntensity);
            
            effect.Parameters["ViewVector"].SetValue(Vector3.One);
            //effect.Parameters["DiffuseLightDirection"].SetValue(shader.DiffuseLightDirection);
            effect.Parameters["DiffuseLightDirection"].SetValue(LightComponent.DiffuseLightDirection); // todo
            effect.Parameters["DiffuseColor"].SetValue(LightComponent.DiffusColor);
            effect.Parameters["DiffuseIntensity"].SetValue(LightComponent.DiffuseIntensity);

            effect.Parameters["CameraPosition"].SetValue(CameraComponent.CameraPosition);
   
            effect.Parameters["FogStart"].SetValue(LightComponent.FogStart);
            effect.Parameters["FogEnd"].SetValue(LightComponent.FogEnd);
            effect.Parameters["FogColor"].SetValue(LightComponent.FogColor);
            effect.Parameters["FogEnabled"].SetValue(LightComponent.FogEnabled);

            effect.Parameters["Shininess"].SetValue(0.9f);
            effect.Parameters["SpecularColor"].SetValue(Color.MediumVioletRed.ToVector4());
            effect.Parameters["SpecularIntensity"].SetValue(0.1f);
            foreach(var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
            }
            //effect.CurrentTechnique.Passes[0].Apply();
        }
    }
}
