using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public abstract class StatusEffect
	{
		public abstract int ParticleDensity { get; }
		public abstract float Magnitude { get; }
		public abstract double Duration { get; }
		public abstract List<Tuple<Emitter, Component>> Emitters { get; }

		public abstract void Begin(ref Entity target);
		public abstract void End(ref Entity target);
		public abstract void Tick(ref Entity target);
		public abstract void Update(ref Entity target, double deltaTime);
		public abstract void Draw(ref Entity target, SpriteBatch spriteBatch);
		public abstract StatusEffect Copy();
	}
}
