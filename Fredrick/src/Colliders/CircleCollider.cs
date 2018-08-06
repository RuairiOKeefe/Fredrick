using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class CircleCollider : Component
	{
		Vector2 _position;
		float _radius;
		float _mass;//try to avoid using

		Vector2 tempMove;

		public CircleCollider(Entity owner) : base(owner)
		{

		}

		public void CheckCollision(RectangleF other)
		{
			Vector2 testMove = new Vector2(tempMove.X, tempMove.Y);
			Vector2 newPos = ((_owner.GetPosition() + testMove));
			bool a = true;

			if (newPos.X < other.CurrentPosition.X - (other.Width / 2))
				a = false;
			if (newPos.X > other.CurrentPosition.X + (other.Width / 2))
				a = false;
			if (newPos.Y < other.CurrentPosition.Y - (other.Height / 2))
				a = false;
			if (newPos.Y > other.CurrentPosition.Y + (other.Height / 2))
				a = false;

			if (a == false)
				return;

			_owner.GetComponent<Projectile>().StopVelX();
			_owner.GetComponent<Projectile>().StopVelY();

		}

		public void CheckCollision(Platform other)
		{
			Vector2 testMove = new Vector2(tempMove.X, tempMove.Y);
		}

		public void CheckCollision(CircleCollider other)
		{
			Vector2 testMove = new Vector2(tempMove.X, tempMove.Y);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void Update(double deltaTime)
		{
			tempMove = _owner.GetComponent<Projectile>().AttemptedPosition;

			foreach (Platform p in ColliderManager.Instance.Platforms)
			{
				if (p.GetOwner() != _owner)
					CheckCollision(p);
			}

			foreach (AABBCollider c in ColliderManager.Instance.Colliders)
			{
				if (c.GetOwner() != _owner)
					CheckCollision(c.Rectangle);
			}
			_owner.Move(tempMove);
		}
	}
}
