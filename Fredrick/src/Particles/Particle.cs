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
		//need to add collision logic and animation code
		Vector2 _position;
		float _rotation;
		Vector2 _scale;
		Vector2 _velocity;
		Vector2 _tempMove;


		double _lifeTime;

		float _restitution = 0.8f;

		bool _fakeDepth;
		float _scaleFactor;

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public float Rotation
		{
			get { return _rotation; }
		}

		public Vector2 Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
		}

		public Particle()
		{
			_position = new Vector2();
			_velocity = new Vector2();
		}

		public Particle(Vector2 position, Vector2 velocity, double lifeTime, bool fakeDepth = false, float scaleFactor = 1.0f)
		{
			_position = position;
			_scale = new Vector2(1);
			_velocity = velocity;
			_lifeTime = lifeTime;

			_fakeDepth = fakeDepth;
			_scaleFactor = scaleFactor;
		}

		public void Revive(Vector2 position, Vector2 velocity, double lifeTime, bool fakeDepth = false, float scaleFactor = 1.0f)
		{
			_position = position;
			_scale = new Vector2(1);
			_velocity = velocity;
			_lifeTime = lifeTime;

			_fakeDepth = fakeDepth;
			_scaleFactor = scaleFactor;
		}

		public bool CheckCollision(RectangleF other)
		{
			bool collided = false;

			Vector2 testMove = new Vector2(_tempMove.X, 0);
			Vector2 newPos = (_position + testMove);

			if (other.Intersect(newPos))
			{
				float distanceX = newPos.X - other.CurrentPosition.X;

				if (distanceX > 0)
				{
					_tempMove.X += (other.Width / 2 - distanceX) * 1.05f;
					_velocity = (_velocity - (2f * Vector2.Dot(_velocity, new Vector2(1, 0))) * new Vector2(1, 0)) * _restitution;
				}
				else
				{
					_tempMove.X += (-other.Width / 2 - distanceX) * 1.05f;
					_velocity = (_velocity - (2f * Vector2.Dot(_velocity, new Vector2(-1, 0))) * new Vector2(-1, 0)) * _restitution;
				}

				collided = true;
			}

			testMove = new Vector2(0, _tempMove.Y);
			newPos = (_position + testMove);

			if (other.Intersect(newPos))
			{
				float distanceY = newPos.Y - other.CurrentPosition.Y;

				if (distanceY > 0)
				{
					_tempMove.Y += (other.Height / 2 - distanceY) * 1.05f;
					_velocity = (_velocity - (2f * Vector2.Dot(_velocity, new Vector2(0, 1))) * new Vector2(0, 1)) * _restitution;
				}
				else
				{
					_tempMove.Y += (-other.Height / 2 - distanceY) * 1.05f;
					_velocity = (_velocity - (2f * Vector2.Dot(_velocity, new Vector2(0, -1))) * new Vector2(0, -1)) * _restitution;
				}

				collided = true;
			}

			return collided;
		}

		public bool CheckCollision(Platform other)
		{
			bool collided = false;
			if (other.PlatformDepth < 0)
			{
				Vector2 testMove = new Vector2(_tempMove.X, _tempMove.Y);
				Vector2 newPos = (_position + testMove);
				{
					float f = (newPos.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if (newPos.Y > y && newPos.Y < y + other.PlatformDepth)
						{
							_tempMove.Y -= (newPos.Y - y);
							_velocity = (_velocity - (2f * Vector2.Dot(_velocity, other.Normal)) * other.Normal) * _restitution;
							collided = true;
						}
					}
				}
			}
			else
			{
				Vector2 testMove = new Vector2(_tempMove.X, _tempMove.Y);
				Vector2 newPos = (_position + testMove);
				{
					float f = (newPos.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if (newPos.Y > y && newPos.Y < y + other.PlatformDepth)
						{
							_tempMove.Y -= (newPos.Y - y);
							_velocity = (_velocity - (2f * Vector2.Dot(_velocity, other.Normal)) * other.Normal) * _restitution;
							collided = true;
						}
					}
				}
			}

			return collided;
		}

		public void Update(double deltaTime, Vector2 acceleration)
		{
			_velocity += acceleration * (float)deltaTime;
			_tempMove = _velocity * (float)deltaTime;

			if (_fakeDepth)
			{
				if (_scaleFactor > 0)
				{
					_scale.X *= 1+(_scaleFactor * (float)deltaTime);
					_scale.Y *= 1 + (_scaleFactor * (float)deltaTime);
				}
				else
				{
					_scale.X /= 1 + (-_scaleFactor * (float)deltaTime);
					_scale.Y /= 1 + (-_scaleFactor * (float)deltaTime);
				}
			}

			int x = (int)Math.Floor(_position.X + _tempMove.X + 0.5f);
			int y = (int)Math.Floor(_position.Y + _tempMove.Y + 0.5f);


			foreach (Entity e in ColliderManager.Instance.Terrain[x, y])
			{
				if (e.GetComponent<Character>() == null)
				{
					if (e.GetComponent<AABBCollider>() != null)
						if (CheckCollision(e.GetComponent<AABBCollider>().Rectangle))
						{
						}
					if (e.GetComponent<Platform>() != null)
						if (CheckCollision(e.GetComponent<Platform>()))
						{
						}
				}
			}

			_position += _tempMove;

			if (_velocity.Length() > 0)
			{
				Vector2 v = _velocity;
				v.Normalize();
				_rotation = (float)Math.Atan2(-v.Y, v.X);
			}
			else
			{
				_rotation = 0;
			}
			_lifeTime -= deltaTime;
		}
	}
}
