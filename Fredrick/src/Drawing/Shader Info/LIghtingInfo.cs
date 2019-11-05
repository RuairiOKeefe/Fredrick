using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class LightingInfo : ShaderInfo
	{
		public Texture2D NormalMap;
		public Material Material;

		public LightingInfo(Texture2D normalMap, Material material)
		{
			NormalMap = normalMap;
			Material = material;
		}

		public override void SetUniforms(Effect shader)
		{
			shader.Parameters["NormalMap"].SetValue(NormalMap);
			//shader.Parameters["Material"].SetValue(Material);
		}

		public override ShaderInfo Copy()
		{
			return new LightingInfo(this.NormalMap, this.Material);
		}
	}
}
