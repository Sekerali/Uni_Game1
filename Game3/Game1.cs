using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Game3
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public static Menu menu;

        public static Model Player { get; private set; }
        public static Model Bullet { get; private set; }
        public static Model Enemy1 { get; private set; }
        public static Model Enemy2 { get; private set; }
        public static Model BossShip { get; private set; }
        public static Model BossTurret { get; private set; }
        public static Model BossShield { get; private set; }
        public static Model BossCore { get; private set; }

        public static Texture2D Floor { get; private set; }

        public static Texture2D HealthFront { get; private set; }
        public static Texture2D HealthBack { get; private set; }
        public static SpriteFont HealthName { get; private set; }

        public static Texture2D MainBack { get; private set; }
        public static Texture2D MainButton { get; private set; }
        public static Texture2D MainHeader { get; private set; }

        public static List<Texture2D> Explosion = new List<Texture2D>();

        public static SpriteFont MainButtonName { get; private set; }
        public static SpriteFont CreditsText { get; private set; }

        public static Song Main { get; private set; }
        public static Song Ingame { get; private set; }
        public static Song Boss { get; private set; }

        public static SoundEffect Explode { get; private set; }
        public static SoundEffect Spawn { get; private set; }
        public static SoundEffect PlayerShoot { get; private set; }
        public static SoundEffect EnemyShoot { get; private set; }
        public static SoundEffect Select { get; private set; }
        public static SoundEffect SelectCredits { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            Content.RootDirectory = @"Content";
        }

        protected override void Initialize()
        {

            base.Initialize();

            menu = new Menu(graphics.GraphicsDevice, this);
            menu.Initialize();

        }

        protected override void LoadContent()
        {
            Floor = Content.Load<Texture2D>("Floor");
            MainBack = Content.Load<Texture2D>("Main_Back");
            MainButton = Content.Load<Texture2D>("Main_Button");
            MainHeader = Content.Load<Texture2D>("Main_Header");
            MainButtonName = Content.Load<SpriteFont>("Main_Button_Text");
            CreditsText = Content.Load<SpriteFont>("credits");
            HealthFront = Content.Load<Texture2D>("Health");
            HealthBack = Content.Load<Texture2D>("Health_Back");
            HealthName = Content.Load<SpriteFont>("Health_Name");
            Player = Content.Load<Model>("Player_Ship");
            Bullet = Content.Load<Model>("Bullet");
            Enemy1 = Content.Load<Model>("Enemy_Ship1");
            Enemy2 = Content.Load<Model>("Enemy_Ship2");
            BossShip = Content.Load<Model>("boss_ship");
            BossTurret = Content.Load<Model>("boss_turret");
            BossShield = Content.Load<Model>("boss_shield");
            BossCore = Content.Load<Model>("boss_core");
            Main = Content.Load<Song>("sound/main");
            Ingame = Content.Load<Song>("sound/ingame");
            Boss = Content.Load<Song>("sound/boss");
            Explode = Content.Load<SoundEffect>("sound/explosion");
            Spawn = Content.Load<SoundEffect>("sound/spawn");
            PlayerShoot = Content.Load<SoundEffect>("sound/playershoot");
            EnemyShoot = Content.Load<SoundEffect>("sound/enemyshoot");
            Select = Content.Load<SoundEffect>("sound/levelselect");
            SelectCredits = Content.Load<SoundEffect>("sound/creditsselect");


            for (int i = 0; i < 64; i++)
            {
                Explosion.Add(Content.Load<Texture2D>("explosion/" + (i + 1)));
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            menu.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            menu.Draw();

            base.Draw(gameTime);
        }

        public static float NextFloat(Random rand, float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }

    }
}
