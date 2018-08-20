﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public sealed class ProjectileBuffer
	{
		private static ProjectileBuffer _instance = null;
		private static readonly object _padlock = new object();

		public static ProjectileBuffer Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new ProjectileBuffer();
					return _instance;
				}
			}
		}

		public const int NUM_PROJECTILES = 10000;

		private Stack<Entity> _inactiveProjectiles;


		public Stack<Entity> InactiveProjectiles
		{
			get { return _inactiveProjectiles; }
			set { _inactiveProjectiles = value; }
		}

		public void Load(Texture2D t)
		{
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity();
				Projectile p = new Projectile(e);
				CircleCollider cc = new CircleCollider(e);
				Renderable r = new Renderable(e, t);

				e.Components.Add(p);
				e.Components.Add(cc);
				e.Components.Add(r);

				_inactiveProjectiles.Push(e);//need to improve
			}
		}

		public ProjectileBuffer()
		{
			_inactiveProjectiles = new Stack<Entity>(NUM_PROJECTILES);

		}
	}
}