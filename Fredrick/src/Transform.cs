using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	class Transform
	{
		private Vector2 position;
		private float rotation;
		private Vector2 scale;

		public Vector2 GetPosition()
		{
			return position;
		}

		public void SetPosition(Vector2 position)
		{
			this.position = position;
		}

		public float GetRotation()
		{
			return rotation;
		}

		public void SetRotation(float rotation)
		{
			this.rotation = rotation;
		}

		public Vector2 GetScale()
		{
			return scale;
		}

		public void SetScale(Vector2 scale)
		{
			this.scale = scale;
		}

		public void Move(Vector2 offset)
		{
			position += offset;
		}
	}
}
