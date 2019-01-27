using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class AnimationResources
	{
		public CharacterRig GetPlayerLegs()
		{
			Bone torso = new Bone("Torso", new Vector2(0), 0, new Vector2(0), new Vector2(0));
			torso.Drawable = new Drawable("Torso", new Vector2(16), 32, 32, 0.15f);
			Bone Head = new Bone("Head", new Vector2(0), 0, new Vector2(2 / 32f, 25 / 32f), new Vector2(0, -5 / 32f));
			Head.Drawable = new Drawable("Head", new Vector2(32), 64, 64, 0.14f);
			torso.AddChild(Head);
			Bone UpperLegFront = new Bone("UpperLegFront", new Vector2(0), 0, new Vector2(0 / 32f, -14 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperLegFront.Drawable = new Drawable("UpperLeg", new Vector2(16), 32, 32, 0.1f);
			torso.AddChild(UpperLegFront);
			Bone LowerLegFront = new Bone("LowerLegFront", new Vector2(0), 0, new Vector2(0.0f, -6 / 32f), new Vector2(0.0f, -9 / 32f));
			LowerLegFront.Drawable = new Drawable("LowerLeg", new Vector2(16), 32, 32, 0.15f);
			UpperLegFront.AddChild(LowerLegFront);
			Bone FootFront = new Bone("FootFront", new Vector2(0), 0, new Vector2(0.0f, -13 / 32f), new Vector2(0.0f, 0.0f));
			FootFront.Drawable = new Drawable("Foot", new Vector2(16), 32, 32, 0.1f);
			LowerLegFront.AddChild(FootFront);
			Bone ToesFront = new Bone("ToesFront", new Vector2(0), 0, new Vector2(2 / 32f, 0.0f), new Vector2(1 / 32f, 0.0f));
			ToesFront.Drawable = new Drawable("Toes", new Vector2(16), 32, 32, 0.1f);
			FootFront.AddChild(ToesFront);

			Bone UpperLegBack = new Bone("UpperLegBack", new Vector2(0), 0, new Vector2(-0 / 32f, -14 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperLegBack.Drawable = new Drawable("UpperLeg", new Vector2(16), 32, 32, 0.1f);
			torso.AddChild(UpperLegBack);
			Bone LowerLegBack = new Bone("LowerLegBack", new Vector2(0), 0, new Vector2(0.0f, -6 / 32f), new Vector2(0.0f, -9 / 32f));
			LowerLegBack.Drawable = new Drawable("LowerLeg", new Vector2(16), 32, 32, 0.15f);
			UpperLegBack.AddChild(LowerLegBack);
			Bone FootBack = new Bone("FootBack", new Vector2(0), 0, new Vector2(0.0f, -13 / 32f), new Vector2(0.0f, 0.0f));
			FootBack.Drawable = new Drawable("Foot", new Vector2(16), 32, 32, 0.1f);
			LowerLegBack.AddChild(FootBack);
			Bone ToesBack = new Bone("ToesBack", new Vector2(0), 0, new Vector2(2 / 32f, 0.0f), new Vector2(1 / 32f, 0.0f));
			ToesBack.Drawable = new Drawable("Toes", new Vector2(16), 32, 32, 0.1f);
			FootBack.AddChild(ToesBack);

			Dictionary<string, RigAnimation> animations = new Dictionary<string, RigAnimation>();

			RigAnimation run = new RigAnimation("Run", new List<RigFrame>(), false, true);

			Dictionary<string, float> playerContactFront = new Dictionary<string, float>();
			playerContactFront.Add("Torso", 0.0f);
			playerContactFront.Add("Head", 0.0f);
			playerContactFront.Add("UpperLegFront", -1.0f);
			playerContactFront.Add("LowerLegFront", -0.2f);
			playerContactFront.Add("FootFront", 0.4f);
			playerContactFront.Add("ToesFront", -0.3f);
			playerContactFront.Add("UpperLegBack", 0.8f);
			playerContactFront.Add("LowerLegBack", 0.6f);
			playerContactFront.Add("FootBack", 0.2f);
			playerContactFront.Add("ToesBack", -0.6f);
			run.RigFrames.Add(new RigFrame(playerContactFront, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerDownFront = new Dictionary<string, float>();
			playerDownFront.Add("Torso", 0.0f);
			playerDownFront.Add("Head", 0.0f);
			playerDownFront.Add("UpperLegFront", -0.5f);
			playerDownFront.Add("LowerLegFront", 1.4f);
			playerDownFront.Add("FootFront", -0.4f);
			playerDownFront.Add("ToesFront", -0.1f);
			playerDownFront.Add("UpperLegBack", -0.8f);
			playerDownFront.Add("LowerLegBack", 1.7f);
			playerDownFront.Add("FootBack", 0.4f);
			playerDownFront.Add("ToesBack", -0.2f);
			run.RigFrames.Add(new RigFrame(playerDownFront, new Vector2(0, 0.3f), 0.12));

			Dictionary<string, float> playerContFirstFront = new Dictionary<string, float>();
			playerContFirstFront.Add("Torso", 0.0f);
			playerContFirstFront.Add("Head", 0.0f);
			playerContFirstFront.Add("UpperLegFront", 0.8f);
			playerContFirstFront.Add("LowerLegFront", 0.3f);
			playerContFirstFront.Add("FootFront", 0.6f);
			playerContFirstFront.Add("ToesFront", -0.3f);
			playerContFirstFront.Add("UpperLegBack", -1.6f);
			playerContFirstFront.Add("LowerLegBack", 1.9f);
			playerContFirstFront.Add("FootBack", 0.5f);
			playerContFirstFront.Add("ToesBack", 0.0f);
			run.RigFrames.Add(new RigFrame(playerContFirstFront, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerContSecondFront = new Dictionary<string, float>();
			playerContSecondFront.Add("Torso", 0.0f);
			playerContSecondFront.Add("Head", 0.0f);
			playerContSecondFront.Add("UpperLegFront", 1.4f);
			playerContSecondFront.Add("LowerLegFront", 0.4f);
			playerContSecondFront.Add("FootFront", 0.8f);
			playerContSecondFront.Add("ToesFront", 0.2f);
			playerContSecondFront.Add("UpperLegBack", -1.8f);
			playerContSecondFront.Add("LowerLegBack", 0.7f);
			playerContSecondFront.Add("FootBack", -0.4f);
			playerContSecondFront.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContSecondFront, new Vector2(0, 0.5f), 0.12));

			Dictionary<string, float> playerContactBack = new Dictionary<string, float>();
			playerContactBack.Add("Torso", 0.0f);
			playerContactBack.Add("Head", 0.0f);
			playerContactBack.Add("UpperLegFront", 0.8f);
			playerContactBack.Add("LowerLegFront", 0.6f);
			playerContactBack.Add("FootFront", 0.2f);
			playerContactBack.Add("ToesFront", -0.6f);
			playerContactBack.Add("UpperLegBack", -1.0f);
			playerContactBack.Add("LowerLegBack", -0.2f);
			playerContactBack.Add("FootBack", 0.4f);
			playerContactBack.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContactBack, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerDownBack = new Dictionary<string, float>();
			playerDownBack.Add("Torso", 0.0f);
			playerDownBack.Add("Head", 0.0f);
			playerDownBack.Add("UpperLegFront", -0.8f);
			playerDownBack.Add("LowerLegFront", 1.7f);
			playerDownBack.Add("FootFront", 0.4f);
			playerDownBack.Add("ToesFront", -0.2f);
			playerDownBack.Add("UpperLegBack", -0.5f);
			playerDownBack.Add("LowerLegBack", 1.4f);
			playerDownBack.Add("FootBack", -0.4f);
			playerDownBack.Add("ToesBack", -0.1f);
			run.RigFrames.Add(new RigFrame(playerDownBack, new Vector2(0, 0.3f), 0.12));

			Dictionary<string, float> playerContFirstBack = new Dictionary<string, float>();
			playerContFirstBack.Add("Torso", 0.0f);
			playerContFirstBack.Add("Head", 0.0f);
			playerContFirstBack.Add("UpperLegFront", -1.6f);
			playerContFirstBack.Add("LowerLegFront", 1.9f);
			playerContFirstBack.Add("FootFront", 0.5f);
			playerContFirstBack.Add("ToesFront", 0.0f);
			playerContFirstBack.Add("UpperLegBack", 0.8f);
			playerContFirstBack.Add("LowerLegBack", 0.3f);
			playerContFirstBack.Add("FootBack", 0.6f);
			playerContFirstBack.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContFirstBack, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerContSecondBack = new Dictionary<string, float>();
			playerContSecondBack.Add("Torso", 0.0f);
			playerContSecondBack.Add("Head", 0.0f);
			playerContSecondBack.Add("UpperLegFront", -1.8f);
			playerContSecondBack.Add("LowerLegFront", 0.7f);
			playerContSecondBack.Add("FootFront", -0.4f);
			playerContSecondBack.Add("ToesFront", -0.3f);
			playerContSecondBack.Add("UpperLegBack", 1.4f);
			playerContSecondBack.Add("LowerLegBack", 0.4f);
			playerContSecondBack.Add("FootBack", 0.8f);
			playerContSecondBack.Add("ToesBack", 0.2f);
			run.RigFrames.Add(new RigFrame(playerContSecondBack, new Vector2(0, 0.5f), 0.12));

			animations.Add("Run", run);
			CharacterRig characterRig = new CharacterRig(null, "PlayerLegs", torso, animations);
			characterRig.Tags.Add("MotionFlip");
			return characterRig;
		}

		public CharacterRig GetPlayerArms()
		{
			Bone shoulder = new Bone("Shoulder", new Vector2(0), 0, new Vector2(0), new Vector2(0));

			Bone UpperArmFront = new Bone("UpperArmFront", new Vector2(0), 0, new Vector2(0 / 32f, 0 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperArmFront.Drawable = new Drawable("UpperArm", new Vector2(16), 32, 32, 0.11f);
			shoulder.AddChild(UpperArmFront);
			Bone LowerArmFront = new Bone("LowerArmFront", new Vector2(0), 0, new Vector2(0.0f, -8 / 32f), new Vector2(0.0f, -6 / 32f));
			LowerArmFront.Drawable = new Drawable("LowerArm", new Vector2(16), 32, 32, 0.12f);
			UpperArmFront.AddChild(LowerArmFront);

			Bone UpperArmBack = new Bone("UpperArmBack", new Vector2(0), 0, new Vector2(-0 / 32f, 0 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperArmBack.Drawable = new Drawable("UpperArm", new Vector2(16), 32, 32, 0.17f);
			shoulder.AddChild(UpperArmBack);
			Bone LowerArmBack = new Bone("LowerArmBack", new Vector2(0), 0, new Vector2(0.0f, -8 / 32f), new Vector2(0.0f, -6 / 32f));
			LowerArmBack.Drawable = new Drawable("LowerArm", new Vector2(16), 32, 32, 0.18f);
			UpperArmBack.AddChild(LowerArmBack);

			Dictionary<string, RigAnimation> animations = new Dictionary<string, RigAnimation>();

			RigAnimation throwing = new RigAnimation("Throwing", new List<RigFrame>(), true, false);

			Dictionary<string, float> playerThrowStart = new Dictionary<string, float>();
			playerThrowStart.Add("Shoulder", -1.5f);
			playerThrowStart.Add("UpperArmFront", -1.8f);
			playerThrowStart.Add("LowerArmFront", -2.2f);
			playerThrowStart.Add("UpperArmBack", 1.6f);
			playerThrowStart.Add("LowerArmBack", -1.6f);
			throwing.RigFrames.Add(new RigFrame(playerThrowStart, new Vector2(0, 22 / 32f), 0.06));

			Dictionary<string, float> playerThrowEnd = new Dictionary<string, float>();
			playerThrowEnd.Add("Shoulder", -1.5f);
			playerThrowEnd.Add("UpperArmFront", 0.2f);
			playerThrowEnd.Add("LowerArmFront", -0.2f);
			playerThrowEnd.Add("UpperArmBack", 2.0f);
			playerThrowEnd.Add("LowerArmBack", -1.5f);
			throwing.RigFrames.Add(new RigFrame(playerThrowEnd, new Vector2(0, 22 / 32f), 0.06));

			Dictionary<string, float> playerThrowRecovery = new Dictionary<string, float>();
			playerThrowRecovery.Add("Shoulder", -1.5f);
			playerThrowRecovery.Add("UpperArmFront", 2.3f);
			playerThrowRecovery.Add("LowerArmFront", -1.7f);
			playerThrowRecovery.Add("UpperArmBack", 2.1f);
			playerThrowRecovery.Add("LowerArmBack", -1.9f);
			throwing.RigFrames.Add(new RigFrame(playerThrowRecovery, new Vector2(0, 22 / 32f), 0.3));

			animations.Add("Throwing", throwing);

			CharacterRig characterRig = new CharacterRig(null, "PlayerArms", shoulder, animations);
			characterRig.Tags.Add("MotionFlip");
			characterRig.Tags.Add("Arms");
			return characterRig;
		}
	}
}
