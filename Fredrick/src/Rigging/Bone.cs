using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Rigging
{
	[Serializable]
	public class Bone
	{
		public String Id { get; set; }
		public Bone Parent { get; private set; } = null;
		public List<Bone> Children { get; private set; }
		public Drawable Drawable { get; set; }
		public Vector2 Position { get; private set; } //Should only be set by update
		public float Rotation { get; set; }
		public float TransformedRotation { get; private set; }
		public Vector2 Connector { get; set; }//Where the parent will connect
		public Vector2 ChildConnector { get; set; }//Where children will connect
		public float Length { get; private set; }

		Bone()
		{
			Children = new List<Bone>();
		}

		public Bone(string id, Vector2 position, float rotation, Vector2 connector, Vector2 childConnector)
		{
			Id = id;
			Children = new List<Bone>();
			Position = position;
			Rotation = rotation;
			Connector = connector;
			ChildConnector = childConnector;
		}

		public Bone(Bone original, Bone parent = null)
		{
			Id = original.Id;
			if (original.Drawable != null)
				Drawable = new Drawable(original.Drawable);
			Parent = parent;
			Children = new List<Bone>();
			foreach (Bone bone in original.Children)
			{
				Children.Add(new Bone(bone, this));
			}
			Position = original.Position;
			Rotation = original.Rotation;
			Connector = original.Connector;
			ChildConnector = original.ChildConnector;
		}

		~Bone()
		{
			if (Parent != null)
			{
				Parent.RemoveChild(this);
			}
		}

		public void AddParent(Bone parent)
		{
			Parent.AddChild(this);
		}

		public void AddChild(Bone child)
		{
			Children.Add(child);
			child.Parent = this;
		}

		public void RemoveParent(Bone parent)
		{
			RemoveChild(this);
		}

		public void RemoveChild(Bone child)
		{
			Children.Remove(child);
			child.Parent = null;
		}

		public void PopulateRig(ref List<Bone> bones)
		{
			if (!bones.Contains(this))
			{
				bones.Add(this);
			}
			foreach (Bone b in Children)
			{
				b.PopulateRig(ref bones);
			}
		}

		public void Load(ContentManager content)
		{
			if (Drawable != null)
				Drawable.Load(content);
			Length = Vector2.Distance(Connector, ChildConnector);
			//Length = 1.4f;
			foreach (Bone b in Children)
			{
				b.Load(content);
			}
		}

		public void Unload()
		{

			foreach (Bone b in Children)
			{
				b.Unload();
			}
		}

		public void Update()
		{
			if (Parent != null)
			{
				TransformedRotation = Rotation + Parent.TransformedRotation;
				float parentSin = (float)Math.Sin(Parent.TransformedRotation);
				float parentCos = (float)Math.Cos(Parent.TransformedRotation);
				float sin = (float)Math.Sin(TransformedRotation);
				float cos = (float)Math.Cos(TransformedRotation);
				Vector2 transformedParentConnector = new Vector2((parentCos * Parent.ChildConnector.X) - (parentSin * Parent.ChildConnector.Y), (parentSin * Parent.ChildConnector.X) + (parentCos * Parent.ChildConnector.Y));
				Vector2 transformedConnector = new Vector2((cos * Connector.X) - (sin * Connector.Y), (sin * Connector.X) + (cos * Connector.Y));
				Position = (Parent.Position + transformedParentConnector) - transformedConnector;
			}
			else
			{
				Position = new Vector2(0, 0);
				TransformedRotation = Rotation;
			}


			foreach (Bone b in Children)
			{
				b.Update();
			}
		}

		public void Draw(SpriteBatch spriteBatch, Component rig, bool spriteFlip, bool rotationFlip, Effect shader, Matrix transformationMatrix)
		{
			if (Drawable != null)
			{
				if (Drawable.ShaderInfo != null && shader != null)
				{
					int flip = spriteFlip ? -1 : 1;
					Drawable.ShaderInfo.SetUniforms(shader, -(rig.Rotation + TransformedRotation), flip);
				}

				spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, shader, transformationMatrix);

				Vector2 inv = new Vector2(1, -1);
				SpriteEffects flipEffect = spriteFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				if (rotationFlip)
				{
					Vector2 xFlip = new Vector2(-1, 1);
					spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (rig.Owner.Position + (rig.Position + Position) * xFlip) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, (rig.Rotation + TransformedRotation), Drawable._origin, rig.Scale, flipEffect, Drawable._layer);
				}
				else
				{
					spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (rig.Owner.Position + rig.Position + Position) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, -(rig.Rotation + TransformedRotation), Drawable._origin, rig.Scale, flipEffect, Drawable._layer);
				}
				spriteBatch.End();

			}
			foreach (Bone b in Children)
			{
				b.Draw(spriteBatch, rig, spriteFlip, rotationFlip, shader, transformationMatrix);
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch, Component rig)
		{
			float sin = (float)Math.Sin(TransformedRotation);
			float cos = (float)Math.Cos(TransformedRotation);
			Vector2 transformedConnector = new Vector2((cos * Connector.X) - (sin * Connector.Y), (sin * Connector.X) + (cos * Connector.Y));
			Vector2 transformedChildConnector = new Vector2((cos * ChildConnector.X) - (sin * ChildConnector.Y), (sin * ChildConnector.X) + (cos * ChildConnector.Y));
			DebugManager.Instance.DrawLine(spriteBatch, rig.Owner.Position + rig.Position + Position + transformedConnector, rig.Owner.Position + rig.Position + Position + transformedChildConnector);
			foreach (Bone b in Children)
			{
				b.DebugDraw(spriteBatch, rig);
			}
		}

		public Bone Copy()
		{
			return new Bone(this);
		}

	}
}
