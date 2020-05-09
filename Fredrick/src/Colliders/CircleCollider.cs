using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src.Colliders
{
	[Serializable]
	public class CircleCollider : Collider
	{
		Vector2 _position;

		public Body Body { get; private set; }
		CircleShape _circle;
		Fixture _fixture;
		public float m_radius;
		public float m_density;

		public CircleCollider(Entity owner, string id, float radius, float density, ColliderCategory colliderCategory, ColliderCategory collidesWith) : base(owner, id)
		{
			m_radius = radius;
			m_density = density;
			m_colliderCategory = colliderCategory;
			m_collidesWith = collidesWith;
		}

		public CircleCollider(Entity owner, CircleCollider original) : base(owner, original)
		{
			m_radius = original.m_radius;
			m_density = original.m_density;
			m_colliderCategory = original.m_colliderCategory;
			m_collidesWith = original.m_collidesWith;
		}

		public void Revive()
		{
			Body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			_circle = new CircleShape(m_radius, m_density);
			_circle.Position = _position;
			_fixture = Body.CreateFixture(_circle);

			Body.BodyType = BodyType.Dynamic;
			Body.UserData = _owner;
			Body.Awake = true;
			Body.Position = _owner.Position;
			Body.CollisionCategories = (Category)m_colliderCategory;
			Body.CollidesWith = (Category)m_collidesWith;

			Body.Restitution = 0.7f;
		}

		public void ApplyForce(Vector2 force)
		{
			Body.ApplyLinearImpulse(force / 2);
		}

		public void ApplyForce(Vector2 force, Vector2 source)
		{
			Body.ApplyLinearImpulse(force, source);
		}

		public void UpdatePosition()
		{
			_owner.Position = Body.Position;
			_owner.Rotation = -Body.Rotation;//Needs to be negative
		}

		public void Kill()
		{
			ColliderManager.Instance.World.RemoveBody(Body);
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

		public override void DrawBatch(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new CircleCollider(owner, this);
		}
	}
}
