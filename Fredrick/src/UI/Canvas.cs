﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Canvas : Component
	{
		public List<TextElement<float>> TextElements { get; set; }

		public Canvas(Entity owner, string id) : base(owner, id)
		{
			TextElements = new List<TextElement<float>>();
		}

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			foreach (var t in TextElements)
			{
				t.Update();
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			foreach (var t in TextElements)
			{
				spriteBatch.DrawString(t.Font, t.Text, t.Position, t.Colour);
			}

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}
