using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public abstract class ShaderInfo
	{
		public struct Material
		{
			public Color Emissive;
			public Color Diffuse;
			public Color Specular;
			public float Shininess;
		}

		public abstract void SetUniforms(Effect shader);

		public abstract ShaderInfo Copy();
	}
}
