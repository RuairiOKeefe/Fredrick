using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src.Rigging
{
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
			Root.Draw(spriteBatch, this, MotionFlip, MotionFlip, shader, transformationMatrix);
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);
			Root.Draw(spriteBatch, this, MotionFlip, MotionFlip, null, Matrix.Identity);//This is wrong we need a cam matrix
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
