using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Vector2SurrogateSelector : ISerializationSurrogate
	{
		public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
		{

			Vector2 vec = (Vector2)obj;
			info.AddValue("X", vec.X);
			info.AddValue("Y", vec.Y);
		}

		// Method called to deserialize a Vector3 object
		public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector2 vec = (Vector2)obj;
			vec.X = (float)info.GetValue("X", typeof(float));
			vec.Y = (float)info.GetValue("Y", typeof(float));
			obj = vec;
			return obj;
		}
	}

	public class RectangleSurrogateSelector : ISerializationSurrogate
	{
		public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
		{

			Rectangle rec = (Rectangle)obj;
			info.AddValue("X", rec.X);
			info.AddValue("Y", rec.Y);
			info.AddValue("Width", rec.Width);
			info.AddValue("Height", rec.Height);
		}

		// Method called to deserialize a Vector3 object
		public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Rectangle rec = (Rectangle)obj;
			rec.X = (int)info.GetValue("X", typeof(int));
			rec.Y = (int)info.GetValue("Y", typeof(int));
			rec.Width = (int)info.GetValue("Width", typeof(int));
			rec.Height = (int)info.GetValue("Height", typeof(int));
			obj = rec;
			return obj;
		}
	}

	public class ColorSurrogateSelector : ISerializationSurrogate
	{
		public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
		{
			Color color = (Color)obj;
			info.AddValue("R", color.R);
			info.AddValue("G", color.G);
			info.AddValue("B", color.B);
			info.AddValue("A", color.A);
		}

		// Method called to deserialize a Vector3 object
		public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{

			Color color = (Color)obj;
			color.R = (byte)info.GetValue("R", typeof(byte));
			color.G = (byte)info.GetValue("R", typeof(byte));
			color.B = (byte)info.GetValue("R", typeof(byte));
			color.A = (byte)info.GetValue("R", typeof(byte));

			obj = color;
			return obj;
		}
	}
	}
