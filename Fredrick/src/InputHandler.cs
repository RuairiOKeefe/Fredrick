using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fredrick.src
{
	public sealed class InputHandler
	{
		private static InputHandler instance = null;
		private static readonly object padlock = new object();

		public static InputHandler Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new InputHandler();
					}
					return instance;
				}
			}
		}

		public enum Action
		{
			Up,
			Down,
			Left,
			Right,
			Jump,
			Interact

		}

		private KeyboardState _currentKeyState;
		private KeyboardState _previousKeyState;

		private MouseState _currentMouseState;
		private MouseState _previousMouseState;

		private Point _currentMousePos;

		private Dictionary<Action, Keys> _keyBindings;

		private float _moveX;
		public float MoveX
		{
			get
			{
				return _moveX;
			}
		}

		InputHandler()
		{
			_currentKeyState = new KeyboardState();
			_previousKeyState = new KeyboardState();
			_keyBindings = new Dictionary<Action, Keys>();

			//Need to construct from file, using for testing currently
			_keyBindings.Add(Action.Up, Keys.W);
			_keyBindings.Add(Action.Down, Keys.S);
			_keyBindings.Add(Action.Left, Keys.A);
			_keyBindings.Add(Action.Right, Keys.D);
			_keyBindings.Add(Action.Jump, Keys.Space);
			_keyBindings.Add(Action.Interact, Keys.E);
		}

		public Keys GetKey(Action action)
		{
			return _keyBindings[action];
		}

		public void SetKey(Action action, Keys key)
		{
			_keyBindings[action] = key;
		}

		public bool IsKeyPressed(Action action)
		{
			Keys key = GetKey(action);
			if (_currentKeyState.IsKeyDown(key) && _previousKeyState.IsKeyUp(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsKeyHeld(Action action)
		{
			Keys key = GetKey(action);
			if (_currentKeyState.IsKeyDown(key) && _previousKeyState.IsKeyDown(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsKeyReleased(Action action)
		{
			Keys key = GetKey(action);
			if (_currentKeyState.IsKeyUp(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsLeftMousePressed()
		{
			if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsLeftMouseHeld()
		{
			if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsLeftMouseReleased()
		{
			if (_currentMouseState.LeftButton == ButtonState.Released)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void GetMoveX()
		{
			float move = 0;
			if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Left) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Left))
			{
				move--;
			}
			if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Right) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Right))
			{
				move++;
			}
			_moveX = move;
		}

		public void Update()
		{
			_previousKeyState = _currentKeyState;
			_currentKeyState = Keyboard.GetState();

			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState();

			_currentMousePos = _currentMouseState.Position;

			GetMoveX();
		}
	}
}
