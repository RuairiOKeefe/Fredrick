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
	public class IKSolver : Component
	{
		public override bool IsDrawn { get { return true; } }

		public Bone Root;
		public List<Bone> Bones = new List<Bone>();
		public bool MotionFlip;
		public string MountId;

		private Vector2 m_target = new Vector2(0);
		private float m_handRotation;

		public IKSolver()
		{
			Tags = new List<string>();
			Scale = new Vector2(1);
		}

		public IKSolver(Entity owner, IKSolver original) : base(owner, original)
		{
			Owner = owner;
			Bones = new List<Bone>();
			Root = original.Root.Copy();
			MountId = original.MountId;
		}

		public void SetTarget(Vector2 target, float rotation)
		{
			if (target != null)
			{
				m_target = target - (Root.ChildConnector + Position + _owner.Position);
				MotionFlip = m_target.X < 0 ? true : false;

				float sin = (float)Math.Sin(Rotation);
				float cos = (float)Math.Cos(Rotation);

				if (Bones.Count > 3)
				{
					float tpx = Bones[3].Connector.X;
					float tpy = Bones[3].Connector.Y;

					m_target -= new Vector2((cos * tpx) - (sin * tpy), (sin * tpx) + (cos * tpy));
				}
			}
			m_handRotation = rotation;
		}

		private bool Solve2BoneIK()
		{
			if (Root == null)
			{
				//Throw
				return false;
			}
			if (Root.Children[0] == null)
			{
				//Throw
				return false;
			}
			if (Root.Children[0].Children[0] == null)
			{
				//Throw
				return false;
			}

			double length1 = Root.Children[0].Length;
			double length2 = Root.Children[0].Children[0].Length;
			double angle1;
			double angle2;

			const double epsilon = 0.0001; // used to prevent division by small numbers

			bool foundValidSolution = true;

			double targetDistSqr = m_target.LengthSquared();

			//===
			// Compute a new value for angle2 along with its cosine
			double sinAngle2;
			double cosAngle2;

			double cosAngle2_denom = 2 * length1 * length2;
			if (cosAngle2_denom > epsilon)
			{
				cosAngle2 = (targetDistSqr - (length1 * length1) - (length2 * length2))
							/ (cosAngle2_denom);

				// if our result is not in the legal cosine range, we can not find a
				// legal solution for the target
				if ((cosAngle2 < -1.0) || (cosAngle2 > 1.0))
					foundValidSolution = false;

				// clamp our value into range so we can calculate the best
				// solution when there are no valid ones
				cosAngle2 = Math.Max(-1, Math.Min(1, cosAngle2));

				// compute a new value for angle2
				angle2 = Math.Acos(cosAngle2);

				// adjust for the desired bend direction
				if (MotionFlip)
					angle2 = -angle2;

				// compute the sine of our angle
				sinAngle2 = Math.Sin(angle2);
			}
			else
			{
				// At least one of the bones had a zero length. This means our
				// solvable domain is a circle around the origin with a radius
				// equal to the sum of our bone lengths.
				double totalLenSqr = (length1 + length2) * (length1 + length2);
				if (targetDistSqr < (totalLenSqr - epsilon)
					|| targetDistSqr > (totalLenSqr + epsilon))
				{
					foundValidSolution = false;
				}

				// Only the value of angle1 matters at this point. We can just
				// set angle2 to zero. 
				angle2 = 0.0;
				cosAngle2 = 1.0;
				sinAngle2 = 0.0;
			}

			//===
			// Compute the value of angle1 based on the sine and cosine of angle2
			double triAdjacent = length1 + length2 * cosAngle2;
			double triOpposite = length2 * sinAngle2;

			double tanY = m_target.Y * triAdjacent - m_target.X * triOpposite;
			double tanX = m_target.X * triAdjacent + m_target.Y * triOpposite;

			// Note that it is safe to call Atan2(0,0) which will happen if targetX and
			// targetY are zero
			angle1 = Math.Atan2(tanY, tanX);

			Root.Children[0].Rotation = (float)(angle1 + Math.PI / 2);//This is being rotated 90 degrees (counter-clockwise) because the sprites for limbs are facing downwards, (ie are rotated 90 degrees clockwise)
			Root.Children[0].Children[0].Rotation = (float)(angle2);
			Root.Children[0].Children[0].Children[0].Rotation = m_handRotation - (float)(angle2 + angle1);

			return foundValidSolution;
		}

		public override void Load(ContentManager content)
		{
			Root.Load(content);
			Root.PopulateRig(ref Bones);
			if (Root.Children[0].Drawable != null)
			{
				if (Root.Children[0].Drawable.ShaderInfo != null)
				{
					ShaderId = Root.Children[0].Drawable.ShaderInfo.ShaderId;
				}
			}
		}

		public override void Unload()
		{
			Root.Unload();
		}

		public override void Update(double deltaTime)
		{
			if (MountId != null)
			{
				var component = Owner.GetComponentWithId(MountId);
				Position = component.Position;
			}
			Solve2BoneIK();
			Root.Update(MotionFlip);
		}

		public override void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformationMatrix)
		{
			Root.Children[0].Draw(spriteBatch, this, MotionFlip, false, shader, transformationMatrix);
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);
			Root.Draw(spriteBatch, this, MotionFlip, false, null, Matrix.Identity);
			spriteBatch.End();
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			Vector2 worldTarget = Root.ChildConnector + Position + _owner.Position + m_target;
			DebugManager.Instance.DrawLine(spriteBatch, worldTarget + new Vector2(-0.2f, 0), worldTarget + new Vector2(0.2f, 0));
			DebugManager.Instance.DrawLine(spriteBatch, worldTarget + new Vector2(0, -0.2f), worldTarget + new Vector2(0, 0.2f));
			Root.DebugDraw(spriteBatch, this);
		}

		public override Component Copy(Entity owner)
		{
			return new IKSolver(owner, this);
		}
	}
}
