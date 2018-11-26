using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class Movable : Component
	{
		//If this component is used with a collider of any kind, this component should be added to the entity before the collider or invalid moves will be possible

		/// <summary>
		/// The location the entity wants to move to
		/// </summary>
		public Vector2 AttemptedPosition { get; set; }
		public Vector2 Velocity { get; set; }
		public Vector2 Acceleration { get; set; }
		public float HorAcc { get; set; }
		public float MaxSpeed { get; set; }

		protected float _friction;

		public Movable(Entity owner, string id = null) : base(owner, id)
		{

		}

		public void StopVelX()
		{
			Velocity = new Vector2(0.0f, Velocity.Y);
		}

		public void StopVelY()
		{
			Velocity = new Vector2(Velocity.X, 0.0f);
		}

		public void ResolveMotion(double deltaTime)
		{
			float tempAccX = Acceleration.X - (_friction * Velocity.X * (float)deltaTime);

			if (Velocity.X > 0)
			{
				if (Velocity.X + tempAccX * (float)deltaTime < 0)
					Velocity = new Vector2(0.0f, Velocity.Y);
				else
					Velocity = new Vector2(Velocity.X + (tempAccX * (float)deltaTime), Velocity.Y);
			}
			else
			{
				if (Velocity.X < 0)
					if (Velocity.X + tempAccX * (float)deltaTime > 0)
						Velocity = new Vector2(0.0f, Velocity.Y);
					else
						Velocity = new Vector2(Velocity.X + (tempAccX * (float)deltaTime), Velocity.Y);
				else
					Velocity = new Vector2(Velocity.X + (tempAccX * (float)deltaTime), Velocity.Y);
			}

			Velocity = new Vector2(Velocity.X, Velocity.Y + (Acceleration.Y * (float)deltaTime));
			AttemptedPosition = Vector2.Multiply(Velocity, (float)deltaTime);

			if (_owner.GetComponent<AABBCollider>() == null && _owner.GetComponent<CircleCollider>() == null)
				_owner.Move(AttemptedPosition);//If this does not contain a collider just move it because nothing will stop it.
		}

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}
