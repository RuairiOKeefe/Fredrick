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

		private AABBTrigger _jumpTrigger;
		private bool _grounded;
		private bool _prevGrounded;
		private double _fallVelocity;

		private int _maxJumps;
		private int _jumpsLeft;
		private bool _jumpWait;
		private double _jumpTime;//How much time between jumps
		private double _jumpTimer;

		private Vector2 _followPosition;
		private Vector2 _followOffset;

		public bool Grounded
		{
			get { return _grounded; }
		}

		public bool PrevGrounded
		{
			get { return _prevGrounded; }
		}

		public double JumpTimer
		{
			get { return _jumpTimer; }
		}

		public double FallVelocity
		{
			get { return _fallVelocity; }
		}

		public Vector2 FollowPosition
		{
			get { return _followPosition; }
		}

		public Character(Entity owner) : base(owner)
		{
			_velocity = new Vector2(0, 0);
			_acceleration = new Vector2(0, 0);
			_horAcc = 30;
			_maxSpeed = 5;
			_acceleration.Y = -9.8f;

			_friction = 100;

			_motionState = State.Standing;
			_maxSprintSpeed = 10;

			_jumpTrigger = new AABBTrigger(_owner);
			_jumpTrigger.Rectangle = new RectangleF(new Vector2(0, -0.5f), 1, 0.5f, 0, 0);
			_maxJumps = 2;
			_jumpWait = false;
			_jumpTime = 0.2;//may want to remove variable?

			_followOffset = new Vector2(5, 0);

		}

		public void Walk(double deltaTime)
		{
			if (_moveCommand != 0)
			{
				_acceleration.X = _horAcc * _moveCommand;
				_friction = 100;

				if (!_sprintCommand)
					if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSpeed))
					{
						_acceleration.X = 0;
					}
					else
						if ((_velocity.X * _velocity.X) > (_maxSprintSpeed * _maxSprintSpeed))
					{
						_acceleration.X = 0;
					}
			}
			else
			{
				_acceleration.X = 0;
				_friction = 600;
			}

			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump))
			{
				if (_grounded)
				{
					_velocity.Y = 10;
					_jumpWait = true;
				}
				else
					if (_jumpsLeft > 0)
				{
					_velocity.Y = 10;
					_jumpsLeft--;
					_jumpWait = true;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 tl = new Vector2(_jumpTrigger.Rectangle.GetPosition().X - _jumpTrigger.Rectangle.Width / 2, _jumpTrigger.Rectangle.GetPosition().Y + _jumpTrigger.Rectangle.Height / 2);
			Vector2 tr = new Vector2(_jumpTrigger.Rectangle.GetPosition().X + _jumpTrigger.Rectangle.Width / 2, _jumpTrigger.Rectangle.GetPosition().Y + _jumpTrigger.Rectangle.Height / 2);
			Vector2 bl = new Vector2(_jumpTrigger.Rectangle.GetPosition().X - _jumpTrigger.Rectangle.Width / 2, _jumpTrigger.Rectangle.GetPosition().Y - _jumpTrigger.Rectangle.Height / 2);
			Vector2 br = new Vector2(_jumpTrigger.Rectangle.GetPosition().X + _jumpTrigger.Rectangle.Width / 2, _jumpTrigger.Rectangle.GetPosition().Y - _jumpTrigger.Rectangle.Height / 2);
			DebugManager.Instance.DrawLine(spriteBatch, tl + _owner.GetPosition(), tr + _owner.GetPosition());
			DebugManager.Instance.DrawLine(spriteBatch, tr + _owner.GetPosition(), br + _owner.GetPosition());
			DebugManager.Instance.DrawLine(spriteBatch, br + _owner.GetPosition(), bl + _owner.GetPosition());
			DebugManager.Instance.DrawLine(spriteBatch, bl + _owner.GetPosition(), tl + _owner.GetPosition());
		}

		public override void Update(double deltaTime)
		{
			_moveCommand = InputHandler.Instance.MoveX;
			_jumpCommand = InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump);
			_sprintCommand = InputHandler.Instance.IsKeyHeld(InputHandler.Action.Sprint);

			if (_jumpWait)
			{
				_jumpTimer += deltaTime;
				if (_jumpTimer >= _jumpTime)
				{
					_jumpWait = false;
					_jumpTimer = 0;
				}
			}

			_prevGrounded = _grounded;

			if (_jumpTrigger.Update(_owner.GetPosition()) && !_jumpWait)
			{
				_grounded = true;
				_jumpsLeft = _maxJumps;
			}
			else
				_grounded = false;

			if (!_grounded)
			{
				_fallVelocity = Velocity.Y;
			}
			else
			{
				if(_prevGrounded)//Gives a frame to check fall trauma
					_fallVelocity = 0;
			}

			switch (_motionState)
			{
				case State.Standing:
					if (_moveCommand != 0)
						_motionState = State.Walking;
					if (!_grounded || _jumpWait)
						_motionState = State.Jumping;
					break;
				case State.Walking:
					if (_sprintCommand)
						_motionState = State.Sprinting;
					if (_moveCommand == 0)
						_motionState = State.Standing;
					if (!_grounded || _jumpWait)
						_motionState = State.Jumping;
					break;
				case State.Sprinting:
					if (!_sprintCommand)
						_motionState = State.Walking;
					if (_moveCommand == 0)
						_motionState = State.Standing;
					if (!_grounded || _jumpWait)
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

					break;
				case State.Walking:

					break;
				case State.Sprinting:

					break;
				case State.Jumping:

					break;
				default:
					throw new Exception("Unrecognised state reached");
			}

			Walk(deltaTime);
			ResolveMotion(deltaTime);

			if (_moveCommand > 0)
			{
				_followPosition = _owner.GetPosition() + _followOffset;
			}
			else
				if (_moveCommand < 0)
			{
				_followPosition = _owner.GetPosition() - _followOffset;
			}
			else
			{
				_followPosition = _owner.GetPosition();
			}
		}
	}
}
