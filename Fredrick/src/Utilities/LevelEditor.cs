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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Fredrick.src
{
	class LevelEditor
	{
		public LevelEditor()
		{

		}

		public void Save(List<Entity> entities)
		{
			using (Stream stream = File.Open("terrainData", FileMode.Create))
			{
				SurrogateSelector surrogateSelector = new SurrogateSelector();
				Vector2SurrogateSelector vector2SS = new Vector2SurrogateSelector();
				RectangleSurrogateSelector rectangleSS = new RectangleSurrogateSelector();
				ColorSurrogateSelector colorSS = new ColorSurrogateSelector();
				surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);
				surrogateSelector.AddSurrogate(typeof(Rectangle), new StreamingContext(StreamingContextStates.All), rectangleSS);
				surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), colorSS);

				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binaryFormatter.SurrogateSelector = surrogateSelector;
				binaryFormatter.Serialize(stream, entities);
			}
		}

		public List<Entity> LoadTerrain(ContentManager Content)
		{
			List<Entity> _terrain = new List<Entity>();

			using (Stream stream = File.Open("terrainData", FileMode.Open))
			{
				SurrogateSelector surrogateSelector = new SurrogateSelector();
				Vector2SurrogateSelector vector2SS = new Vector2SurrogateSelector();
				RectangleSurrogateSelector rectangleSS = new RectangleSurrogateSelector();
				ColorSurrogateSelector colorSS = new ColorSurrogateSelector();
				surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);
				surrogateSelector.AddSurrogate(typeof(Rectangle), new StreamingContext(StreamingContextStates.All), rectangleSS);
				surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), colorSS);

				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binaryFormatter.SurrogateSelector = surrogateSelector;
				_terrain = (List<Entity>)binaryFormatter.Deserialize(stream);
			}

			return _terrain;
		}

		public void AddObject(Vector2 position, int blockID)
		{
		}

		public void RemoveObject(Vector2 position)
		{

		}
	}
}
