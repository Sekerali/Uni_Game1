using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    abstract class Entity
    {
        protected Model Model;
        public Vector3 diffuseColor;
        public Vector3 ambientLightColor;

        public Vector3 Position;
        public float Orientation;
        public float AngleY;
        public float AngleX;
        public float Radius;
        public float Scale;
        public bool IsExpired;

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(Camera camera)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.DiffuseColor = diffuseColor;
                    effect.AmbientLightColor = ambientLightColor;
                    

                    effect.World = GetWorldMatrix();
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }

        Matrix GetWorldMatrix()
        {
            Matrix translationMatrix = Matrix.CreateTranslation(Position);
            Matrix rotationMatrixZ = Matrix.CreateRotationZ(Orientation);
            Matrix rotationMatrixY = Matrix.CreateRotationY(AngleY);
            Matrix rotationMatrixX = Matrix.CreateRotationX(AngleX);
            Matrix scale = Matrix.CreateScale(Scale);

            Matrix combined = scale * rotationMatrixX * rotationMatrixZ * rotationMatrixY * translationMatrix;

            return combined;

        }


    }
}
