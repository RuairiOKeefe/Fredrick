using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Audio
{
	[Serializable]
	public class Sound
	{

		private List<string> m_soundEffectIds = new List<string>();
		[NonSerialized]
		private List<SoundEffect> m_soundEffects = new List<SoundEffect>();
		private float m_volume = 1f;
		private float m_pitchRange;

		public Sound()
		{ }

		public Sound(Sound original)
		{
			foreach (string s in original.m_soundEffectIds)
			{
				m_soundEffectIds.Add(s);
			}
		}

		public void AddSoundEffect(string soundEffectId)
		{
			m_soundEffectIds.Add(soundEffectId);
		}

		public void Play()
		{
			SoundEffect.MasterVolume = 1;
			m_soundEffects[0].Play();
		}

		public void Load(ContentManager content)
		{
			foreach (string s in m_soundEffectIds)
			{
				SoundEffect soundEffect = content.Load<SoundEffect>(s);
				m_soundEffects.Add(soundEffect);
			}
		}

		public void Unload()
		{

		}

		public Sound Copy()
		{
			return new Sound(this);
		}
	}
}
