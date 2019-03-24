using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class AABBTrigger : Component
	{
		private int _index;//index in ColliderManager
		private RectangleF _rectangle;

		private List<AABBCollider> _aabbHits;
		private List<Platform> _platformHits;


		public RectangleF Rectangle
		{
			get { return _rectangle; }
			set { _rectangle = value; }
		}

		public List<AABBCollider> AABBHits
		{
			get { return _aabbHits; }
			set { _aabbHits = value; }
		}

		public List<Platform> PlatformHits
		{
			get { return _platformHits; }
			set { _platformHits = value; }
		}

		public AABBTrigger()
		{
		}

		public AABBTrigger(Entity owner) : base(owner)
		{
			_aabbHits = new List<AABBCollider>();
			_platformHits = new List<Platform>();
		}

		public AABBTrigger(Entity owner, AABBTrigger original) : base(owner, original.Id)
		{
			_rectangle = original.Rectangle;
			_aabbHits = new List<AABBCollider>();
			_platformHits = new List<Platform>();
		}

		public bool CheckCollision(AABBCollider other)
		{
			if (_rectangle.Intersect(other.Rectangle))
			{
				_aabbHits.Add(other);
				return true;
			}
			return false;
		}

		public bool CheckCollision(Platform other)
		{
			float f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
																																																	   //Debug.Print(f.ToString());
			if (f > 0 && f < 1)
			{
				float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;//desired y coordinate

				if (((_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) > (y + other.PlatformDepth)) && ((_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) < y))
				{
					_platformHits.Add(other);
					return true;
				}
			}
			return false;
		}

		public bool Update(Vector2 position)
		{
			bool trigger = false;
			_aabbHits.Clear();
			_platformHits.Clear();

			_rectangle.UpdatePosition(position);

			foreach (Platform p in ColliderManager.Instance.Platforms)
			{
				if (p.Owner != _owner)
					if (CheckCollision(p))
						trigger = true;
			}

			foreach (AABBCollider c in ColliderManager.Instance.Colliders)
			{
				if (c.Owner != _owner)
					if (CheckCollision(c))
						trigger = true;
			}

			return trigger;
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

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			Vector2 tl = new Vector2(Rectangle.Position.X - Rectangle.Width / 2, Rectangle.Position.Y + Rectangle.Height / 2);
			Vector2 tr = new Vector2(Rectangle.Position.X + Rectangle.Width / 2, Rectangle.Position.Y + Rectangle.Height / 2);
			Vector2 bl = new Vector2(Rectangle.Position.X - Rectangle.Width / 2, Rectangle.Position.Y - Rectangle.Height / 2);
			Vector2 br = new Vector2(Rectangle.Position.X + Rectangle.Width / 2, Rectangle.Position.Y - Rectangle.Height / 2);
			DebugManager.Instance.DrawLine(spriteBatch, tl + _owner.Position, tr + _owner.Position);
			DebugManager.Instance.DrawLine(spriteBatch, tr + _owner.Position, br + _owner.Position);
			DebugManager.Instance.DrawLine(spriteBatch, br + _owner.Position, bl + _owner.Position);
			DebugManager.Instance.DrawLine(spriteBatch, bl + _owner.Position, tl + _owner.Position);
		}

		public override Component Copy(Entity owner)
		{
			return new AABBTrigger(owner, this);
		}
	}
}
