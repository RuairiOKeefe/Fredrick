using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fredrick.src.Rigging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fredrick.src
{
	[Serializable]
	public class Character : Movable
	{

		private MovementStateMachine m_movementState;
		public MovementStateMachine.Action MovementState { get { return m_movementState.CurrentAction; } }//temp for debugging

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
		public float FallVelocity { get; set; }

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

			JumpTrigger = new AABBTrigger(owner);
			JumpTrigger.Rectangle = new RectangleF(new Vector2(0, -1.0f), 1, 0.5f);

			JumpDuration = 0.3f;
			_jumpClock = 0;
			JumpSpeed = 14.0f;
			FallAcceleration = -40.0f;
			TerminalVelocity = -30.0f;
			MaxJumps = 1;
			JumpWait = false;
			JumpDelay = 0.1;//may want to remove variable?

			FollowOffset = new Vector2(5, 0);

			m_movementState = new MovementStateMachine(this, MovementStateMachine.Action.Standing);
		}

		public Character(MovementStateMachine movementState, Vector2 prevAcceleration, float horAcc, float maxSpeed, float groundFriction, float airFriction, float movingFriction, float airMove, AABBTrigger jumpTrigger, bool grounded, bool prevGrounded, double jumpDuration, float jumpSpeed, float fallAcceleration, float terminalVelocity, int maxJumps, double jumpDelay)
		{
			Velocity = new Vector2(0, 0);
			Acceleration = new Vector2(0, 0);
			m_movementState = movementState;
			PrevAcceleration = prevAcceleration;
			HorAcc = horAcc;
			MaxSpeed = maxSpeed;

			GroundFriction = groundFriction;
			AirFriction = airFriction;
			MovingFriction = movingFriction;
			AirMove = airMove;

			JumpTrigger = new AABBTrigger(_owner, jumpTrigger);

			Grounded = grounded;
			PrevGrounded = prevGrounded;
			FallVelocity = 0f;
			JumpDuration = jumpDuration;
			_jumpClock = 0;
			JumpSpeed = jumpSpeed;
			FallAcceleration = fallAcceleration;
			TerminalVelocity = terminalVelocity;
			MaxJumps = maxJumps;
			JumpWait = false;
			JumpDelay = jumpDelay;
		}

		public Character(Entity owner, Character original) : base(owner, original)
		{
			m_movementState = new MovementStateMachine(this);
			PrevAcceleration = original.PrevAcceleration;
			HorAcc = original.HorAcc;
			MaxSpeed = original.MaxSpeed;

			GroundFriction = original.GroundFriction;
			AirFriction = original.AirFriction;
			MovingFriction = original.MovingFriction;
			AirMove = original.AirMove;

			JumpTrigger = new AABBTrigger(owner, original.JumpTrigger);

			Grounded = false;
			PrevGrounded = original.PrevGrounded;
			FallVelocity = original.FallVelocity;
			JumpDuration = original.JumpDuration;
			_jumpClock = 0;
			JumpSpeed = original.JumpSpeed;
			FallAcceleration = original.FallAcceleration;
			TerminalVelocity = original.TerminalVelocity;
			MaxJumps = original.MaxJumps;
			JumpWait = false;
			JumpDelay = original.JumpDelay;
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
					foreach (Component c in _owner.Components)
					{
						if (c is Emitter && c.Tags.Contains("Jumping"))
						{
							Emitter e = c as Emitter;
							e.IsEmitting = true;
						}
					}
				}
				else
					if (_jumpsLeft > 0)
				{
					Velocity = new Vector2(Velocity.X, JumpSpeed);
					_jumpsLeft--;
					JumpWait = true;
					_jumpClock = JumpDuration;
					Acceleration = new Vector2(Acceleration.X, 0);
					foreach (Component c in _owner.Components)
					{
						if (c is Emitter && c.Tags.Contains("Jumping"))
						{
							Emitter e = c as Emitter;
							e.IsEmitting = true;
						}
					}
				}
			}

			if (Acceleration.Y == 0 && _jumpClock < 0 || Velocity.Y == 0)
			{
				Acceleration = new Vector2(Acceleration.X, FallAcceleration);
			}

			if (Velocity.Y < TerminalVelocity)
				Velocity = new Vector2(Velocity.X, TerminalVelocity);
		}

		private void TryLanding()
		{
			PrevGrounded = Grounded;

			if (JumpTrigger.Update(Owner.Position) && !JumpWait)
			{
				Grounded = true;
				_jumpsLeft = MaxJumps;
			}
			else
			{
				Grounded = false;
			}

			if (!Grounded)
			{
				FallVelocity = Velocity.Y;
			}
			else
			{
				if (!PrevGrounded)
				{
					if (FallVelocity < -0.5f)
					{
						//Emit Particles
						foreach (Component c in _owner.Components)
						{
							if (c is Emitter && c.Tags.Contains("Landing"))
							{
								Emitter e = c as Emitter;
								e.Emit();
							}
						}
						float trauma = -((FallVelocity + 0.5f) / 40.0f);
						if (trauma > 0.6f)
							trauma = 0.6f;
						ScreenShakeManager.Instance.AddTrauma(trauma);
						FallVelocity = 0;
					}

				}
			}

		}

		public override void Load(ContentManager content)
		{
			JumpTrigger.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			if (Owner.GetDerivedComponent<Controller>() != null)
			{
				MoveCommand = Owner.GetDerivedComponent<Controller>().Movement;
				JumpCommand = Owner.GetDerivedComponent<Controller>().Jump;
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
					foreach (Component c in _owner.Components)
					{
						if (c is Emitter && c.Tags.Contains("Jumping"))
						{
							Emitter e = c as Emitter;
							e.IsEmitting = false;
						}
					}
				}
			}

			_jumpClock -= deltaTime;

			TryLanding();

			Walk(deltaTime);
			ResolveMotion(deltaTime);

			if (MoveCommand > 0)
			{
				FollowPosition = Owner.Position + FollowOffset;
			}
			else
				if (MoveCommand < 0)
			{
				FollowPosition = Owner.Position - FollowOffset;
			}
			else
			{
				FollowPosition = Owner.Position;
			}

			if (MoveCommand > 0)
				_facingRight = true;
			if (MoveCommand < 0)
				_facingRight = false;

			foreach (Component c in Owner.Components)
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
			m_movementState.Update();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			JumpTrigger.DebugDraw(spriteBatch);
		}

		public override Component Copy(Entity owner)
		{
			return new Character(owner, this);
		}
	}
}
