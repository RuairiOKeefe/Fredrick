using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
			public String name;
			public RenderableData renderableData;
			public AABBData aabbData;
		}

		private Dictionary<Tuple<int, int>, Block> _blocks;
		private List<Tuple<Tuple<int, int>, Vector2>> _terrainData;

		public LevelEditor()
		{
			_blocks = new Dictionary<Tuple<int, int>, Block>();
			_blocks.Add(new Tuple<int, int>(1, 1), new Block());

		}

		public void Save(List<Entity> entities)
		{
			var terrainSerializer = new XmlSerializer(entities.GetType());
			using (var writer = XmlWriter.Create("TerrainData.xml"))
			{
				terrainSerializer.Serialize(writer, entities);
			}
		}

		public List<Entity> LoadTerrain(ContentManager Content)
		{
			List<Entity> _terrain = new List<Entity>();

			var terrainSerializer = new XmlSerializer(typeof(List<Entity>));
			using (var reader = XmlReader.Create("TerrainData.xml"))
			{
				_terrain = (List<Entity>)terrainSerializer.Deserialize(reader);
			}

			return _terrain;
		}

		public void AddObject(Vector2 position, int blockID)
		{
			Block b = _blocks[new Tuple<int, int>(blockID, blockID)];
		}

		public void RemoveObject(Vector2 position)
		{

		}
	}
}
