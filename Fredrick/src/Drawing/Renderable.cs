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
	public class Renderable : Component
	{
		public override bool IsDrawn { get { return true; } }

		protected bool _facingRight;

		public Drawable Drawable { get; set; }

		public Renderable()
		{
			_owner = null;
			Position = new Vector2(0);
			Scale = new Vector2(1);
		}

		public Renderable(Entity owner, string id, string spriteName, Vector2 origin, Vector2 position, Vector2 scale, int width = 32, int height = 32, int startX = 0, int startY = 0, float layer = 0.1f) : base(owner, id)
		{
			Drawable = new Drawable(spriteName, origin, width, height, startX, startY, layer);
			Position = position;
			Scale = scale;
			_facingRight = true;
		}

		public Renderable(Entity owner, Renderable original) : base(owner, original.Id, original.Tags, original.Active)
		{
			Position = original.Position;
			Rotation = original.Rotation;
			Scale = original.Scale;
			Drawable = new Drawable(original.Drawable);
			_facingRight = true;
		}

		public void Flip(bool faceRight)
		{
			if (faceRight != _facingRight)
			{
				_facingRight = !_facingRight;
				if (_facingRight)
				{
					Drawable._spriteEffects = SpriteEffects.None;
				}
				else
				{
					Drawable._spriteEffects = SpriteEffects.FlipHorizontally;
				}
			}
		}

		public override void Load(ContentManager content)
		{
			Drawable.Load(content);
			if (Drawable.ShaderInfo != null)
			{
				ShaderId = Drawable.ShaderInfo.ShaderId;
			}
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			Drawable.Animate(deltaTime);
		}

		public override void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformationMatrix)
		{
			if (Drawable.ShaderInfo != null)
				Drawable.ShaderInfo.SetUniforms(shader);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, shader, transformationMatrix);
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (Position + _owner.Position) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, _owner.Rotation + Rotation, Drawable._origin, Scale, Drawable._spriteEffects, Drawable._layer);
			spriteBatch.End();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (Position + _owner.Position) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, _owner.Rotation + Rotation, Drawable._origin, Scale, Drawable._spriteEffects, Drawable._layer);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Renderable(owner, this);
		}
	}
}
