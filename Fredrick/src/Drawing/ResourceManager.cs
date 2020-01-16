using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	public sealed class ResourceManager
	{
		private static ResourceManager instance = null;
		private static readonly object padlock = new object();

		public static ResourceManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new ResourceManager();
					}
					return instance;
				}
			}
		}

		public Dictionary<String, Texture2D> Textures { get; set; }

		public ResourceManager()
		{
			Textures = new Dictionary<string, Texture2D>();
		}

		public void AddTexture(ContentManager content, string spriteName)
		{
			if (!Textures.ContainsKey(spriteName))
			{
				Textures.Add(spriteName, content.Load<Texture2D>(spriteName));
			}
		}
	}
}
