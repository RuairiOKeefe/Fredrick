using Fredrick.src.Colliders;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	[Serializable]
	public class Particle
	{
		public Drawable Drawable;

		public Vector2 Position;
		public float Rotation;
		public Vector2 Scale;
		public Vector2 Velocity;
		Vector2 m_acceleration;
		Vector2 _tempMove;
		bool _collide;

		public double Lifetime;
		public double InitLifetime;
		bool _reduceLifeOnCollision;

		float _restitution;

		bool _fakeDepth;
		float _scaleFactor;

		public List<Tuple<Color, double>> LerpColours
		{
			get { return m_lerpColours; }
			set
			{
				m_lerpColours = value;
				m_lerpColours.Sort((x, y) => x.Item2.CompareTo(y.Item2));
			}
		}

		protected List<Tuple<Color, double>> m_lerpColours;

		public Particle()
		{
			Position = new Vector2();
			Velocity = new Vector2();
		}

		public Particle(Drawable drawable, Vector2 position, Vector2 scale, Vector2 velocity, Vector2 acceleration, double lifeTime, bool collide = false, bool reduceLifeOnCollision = false, float restitution = 0.5f, bool fakeDepth = false, float scaleFactor = 1.0f, List<Tuple<Color, double>> lerpColours = null)
		{
			Drawable = drawable;
			Position = position;
			Scale = scale;
			Velocity = velocity;
			m_acceleration = acceleration;
			_collide = collide;

			Lifetime = lifeTime;
			InitLifetime = lifeTime;
			_reduceLifeOnCollision = reduceLifeOnCollision;
			_restitution = restitution;

			_fakeDepth = fakeDepth;
			_scaleFactor = scaleFactor;
			m_lerpColours = new List<Tuple<Color, double>>();
			if (lerpColours != null)
				m_lerpColours = lerpColours;
		}

		public void Revive(Drawable drawable, Vector2 position, Vector2 scale, Vector2 velocity, Vector2 acceleration, double lifeTime, bool collide = false, bool reduceLifeOnCollision = false, float restitution = 0.5f, bool fakeDepth = false, float scaleFactor = 1.0f, List<Tuple<Color, double>> lerpColours = null)
		{
			Drawable = drawable;
			Position = position;
			Scale = scale;
			Velocity = velocity;
			m_acceleration = acceleration;
			_collide = collide;

			Lifetime = lifeTime;
			InitLifetime = lifeTime;
			_reduceLifeOnCollision = reduceLifeOnCollision;
			_restitution = restitution;

			_fakeDepth = fakeDepth;
			_scaleFactor = scaleFactor;
			m_lerpColours = new List<Tuple<Color, double>>();
			if (lerpColours != null)
				m_lerpColours = lerpColours;
		}

		public bool CheckCollision(RectangleF other)
		{
			bool collided = false;

			Vector2 testMove = new Vector2(_tempMove.X, 0);
			Vector2 newPos = (Position + testMove);

			if (other.Intersect(newPos))
			{
				float distanceX = newPos.X - other.CurrentPosition.X;

				if (distanceX > 0)
				{
					_tempMove.X += (other.Width / 2 - distanceX) * 1.05f;
					Velocity = (Velocity - (2f * Vector2.Dot(Velocity, new Vector2(1, 0))) * new Vector2(1, 0)) * _restitution;
				}
				else
				{
					_tempMove.X += (-other.Width / 2 - distanceX) * 1.05f;
					Velocity = (Velocity - (2f * Vector2.Dot(Velocity, new Vector2(-1, 0))) * new Vector2(-1, 0)) * _restitution;
				}

				collided = true;
			}

			testMove = new Vector2(0, _tempMove.Y);
			newPos = (Position + testMove);

			if (other.Intersect(newPos))
			{
				float distanceY = newPos.Y - other.CurrentPosition.Y;

				if (distanceY > 0)
				{
					_tempMove.Y += (other.Height / 2 - distanceY) * 1.05f;
					Velocity = (Velocity - (2f * Vector2.Dot(Velocity, new Vector2(0, 1))) * new Vector2(0, 1)) * _restitution;
				}
				else
				{
					_tempMove.Y += (-other.Height / 2 - distanceY) * 1.05f;
					Velocity = (Velocity - (2f * Vector2.Dot(Velocity, new Vector2(0, -1))) * new Vector2(0, -1)) * _restitution;
				}

				collided = true;
			}

			return collided;
		}

		public bool CheckCollision(Platform other)
		{
			Vector2 testMove = new Vector2(_tempMove.X, _tempMove.Y);
			Vector2 newPos = (Position + testMove);
			bool collided = false;
			if (other.PlatformDepth < 0)
			{
				{
					float f = (newPos.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if (newPos.Y < y && newPos.Y > y + other.PlatformDepth)
						{
							_tempMove.Y -= (newPos.Y - y);
							Velocity = (Velocity - (2f * Vector2.Dot(Velocity, other.Normal)) * other.Normal) * _restitution;
							collided = true;
						}
					}
				}
			}
			else
			{
				newPos = (Position + testMove);
				{
					float f = (newPos.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if (newPos.Y > y && newPos.Y < y + other.PlatformDepth)
						{
							_tempMove.Y -= (newPos.Y - y);
							Velocity = (Velocity - (2f * Vector2.Dot(Velocity, other.Normal)) * other.Normal) * _restitution;
							collided = true;
						}
					}
				}
			}

			return collided;
		}

		public void Update(double deltaTime)
		{
			Velocity += m_acceleration * (float)deltaTime;
			_tempMove = Velocity * (float)deltaTime;

			if (_fakeDepth)
			{
				if (_scaleFactor > 0)
				{
					Scale.X *= 1 + (_scaleFactor * (float)deltaTime);
					Scale.Y *= 1 + (_scaleFactor * (float)deltaTime);
				}
				else
				{
					Scale.X /= 1 + (-_scaleFactor * (float)deltaTime);
					Scale.Y /= 1 + (-_scaleFactor * (float)deltaTime);
				}
			}

			if (_collide)
			{
				int x = Math.Min(Math.Max((int)Math.Floor(Position.X + _tempMove.X + 0.5f), 0), 1000);
				int y = Math.Min(Math.Max((int)Math.Floor(Position.Y + _tempMove.Y + 0.5f), 0), 1000);

				bool collided = false;
				foreach (Entity e in ColliderManager.Instance.Terrain[x, y])
				{
					if (e.GetComponent<Character>() == null)
					{
						if (e.GetComponent<AABBCollider>() != null)
							if (CheckCollision(e.GetComponent<AABBCollider>().Rectangle))
							{
								collided = true;
							}
						if (e.GetComponent<Platform>() != null)
							if (CheckCollision(e.GetComponent<Platform>()))
							{
								collided = true;
							}
					}
				}
				if (collided && _reduceLifeOnCollision)
					Lifetime /= 2.0;
			}

			Position += _tempMove;

			if (Velocity.Length() > 0)
			{
				Vector2 v = Velocity;
				v.Normalize();
				Rotation = (float)Math.Atan2(-v.Y, v.X);
			}
			else
			{
				Rotation = 0;
			}
			Lifetime -= deltaTime;
		}
	}
}
