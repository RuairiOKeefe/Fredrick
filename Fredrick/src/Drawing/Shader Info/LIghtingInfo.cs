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
	public class LightingInfo : ShaderInfo
	{
		public override string ShaderId { get { return "Lighting"; } }
		public string NormalMapName;
		public Material Material;

		public LightingInfo(string normalMapName, Material material)
		{
			NormalMapName = normalMapName;
			Material = material;
		}

		public override void SetUniforms(Effect shader, float rotation = 0)
		{
			shader.Parameters["NormalMap"].SetValue(ResourceManager.Instance.Textures[NormalMapName]);
			shader.Parameters["Rotation"].SetValue(rotation);
			//shader.Parameters["Material"].SetValue(Material);
		}

		public override void Load(ContentManager content)
		{
			ResourceManager.Instance.AddTexture(content, NormalMapName);
		}

		public override ShaderInfo Copy()
		{
			return new LightingInfo(this.NormalMapName, this.Material);
		}
	}
}
