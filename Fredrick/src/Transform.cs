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
		protected Vector2 position;
		protected float rotation;
		protected float scale;

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

		public float GetScale()
		{
			return scale;
		}

		public void SetScale(float scale)
		{
			this.scale = scale;
		}

		public void Move(Vector2 offset)
		{
			position += offset;
		}
	}
}
