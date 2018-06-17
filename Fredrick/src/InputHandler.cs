using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Fredrick.src
{
	public class InputHandler
	{
		enum Action
		{
			Up,
			Down,
			Left,
			Right,
			Jump,
			Fire,
			Interact

		}

		private Dictionary<Action, Keys> _keyBindings;
		

		InputHandler()
		{
			_keyBindings.Add(Action.Up, Keys.W);
			_keyBindings.Add(Action.Down, Keys.S);
			_keyBindings.Add(Action.Left, Keys.A);
			_keyBindings.Add(Action.Right, Keys.D);
			_keyBindings.Add(Action.Jump, Keys.Space);
			_keyBindings.Add(Action.Fire, Keys.LeftShift);
			_keyBindings.Add(Action.Interact, Keys.E);

		}
	}
}
