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

		private float _groundFriction;
		private float _airFriction;
		private float _movingFriction;
		private float _airMoveSlow;
		private float _airMoveFast;
		private bool _fastJump;

		private AABBTrigger _jumpTrigger;
		private bool _grounded;
		private bool _prevGrounded;
		private double _fallVelocity;


		private double _jumpDuration;
		private double _jumpClock;
		private float _jumpSpeed;
		private float _fallAcceleration;
		private float _terminalVelocity;
		private int _maxJumps;
		private int _jumpsLeft;
		private bool _jumpWait;
		private double _jumpDelay;//How much time between jumps
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
			_horAcc = 20;
			_maxSpeed = 8;
			_acceleration.Y = -9.8f;

			_groundFriction = 600;
			_airFriction = 10;
			_movingFriction = 100;
			_airMoveSlow = 0.1f;
			_airMoveFast = 0.5f;

			_motionState = State.Standing;

			_jumpTrigger = new AABBTrigger(_owner);
			_jumpTrigger.Rectangle = new RectangleF(new Vector2(0, -0.5f), 1, 0.5f, 0, 0);

			_jumpDuration = 0.2f;
			_jumpClock = 0;
			_jumpSpeed = 20.0f;
			_fallAcceleration = -40.0f;
			_terminalVelocity = -30.0f;
			_maxJumps = 2;
			_jumpWait = false;
			_jumpDelay = 0.2;//may want to remove variable?

			_followOffset = new Vector2(5, 0);

		}

		public void Walk(double deltaTime)
		{
			if (_grounded)
			{
				if ((_velocity.X * _velocity.X) / (_maxSpeed * _maxSpeed) > 0.5f)
					_fastJump = true;
				if ((_velocity.X * _velocity.X) / (_maxSpeed * _maxSpeed) < 0.2f)
					_fastJump = false;

				if (_moveCommand != 0)
				{
					_acceleration.X = _horAcc * _moveCommand;
					_friction = _movingFriction;

					if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSpeed) && _velocity.X * _moveCommand > 0)
					{
						_acceleration.X = 0;
					}
				}
				else
				{
					_acceleration.X = 0;
					_friction = _groundFriction;
				}
			}
			else
			{
				_friction = _airFriction;

				if (_moveCommand != 0)
				{
					if (_fastJump)
						_acceleration.X = _horAcc * _moveCommand * _airMoveFast;
					else
						_acceleration.X = _horAcc * _moveCommand * _airMoveSlow;

					if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSpeed) && _velocity.X * _moveCommand > 0)
					{
						_acceleration.X = 0;
					}
				}
			}

			if (_jumpCommand)
			{
				if (_grounded)
				{
					_velocity.Y = _jumpSpeed;
					_jumpWait = true;
					_jumpClock = _jumpDuration;
					_acceleration.Y = 0;
				}
				else
					if (_jumpsLeft > 0)
				{
					_velocity.Y = _jumpSpeed;
					_jumpsLeft--;
					_jumpWait = true;
					_jumpClock = _jumpDuration;
					_acceleration.Y = 0;
				}
			}

			if (_acceleration.Y == 0 && _jumpClock < 0)
			{
				_acceleration.Y = _fallAcceleration;
			}

			if (_velocity.Y < _terminalVelocity)
				_velocity.Y = _terminalVelocity;
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

			if (_jumpWait)
			{
				_jumpTimer += deltaTime;
				if (_jumpTimer >= _jumpDelay)
				{
					_jumpWait = false;
					_jumpTimer = 0;
				}
			}

			_jumpClock -= deltaTime;
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
				if (_prevGrounded)//Gives a frame to check fall trauma
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
