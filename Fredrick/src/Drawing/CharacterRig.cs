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
	public class Bone
	{
		public String Id { get; set; }
		public Bone Parent { get; private set; } = null;
		public List<Bone> Children { get; private set; }
		public Drawable Drawable { get; set; }
		public Vector2 Position { get; set; } //Is really the Transformed Position, but I think 0,0 is the local center //=(Parent Pos + Transformed ParentConnector) +- Transformed Connector ....probably idk i'm only mostly good at maths and did this in like 2 minutes
		public float Rotation { get; set; }
		public float TransformedRotation { get; private set; }
		public Vector2 ParentConnector { get; set; }
		public Vector2 Connector { get; set; }

		Bone()
		{
			Children = new List<Bone>();
		}

		public Bone(string id, Vector2 position, float rotation, Vector2 parentConnector, Vector2 connector)
		{
			Id = id;
			Children = new List<Bone>();
			Position = position;
			Rotation = rotation;
			ParentConnector = parentConnector;
			Connector = connector;
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
			Drawable.Load(content);
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
			Rotation += 0.02f;
			if (Parent != null)
			{
				TransformedRotation = Rotation + Parent.TransformedRotation;//my guess is the inverse is fucking it somehow?
				float parentSin = (float)Math.Sin(-Parent.TransformedRotation);
				float parentCos = (float)Math.Cos(-Parent.TransformedRotation);
				float sin = (float)Math.Sin(-TransformedRotation);
				float cos = (float)Math.Cos(-TransformedRotation);
				Vector2 transformedParentConnector = new Vector2((parentCos * ParentConnector.X) - (parentSin * ParentConnector.Y), (parentSin * ParentConnector.X) + (parentCos * ParentConnector.Y));
				Vector2 transformedConnector = new Vector2((cos * Connector.X) - (sin * Connector.Y), (sin * Connector.X) + (cos * Connector.Y));
				Position = (Parent.Position + transformedParentConnector) + transformedConnector;
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

		public void Draw(SpriteBatch spriteBatch, CharacterRig rig)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName],  (rig.Owner.Position + rig.Position + Position) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, rig.Rotation + TransformedRotation, Drawable._origin, rig.Scale, Drawable._spriteEffects, Drawable._layer);

			foreach (Bone b in Children)
			{
				b.Draw(spriteBatch, rig);
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch, CharacterRig rig)
		{
			foreach (Bone b in Children)
			{
				b.DebugDraw(spriteBatch, rig);
			}
		}
	}

	[Serializable]
	public class BoneFrame ///PROBABLY DON'T NEED BONE INFO, so float can be used instead
	{
		public Bone Bone { get; set; }
		public float Rotation { get; set; }

	}

	[Serializable]
	public class RigFrame
	{
		public Dictionary<string, float> BoneFrames;
		public Vector2 Position;//Main bone position
		public double FrameTime;

		public RigFrame(Dictionary<string, float> boneFrames, Vector2 position, double frameTime)
		{
			BoneFrames = boneFrames;
			Position = position;
			FrameTime = frameTime;
		}
	}

	[Serializable]
	public class CharacterRig : Component
	{
		public Bone Root;
		public List<Bone> Bones;
		public List<RigFrame> Animation;

		public CharacterRig()
		{
			Scale = new Vector2(1.0f);
			Bones = new List<Bone>();
			Animation = new List<RigFrame>();
		}

		public CharacterRig(Entity owner, string id, Bone root, List<RigFrame> animation) : base(owner, id)
		{
			Scale = new Vector2(1.0f);
			Root = root;
			Animation = animation;
			Bones = new List<Bone>();
			Animation = new List<RigFrame>();
		}

		public override void Load(ContentManager content)
		{
			Root.Load(content);
			Root.PopulateRig(ref Bones);
		}

		public override void Unload()
		{
			Root.Unload();
		}

		public override void Update(double deltaTime)
		{
			float lerpValue = 0.5f;
			int lerpPoint = 1;
			//get index and ratio then set position lerp
			foreach (Bone b in Bones)
			{
				//b.Rotation = Animation[lerpPoint].BoneFrames[b.Id] * (1 - lerpValue) + Animation[lerpPoint + 1 % Animation.Count].BoneFrames[b.Id] * (lerpValue);//This?
			}

			Root.Update();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Root.Draw(spriteBatch, this);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			Root.DebugDraw(spriteBatch, this);
		}

		public override Component Copy(Entity owner)
		{
			return null;
		}
	}
}
