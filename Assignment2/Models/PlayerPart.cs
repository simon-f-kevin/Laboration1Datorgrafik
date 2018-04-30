using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment2.Models
{
    public class PlayerPart : Player
    {
        public Vector3 JointPosition;

        public PlayerPart(Vector3 pos, Vector3 rot, Vector3 scale, GraphicsDevice device, Vector3 JointPos) : base(pos, rot, scale, device)
        {
            JointPosition = JointPos;
        }

        public override void Update()
        {
            WorldMatrix = Matrix.Identity * Matrix.CreateTranslation(Position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
                Matrix.CreateTranslation(JointPosition);

            foreach (Player child in Children)
                child.Update();
        }

        public override void Draw(BasicEffect effect, Matrix worldMatrix)
        {
            effect.World = worldMatrix * WorldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

            foreach(Player child in Children)
            {
                child.Draw(effect, worldMatrix * WorldMatrix);
            }

        }
    }
}
