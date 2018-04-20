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

        Texture2D tree;
        Texture2D minion;
        Texture2D background;
        Texture2D start;

        Vector2 minionPos;
        Vector2 treepos;
        Vector2 backgroundPos;
        Vector2 startPos;

        Vector2 minionSpeed = new Vector2(0.0f, 0.0f);
        Vector2 treeSpeed = new Vector2(0, 0);
        float spriteAngle1 = 0;

        int minionHeight;
        int minionWidth;

        KeyboardState keystate;
        KeyboardState prevkeystate;
        SoundEffect soundEffect;
        SoundEffect soundEffect2;
        SoundEffectInstance chimes;
        SoundEffectInstance mario;

        int minionStartPos_X, minionStartPos_Y;

        bool is_jumping;
        bool active;

        double timeAlive;
        double timeStart;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1080;
            is_jumping = false;
            active = false;
            minionStartPos_X = 100;
            minionStartPos_Y = 500;
            timeAlive = 0;
            timeStart = 0;
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

            minion = Content.Load<Texture2D>("minion");
            background = Content.Load<Texture2D>("road");

            tree = Content.Load<Texture2D>("tree");
            start = Content.Load<Texture2D>("start");


            soundEffect = Content.Load<SoundEffect>("NFF-cowbell-big");
            soundEffect2 = Content.Load<SoundEffect>("Super Mario Bros. medley");
            chimes = soundEffect.CreateInstance();
            mario = soundEffect2.CreateInstance();
            mario.IsLooped = true;
            mario.Play();

            minionPos.X = 100;
            minionPos.Y = 500;
            backgroundPos.X = 0;
            backgroundPos.Y = 0;

            startPos.Y = 360;
            startPos.X = 540;

            treepos.X = 1080;
            treepos.Y = 500 - (tree.Height - minion.Height);



            minionHeight = minion.Bounds.Height;
            minionWidth = minion.Bounds.Width;

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

            keystate = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Move the sprite around

            //UpdateSprite(gameTime, ref obstaclepos1, ref spriteSpeed1);
            //CheckForCollision();

            if(!active)
            {
                if(keystate.IsKeyDown(Keys.S))
                {
                    active = true;
                    treeSpeed = new Vector2(-300f, 0);
                    timeStart = gameTime.ElapsedGameTime.TotalSeconds;
                    timeAlive = 0;


                }
                else
                {
                    return;
                }
            }

            timeAlive += gameTime.ElapsedGameTime.TotalSeconds;
            if (chimes.State != SoundState.Playing && mario.State == SoundState.Paused)
            {
                mario.Play();
            }

            if(!is_jumping)
            {

                if (keystate.IsKeyDown(Keys.Space))
                    do_jump(15f);


            }
            else
            {
                minionSpeed.Y += 0.5f;
                minionPos += minionSpeed;
                if (minionPos.Y >= 500)
                {
                    is_jumping = false;
                }
                
            }
            treepos += treeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            prevkeystate = keystate;
            CheckForCollision();
            base.Update(gameTime);
        }


        private void do_jump(float speed)
        {
            is_jumping = true;

            minionSpeed = new Vector2(0.0f, -speed);

        }
        void UpdateSprite(GameTime gameTime, ref Vector2 spritePosition, ref Vector2 spriteSpeed)
        {

            // Move the sprite by speed, scaled by elapsed time 

            spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteAngle1 += 0.01f;
            int MaxX = graphics.GraphicsDevice.Viewport.Width - tree.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - tree.Height;
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
        {

            int offset = 10;
            BoundingBox bb1 = new BoundingBox(new Vector3(treepos.X - (tree.Width / 2-offset), treepos.Y - (tree.Height / 2 - offset), 0), new Vector3(treepos.X + (tree.Width / 2 - offset), 
                treepos.Y + (tree.Height / 2 - offset), 0));

            BoundingBox bb2 = new BoundingBox(new Vector3(minionPos.X - (minionWidth / 2), minionPos.Y - (minionHeight / 2), 0), new Vector3(minionPos.X + (minionWidth / 2), minionPos.Y + (minionHeight / 2), 0));

            if (bb1.Intersects(bb2))
            {

                mario.Pause();
                chimes.Play();
                Reset();

            }
            
     
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

            spriteBatch.Draw(background, backgroundPos, Color.White);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(tree, treepos, Color.White);
            spriteBatch.Draw(minion, minionPos, Color.White);
            spriteBatch.End();
            
            if (!active)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(start, startPos, Color.White);
                spriteBatch.End();
            }

                SpriteFont font;
                font = Content.Load < SpriteFont>("Time");
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.DrawString(font, timeAlive.ToString("0.##"), new Vector2(1000, 50), Color.Black);
                spriteBatch.End();
            

     
            base.Draw(gameTime);
        }

        private void Reset()
        {
            treeSpeed = new Vector2(0, 0);
            treepos.X = 1080;
            treepos.Y = 500 - (tree.Height - minion.Height);
            active = false;
            minionPos.X = minionStartPos_X;
            minionPos.Y = minionStartPos_Y;
            
        }
    }
}
