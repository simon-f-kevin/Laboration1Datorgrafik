using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class House : AbstractModel, IModel
    {
        private Texture2D texture;

        public House(Model houseModel, Vector3 pos, Texture2D texture)
        {
            model = houseModel;
            model.Bones[0].Transform = Matrix.CreateTranslation(pos);
            this.texture = texture;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = model.Bones[0].Transform;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                    effect.Texture = texture;
                    effect.TextureEnabled = true;

                    mesh.Draw();
                }
            }
        }

    }
}
