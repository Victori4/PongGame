using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Models.Sprites
{
    public class Ball : Sprite
    {
        private float _timer = 0f; // used for incrementing speed, longer game, faster ball
        private Vector2? _startPosition = null; // question mark means this is nullable
        private float? _startSpeed;
        private bool _isPlaying;

        public Score Score;
        public int SpeedIncrementSpan = 10; // how often speed will increment (every 10 secs)

        public Ball(Texture2D texture) : base(texture)
        {
            Speed = 3f;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (_startPosition == null)
            {
                _startPosition = Position;
                _startSpeed = Speed;
                Restart();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _isPlaying = true;
            }
            if (!_isPlaying)
            {
                return;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > SpeedIncrementSpan)
            {
                Speed++;
                _timer = 0;
            }

            foreach (var sprite in sprites)
            {
                if(sprite == this)
                {
                    continue;
                }
               
                // hit sprite and bounce in upward direction (if it hits bottom or the top, it will bounce back)
                if (this.Velocity.X > 0 && this.IsTouchingLeft(sprite))
                {
                    this.Velocity.X = -this.Velocity.X;
                }

                if (this.Velocity.X < 0 && this.IsTouchingRight(sprite))
                {
                    this.Velocity.X = -this.Velocity.X;
                }

                if (this.Velocity.Y > 0 && this.IsTouchingTop(sprite))
                {
                    this.Velocity.Y = -this.Velocity.Y;
                }

                if (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite))
                {
                    this.Velocity.Y = -this.Velocity.Y;
                }
            }

            if (Position.Y <= 0 || Position.Y + _texture.Height >= Game1.ScreenHeight)
            {
                Velocity.Y = -Velocity.Y;
            }
            
            if (Position.X <= 0)
            {
                Score.Score2++;
                Restart();
            }

            if (Position.X + _texture.Width >= Game1.ScreenWidth) 
            {
                Score.Score1++;
                Restart();
            }

            Position += Velocity * Speed;
        }

        public void Restart()
        {
            var direction = Game1.Random.Next(0, 4);

            // 4 diagnols
            switch (direction)
            {
                case 0:
                    Velocity = new Vector2(1, 1);
                    break;

                case 1:
                    Velocity = new Vector2(1, -1);
                    break;

                case 2:
                    Velocity = new Vector2(-1, -1);
                    break;

                case 3:
                    Velocity = new Vector2(-1, 1);
                    break;
            }

            Position = (Vector2)_startPosition; // bc it is nullable, you have to cast it
            Speed = (float)_startSpeed;
            _timer = 0;
            _isPlaying = false;
        }
    }

}

