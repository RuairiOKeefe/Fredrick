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

		public float Radius;

		private Vector2 m_startPosition;
		private CircleShape m_circle;

		public CircleCollider(Entity owner, string id, float radius, float density, ColliderCategory colliderCategory, ColliderCategory collidesWith) : base(owner, id)
		{
			Radius = radius;
			m_density = density;
			m_colliderCategory = colliderCategory;
			m_collidesWith = collidesWith;
		}

		public CircleCollider(Entity owner, CircleCollider original) : base(owner, original)
		{
			Radius = original.Radius;
			m_density = original.m_density;
			m_colliderCategory = original.m_colliderCategory;
			m_collidesWith = original.m_collidesWith;
		}

		public void Revive()
		{
			m_body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			m_circle = new CircleShape(Radius, m_density);
			m_circle.Position = m_startPosition;
			m_fixture = m_body.CreateFixture(m_circle);

			m_body.BodyType = BodyType.Dynamic;
			m_body.UserData = _owner;
			m_body.Awake = true;
			m_body.Position = _owner.Position;
			m_body.CollisionCategories = (Category)m_colliderCategory;
			m_body.CollidesWith = (Category)m_collidesWith;

			m_body.Restitution = 0.7f;
		}

		public void ApplyForce(Vector2 force)
		{
			m_body.ApplyLinearImpulse(force / 2);
		}

		public void ApplyForce(Vector2 force, Vector2 source)
		{
			m_body.ApplyLinearImpulse(force, source);
		}

		public void UpdatePosition()
		{
			_owner.Position = m_body.Position;
			_owner.Rotation = -m_body.Rotation;//Needs to be negative
		}

		public void Kill()
		{
			ColliderManager.Instance.World.RemoveBody(m_body);
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
