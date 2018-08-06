using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	class Motion
	{
		/*
		 * An rough implementation of rk4, currently not being used but here for prosperity
		 */
		public class Derivative
		{
			public Vector2 dx = new Vector2(0, 0);
			public Vector2 dv = new Vector2(0, 0);
		}

		protected Vector2 _position;//x
		protected Vector2 _velocity;//v
		protected Vector2 _acceleration;

		Derivative compute(double deltaTime, Derivative d)
		{
			Vector2 x = _position + Vector2.Multiply(d.dx, (float)deltaTime);
			Vector2 v = _velocity + Vector2.Multiply(d.dv, (float)deltaTime);

			Derivative output = new Derivative();
			output.dx = v;
			output.dv = _acceleration;

			return output;
		}

		public void UpdateMotion(double deltaTime)
		{
			Derivative a, b, c, d;
			a = compute(0.0f, new Derivative());
			b = compute(deltaTime * 0.5f, a);
			c = compute(deltaTime * 0.5f, b);
			d = compute(deltaTime, c);

			Vector2 dxdt = 1.0f / 6.0f * (a.dx + 2.0f * (b.dx + c.dx) + d.dx);
			Vector2 dvdt = 1.0f / 6.0f * (a.dv + 2.0f * (b.dv + c.dv) + d.dv);

			_position = _position + Vector2.Multiply(dxdt, (float)deltaTime);
			_velocity = _velocity + Vector2.Multiply(dvdt, (float)deltaTime);
		}
	}
}
