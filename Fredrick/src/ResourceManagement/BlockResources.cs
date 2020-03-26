using Fredrick.src.Entity_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.ResourceManagement
{
	public sealed class BlockKey : Tuple<Material, Shape>
	{
		public BlockKey(Material item1, Shape item2) : base(item1, item2)
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

		private Dictionary<BlockKey, Entity> blocks = new Dictionary<BlockKey, Entity>();

		public BlockResources()
		{

		}

		public Entity GetBlock(Material material, Shape shape)
		{
			return new Entity(blocks[new BlockKey(material, shape)]);
		}

		private void AddBlock(Material material, Shape shape, Entity entity)
		{
			blocks.Add(new BlockKey(material, shape), new Entity(entity));
		}

		private void InitBlocks()
		{
			foreach (Shape shape in (Shape[])Enum.GetValues(typeof(Shape)))
			{
				//Empty blocks should not be used anywhere, this is a failsafe
				AddBlock(Material.Empty, shape, null);
			}
			InitDirtBlocks();
		}

		private void InitDirtBlocks()
		{
			AddBlock(Material.Dirt, Shape.Block, new Entity());
		}
	}
}
