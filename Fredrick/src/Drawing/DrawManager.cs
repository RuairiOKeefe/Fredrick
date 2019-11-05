using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class DrawManager
	{
		private List<Effect> shaders;
		private Dictionary<Type, List<Component>> drawComponents { get; set; }


		public void Load(ContentManager content)
		{

		}

		public void DrawComponents()
		{

		}

		public void DrawBatch()
		{

		}
		public void Draw()
		{
			DrawComponents();
			DrawBatch();
		}
	}
}
