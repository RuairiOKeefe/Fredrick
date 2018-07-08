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
	public class Character : Movable
	{

		public enum State
		{
			Standing,
			Walking,
			Sprinting,
			Jumping
		}

		private State _motionState;
		public State MotionState
		{
			get { return _motionState; }
			set { _motionState = value; }
		}

		//Commands are simply what the related controller wants to do at a given instance, in case of player this represents button inputs
		protected float _moveCommand;//Horizontal movement command
		protected bool _jumpCommand;//Jumping command
		protected bool _sprintCommand;

		private float _maxSprintSpeed;

		public Character(Entity owner) : base(owner)
		{
			_velocity = new Vector2(0, 0);
			_acceleration = new Vector2(0, 0);
			_horAcc = 30;
			_maxSpeed = 5;
			_acceleration.Y = -9.8f;

			_friction = 100;

			_motionState = State.Standing;
			_maxSprintSpeed = 20;
		}

		public void Walk(double deltaTime)
		{
			if (_moveCommand != 0)
			{
				_acceleration.X = _horAcc * _moveCommand;
				_friction = 100;
				if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSpeed))
				{
					_acceleration.X = 0;
				}
			}
			else
			{
				_acceleration.X = 0;
				_friction = 600;
			}


		}

		public void Sprint(double deltaTime)
		{
			if (_moveCommand != 0)
			{
				_acceleration.X = _horAcc * _moveCommand*10;
				_friction = 100;
				if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSprintSpeed))
				{
					_acceleration.X = 0;
				}
			}
			else
			{
				_acceleration.X = 0;
				_friction = 600;
			}
		}

		public void Jump(double deltaTime)
		{

			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump))
			{
				_velocity.Y = 10;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public override void Update(double deltaTime)
		{
			_moveCommand = InputHandler.Instance.MoveX;
			_jumpCommand = InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump);
			_sprintCommand = InputHandler.Instance.IsKeyHeld(InputHandler.Action.Sprint);

			switch (_motionState)//This should be managed by a "brain" component, which will tell all "body parts" what state they are in
			{
				case State.Standing:
					if (_moveCommand != 0)
						_motionState = State.Walking;
					if (_jumpCommand)
						_motionState = State.Jumping;
					break;
				case State.Walking:
					if (_sprintCommand)
						_motionState = State.Sprinting;
					if (_moveCommand == 0)
						_motionState = State.Standing;
					if (_jumpCommand)
						_motionState = State.Jumping;
					break;
				case State.Sprinting:
					if (!_sprintCommand)
						_motionState = State.Walking;
					if (_moveCommand == 0)
						_motionState = State.Standing;
					if (_jumpCommand)
						_motionState = State.Jumping;
					break;
				case State.Jumping:
					if (!_jumpCommand)
						_motionState = State.Walking;//Need proper handler for landing
					break;
				default:
					throw new Exception("Unrecognised state reached");
			}

			switch (_motionState)
			{
				case State.Standing:
					//Twiddle thumbs?
					break;
				case State.Walking:
					Walk(deltaTime);
					break;
				case State.Sprinting:
					Sprint(deltaTime);
					break;
				case State.Jumping:
					Jump(deltaTime);
					break;
				default:
					throw new Exception("Unrecognised state reached");
			}

			//Walk(deltaTime);
			ResolveMotion(deltaTime);
		}
	}
}
