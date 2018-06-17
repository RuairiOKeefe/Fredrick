using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fredrick.src
{
	internal class Character : Component
	{
		private KeyboardState _currentKeyState;
		private KeyboardState _previousKeyState;

		public Character(Entity owner) : base(owner)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public override void Update(double deltaTime)
		{
			_currentKeyState = Keyboard.GetState();
			if (_currentKeyState.IsKeyDown(Keys.A))
				_owner.Move(new Vector2(-1, 0));
			if (_currentKeyState.IsKeyDown(Keys.D))
				_owner.Move(new Vector2(1, 0));
		}
	}
}
