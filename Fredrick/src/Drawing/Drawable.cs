using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Drawable
	{
		public Texture2D _sprite;
		public int _spriteSize;
		public Rectangle _sourceRectangle;//The region of the sprite sheet that will be used
		public Vector2 _origin;//The centerpoint of the sprite
		public SpriteEffects _spriteEffects; //just controls flipping from the look of it leave as 0 for none
		public float _layer;
		public Color _colour;
		public int _width;
		public int _height;

		public Dictionary<int, Animation> _animations;//Stores an int key
		public int _currentAnim;//which animation is currently being used
		public bool _transition;//Does a transition need to occur
		public int _nextAnim;//The animation to be transitioned to

		public Drawable(Texture2D sprite)
		{
			_sprite = sprite;
			_spriteSize = 32;

			_origin = new Vector2(16, 16);
			_layer = 0.1f;
			_colour = new Color(255, 255, 255, 255);
			_width = 32;
			_height = 32;
			_sourceRectangle = new Rectangle(0, 0, _width, _height);
			_animations = new Dictionary<int, Animation>();
			_currentAnim = 0;
			_transition = false;
			_nextAnim = 0;
		}

		public void AddAnimation(int key, int startX, int startY, int frames, float frameRate)
		{
			_animations.Add(key, new Animation(this._sprite.Width, this._sprite.Height, startX, startY, _width, _height, frames, frameRate));
			if (_animations.Count == 1)
			{
				_currentAnim = 0;
				_transition = false;
				_nextAnim = 0;
			}
		}

		public void TransitionAnim(int nextAnim)
		{
			if (nextAnim != _currentAnim)
			{
				_nextAnim = nextAnim;
				_transition = true;
			}
		}

		public void TryTransition()
		{
			if (_animations[_currentAnim].GetCurrentFrame() == 0)
			{
				_animations[_nextAnim].TransitionInAnim(_animations[_currentAnim].GetNextFrame());
				_currentAnim = _nextAnim;
				_transition = false;
			}
		}

		public void Animate(double deltaTime)
		{
			if (_animations.Count > 0)
			{
				_sourceRectangle.Location = _animations[_currentAnim].UpdateAnimation(deltaTime);
				if (_transition)
					TryTransition();
			}
		}
	}
}
