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

		private Dictionary<String, Texture2D> _textures;
		
		public Dictionary<String, Texture2D> Textures
		{
			get { return _textures; }
			set { _textures = value; }
		}

		public ResourceManager()
		{
			_textures = new Dictionary<string, Texture2D>();
		}

		public void AddTexture(ContentManager content, string spriteName)
		{
			if (!_textures.ContainsKey(spriteName))
			{
				_textures.Add(spriteName, content.Load<Texture2D>(spriteName));
			}
		}
	}
}
