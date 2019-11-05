using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	[Serializable]
	public class Platform : Component
	{

		protected Vector2 _currentPosition;//current position of collider to be tested
		protected float _width;
		protected float _height;

		protected float _offsetX;
		protected float _offsetY;

		/// <summary>
		/// Height of the surface of the leftmost point on the platform, should not excede height
		/// </summary>
		protected float _lHeight;

		/// <summary>
		/// Height of the surface of the rightmost point on the platform, should not excede height
		/// </summary>
		protected float _rHeight;

		protected float _platformDepth;
		protected Vector2 _normal;

		protected Texture2D _lineTex;//A simple 1x1 texture to be used for line rendering in debug

		private List<int[]> cells;

		private int minX, maxX, minY, maxY;

		public float Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public float Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public Vector2 CurrentPosition
		{
			get { return _currentPosition; }
			set { _currentPosition = value; }
		}

		public float LHeight
		{
			get { return _lHeight; }
			set { _lHeight = value; }
		}

		public float RHeight
		{
			get { return _rHeight; }
			set { _rHeight = value; }
		}

		public float PlatformDepth
		{
			get { return _platformDepth; }
			set { _platformDepth = value; }
		}

		public Vector2 Normal
		{
			get { return _normal; }
			set { _normal = value; }
		}

		public Platform()
		{
		}

		public Platform(Entity owner, Vector2 position, float width, float height, float offsetX, float offsetY, float lHeight, float rHeight, float platformDepth) : base(owner)
		{
			Position = position;
			_currentPosition = owner.Position + Position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;

			_lHeight = lHeight;
			_rHeight = rHeight;
			_platformDepth = platformDepth;
		}

		public void SetCells(bool addToScene)
		{
			minX = Math.Max((int)Math.Floor(_currentPosition.X - (_width / 2) + 0.5), 0);
			maxX = Math.Min((int)Math.Floor(_currentPosition.X + (_width / 2) + 0.5), 1000);
			minY = Math.Max((int)Math.Floor(_currentPosition.Y - (_height / 2) + 0.5), 0);
			maxY = Math.Min((int)Math.Floor(_currentPosition.Y + (_height / 2) + 0.5), 1000);

			for (int i = minX; i < maxX + 1; i++)
			{
				for (int j = minY; j < maxY + 1; j++)
				{
					if (addToScene)
						ColliderManager.Instance.Terrain[i, j].Add(_owner);
					cells.Add(new int[2] { i, j });
				}
			}
		}

		public void ClearCells(bool removeFromScene)
		{
			if (removeFromScene)
				foreach (int[] c in cells)
				{
					ColliderManager.Instance.Terrain[c[0], c[1]].Remove(_owner);
				}
			cells.Clear();
		}

		public override void Load(ContentManager content)
		{
			_currentPosition = _owner.Position + Position;

			Body body;
			EdgeShape shape;
			Fixture fixture;

			body = new Body(ColliderManager.Instance.World, _currentPosition, 0, BodyType.Static);
			body.UserData = _owner;
			shape = new EdgeShape(new Vector2(-_width / 2, _lHeight), new Vector2(_width / 2, _rHeight));
			fixture = body.CreateFixture(shape);

			Vector2 a = new Vector2();
			Vector2 b = new Vector2();
			if (_platformDepth < 0)
			{
				a = new Vector2(-_width / 2, _rHeight);
				b = new Vector2(_width / 2, _lHeight);
			}
			else
			{
				a = new Vector2(-_width / 2, _lHeight);
				b = new Vector2(_width / 2, _rHeight);
			}
			Vector2 v = b - a;
			_normal = new Vector2(v.Y, -v.X);
			_normal.Normalize();

			ColliderManager.Instance.Platforms.Add(this);
			_lineTex = DebugManager.Instance.LineTex;

			cells = new List<int[]>();
			SetCells(true);
		}

		public override void Unload()
		{
			ClearCells(true);
			foreach (Body b in ColliderManager.Instance.World.BodyList)
			{
				if (b.UserData == (object)_owner)
				{
					ColliderManager.Instance.World.RemoveBody(b);
				}
			}
		}

		public override void Update(double deltaTime)
		{

		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			DebugManager.Instance.DrawLine(spriteBatch,
			new Vector2(_currentPosition.X - (Width / 2), (_currentPosition.Y + LHeight)),
			new Vector2(_currentPosition.X + (Width / 2), (_currentPosition.Y + RHeight))
			);

			DebugManager.Instance.DrawLine(spriteBatch,
			new Vector2(_currentPosition.X - (Width / 2), (_currentPosition.Y + LHeight + PlatformDepth)),
			new Vector2(_currentPosition.X + (Width / 2), (_currentPosition.Y + RHeight + PlatformDepth))
			);
		}

		public override Component Copy(Entity owner)
		{
			return new Platform();
		}
	}
}
