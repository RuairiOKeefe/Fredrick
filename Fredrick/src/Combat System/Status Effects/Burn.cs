using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Burn : StatusEffect
	{
		public override int ParticleDensity { get { return 200; } }
		public override float Magnitude { get { return 10.0f; } }
		public override double Duration { get { return 5.0; } }
		public override List<Tuple<Emitter, Component>> Emitters { get; }

		public Burn()
		{
			Emitters = new List<Tuple<Emitter, Component>>();
		}

		public override void Begin(ref Entity target)
		{
			foreach (Component c in target.Components)
			{
				if (c.Id == "Burn")
				{
					(c as Emitter).MaxParticles = 0;//Mark for deletion once particle count reaches zero
				}

				if (c is Renderable && c.Tags.Contains("Body"))
				{
					Emitter burn = new Emitter(target, Resources.Instance.Emitters["Fire"]);
					Emitters.Add(new Tuple<Emitter, Component>(burn, c));
					burn.SpawnWidth = (c as Renderable).Drawable._width / 2 / (c as Renderable).Drawable._spriteSize;//Find a way to remove this magic
					burn.SpawnHeight = (c as Renderable).Drawable._height / (c as Renderable).Drawable._spriteSize;
					burn.MaxParticles = (int)((c as Renderable).Drawable._width / (c as Renderable).Drawable._spriteSize * (c as Renderable).Drawable._height / (c as Renderable).Drawable._spriteSize * ParticleDensity);
					burn.EmissionCount = Emitters[Emitters.Count - 1].Item1.MaxParticles / 100;
					burn.Id = "Burn";
				}
			}
			foreach (Tuple<Emitter, Component> e in Emitters)
			{
				target.Components.Add(e.Item1);
			}
		}

		public override void End(ref Entity target)
		{
			for (int i = target.Components.Count - 1; i >= 0; i--)
			{
				Component c = target.Components[i];
				if (c.Id == "Burn")
				{
					target.Components.Remove(c);
				}
			}
			Emitters.Clear();
		}

		public override void Tick(ref Entity target)
		{
			if (target.GetComponent<Damageable>() != null)
			{
				target.GetComponent<Damageable>().DealDamage(new Attack(Attack.DamageType.Fire, null, Magnitude));
			}
		}

		public override void Update(ref Entity target, double deltaTime)
		{
			foreach (Tuple<Emitter, Component> e in Emitters)
			{
				e.Item1.Position = e.Item2.Owner.Position + e.Item2.Position;
				e.Item1.Rotation = e.Item2.Owner.Rotation + e.Item2.Rotation;
			}
		}

		public override void Draw(ref Entity target, SpriteBatch spriteBatch)
		{

		}

		public override StatusEffect Copy()
		{
			return new Burn();
		}
	}
}
