using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Models;
using Pong.Models.Sprites;
using System;
using System.Collections.Generic;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int ScreenWidth;
        public static int ScreenHeight;
        public static Random Random;

        private Score _score;
        private List<Sprite> _sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            Random = new Random();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var batTexture = Content.Load<Texture2D>("paddle");
            var ballTexture = Content.Load<Texture2D>("ball");
            var tableTexture = Content.Load<Texture2D>("table");

            _score = new Score(Content.Load<SpriteFont>("Font"));



            _sprites = new List<Sprite>()
            {
                new Sprite(tableTexture),
                new Bat(batTexture)
                {
                    Position = new Vector2 (20, (ScreenHeight /2) - (batTexture.Height /2)),
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,

                    }
                },
             new Bat(batTexture)
             {
                 Position = new Vector2(ScreenWidth - 20 - batTexture.Width, (ScreenHeight / 2) - (batTexture.Height / 2)),
                 Input = new Input()
                 {
                     Up = Keys.Up,
                     Down = Keys.Down,
                 }
             },
             new Ball(ballTexture)
             {
                 Position = new Vector2((ScreenWidth / 2) - (ballTexture.Width/2), (ScreenHeight/2) - (ballTexture.Height/2)),
                 Score = _score,
             }
         };

    }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime, _sprites);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);

                _score.Draw(_spriteBatch);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
