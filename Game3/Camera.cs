using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game3
{
    public class Camera
    {

        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        Vector3 position = new Vector3(0, 40, 200);

        float angle;

        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = new Vector3(0, 0, 0);
                /*var rotationMatrix = Matrix.CreateRotationZ(angle);
                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += position;*/

                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt(
                    position, lookAtVector, upVector);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 2000;
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            // We'll be doing some input-based movement here
        }
    }

}
