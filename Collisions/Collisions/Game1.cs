using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Collisions
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D obstacle1;
        Texture2D character;

        Vector2 characterpos;
        Vector2 obstaclepos1;

        Vector2 spriteSpeed1 = new Vector2(0.0f, 0.0f);
        Vector2 spriteSpeed2 = new Vector2(100.0f, 100.0f);
        float spriteAngle1 = 0;

        int sprite1Height;
        int sprite1Width;


        SoundEffect soundEffect;
        SoundEffect soundEffect2;
        SoundEffectInstance chimes;
        SoundEffectInstance mario;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures

            spriteBatch = new SpriteBatch(GraphicsDevice);

            obstacle1 = Content.Load<Texture2D>("cactus");
            obstacle1 = Content.Load<Texture2D>("Minion");


            soundEffect = Content.Load<SoundEffect>("NFF-cowbell-big");
            soundEffect2 = Content.Load<SoundEffect>("Super Mario Bros. medley");
            chimes = soundEffect.CreateInstance();
            mario = soundEffect2.CreateInstance();
            mario.IsLooped = true;
            mario.Play();

            characterpos.X = 100;
            characterpos.Y = 500;



            sprite1Height = obstacle1.Bounds.Height;
            sprite1Width = obstacle1.Bounds.Width;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allow the game to exit

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Move the sprite around

            UpdateSprite(gameTime, ref obstaclepos1, ref spriteSpeed1);
            CheckForCollision();
            if(chimes.State != SoundState.Playing && mario.State == SoundState.Paused)
            {
                mario.Play();
            }
            base.Update(gameTime);
        }

        void UpdateSprite(GameTime gameTime, ref Vector2 spritePosition, ref Vector2 spriteSpeed)
        {

            // Move the sprite by speed, scaled by elapsed time 

            spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteAngle1 += 0.01f;
            int MaxX = graphics.GraphicsDevice.Viewport.Width - obstacle1.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - obstacle1.Height;
            int MinY = 0;

            // Check for bounce 

            if (spritePosition.X > MaxX)
            {

                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }

            else if (spritePosition.X < MinX)
            {

                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {

                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }

            else if (spritePosition.Y < MinY)
            {

                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }
        }

        void CheckForCollision()
        {/*
            

            BoundingBox bb1 = new BoundingBox(new Vector3(obstaclepos1.X - (sprite1Width / 2), obstaclepos1.Y - (sprite1Height / 2), 0), new Vector3(obstaclepos1.X + (sprite1Width / 2), obstaclepos1.Y + (sprite1Height / 2), 0));

            BoundingBox bb2 = new BoundingBox(new Vector3(spritePosition2.X - (sprite2Width / 2), spritePosition2.Y - (sprite2Height / 2), 0), new Vector3(spritePosition2.X + (sprite2Width / 2), spritePosition2.Y + (sprite2Height / 2), 0));

            if (bb1.Intersects(bb2))
            {

                mario.Pause();
                chimes.Play();

            }
            */
     
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprite

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.Draw(texture1, spritePosition1, Color.White);
            spriteBatch.End();


     
            base.Draw(gameTime);
        }
    }
}
