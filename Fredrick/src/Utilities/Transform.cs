using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	[Serializable]
	public class Transform
	{
		public Vector2 Position { get; set; }

		public float Rotation { get; set; }

		public Vector2 Scale { get; set; }

		public void Move(Vector2 offset)
		{
			Position += offset;
		}
	}
}
