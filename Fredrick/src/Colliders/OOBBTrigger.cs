using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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
	public class OOBBTrigger : Collider
	{
		Vector2 _position;

		public Body Body { get; private set; }
		public float m_width;
		public float m_height;
		public float m_density;

		public OOBBTrigger(Entity owner, string id, Vector2 position, float width, float height, float density, ColliderCategory colliderCategory, ColliderCategory collidesWith) : base(owner, id)
		{
			_position = position;
			m_width = width;
			m_height = height;
			m_density = density;
			m_colliderCategory = colliderCategory;
			m_collidesWith = collidesWith;
		}

		public OOBBTrigger(Entity owner, OOBBTrigger original) : base(owner, original.Id)
		{
			_position = original._position;
			m_width = original.m_width;
			m_height = original.m_height;
			m_density = original.m_density;
			m_colliderCategory = original.m_colliderCategory;
			m_collidesWith = original.m_collidesWith;
		}

		public void Revive()
		{
			Active = true;
			Body = BodyFactory.CreateRectangle(ColliderManager.Instance.World, m_width, m_height, m_density);
			Body.BodyType = BodyType.Dynamic;
			Body.Position = _owner.Position;
			Body.Rotation = _owner.Rotation;
			
			Body.UserData = _owner;
			Body.Awake = true;
			Body.IsSensor = true;
			Body.IsBullet = true;
			Body.CollisionCategories = (Category)m_colliderCategory;
			Body.CollidesWith = (Category)m_collidesWith;
			Body.ApplyForce(new Vector2(5));
		}

		public void UpdatePosition()
		{
			Body.Position = _owner.Position;
			Body.Rotation = -_owner.Rotation;//Needs to be negative
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
			Body.Position = _owner.Position;
			Body.Rotation = -_owner.Rotation;
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
		}

		public override Component Copy(Entity owner)
		{
			return new OOBBTrigger(owner, this);
		}
	}
}
