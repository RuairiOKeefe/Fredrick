using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
			Interact,
			Sprint,
			Debug

		}

		private KeyboardState _currentKeyState;
		private KeyboardState _previousKeyState;

		private MouseState _currentMouseState;
		private MouseState _previousMouseState;

		private Point _screenMousePosition;
		private Vector2 _worldMousePosition;

		private Dictionary<Action, Keys> _keyBindings;

		private float _moveX;

		public Vector2 WorldMousePosition
		{
			get { return _worldMousePosition; }
		}

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
			_keyBindings.Add(Action.Sprint, Keys.LeftShift);
			_keyBindings.Add(Action.Debug, Keys.Z);
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

		public Point GetMouseScreenPos()
		{
			return _screenMousePosition;
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

		public void Update(Matrix viewMatrix)
		{
			_previousKeyState = _currentKeyState;
			_currentKeyState = Keyboard.GetState();

			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState();

			_screenMousePosition = _currentMouseState.Position;

			Vector2 mousePosition = new Vector2(_screenMousePosition.X, _screenMousePosition.Y);
			_worldMousePosition = Vector2.Transform(mousePosition, Matrix.Invert(viewMatrix));
			_worldMousePosition.X = _worldMousePosition.X / 32;
			_worldMousePosition.Y = -_worldMousePosition.Y / 32;
			GetMoveX();
		}
	}
}
