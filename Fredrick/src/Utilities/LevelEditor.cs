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

		public void Save()
		{
			var serializer = new XmlSerializer(_terrainData.GetType());
			using (var writer = XmlWriter.Create("TerrainData.xml"))
			{
				serializer.Serialize(writer, _terrainData);
			}
		}

		public List<Entity> LoadTerrain(ContentManager Content)
		{
			List<Entity> _terrain = new List<Entity>();

			var serializer = new XmlSerializer(typeof(List<Tuple<Tuple<int, int>, Vector2>>));
			using (var reader = XmlReader.Create("TerrainData.xml"))
			{
				_terrainData = (List<Tuple<Tuple<int, int>, Vector2>>)serializer.Deserialize(reader);
			}

			foreach (Tuple<Tuple<int, int>, Vector2> t in _terrainData)
			{
				Block b = _blocks[t.Item1];
				Entity e = new Entity();
				e.SetPosition(t.Item2);
				if (b.renderableData != null)
				{
					RenderableData rd = b.renderableData;
					Texture2D tex = Content.Load<Texture2D>(rd.texture);
					Renderable r = new Renderable(e, tex, rd.origin, rd.position, rd.scale, rd.width, rd.height, rd.layer);
					e.Components.Add(r);
				}
				if (b.aabbData != null)
				{
					AABBData cd = b.aabbData;
					AABBCollider c = new AABBCollider(e, cd.position, cd.width, cd.height);
					e.Components.Add(c);
				}
				_terrain.Add(e);
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
