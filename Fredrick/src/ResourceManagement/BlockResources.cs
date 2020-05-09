using Fredrick.src.Colliders;
using Fredrick.src.Drawing.Shader_Info;
using Fredrick.src.Entity_System;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.ResourceManagement
{
	using BlockMultiDictionary = Dictionary<Material, Dictionary<Shape, Dictionary<Facing, List<Entity>>>>;

	public sealed class BlockKey : Tuple<Material, Shape, Facing, uint>
	{
		public BlockKey(Material item1, Shape item2, Facing item3, uint item4) : base(item1, item2, item3, item4)
		{
		}
		public Material DataA => Item1;

		public Shape DataB => Item2;
	}

	public sealed class BlockResources
	{
		private static BlockResources instance = null;
		private static readonly object padlock = new object();

		public static BlockResources Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new BlockResources();
					}
					return instance;
				}
			}
		}

		private BlockMultiDictionary blocks = new BlockMultiDictionary();

		public BlockResources()
		{
			blocks = new BlockMultiDictionary();
			foreach (Material m in (Material[])Enum.GetValues(typeof(Material)))
			{
				blocks.Add(m, new Dictionary<Shape, Dictionary<Facing, List<Entity>>>());
				foreach (Shape s in (Shape[])Enum.GetValues(typeof(Shape)))
				{
					blocks[m].Add(s, new Dictionary<Facing, List<Entity>>());
					foreach (Facing f in (Facing[])Enum.GetValues(typeof(Facing)))
					{
						blocks[m][s].Add(f, new List<Entity>());
					}
				}
			}
			InitBlocks();
		}

		public Entity GetBlock(Material material, Shape shape, Facing facing, int variant)
		{
			Entity entity = null;
			if (blocks[material] != null)
			{
				if (blocks[material][shape] != null)
				{
					if (blocks[material][shape][facing] != null)
					{

						if (blocks[material][shape][facing][variant] != null)
						{
							entity = new Entity(blocks[material][shape][facing][variant]);
						}
					}
				}
			}
			return entity;
		}

		private void AddBlock(Material material, Shape shape, Facing facing, Entity entity)
		{
			blocks[material][shape][facing].Add(new Entity(entity));
		}

		private void InitBlocks()
		{
			InitDirtBlocks();
		}

		private void InitDirtBlocks()
		{
			ShaderInfo.Material dirtMaterial;
			dirtMaterial.Emissive = new Color(50, 45, 40, 255);
			dirtMaterial.Diffuse = new Color(100, 150, 100, 255);
			dirtMaterial.Specular = new Color(255, 255, 255, 255);
			dirtMaterial.Shininess = 0.8f;
			TileLightingInfo dirtLighting = new TileLightingInfo("DirtTilesNormal", "DirtTextureNormal", dirtMaterial);

			int x = 0;
			int y = 0;
			const int spriteSize = 16;
			const int spacing = 2;
			foreach (Facing f in (Facing[])Enum.GetValues(typeof(Facing)))
			{
				for (int i = 0; i < 3; i++)
				{
					int startX = (spacing / 2) + (spriteSize + spacing) * x;
					int startY = (spacing / 2) + (spriteSize + spacing) * y;

					Renderable r = new Renderable(null, "DirtBlockNone", "DirtTiles", new Vector2(7.5f), new Vector2(0), new Vector2(1), 16, 16, startX, startY, 0.1f);
					r.Drawable.ShaderInfo = dirtLighting;
					r.DrawBatched = true;
					ColliderCategory colliderCategory = ColliderCategory.Terrain;
					ColliderCategory collidesWith = ColliderCategory.All & ~ColliderCategory.Trigger;
					AABBCollider c = new AABBCollider(null, "DirtBlock" + i, new Vector2(0), 0.5f, 0.5f, colliderCategory, collidesWith);
					Entity e = new Entity();
					e.Components.Add(r);
					e.Components.Add(c);
					AddBlock(Material.Dirt, Shape.Block, f, e);

					x++;
					if (x > 6)
					{
						x = 0;
						y++;
					}
				}
			}
		}
	}
}
