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
		public enum Justification
		{
			Left,
			Right,
			Center,
			Manual
		}

		public SpriteFont Font { get; set; }
		public string PrependText { get; set; }
		public string AppendText { get; set; }
		public string Text { get; set; }
		public Vector2 Position { get; set; }
		public Color Colour { get; set; }
		public float Rotation { get; set; }
		public Justification Alignment { get; set; }
		/// <summary>
		/// Will be overwritten if alignment isn't manual
		/// </summary>
		public Vector2 Origin { get; set; }
		public float Scale { get; set; }
		public SpriteEffects SpriteEffect { get; set; }
		public float LayerDepth { get; set; }

		public object ContentObject { get; set; }
		public string PropertyName { get; set; }

		public TextElement()
		{
			Text = "";
			Scale = 1;
			Alignment = Justification.Left;
		}

		public TextElement(SpriteFont font, Vector2 position, Color colour, float rotation = 0.0f, Justification alignment = Justification.Left, float scale = 1.0f, SpriteEffects spriteEffect = SpriteEffects.None, float layerDepth = 0.0f)
		{
			Font = font;
			Text = "";
			Position = position;
			Colour = colour;
			Rotation = rotation;
			Alignment = alignment;
			Scale = scale;
			SpriteEffect = spriteEffect;
			LayerDepth = layerDepth;
		}

		public void AddContent(string prependText, string appendText, object contentObject, string propertyName)
		{
			PrependText = prependText;
			AppendText = appendText;
			ContentObject = contentObject;
			PropertyName = propertyName;
		}

		public void Update()
		{
			if (ContentObject.GetType().GetProperty(PropertyName) != null)
				Text = PrependText + ContentObject.GetType().GetProperty(PropertyName).GetValue(ContentObject, null).ToString() + AppendText;

			Vector2 size = Font.MeasureString(Text);
			switch (Alignment)
			{
				case (Justification.Left):
					Origin = new Vector2(0);
					break;
				case (Justification.Right):
					Origin = new Vector2(size.X, 0);
					break;
				case (Justification.Center):
					Origin = new Vector2(size.X / 2, 0);
					break;
			}
		}
	}
}
