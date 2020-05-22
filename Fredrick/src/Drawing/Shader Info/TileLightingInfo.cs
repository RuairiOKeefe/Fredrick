using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src.Drawing.Shader_Info
{
	[Serializable]
	public class TileLightingInfo : ShaderInfo
	{
		public override string ShaderId { get { return "TileLighting"; } }
		public string TileNormalMapName;
		public string DetailNormalMapName;
		public Material Material;

		public TileLightingInfo(string tileNormalMapName, string detailNormalMapName, Material material)
		{
			TileNormalMapName = tileNormalMapName;
			DetailNormalMapName = detailNormalMapName;
			Material = material;
		}

		public override void SetUniforms(Effect shader, float rotation = 0, int flip = 1)
		{
			shader.Parameters["TileNormalMap"].SetValue(ResourceManager.Instance.Textures[TileNormalMapName]);
			shader.Parameters["DetailNormalMap"].SetValue(ResourceManager.Instance.Textures[DetailNormalMapName]);
			//shader.Parameters["Material"].SetValue(Material);
		}

		public override void Load(ContentManager content)
		{
			ResourceManager.Instance.AddTexture(content, TileNormalMapName);
			ResourceManager.Instance.AddTexture(content, DetailNormalMapName);
		}

		public override ShaderInfo Copy()
		{
			return new TileLightingInfo(this.TileNormalMapName, this.DetailNormalMapName, this.Material);
		}
	}
}
