using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	class Animation
	{
		private int _spriteWidth;
		private int _spriteHeight;
		private int _startX;
		private int _startY;
		private int _width;
		private int _height;
		private int _currentX;
		private int _currentY;

		private int _frames;//number of frames in the animaion
		private int _currentFrame;
		private double _framerate;
		private double _frameTime;
		private double _nextFrame;

		public Animation()//Constructor shouldn't be used as there are no way to set fields currently, as animations shouldn't be dynamic
		{
			_spriteWidth = 0;
			_spriteHeight = 0;
			_startX = 0;
			_startY = 0;
			_width = 0;
			_height = 0;

			_frames = 0;
			_framerate = 30;

			_currentX = _startX;
			_currentY = _startY;
			_currentFrame = 0;
			_frameTime = 1.0 / _framerate;
			_nextFrame = 0;
		}

		public Animation(int spriteWidth, int spriteHeight, int startX, int startY, int width, int height, int frames, float framerate)
		{
			this._spriteWidth = spriteWidth;
			this._spriteHeight = spriteHeight;
			this._startX = startX;
			this._startY = startY;
			this._width = width;
			this._height = height;

			this._frames = frames;
			this._framerate = framerate;


			_currentX = this._startX;
			_currentY = this._startY;
			_currentFrame = 0;
			_frameTime = 1.0 / _framerate;
			_nextFrame = 0;
		}

		public Point UpdateAnimation(double deltaTime)
		{
			_nextFrame += deltaTime;
			if (_nextFrame >= _frameTime)
			{
				_nextFrame -= _frameTime;
				_currentFrame++;
				if (_currentFrame >= _frames)
					_currentFrame = 0;

				if (_currentFrame == 0)
				{
					_currentX = _startX;
					_currentY = _startY;
				}
				else
				{
					_currentX += _width;
					if (_currentX >= _spriteWidth)
					{
						_currentX = 0;
						_currentY += _height;
					}
					if (_currentY >= _spriteHeight)
					{
						_currentX = 0;
						_currentY = 0;
					}
				}
			}

			return new Point(_currentX, _currentY);
		}
	}
}
