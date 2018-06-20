using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	class Camera
	{
		protected float _viewportWidth;
		protected float _viewportHeight;
		protected float _zoom;
		protected Matrix _transform;
		protected Vector2 _position;
		protected float _rotation;

		public Camera()
		{
			_viewportWidth = 1920;
			_viewportHeight = 1080;
			_zoom = 1.0f;
			_rotation = 0.0f;
			_position = Vector2.Zero;
		}

		public Camera(float width, float height)
		{
			_viewportWidth = width;
			_viewportHeight = height;
			_zoom = 1.0f;
			_rotation = 0.0f;
			_position = Vector2.Zero;
		}

		public float Zoom
		{
			get { return _zoom; }
			set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
		}

		public float Rotation
		{
			get { return _rotation; }
			set { _rotation = value; }
		}

		public void Move(Vector2 amount)
		{
			_position += amount;
		}

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}


		public Matrix get_transformation(GraphicsDevice graphicsDevice)
		{
			_transform = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
													Matrix.CreateRotationZ(Rotation) *
													Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
													Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f, _viewportHeight * 0.5f, 0));
			return _transform;
		}
	}
}
