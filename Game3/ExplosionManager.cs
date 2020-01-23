using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace Game3
{
    static class ExplosionManager
    {
        public static List<Explosion> explosions = new List<Explosion>();
        public static List<Explosion> addingexplosions = new List<Explosion>();
        static bool isUpdating = false;
        public static GraphicsDevice graphicsDevice;

        public static void Add(Explosion explosion)
        {
            if (!isUpdating)
                explosions.Add(explosion);
            else
                addingexplosions.Add(explosion);
        }

        public static void Initialize(GraphicsDevice graphicsdevice)
        {
            graphicsDevice = graphicsdevice;
        }


        public static void Update(GameTime gameTime)
        {
            isUpdating = true;

            foreach (var expl in explosions)
                expl.Update(gameTime);

            isUpdating = false;

            foreach (var expl in addingexplosions)
                explosions.Add(expl);

            addingexplosions.Clear();

            explosions = explosions.Where(x => !x.IsExpired).ToList();
        }

        public static void Draw(Camera camera)
        {
            foreach (var expl in explosions)
                expl.Draw(camera);
        }

    }
}
