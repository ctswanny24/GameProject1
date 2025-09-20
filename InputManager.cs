using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameProject1.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputExample
{

    public class InputManager
    {
        KeyboardState currentKeyboardState;
        KeyboardState priorKeyboardState;
        MouseState currentMouseState;
        MouseState priorMouseState;
        GamePadState currentGamePadState;
        GamePadState priorGamePadState;

        /// <summary>
        /// Property to return boolean if the player is crouched.
        /// </summary>
        public bool Crouched { get; private set; }

        /// <summary>
        /// Returns float based on if crouched or not. If crouched, player moves at 60% efficiency.
        /// </summary>
        private float CrouchPenalty
        {
            get
            {
                if (Crouched)
                {
                    return 0.6f;
                }
                else
                {
                    return 1.0f;
                }
            }
            set
            {
                this.CrouchPenalty = _crouchPenalty;
            }
        }
        private float _crouchPenalty;

        public CharacterStates State;

        /// <summary>
        /// The current direction
        /// </summary>
        public Vector2 Movement { get; private set; }

        public Direction Direction { get; private set; }

        /// <summary>
        /// If the user has requested to exit the game
        /// </summary>
        public bool Exit { get; private set; } = false;

        public void Update(GameTime gameTime)
        {
            #region State Updating
            priorKeyboardState = currentKeyboardState;
            priorMouseState = currentMouseState;
            priorGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(0);
            #endregion

            #region Direction Input

            //Get direction from gamepad
            Movement = currentGamePadState.ThumbSticks.Right * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(currentKeyboardState.IsKeyDown(Keys.C)|| currentKeyboardState.IsKeyDown(Keys.LeftControl))
            {
                Crouched = true;
                
            }
            else
            {
                Crouched = false;
            }

            //Get positon from keyboard
            if (currentKeyboardState.GetPressedKeyCount() == 0)
            {
                State = CharacterStates.Idle;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A))
            {
                Movement += new Vector2(-150 * (float)gameTime.ElapsedGameTime.TotalSeconds * CrouchPenalty, 0);
                if(Direction == Direction.Right)
                {
                    State = CharacterStates.Turning;
                }
                else
                {
                    State = CharacterStates.Running;
                }
                Direction = Direction.Left;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D))
            {
                Movement += new Vector2(150 * (float)gameTime.ElapsedGameTime.TotalSeconds * CrouchPenalty, 0);
                if(Direction == Direction.Left)
                {
                    State = CharacterStates.Turning;
                }
                else
                {
                    State = CharacterStates.Running;
                }
                Direction = Direction.Right;
            }
            //if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W))
            //{
            //    Movement += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds * CrouchPenalty);
            //    Direction = Direction.Up;
            //    State = CharacterStates.Running;
            //}
            //if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S))
            //{
            //    Movement += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds * CrouchPenalty);
            //    Direction = Direction.Down;
            //    State = CharacterStates.Running;
            //}
            if (currentKeyboardState.IsKeyDown(Keys.Space) && currentKeyboardState.GetPressedKeyCount() <= 1 && priorKeyboardState.IsKeyUp(Keys.Space))
            {
                State = CharacterStates.Attacking;
            }

            #endregion

            #region Exit Input
            if(currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit = true;
            }

            #endregion

        }
    }
}
