using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Assignment2.Models
{
    abstract class Player
    {
        protected Texture2D Texture { get; set; }
        protected VertexBuffer VBuffer;
        protected IndexBuffer IBuffer;
        protected abstract int NumVertices { get; set; }
        protected abstract int NumPrimitives { get; set; }
        protected abstract int NumIndices { get; set; }
        protected Matrix ObjectWorld;
        protected Matrix Scale { get; set; }
        protected Quaternion Rotation { get; set; }
        protected Vector3 Position { get; set; }
        protected GraphicsDevice Graphics { get; set; }
        public List<Player> Children { get; set; }

        /// <summary>
        /// Drawable game object with a texture
        /// </summary>
        /// <param name="graphics">GraphicsDevice to draw to</param>
        /// <param name="texture">Texture to draw on object</param>
        public Player(GraphicsDevice graphics, Texture2D texture)
        {
            Graphics = graphics;
            Texture = texture;
            ObjectWorld = Matrix.Identity;
            Scale = Matrix.Identity;
            Rotation = Quaternion.Identity;
            Position = Vector3.Zero;
            Children = new List<Player>();
        }

        public virtual void Update(GameTime gameTime)
        {
            ObjectWorld = Scale * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
            Children.ForEach(x => x.Update(gameTime));
        }

        public virtual void Draw(BasicEffect effect, Matrix parentObjectWorld)
        {
            effect.TextureEnabled = true;
            effect.Texture = Texture;
            effect.World = parentObjectWorld * ObjectWorld;

            Graphics.SetVertexBuffer(VBuffer);
            Graphics.Indices = IBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                pass.Apply();

            Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, 0, NumPrimitives);

            Children.ForEach(x => x.Draw(effect, ObjectWorld));
        }

        public abstract void InitVertices();
        public abstract void InitIndices();
        
    }

    class WalkingModel : Player
    {
        private const float MAXROTATION = MathHelper.PiOver4;
        private float speed = 0, rotationSpeed = 0.003f, modelRotation = 0;
        private Model model;
        private bool direction = true;

        private Matrix leftArmMatrix;
        private Matrix leftLegMatrix;
        private Matrix rightArmMatrix;
        private Matrix rightLegMatrix;

        private Matrix planeObjectWorld = Matrix.Identity;

        private Matrix[] transformMatrices;

        protected override int NumVertices { get; set; }
        protected override int NumPrimitives { get; set; }
        protected override int NumIndices { get; set; }

        public WalkingModel(GraphicsDevice graphics, Texture2D texture, Model model, BasicEffect effect) : base(graphics, texture)
        {
            this.model = model;
            Texture = texture;

            transformMatrices = new Matrix[model.Bones.Count];

            Scale = Matrix.CreateScale(1f);
            Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2) * Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart mp in mesh.MeshParts)
                    mp.Effect = effect;
            }

            leftArmMatrix = model.Bones["LeftArm"].Transform;
            rightArmMatrix = model.Bones["RightArm"].Transform;
            leftLegMatrix = model.Bones["LeftLeg"].Transform;
            rightLegMatrix = model.Bones["RightLeg"].Transform;

        }

        public override void InitVertices() { }
        public override void InitIndices() { }

        public override void Update(GameTime gameTime)
        {

            float rotation = 0;
            speed = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                speed = 0.1f * (float)gameTime.ElapsedGameTime.Milliseconds;

                if (modelRotation < MAXROTATION)
                {
                    if (direction)
                        modelRotation += speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        modelRotation -= speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    if (!direction)
                        modelRotation -= speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        modelRotation += speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (modelRotation > MAXROTATION || modelRotation < -MAXROTATION)
                    direction = !direction;

                model.Bones["RightArm"].Transform = TransformPart(rightArmMatrix, modelRotation);
                model.Bones["LeftArm"].Transform = TransformPart(leftArmMatrix, -modelRotation);
                model.Bones["RightLeg"].Transform = TransformPart(rightLegMatrix, -modelRotation);
                model.Bones["LeftLeg"].Transform = TransformPart(leftLegMatrix, modelRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotation = 0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotation = -0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds;
            }

            Rotation *= Quaternion.CreateFromYawPitchRoll(rotation, 0, 0);
            //planeObjectWorld = Matrix.CreateFromQuaternion(Rotation);
            //Position += planeObjectWorld.Forward * speed;

        }

        /// <summary>
        /// Transform a model part
        /// </summary>
        /// <param name="origin">Original transformation matrix</param>
        /// <param name="rotation">Rotation (radians)</param>
        /// <returns></returns>
        private Matrix TransformPart(Matrix origin, float rotation)
        {
            Vector3 originPosition = origin.Translation;

            origin.Translation = Vector3.Zero;
            origin *= Matrix.CreateRotationX(rotation);
            origin.Translation = originPosition;

            return origin;
        }

        public override void Draw(BasicEffect effect, Matrix parentObjectWorld)
        {
            effect.TextureEnabled = true;
            effect.Texture = Texture;

            model.CopyAbsoluteBoneTransformsTo(transformMatrices);

            ObjectWorld = Matrix.Identity * Scale * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                effect.World = transformMatrices[mesh.ParentBone.Index] * ObjectWorld * parentObjectWorld;

                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique.Passes[0].Apply();
                }
                mesh.Draw();
            }
        }
    }
}
