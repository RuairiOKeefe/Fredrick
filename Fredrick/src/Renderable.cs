using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	internal class Renderable : Component
	{
		private Texture2D _sprite;
		private Rectangle _sourceRectangle;//The region of the sprite sheet that will be used
		private Vector2 _origin;//The centerpoint of the sprite
		private SpriteEffects _spriteEffects; //just controls flipping from the look of it leave as 0 for none
		private float _layer;
		private Color _colour;
		private int _width;
		private int _height;
		private Dictionary<int, Animation> _animations;//Stores an int key
		private int _currentAnim;//which animation is currently being used

		public Renderable(ComponentOwner owner, Texture2D sprite) : base(owner)
		{
			this._sprite = sprite;

			_origin = new Vector2(16, 16);
			_position = new Vector2(32, 32);
			_scale = new Vector2(1, 1);
			_layer = 1;
			_colour = new Color(255, 255, 255, 255);
			_width = 32;
			_height = 32;
			_sourceRectangle = new Rectangle(0, 0, _width, _height);
			_animations = new Dictionary<int, Animation>();
			_animations.Add(0, new Animation(this._sprite.Width, this._sprite.Height, 0, 0, _width, _height, 4, 30));
			_currentAnim = 0;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_sprite, _position, _sourceRectangle, _colour, _rotation, _origin, _scale, _spriteEffects, _layer);
		}

		public override void Update(double deltaTime)
		{
			Point sourcePos = _animations[_currentAnim].UpdateAnimation(deltaTime);
			_sourceRectangle.Location = sourcePos;
		}
	}
}
