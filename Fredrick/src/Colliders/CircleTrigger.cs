using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Colliders
{
	[Serializable]
	public class CircleTrigger : Trigger
	{

		Vector2 _position;

		[NonSerialized]
		CircleShape _circle;

		public float m_radius;

		public bool IsDetected { get; private set; }
		public double DetectionDuration;// may want to stick in ai instead of here, and make ai callback this ect.


		public CircleTrigger(Entity owner, string id, float radius, ColliderCategory colliderCategory, ColliderCategory collidesWith) : base(owner, id)
		{
			m_radius = radius;
			m_colliderCategory = colliderCategory;
			m_collidesWith = collidesWith;
		}

		public CircleTrigger(Entity owner, CircleTrigger original) : base(owner, original)
		{
			m_radius = original.m_radius;
			m_colliderCategory = original.m_colliderCategory;
			m_collidesWith = original.m_collidesWith;
		}

		public void Revive()
		{
			m_body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			_circle = new CircleShape(m_radius, 0);
			_circle.Position = _position;
			m_fixture = m_body.CreateFixture(_circle);

			m_body.BodyType = BodyType.Dynamic;
			m_body.UserData = _owner;
			m_body.Awake = true;
			m_body.IsSensor = true;
			m_body.Position = _owner.Position;
			m_body.CollisionCategories = (Category)m_colliderCategory;
			m_body.CollidesWith = (Category)m_collidesWith;

			m_body.Restitution = 0.7f;
		}

		public void UpdatePosition()
		{
			m_body.Position = _owner.Position;
			m_body.Rotation = -_owner.Rotation;//Needs to be negative
		}

		public void Kill()
		{
			ColliderManager.Instance.World.RemoveBody(m_body);
		}

		private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			IsDetected = true;
			return true;
		}

		public override void Load(ContentManager content)
		{
			throw new NotImplementedException();
		}

		public override void Unload()
		{
			throw new NotImplementedException();
		}

		public override void Update(double deltaTime)
		{
			UpdatePosition();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		public override Component Copy(Entity owner)
		{
			throw new NotImplementedException();
		}
	}
}
