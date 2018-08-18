using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace Fredrick.src
{
	public class CircleCollider : Component
	{
		Vector2 _position;

		Body _body;
		CircleShape _circle;
		Fixture _fixture;

		//Vector2 tempMove;

		public CircleCollider(Entity owner) : base(owner)
		{
			_body = new Body(ColliderManager.Instance.World, _owner.GetPosition() + _position, 0, BodyType.Dynamic);//can I attach entities
			_circle = new CircleShape(0.5f, 1.0f);
			_circle.Position = owner.GetPosition() + _position;
			_fixture = _body.CreateFixture(_circle);
		}



		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void Update(double deltaTime)
		{

		}
	}
}
