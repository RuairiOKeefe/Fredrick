using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	class LevelEditor
	{
		public class RenderableData
		{
			public String texture;
			public Vector2 origin;
			public Vector2 position;
			public Vector2 scale;
			public int width;
			public int height;
			public float layer;
		}

		public class AABBData
		{
			public Vector2 position;
			public float width;
			public float height;
		}

		public class Block
		{
			String name;
			RenderableData renderableData;
			AABBData aabbData;
		}

		private Dictionary<int, Block> _blocks;

		public LevelEditor()
		{
			_blocks = new Dictionary<int, Block>();

		}

		public void Save()
		{

		}

		public void Load()
		{

		}

		public void AddObject(Vector2 position, int blockID)
		{
			Block b = _blocks[blockID];
		}

		public void RemoveObject(Vector2 position)
		{

		}
	}
}
