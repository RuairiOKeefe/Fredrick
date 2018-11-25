using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class StatusHandler : Component
	{
		public class Timer
		{
			public double TimeRemaining { get; set; }
			public double NextTick { get; set; }

			public Timer(double timeRemaining)
			{
				TimeRemaining = timeRemaining;
				NextTick = 0;
			}
		}

		public const double TickRate = 10;

		public List<Tuple<StatusEffect, Timer>> Statuses { get; private set; }

		public StatusHandler(Entity owner, string id) : base(owner, id)
		{
			Statuses = new List<Tuple<StatusEffect, Timer>>();
		}

		public void AddStatus(StatusEffect status)
		{
			foreach (var s in Statuses)
			{
				if (s.Item1.GetType() == status.GetType())
				{
					Statuses.Remove(s);
					break;
				}
			}
			Statuses.Add(new Tuple<StatusEffect, Timer>(status, new Timer(status.Duration)));
			status.Begin(ref _owner);
		}

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			foreach (var s in Statuses)
			{
				s.Item2.NextTick -= deltaTime;
				s.Item2.TimeRemaining -= deltaTime;

				if (s.Item2.NextTick <= 0)
				{
					s.Item1.Tick(ref _owner);
					s.Item2.NextTick += (1.0 / TickRate);
				}

				if (s.Item2.TimeRemaining <= 0)
				{
					s.Item1.End(ref _owner);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}
