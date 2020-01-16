using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class Drawable
	{
		public String _spriteName;
		public int _spriteSize;
		public Rectangle _sourceRectangle;//The region of the sprite sheet that will be used
		public Vector2 _origin;//The centerpoint of the sprite
		public SpriteEffects _spriteEffects; //just controls flipping from the look of it leave as 0 for none
		public float _layer;
		public Color _colour;
		public int _width;
		public int _height;
		public int _spriteWidth;
		public int _spriteHeight;

		public ShaderInfo ShaderInfo;

		public List<Animation> _animations;//Stores an int key
		public int _currentAnim;//which animation is currently being used

		public Drawable()
		{
		}

		public Drawable(string spriteName, Vector2 origin, int width = 32, int height = 32, float layer = 0.1f)
		{
			_spriteName = spriteName;
			_spriteSize = 32;
			_origin = origin;
			_width = width;
			_height = height;
			_layer = layer;
			_colour = new Color(255, 255, 255, 255);

			_sourceRectangle = new Rectangle(0, 0, _width, _height);
			_animations = new List<Animation>();
			_currentAnim = 0;
		}

		public Drawable(Drawable original)
		{
			_spriteName = original._spriteName;
			_spriteSize = original._spriteSize;
			_origin = original._origin;
			_width = original._width;
			_height = original._height;
			_layer = original._layer;
			_colour = original._colour;

			_sourceRectangle = original._sourceRectangle;
			_animations = new List<Animation>();
			_animations = original._animations;
			_currentAnim = 0;
		}

		public void Load(ContentManager content)
		{
			ResourceManager.Instance.AddTexture(content, _spriteName);
			_spriteWidth = ResourceManager.Instance.Textures[_spriteName].Width;
			_spriteHeight = ResourceManager.Instance.Textures[_spriteName].Height;
			foreach (Animation a in _animations)
			{
				a.SpriteWidth = _spriteWidth;
				a.SpriteHeight = _spriteHeight;
			}
		}

		public void AddAnimation(int startX, int startY, int frames, float frameRate, Animation.OnEnd onEnd, int nextAnim)
		{
			_animations.Add(new Animation(_spriteWidth, _spriteHeight, startX, startY, _width, _height, frames, frameRate, onEnd, nextAnim));
			if (_animations.Count == 1)
			{
				_currentAnim = 0;
			}
		}

		public void TransitionAnim(int nextAnim)
		{
			if (nextAnim != _currentAnim)
			{
				_animations[nextAnim].TransitionInAnim(_animations[_currentAnim].GetNextFrame());
				_currentAnim = nextAnim;
			}
		}

		public void RestartAnim()
		{
			_animations[_currentAnim].TransitionInAnim(_animations[_currentAnim].GetNextFrame());
		}

		public void Animate(double deltaTime)
		{
			if (_animations.Count > 0)
			{
				bool transition;
				_sourceRectangle.Location = _animations[_currentAnim].UpdateAnimation(deltaTime, out transition);
				if (transition)
					TransitionAnim(_animations[_currentAnim].NextAnim);
			}
		}
	}
}
