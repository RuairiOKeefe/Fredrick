using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Camera
	{
		public float ViewportWidth;
		public float ViewportHeight;
		public float Zoom;
		protected Matrix _transform;
		public Vector2 Position;
		public float Rotation;

		public Camera()
		{
			ViewportWidth = 1920;
			ViewportHeight = 1080;
			Zoom = 1.0f;
			Rotation = 0.0f;
			Position = Vector2.Zero;
		}

		public Camera(float width, float height)
		{
			ViewportWidth = width;
			ViewportHeight = height;
			Zoom = 1.0f;
			Rotation = 0.0f;
			Position = Vector2.Zero;
		}

		public virtual void Update(double deltaTime)
		{

		}

		public Matrix Get_Transformation(GraphicsDevice graphicsDevice)
		{
			_transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
													Matrix.CreateRotationZ(Rotation) *
													Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
													Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
			return _transform;
		}
	}
}
