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
		public Entity Subject { get; set; } //The object being followed
		public Vector2 OffsetAmount { get; set; }
		public Vector2 GoalOffset { get; set; }
		public Vector2 CurrentOffset { get; set; }
		public float CameraSpeed;

		private Vector2 _shakenPosition;
		private float _offsetPositionScale;

		private float _shakenRotation { get; set; }
		private float _offsetRotationScale { get; set; }

		public double Trauma { get; set; }
		private double _traumaDecay;

		/// <summary>
		/// A counter used for sampling noise
		/// </summary>
		private float _sampleCounter;
		private const float _counterReset = 1000.0f;//Required as counter gives less varied noise when it excedes a threshold

		private FastNoise _noiseOffX;
		private FastNoise _noiseOffY;
		private FastNoise _noiseRot;

		private Random rnd;
		private int _seed;

		FollowCamera()
		{
			ViewportWidth = 1920;
			ViewportHeight = 1080;
			Zoom = 1.0f;
			Rotation = 0.0f;
			Position = Vector2.Zero;

			_offsetPositionScale = 1;
			_offsetRotationScale = 0.2f;
			_traumaDecay = 2.0f;

			_sampleCounter = 0;

			rnd = new Random();
			_seed = rnd.Next(int.MaxValue);

			_noiseOffX = new FastNoise(_seed);
			_noiseOffY = new FastNoise(_seed + 1);
			_noiseRot = new FastNoise(_seed + 2);

			_noiseOffX.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseOffY.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseRot.SetNoiseType(FastNoise.NoiseType.Perlin);
		}

		public FollowCamera(float width, float height, Entity subject, float zoom = 1.0f, float offsetPositionScale = 1.0f, float offsetRotationScale = 0.2f, float cameraSpeed = 1.0f)
		{
			ViewportWidth = width;
			ViewportHeight = height;
			Subject = subject;

			Zoom = zoom;
			Rotation = 0.0f;
			Position = Vector2.Zero;

			_offsetPositionScale = offsetPositionScale;
			_offsetRotationScale = offsetRotationScale;
			_traumaDecay = 1.0f;
			CameraSpeed = cameraSpeed;

			_sampleCounter = 0;

			rnd = new Random();
			_seed = rnd.Next(int.MaxValue);

			_noiseOffX = new FastNoise(_seed);
			_noiseOffY = new FastNoise(_seed + 1);
			_noiseRot = new FastNoise(_seed + 2);

			_noiseOffX.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseOffY.SetNoiseType(FastNoise.NoiseType.Perlin);
			_noiseRot.SetNoiseType(FastNoise.NoiseType.Perlin);
		}

		public void UpdateFollowPosition(double deltaTime)
		{
			if (Subject != null)
			{
				if (Subject.GetComponent<Character>() != null)
				{
					//Needs to be moved to trauma probe class
					if (Subject.GetComponent<Character>().Grounded && !Subject.GetComponent<Character>().PrevGrounded)
					{
						Trauma += (-Subject.GetComponent<Character>().FallVelocity * 5.0f);
					}

					GoalOffset = Subject.GetComponent<Character>().Velocity.Length() > 0.1f ? Vector2.Normalize(Subject.GetComponent<Character>().Velocity) : new Vector2(0);
					GoalOffset *= OffsetAmount;
					if (float.IsNaN(GoalOffset.X) || float.IsNaN(GoalOffset.Y))
						GoalOffset = new Vector2(0);
					CurrentOffset += (GoalOffset - CurrentOffset) * (float)deltaTime * CameraSpeed;
				}
				Position = Subject.Position + CurrentOffset;
			}
		}

		public new void Update(double deltaTime)
		{
			UpdateFollowPosition(deltaTime);

			_sampleCounter += (float)deltaTime * 1000;
			if (_sampleCounter > _counterReset)
				_sampleCounter = 0;

			Trauma -= deltaTime / _traumaDecay;

			if (Trauma > 1)
				Trauma = 1;

			if (Trauma < 0)
				Trauma = 0;

			float shake = (float)(Trauma * Trauma * Trauma);

			Vector2 shakePositionOffset = new Vector2
			{
				X = _noiseOffX.GetNoise(_sampleCounter, _sampleCounter) * shake * _offsetPositionScale,
				Y = _noiseOffY.GetNoise(_sampleCounter, _sampleCounter) * shake * _offsetPositionScale
			};

			float shakeRotationOffset = _noiseRot.GetNoise(_sampleCounter, _sampleCounter) * shake * _offsetRotationScale;

			_shakenPosition = Position + shakePositionOffset;
			_shakenRotation = Rotation + shakeRotationOffset;
		}

		public new Matrix Get_Transformation(GraphicsDevice graphicsDevice)
		{
			_transform = Matrix.CreateTranslation(new Vector3(-_shakenPosition.X * 32, _shakenPosition.Y * 32, 0)) *
													Matrix.CreateRotationZ(_shakenRotation) *
													Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
													Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
			return _transform;
		}
	}
}
