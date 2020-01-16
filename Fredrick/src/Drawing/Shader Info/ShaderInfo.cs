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
		public struct Material
		{
			public Color Emissive;
			public Color Diffuse;
			public Color Specular;
			public float Shininess;
		}

		public virtual string ShaderId { get { return ""; } }

		public abstract void SetUniforms(Effect shader);

		public abstract void Load(ContentManager content);

		public abstract ShaderInfo Copy();
	}
}
