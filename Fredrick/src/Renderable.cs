using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Renderable : Component
	{
		protected Texture2D _sprite;
		protected int _spriteSize;
		protected Rectangle _sourceRectangle;//The region of the sprite sheet that will be used
		protected Vector2 _origin;//The centerpoint of the sprite
		protected SpriteEffects _spriteEffects; //just controls flipping from the look of it leave as 0 for none
		protected float _layer;
		protected Color _colour;
		protected int _width;
		protected int _height;
		protected Dictionary<int, Animation> _animations;//Stores an int key
		protected int _currentAnim;//which animation is currently being used
		protected bool _transition;//Does a transition need to occur
		protected int _nextAnim;//The animation to be transitioned to

		public Renderable(Entity owner, Texture2D sprite) : base(owner)
		{
			_sprite = sprite;
			_spriteSize = 32;

			_origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(1, 1);
			_layer = 1;
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

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(_sprite, (_position + _owner.GetPosition()) * inv * _spriteSize, _sourceRectangle, _colour, _rotation, _origin, _scale, _spriteEffects, _layer);
		}

		public override void Update(double deltaTime)
		{
			if (_animations.Count > 0)
			{
				Point sourcePos = _animations[_currentAnim].UpdateAnimation(deltaTime);
				_sourceRectangle.Location = sourcePos;
				if (_transition)
					TryTransition();
			}
		}
	}
}
