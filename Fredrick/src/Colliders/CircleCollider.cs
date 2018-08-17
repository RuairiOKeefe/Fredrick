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
			_radius = 0.5f;
		}

		static Vector2 LineIntersection(Vector2 startA, Vector2 endA, Vector2 startB, Vector2 endB)
		{
			float aA = endA.Y - startA.Y;
			float bA = startA.X - endA.X;
			float cA = aA * startA.X + bA * startA.Y;

			float aB = endB.Y - startB.Y;
			float bB = startB.X - endB.X;
			float cB = aB * startB.X + bB * startB.Y;

			float delta = aA * bB - aB * bA;

			//If lines are parallel, the result will be (NaN, NaN)
			return delta == 0 ? new Vector2(float.NaN, float.NaN) : new Vector2((bB * cA - bA * cB) / delta, (aA * cB - aB * cA) / delta);
		}

		public void CheckCollision(RectangleF other)
		{
			Vector2 testMove = new Vector2(tempMove.X, tempMove.Y);
			Vector2 newPos = ((_owner.GetPosition() + testMove));
			bool a = true;



			if (newPos.X - other.CurrentPosition.X < (other.Width / 2) + _radius)
				a = false;
			if (newPos.Y - other.CurrentPosition.Y < (other.Height / 2) + _radius)
				a = false;

			if (newPos.X - other.CurrentPosition.X <= (other.Width / 2))
				a = true;
			if (newPos.Y - other.CurrentPosition.Y <= (other.Height / 2))
				a = true;

			float cornerDistance_sq = (newPos.X - other.CurrentPosition.X - other.Width / 2) * (newPos.X - other.CurrentPosition.X - other.Width / 2) + (newPos.Y - other.CurrentPosition.Y - other.Height / 2) * (newPos.Y - other.CurrentPosition.Y - other.Height / 2);

			a = (cornerDistance_sq <= (_radius * _radius));


			if (a == false)
				return;


			Vector2 closestInter;
			float shortest = float.MaxValue;

			//get side to set position to place at then bounce off side of face
			for (int i = 0; i < 4; i++)
			{
				//make not from center, but instead at oriented offset
				Vector2 intersection = LineIntersection(_owner.GetPosition() + _position, newPos, other.Corners[i % 4], other.Corners[(i + 1) % 4]);
				if (intersection != new Vector2(float.NaN))
				{
					if (Vector2.Distance(_owner.GetPosition() + _position, intersection) < shortest)
					{
						shortest = Vector2.Distance(_owner.GetPosition() + _position, intersection);
						closestInter = intersection;
					}
				}
			}
			//find side of collision
			//find pos of coll on side
			//get remaining dist of motion
			//get reflection
			//

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
