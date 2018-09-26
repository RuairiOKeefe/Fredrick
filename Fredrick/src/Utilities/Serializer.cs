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
	class Serializer
	{
		SurrogateSelector _surrogateSelector;

		public Serializer()
		{
			_surrogateSelector = new SurrogateSelector();
			Vector2SurrogateSelector vector2SS = new Vector2SurrogateSelector();
			RectangleSurrogateSelector rectangleSS = new RectangleSurrogateSelector();
			ColorSurrogateSelector colorSS = new ColorSurrogateSelector();
			_surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);
			_surrogateSelector.AddSurrogate(typeof(Rectangle), new StreamingContext(StreamingContextStates.All), rectangleSS);
			_surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), colorSS);
		}

		public void Save(string filename, List<Entity> entities)
		{
			using (Stream stream = File.Open(filename, FileMode.Create))
			{
				

				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binaryFormatter.SurrogateSelector = _surrogateSelector;
				binaryFormatter.Serialize(stream, entities);
			}
		}

		public List<Entity> Load(string filename)
		{
			List<Entity> entities = new List<Entity>();

			using (Stream stream = File.Open(filename, FileMode.Open))
			{
				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binaryFormatter.SurrogateSelector = _surrogateSelector;
				entities = (List<Entity>)binaryFormatter.Deserialize(stream);
			}

			return entities;
		}
	}
}
