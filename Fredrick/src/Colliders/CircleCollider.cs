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

namespace Fredrick.src
{
	public class CircleCollider : Component
	{
		Vector2 _position;

		Body _body;
		CircleShape _circle;
		Fixture _fixture;

		public CircleCollider()
		{
		}

		public CircleCollider(Entity owner) : base(owner)
		{
			//_body = new Body(ColliderManager.Instance.World, _owner.GetPosition(), 0, BodyType.Dynamic);
			//_body.BodyType = BodyType.Dynamic;
			//_body.Mass = 1;
			//_body.UserData = _owner;
			//_body.Awake = true;
			//_circle = new CircleShape(0.5f, 1.0f);
			//_circle.Position = _position;
			//_fixture = _body.CreateFixture(_circle);
		}

		public void Revive()
		{
			_body = new Body(ColliderManager.Instance.World, _owner.GetPosition(), 0, BodyType.Dynamic);
			_circle = new CircleShape(0.25f, 1.0f);
			_circle.Position = _position;
			_fixture = _body.CreateFixture(_circle);

			_body.BodyType = BodyType.Dynamic;
			_body.UserData = _owner;
			_body.Awake = true;
			_body.Position = _owner.GetPosition();


			_body.Restitution = 0.7f;
		}

		public void ApplyForce(Vector2 force)
		{
			_body.ApplyLinearImpulse(force / 2);
		}

		public void ApplyForce(Vector2 force, Vector2 source)
		{
			_body.ApplyLinearImpulse(force, source);
		}

		public void UpdatePosition()
		{
			_owner.SetPosition(_body.Position);
			_owner.SetRotation(-_body.Rotation);//Needs to be negative
		}

		public void Kill()
		{
			ColliderManager.Instance.World.RemoveBody(_body);
		}

		public override void Load(ContentManager content)
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
