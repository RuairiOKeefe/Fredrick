using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public abstract class StatusEffect
	{
		public enum Effect
		{
			Burning,
			Electrocuted,
			Slowed,
			Stunned,
			Blinded

		}

		public const int tickrate = 10;
		private double _nextTick;

		public float Magnitude { get; set; }
		public double Duration { get; set; }
		public double TimeRemaining { get; }

		public StatusEffect()
		{
			TimeRemaining = Duration;
		}

		public abstract void Begin(ref Entity target);
		public abstract void End(ref Entity target);
		public abstract void Tick(ref Entity target);

		public void Update(double deltaTime, Entity target)
		{
			if (TimeRemaining <= 0)
			{
				Begin(ref target);
			}

			_nextTick -= deltaTime;
			Duration -= deltaTime;

			if (_nextTick <= 0)
			{
				Tick(ref target);
				_nextTick = 1.0 / tickrate;
			}

			if (TimeRemaining <= 0)
			{
				End(ref target);
			}
		}
	}
}
