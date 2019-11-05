using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class StatusHandler : Component
	{
		[Serializable]
		public class Timer
		{
			public double TimeRemaining { get; set; }
			public double NextTick { get; set; }

			public Timer(double timeRemaining, double nextTick = 0)
			{
				TimeRemaining = timeRemaining;
				NextTick = nextTick;
			}
		}

		public const double TickRate = 10;

		public List<Tuple<StatusEffect, Timer>> Statuses { get; private set; } = new List<Tuple<StatusEffect, Timer>>();

		public StatusHandler()
		{

		}

		public StatusHandler(Entity owner, string id) : base(owner, id)
		{

		}

		public StatusHandler(Entity owner, StatusHandler original):base(owner, original.Id)
		{
			Statuses = original.Statuses;
		}

		public void AddStatus(StatusEffect status)
		{
			double nextTick = 0;
			foreach (var s in Statuses)
			{
				if (s.Item1.GetType() == status.GetType())
				{
					nextTick = s.Item2.NextTick;
					Statuses.Remove(s);
					break;
				}
			}
			Statuses.Add(new Tuple<StatusEffect, Timer>(status, new Timer(status.Duration, nextTick)));
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
			for (int i = Statuses.Count - 1; i >= 0; i--)
			{
				Tuple<StatusEffect, Timer> s = Statuses[i];
				s.Item1.Update(ref _owner, deltaTime);
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
					Statuses.Remove(s);
				}
			}
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			foreach (var s in Statuses)
			{
				s.Item1.Draw(ref _owner, spriteBatch);
			}
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new StatusHandler(owner, this);
		}
	}
}
