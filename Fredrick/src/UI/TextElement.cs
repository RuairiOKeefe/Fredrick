using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	public class TextElement
	{
		public SpriteFont Font { get; set; }
		public string PrependText { get; set; }
		public string AppendText { get; set; }
		public string Text { get; set; }
		public Vector2 Position { get; set; }
		public Color Colour { get; set; }

		public Object Object { get; set; }
		public string PropertyName { get; set; }

		public TextElement()
		{

		}

		public void Update()
		{
			Text = PrependText + Object.GetType().GetProperty(PropertyName).GetValue(Object, null).ToString() + AppendText;
		}
	}
}
