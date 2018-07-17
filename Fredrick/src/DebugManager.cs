using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public sealed class DebugManager
	{
		private static DebugManager instance = null;
		private static readonly object padlock = new object();

		public static DebugManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new DebugManager();
					}
					return instance;
				}
			}
		}

		private Texture2D _lineTex;//A simple 1x1 texture to be used for line rendering in debug
		public Texture2D LineTex
		{
			get { return _lineTex; }
			set { _lineTex = value; }
		}

		public void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
		{
			Vector2 edge = end - start;
			// calculate angle to rotate line
			float angle = (float)Math.Atan2(-edge.Y, edge.X);


			//spriteBatch.Draw(_lineTex,
			//	new Rectangle(// rectangle defines shape of line and position of start of line
			//		(int)(start.X * 32),
			//		-(int)(start.Y * 32),
			//		(int)(edge.Length() * 32), //sb will strech the texture to fill this rectangle
			//		1), //width of line, change this to make thicker line
			//	null,
			//	Color.Yellow, //colour of line
			//	angle,     //angle of line (calulated above)
			//	new Vector2(0, 0), // point in line about which to rotate
			//	SpriteEffects.None,
			//	0);

		}
	}
}
