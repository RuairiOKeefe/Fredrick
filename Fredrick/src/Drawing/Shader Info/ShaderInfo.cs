using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	[Serializable]
	public abstract class ShaderInfo
	{
		[Serializable]
		public struct Material
		{
			public Color Emissive;
			public Color Diffuse;
			public Color Specular;
			public float Shininess;
		}

		public virtual string ShaderId { get { return ""; } }

		public abstract void SetUniforms(Effect shader, float rotation = 0);

		public abstract void Load(ContentManager content);

		public abstract ShaderInfo Copy();
	}
}
