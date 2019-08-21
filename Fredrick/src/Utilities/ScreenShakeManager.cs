using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public sealed class ScreenShakeManager
	{
		private static ScreenShakeManager _instance = null;
		private static readonly object _padlock = new object();

		public static ScreenShakeManager Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
					{
						_instance = new ScreenShakeManager();
					}
					return _instance;
				}
			}

		}

		private class LocalTrauma
		{
			public float m_trauma;
			public Vector2 m_position;
			public bool m_global;


			public LocalTrauma(float trauma)
			{
				m_trauma = trauma;
				m_position = new Vector2(0);
				m_global = true;
			}

			public LocalTrauma(float trauma, Vector2 position)
			{
				m_trauma = trauma;
				m_position = position;
				m_global = false;
			}

			public void TraumaDecay(float decay)
			{
				m_trauma -= decay;
			}
		}

		private const float TraumaDecay = 3.0f;//seconds taken to completely deplete
		private const float CloseRange = 5.0f;
		private const float TraumaHalfLife = 3.0f;

		List<LocalTrauma> m_traumas;



		public ScreenShakeManager()
		{
			m_traumas = new List<LocalTrauma>();

		}

		public void AddTrauma(float trauma)
		{
			m_traumas.Add(new LocalTrauma(trauma));
		}

		public void AddTrauma(float trauma, Vector2 position)
		{
			m_traumas.Add(new LocalTrauma(trauma, position));
		}

		public void Update(double deltaTime)
		{
			float traumaDecayDelta = (float)deltaTime / TraumaDecay;

			for (int i = (m_traumas.Count - 1); i >= 0; i--)
			{
				m_traumas[i].TraumaDecay(traumaDecayDelta);
				if (m_traumas[i].m_trauma < 0)
				{
					m_traumas.RemoveAt(i);
				}
			}
		}

		public float GetScreenShake(Vector2 detectorPosition, float scale = 1.0f /* unused for now*/)
		{
			float screenShake = 0;
			float totalTrauma = 0;

			foreach (LocalTrauma localTrauma in m_traumas)
			{
				if (!localTrauma.m_global)
				{
					float distance = Vector2.Distance(detectorPosition, localTrauma.m_position);

					if (distance < CloseRange)
					{
						distance = 1.0f;
					}
					else
					{
						distance -= CloseRange;
						distance /= TraumaHalfLife;
						distance += 1.0f;
					}

					totalTrauma += (localTrauma.m_trauma / distance);
				}
				else
				{
					totalTrauma += localTrauma.m_trauma;
				}
			}

			if (totalTrauma > 1.0f)
			{
				totalTrauma = 1.0f;
			}

			screenShake = totalTrauma * totalTrauma * totalTrauma;

			return screenShake;
		}
	}
}
