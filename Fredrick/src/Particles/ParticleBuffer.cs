using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Fredrick.src
{
	public class ParticleBuffer
	{
		private static ParticleBuffer _instance = null;
		private static readonly object _padlock = new object();

		public static ParticleBuffer Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new ParticleBuffer();
					return _instance;
				}
			}
		}

		public const int NUM_PARTICLES = 2000000;

		private Stack<Particle> _inactiveParticles;

		public Stack<Particle> InactiveParticles
		{
			get { return _inactiveParticles; }
			set { _inactiveParticles = value; }
		}

		public ParticleBuffer()
		{
			_inactiveParticles = new Stack<Particle>(NUM_PARTICLES);
			for (int i = 0; i < NUM_PARTICLES; i++)
			{
				_inactiveParticles.Push(new Particle());
			}
		}
	}
}