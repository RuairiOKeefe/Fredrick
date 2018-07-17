using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class FollowCamera : Camera
	{
		protected Vector2 _shakenPosition;
		protected Vector2 _offset;
		protected double _offsetPosScale;

		float _shakenRotation;
		float _offsetRotation;
		float _offsetRotScale;

		protected double _trauma;
		protected double _shake;

		float _time;

		private FastNoise _noiseOffX;
		private FastNoise _noiseOffY;
		private FastNoise _noiseRot;

		Random rnd;
		private int _seed;

		Entity _subject; //The object being followed

		public void SetSubject(Entity subject)
		{
			_subject = subject;
		}

		public double Trauma
		{
			get { return _trauma; }
			set { _trauma = value; }
		}

		FollowCamera()
		{
			_viewportWidth = 1920;
			_viewportHeight = 1080;
			_zoom = 1.0f;
			_rotation = 0.0f;
			_position = Vector2.Zero;

			_offsetPosScale = 1;
			_offsetRotScale = 0.2f;

			_time = 0;

			rnd = new Random();
			_seed = rnd.Next(int.MaxValue);

			_noiseOffX = new FastNoise(_seed);
			_noiseOffY = new FastNoise(_seed + 1);
			_noiseRot = new FastNoise(_seed + 2);

			_noiseOffX.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseOffY.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseRot.SetNoiseType(FastNoise.NoiseType.Perlin);
		}

		public FollowCamera(float width, float height)
		{
			_viewportWidth = width;
			_viewportHeight = height;
			_zoom = 2.0f;
			_rotation = 0.0f;
			_position = Vector2.Zero;

			_offsetPosScale = 1;
			_offsetRotScale = 0.2f;

			_time = 0;

			rnd = new Random();
			_seed = rnd.Next(int.MaxValue);

			_noiseOffX = new FastNoise(_seed);
			_noiseOffY = new FastNoise(_seed + 1);
			_noiseRot = new FastNoise(_seed + 2);

			_noiseOffX.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseOffY.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseRot.SetNoiseType(FastNoise.NoiseType.Perlin);
		}

		public new void Update(double deltaTime)
		{
			if (_subject.GetComponent<Character>() != null)
			{
				_position += (_subject.GetComponent<Character>().FollowPosition - _position) * 10.0f * (float)deltaTime;
				if (_subject.GetComponent<Character>().Grounded && !_subject.GetComponent<Character>().PrevGrounded)
				{
					_trauma = (-_subject.GetComponent<Character>().FallVelocity / 10);
					if (_trauma > 1)
						_trauma = 1;
				}
			}
			_time += (float)deltaTime * 1000;
			_trauma -= deltaTime;
			if (_trauma < 0)
				_trauma = 0;

			_shake = _trauma * _trauma * _trauma;

			_offset.X = _noiseOffX.GetNoise(_time, _time) * (float)_shake * (float)_offsetPosScale;
			_offset.Y = _noiseOffY.GetNoise(_time, _time) * (float)_shake * (float)_offsetPosScale;
			_offsetRotation = _noiseRot.GetNoise(_time, _time) * (float)_shake * (float)_offsetRotScale;

			_shakenPosition = _position + _offset;
			_shakenRotation = _rotation + _offsetRotation;
		}

		public new Matrix Get_Transformation(GraphicsDevice graphicsDevice)
		{
			_transform = Matrix.CreateTranslation(new Vector3(-_shakenPosition.X * 32, _shakenPosition.Y * 32, 0)) *
													Matrix.CreateRotationZ(_shakenRotation) *
													Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
													Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f, _viewportHeight * 0.5f, 0));
			return _transform;
		}
	}
}
