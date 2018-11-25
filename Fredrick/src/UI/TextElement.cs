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
	public class TextElement<T>
	{
		public SpriteFont Font { get; set; }
		public string PrependText { get; set; }
		public string AppendText { get; set; }
		public string Text { get; set; }
		public Vector2 Position { get; set; }
		public Color Colour { get; set; }

		private T data;

		public T GetData()
		{
			return data;
		}

		public void SetData(T value)//Add a method to search through entities to find required data using tags?
		{
			data = value;
		}

		public TextElement()
		{
			
		}

		public void Update()
		{
			Text = PrependText + data + AppendText;
		}
	}
}
