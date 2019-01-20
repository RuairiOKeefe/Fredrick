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
	[Serializable]
	public class Character : Movable
	{

		public enum State
		{
			Standing,
			Walking,
			Jumping
		}

		public State MotionState { get; set; }

		public float MoveCommand { get; set; }
		public bool JumpCommand { get; set; }

		public Vector2 PrevAcceleration { get; set; }
		public float GroundFriction { get; set; }
		public float AirFriction { get; set; }
		public float MovingFriction { get; set; }
		public float AirMove { get; set; }

		public AABBTrigger JumpTrigger { get; set; }
		public bool Grounded { get; set; }
		public bool PrevGrounded { get; set; }
		public double FallVelocity { get; set; }

		public double JumpDuration { get; set; }
		private double _jumpClock;
		public float JumpSpeed { get; set; }
		public float FallAcceleration { get; set; }
		public float TerminalVelocity { get; set; }
		public int MaxJumps { get; set; }
		private int _jumpsLeft;
		public bool JumpWait { get; set; }
		public double JumpDelay { get; set; }//How much time between jumps
		private double _jumpTimer;

		public Vector2 FollowPosition;
		public Vector2 FollowOffset;

		private bool _facingRight;

		public double JumpTimer
		{
			get { return _jumpTimer; }
		}

		public Character(Entity owner) : base(owner)
		{
			Velocity = new Vector2(0, 0);
			Acceleration = new Vector2(0, 0);
			HorAcc = 16;
			MaxSpeed = 8;

			GroundFriction = 1000;
			AirFriction = 40;
			MovingFriction = 100;
			AirMove = 0.4f;

			MotionState = State.Standing;

			JumpTrigger = new AABBTrigger(_owner);
			JumpTrigger.Rectangle = new RectangleF(new Vector2(0, -1.0f), 1, 0.5f);

			JumpDuration = 0.3f;
			_jumpClock = 0;
			JumpSpeed = 14.0f;
			FallAcceleration = -40.0f;
			TerminalVelocity = -30.0f;
			MaxJumps = 2;
			JumpWait = false;
			JumpDelay = 0.2;//may want to remove variable?

			FollowOffset = new Vector2(5, 0);

		}

		public void Walk(double deltaTime)
		{
			PrevAcceleration = Acceleration;
			if (Grounded)
			{
				if (MoveCommand != 0)
				{
					Acceleration = new Vector2(HorAcc * MoveCommand, Acceleration.Y);
					_friction = MovingFriction;

					if ((Velocity.X * Velocity.X) > (MaxSpeed * MaxSpeed) && Velocity.X * MoveCommand > 0)
					{
						Velocity = new Vector2(MaxSpeed * Math.Sign(MoveCommand), Velocity.Y);
						Acceleration = new Vector2(0, Acceleration.Y);
					}
				}
				else
				{
					Acceleration = new Vector2(0, Acceleration.Y);
					_friction = GroundFriction;
				}
			}
			else
			{
				_friction = AirFriction;

				if (MoveCommand != 0)
				{
					Acceleration = new Vector2(HorAcc * MoveCommand * AirMove, 0);


				}
				if ((Velocity.X * Velocity.X) > (MaxSpeed * MaxSpeed) && Velocity.X * MoveCommand > 0)
				{
					Velocity = new Vector2(MaxSpeed * Math.Sign(MoveCommand), Velocity.Y);
					Acceleration = new Vector2(0, Acceleration.Y);
				}
			}

			if (JumpCommand)
			{
				if (Grounded)
				{
					Velocity = new Vector2(Velocity.X, JumpSpeed);
					JumpWait = true;
					_jumpClock = JumpDuration;
					Acceleration = new Vector2(Acceleration.X, 0);
				}
				else
					if (_jumpsLeft > 0)
				{
					Velocity = new Vector2(Velocity.X, JumpSpeed);
					_jumpsLeft--;
					JumpWait = true;
					_jumpClock = JumpDuration;
					Acceleration = new Vector2(Acceleration.X, 0);
				}
			}

			if (Acceleration.Y == 0 && _jumpClock < 0 || Velocity.Y == 0)
			{
				Acceleration = new Vector2(Acceleration.X, FallAcceleration);
			}

			if (Velocity.Y < TerminalVelocity)
				Velocity = new Vector2(Velocity.X, TerminalVelocity);
		}

		public void StateHandler()
		{
			switch (MotionState)
			{
				case State.Standing:
					if (MoveCommand != 0)
						MotionState = State.Walking;
					if (!Grounded)
						MotionState = State.Jumping;
					break;
				case State.Walking:
					if (MoveCommand == 0)
						MotionState = State.Standing;
					if (!Grounded)
						MotionState = State.Jumping;
					break;
				case State.Jumping:
					if (Grounded)
						MotionState = State.Walking;//Need proper handler for landing
					break;
				default:
					throw new Exception("Unrecognised state reached");
			}

			switch (MotionState)
			{
				case State.Standing:
					foreach (Component c in _owner.Components)
					{
						if (c.Tags.Contains("Legs"))
						{
							if (c is Renderable)
							{
								(c as Renderable).Drawable.TransitionAnim(0);
							}
						}
					}
					break;
				case State.Walking:
					foreach (Component c in _owner.Components)
					{
						if (c.Tags.Contains("Legs"))
						{
							if (c is Renderable)
							{
								(c as Renderable).Drawable.TransitionAnim(1);//Add case to stop animation when contacting a wall
							}
						}
						if (Velocity.X > 3)
						{

						}
					}
					break;
				case State.Jumping:
					foreach (Component c in _owner.Components)
					{
						if (c.Tags.Contains("Legs"))
						{
							if (c is Renderable)
							{
								if (JumpCommand || !((c as Renderable).Drawable._currentAnim == 2 || ((c as Renderable).Drawable._currentAnim == 3) && (Velocity.Y < 0)))
								{
									(c as Renderable).Drawable.TransitionAnim(2);
								}
								if (Velocity.Y < 0)
								{
									(c as Renderable).Drawable.TransitionAnim(3);
								}
							}
						}
					}
					break;
				default:
					throw new Exception("Unrecognised state reached");
			}
		}

		public override void Update(double deltaTime)
		{
			if (_owner.GetDerivedComponent<Controller>() != null)
			{
				MoveCommand = _owner.GetDerivedComponent<Controller>().Movement;
				JumpCommand = _owner.GetDerivedComponent<Controller>().Jump;
			}
			else
			{
				MoveCommand = 0;
				JumpCommand = false;
			}

			if (JumpWait)
			{
				_jumpTimer += deltaTime;
				if (_jumpTimer >= JumpDelay)
				{
					JumpWait = false;
					_jumpTimer = 0;
				}
			}

			_jumpClock -= deltaTime;
			PrevGrounded = Grounded;

			if (JumpTrigger.Update(_owner.Position) && !JumpWait)
			{
				Grounded = true;
				_jumpsLeft = MaxJumps;
			}
			else
				Grounded = false;

			if (!Grounded)
			{
				FallVelocity = Velocity.Y;
			}
			else
			{
				if (PrevGrounded)//Gives a frame to check fall trauma
					FallVelocity = 0;
			}

			StateHandler();

			Walk(deltaTime);
			ResolveMotion(deltaTime);

			if (MoveCommand > 0)
			{
				FollowPosition = _owner.Position + FollowOffset;
			}
			else
				if (MoveCommand < 0)
			{
				FollowPosition = _owner.Position - FollowOffset;
			}
			else
			{
				FollowPosition = _owner.Position;
			}

			if (MoveCommand > 0)
				_facingRight = true;
			if (MoveCommand < 0)
				_facingRight = false;

			foreach (Component c in _owner.Components)
			{
				if (c.Tags.Contains("MotionFlip"))
				{
					if (c is Renderable)
					{
						(c as Renderable).Flip(_facingRight);
					}
					if (c is CharacterRig)
					{
						(c as CharacterRig).MotionFlip = !_facingRight;
					}
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			JumpTrigger.DebugDraw(spriteBatch);
		}
	}
}
