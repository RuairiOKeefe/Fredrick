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
		public Vector2 Position { get; private set; } //Should only be set by update
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
			ParentConnector = original.ParentConnector;
			Connector = original.Connector;
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
			//Rotation += 0.02f;
			if (Parent != null)
			{
				TransformedRotation = Rotation + Parent.TransformedRotation;
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

		public void Draw(SpriteBatch spriteBatch, CharacterRig rig, bool motionFlip)
		{
			if (Drawable != null)
			{
				Vector2 inv = new Vector2(1, -1);
				if (motionFlip)
				{
					Vector2 xFlip = new Vector2(-1, 1);
					spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (rig.Owner.Position + (rig.Position + Position) * xFlip) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, -(rig.Rotation + TransformedRotation), Drawable._origin, rig.Scale, SpriteEffects.FlipHorizontally, Drawable._layer);
				}
				else
				{
					spriteBatch.Draw(ResourceManager.Instance.Textures[Drawable._spriteName], (rig.Owner.Position + rig.Position + Position) * inv * Drawable._spriteSize, Drawable._sourceRectangle, Drawable._colour, rig.Rotation + TransformedRotation, Drawable._origin, rig.Scale, Drawable._spriteEffects, Drawable._layer);
				}

			}
			foreach (Bone b in Children)
			{
				b.Draw(spriteBatch, rig, motionFlip);
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch, CharacterRig rig)
		{
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

	[Serializable]
	public class RigFrame
	{
		public Dictionary<string, float> BoneFrames;//bool clockwise
		public Vector2 Position;//Main bone position
		public double FrameTime;

		public RigFrame(Dictionary<string, float> boneFrames, Vector2 position, double frameTime)
		{
			BoneFrames = boneFrames;
			Position = position;
			FrameTime = frameTime;
		}

		public RigFrame(List<Bone> bones, Vector2 position, double frameTime)
		{
			BoneFrames = new Dictionary<string, float>();
			foreach (Bone b in bones)
			{
				BoneFrames.Add(b.Id, b.Rotation);
			}
			Position = position;
			FrameTime = frameTime;
		}

		public RigFrame(RigFrame original)
		{
			BoneFrames = new Dictionary<string, float>();
			foreach (KeyValuePair<string, float> bone in original.BoneFrames)
			{
				BoneFrames.Add(bone.Key, bone.Value);
			}
			Position = original.Position;
			FrameTime = original.FrameTime;
		}

		public RigFrame Copy()
		{
			return new RigFrame(this);
		}
	}

	[Serializable]
	public class RigAnimation
	{
		public string Id;
		public List<RigFrame> RigFrames;
		public bool Loop;
		public bool Over;

		public RigAnimation()
		{
			Id = "";
			RigFrames = new List<RigFrame>();
			Loop = true;
			Over = false;
		}

		public RigAnimation(string id, List<RigFrame> rigFrames, bool loop, bool over = false)
		{
			Id = id;
			RigFrames = rigFrames;
			Loop = loop;
			Over = over;
		}

		public RigAnimation(RigAnimation original)
		{
			Id = original.Id;
			RigFrames = new List<RigFrame>();
			foreach (RigFrame rig in original.RigFrames)
			{
				RigFrames.Add(rig.Copy());
			}
			Loop = original.Loop;
			Over = original.Over;
		}

		public RigAnimation Copy()
		{
			return new RigAnimation(this);
		}
	}

	[Serializable]
	public class CharacterRig : Component
	{
		public override bool IsDrawn { get { return true; } }

		public Bone Root;
		public List<Bone> Bones;//Bones are set during load, created from root
		public RigFrame PreviousFrame;
		public Dictionary<string, RigAnimation> Animations;
		public RigAnimation CurrentAnimation;
		public int NextFrame { get; private set; }
		public bool MotionFlip;
		double m_frameTime;
		public Vector2 OverridePosition;
		public float OverrideRotation;
		public float NextOverrideRotation;
		public string MountId;//If not blank will search for component to track
		private float m_previousOverrideRotation;
		private bool m_loopOverride;// Will the override continue until a new one is set?


		public CharacterRig()
		{
			CurrentAnimation = new RigAnimation();
			Animations = new Dictionary<string, RigAnimation>();
			Bones = new List<Bone>();
			Scale = new Vector2(1.0f);
		}

		public CharacterRig(Entity owner, string id, List<string> tags, Bone root, Dictionary<string, RigAnimation> animations) : base(owner, id, tags, true, false)
		{
			CurrentAnimation = new RigAnimation();
			Animations = new Dictionary<string, RigAnimation>();
			Bones = new List<Bone>();
			Scale = new Vector2(1.0f);
			Root = root;
			Animations = animations;
			CurrentAnimation = Animations.First().Value;
			PreviousFrame = CurrentAnimation.RigFrames[0];

		}

		public CharacterRig(Entity owner, CharacterRig original) : base(owner, original.Id, original.Tags, original.Active)
		{
			Animations = new Dictionary<string, RigAnimation>();
			foreach (KeyValuePair<string, RigAnimation> anim in original.Animations)
			{
				Animations.Add(anim.Key, anim.Value.Copy());
			}
			CurrentAnimation = original.CurrentAnimation.Copy();
			Bones = new List<Bone>();
			Tags = original.Tags;
			Scale = original.Scale;
			Rotation = original.Rotation;
			Root = original.Root.Copy();
			PreviousFrame = CurrentAnimation.RigFrames[0].Copy();
			OverridePosition = new Vector2(0);
			MountId = original.MountId;
		}

		public void RestartAnim()
		{
			PreviousFrame = CurrentAnimation.RigFrames[0];
			NextFrame = 0;
			m_frameTime = 0;
			CurrentAnimation.Over = false;
		}

		public void SwitchToAnim(string id, bool smooth = true)
		{
			if (smooth)
			{
				PreviousFrame = new RigFrame(Bones, Position, 0);
				CurrentAnimation = Animations[id];
				NextFrame = 0;
				m_frameTime = 0;
			}
			else
			{
				CurrentAnimation = Animations[id];
				PreviousFrame = CurrentAnimation.RigFrames[0];
				NextFrame = 1;
				m_frameTime = 0;
			}
			CurrentAnimation.Over = false;
		}

		public void SetOverrideRotation(float rotation, bool loopOverride)
		{
			NextOverrideRotation = rotation;
			m_loopOverride = loopOverride;
		}

		public override void Load(ContentManager content)
		{
			Root.Load(content);
			Root.PopulateRig(ref Bones);
			m_frameTime = 0.0;
			NextFrame = 1;
			if (Root.Drawable != null)
			{
				if (Root.Drawable.ShaderInfo != null)
				{
					ShaderId = Root.Drawable.ShaderInfo.ShaderId;
				}
			}
		}

		public override void Unload()
		{
			Root.Unload();
		}

		public override void Update(double deltaTime)
		{
			float lerpValue;

			if (MountId != null)
			{
				var component = Owner.GetComponentWithId(MountId);
				OverridePosition = component.Position;
			}

			if (CurrentAnimation.RigFrames.Count > 1 && !CurrentAnimation.Over)
			{
				m_frameTime += deltaTime;
			}

			if (m_frameTime > CurrentAnimation.RigFrames[NextFrame].FrameTime)
			{
				m_frameTime = m_frameTime % CurrentAnimation.RigFrames[NextFrame].FrameTime;
				PreviousFrame = new RigFrame(Bones, Position - OverridePosition, m_frameTime);
				NextFrame += 1;
				if (NextFrame > CurrentAnimation.RigFrames.Count - 1)
				{
					if (CurrentAnimation.Loop)
					{
						NextFrame = NextFrame % (CurrentAnimation.RigFrames.Count);
					}
					else
					{
						NextFrame = NextFrame % (CurrentAnimation.RigFrames.Count);
						PreviousFrame = new RigFrame(Bones, Position - OverridePosition, m_frameTime);
						CurrentAnimation.Over = true;
					}

					if (!m_loopOverride)
					{
						NextOverrideRotation = 0;
					}
				}

				m_previousOverrideRotation = OverrideRotation;
				OverrideRotation = NextOverrideRotation;
			}

			lerpValue = (float)(m_frameTime / CurrentAnimation.RigFrames[NextFrame].FrameTime);

			Position = (PreviousFrame.Position * (1 - lerpValue) + CurrentAnimation.RigFrames[NextFrame].Position * (lerpValue)) + OverridePosition;

			foreach (Bone b in Bones)
			{
				if (b.Parent == null)
				{
					float prevFrameRot = PreviousFrame.BoneFrames[b.Id] + m_previousOverrideRotation;
					float nextFrameRot = CurrentAnimation.RigFrames[NextFrame].BoneFrames[b.Id] + OverrideRotation;
					if (MotionFlip)
					{
						b.Rotation = (prevFrameRot * (1 - lerpValue) - nextFrameRot * (lerpValue));
					}
					else
					{
						b.Rotation = (prevFrameRot * (1 - lerpValue) + nextFrameRot * (lerpValue));
					}
				}
				else
				{
					b.Rotation = (PreviousFrame.BoneFrames[b.Id] * (1 - lerpValue) + CurrentAnimation.RigFrames[NextFrame].BoneFrames[b.Id] * (lerpValue));
				}
			}

			Root.Update();
		}

		public override void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformationMatrix)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, shader, transformationMatrix);
			Root.Draw(spriteBatch, this, MotionFlip);
			spriteBatch.End();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);
			Root.Draw(spriteBatch, this, MotionFlip);
			spriteBatch.End();
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			Root.DebugDraw(spriteBatch, this);
		}

		public override Component Copy(Entity owner)
		{
			return new CharacterRig(owner, this);
		}
	}
}
