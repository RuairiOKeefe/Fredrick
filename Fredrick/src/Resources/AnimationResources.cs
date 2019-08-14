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
			Dictionary<string, RigAnimation> animations = new Dictionary<string, RigAnimation>();

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
			Bone FootFront = new Bone("FootFront", new Vector2(0), 0, new Vector2(0.0f, -10 / 32f), new Vector2(0.0f, 0.0f));
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
			Bone FootBack = new Bone("FootBack", new Vector2(0), 0, new Vector2(0.0f, -10 / 32f), new Vector2(0.0f, 0.0f));
			FootBack.Drawable = new Drawable("Foot", new Vector2(16), 32, 32, 0.1f);
			LowerLegBack.AddChild(FootBack);
			Bone ToesBack = new Bone("ToesBack", new Vector2(0), 0, new Vector2(2 / 32f, 0.0f), new Vector2(1 / 32f, 0.0f));
			ToesBack.Drawable = new Drawable("Toes", new Vector2(16), 32, 32, 0.1f);
			FootBack.AddChild(ToesBack);

			RigAnimation standing = new RigAnimation("Stand", new List<RigFrame>(), true, false);

			Dictionary<string, float> feetAppart = new Dictionary<string, float>();
			feetAppart.Add("Torso", 0.0f);
			feetAppart.Add("Head", 0.0f);
			feetAppart.Add("UpperLegFront", 0.2f);
			feetAppart.Add("LowerLegFront", 0.1f);
			feetAppart.Add("FootFront", 0.4f);
			feetAppart.Add("ToesFront", -0.4f);
			feetAppart.Add("UpperLegBack", -0.3f);
			feetAppart.Add("LowerLegBack", 0.3f);
			feetAppart.Add("FootBack", 0.1f);
			feetAppart.Add("ToesBack", -0.1f);
			standing.RigFrames.Add(new RigFrame(feetAppart, new Vector2(0, 11 / 32f), 0.4));

			Dictionary<string, float> feetAppart2 = new Dictionary<string, float>();
			feetAppart2.Add("Torso", 0.0f);
			feetAppart2.Add("Head", 0.0f);
			feetAppart2.Add("UpperLegFront", 0.25f);
			feetAppart2.Add("LowerLegFront", 0.15f);
			feetAppart2.Add("FootFront", 0.5f);
			feetAppart2.Add("ToesFront", -0.5f);
			feetAppart2.Add("UpperLegBack", -0.35f);
			feetAppart2.Add("LowerLegBack", 0.35f);
			feetAppart2.Add("FootBack", 0.2f);
			feetAppart2.Add("ToesBack", -0.2f);
			standing.RigFrames.Add(new RigFrame(feetAppart2, new Vector2(0, 12 / 32f), 0.4));

			animations.Add("Standing", standing);


			RigAnimation run = new RigAnimation("Running", new List<RigFrame>(), true, false);

			Dictionary<string, float> playerContactFront = new Dictionary<string, float>();
			playerContactFront.Add("Torso", 0.0f);
			playerContactFront.Add("Head", 0.0f);
			playerContactFront.Add("UpperLegFront", -1.0f);
			playerContactFront.Add("LowerLegFront", -0.2f);
			playerContactFront.Add("FootFront", 0.4f);
			playerContactFront.Add("ToesFront", -0.3f);
			playerContactFront.Add("UpperLegBack", 0.4f);
			playerContactFront.Add("LowerLegBack", 1.6f);
			playerContactFront.Add("FootBack", 0.2f);
			playerContactFront.Add("ToesBack", -0.6f);
			run.RigFrames.Add(new RigFrame(playerContactFront, new Vector2(0, 11 / 32f), 0.12));

			Dictionary<string, float> playerDownFront = new Dictionary<string, float>();
			playerDownFront.Add("Torso", 0.0f);
			playerDownFront.Add("Head", 0.0f);
			playerDownFront.Add("UpperLegFront", -0.5f);
			playerDownFront.Add("LowerLegFront", 1.4f);
			playerDownFront.Add("FootFront", -0.4f);
			playerDownFront.Add("ToesFront", -0.1f);
			playerDownFront.Add("UpperLegBack", -0.8f);
			playerDownFront.Add("LowerLegBack", 2.2f);
			playerDownFront.Add("FootBack", 0.4f);
			playerDownFront.Add("ToesBack", -0.2f);
			run.RigFrames.Add(new RigFrame(playerDownFront, new Vector2(0, 8 / 32f), 0.12));

			Dictionary<string, float> playerContFirstFront = new Dictionary<string, float>();
			playerContFirstFront.Add("Torso", 0.0f);
			playerContFirstFront.Add("Head", 0.0f);
			playerContFirstFront.Add("UpperLegFront", 0.4f);
			playerContFirstFront.Add("LowerLegFront", 0.3f);
			playerContFirstFront.Add("FootFront", 0.6f);
			playerContFirstFront.Add("ToesFront", -0.3f);
			playerContFirstFront.Add("UpperLegBack", -1.6f);
			playerContFirstFront.Add("LowerLegBack", 1.9f);
			playerContFirstFront.Add("FootBack", 0.5f);
			playerContFirstFront.Add("ToesBack", 0.0f);
			run.RigFrames.Add(new RigFrame(playerContFirstFront, new Vector2(0, 11 / 32f), 0.12));

			Dictionary<string, float> playerContSecondFront = new Dictionary<string, float>();
			playerContSecondFront.Add("Torso", 0.0f);
			playerContSecondFront.Add("Head", 0.0f);
			playerContSecondFront.Add("UpperLegFront", 0.6f);
			playerContSecondFront.Add("LowerLegFront", 1.6f);
			playerContSecondFront.Add("FootFront", 0.8f);
			playerContSecondFront.Add("ToesFront", 0.2f);
			playerContSecondFront.Add("UpperLegBack", -1.8f);
			playerContSecondFront.Add("LowerLegBack", 0.7f);
			playerContSecondFront.Add("FootBack", -0.4f);
			playerContSecondFront.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContSecondFront, new Vector2(0, 14 / 32f), 0.12));

			Dictionary<string, float> playerContactBack = new Dictionary<string, float>();
			playerContactBack.Add("Torso", 0.0f);
			playerContactBack.Add("Head", 0.0f);
			playerContactBack.Add("UpperLegFront", 0.4f);
			playerContactBack.Add("LowerLegFront", 1.6f);
			playerContactBack.Add("FootFront", 0.2f);
			playerContactBack.Add("ToesFront", -0.6f);
			playerContactBack.Add("UpperLegBack", -1.0f);
			playerContactBack.Add("LowerLegBack", -0.2f);
			playerContactBack.Add("FootBack", 0.4f);
			playerContactBack.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContactBack, new Vector2(0, 11 / 32f), 0.12));

			Dictionary<string, float> playerDownBack = new Dictionary<string, float>();
			playerDownBack.Add("Torso", 0.0f);
			playerDownBack.Add("Head", 0.0f);
			playerDownBack.Add("UpperLegFront", -0.8f);
			playerDownBack.Add("LowerLegFront", 2.2f);
			playerDownBack.Add("FootFront", 0.4f);
			playerDownBack.Add("ToesFront", -0.2f);
			playerDownBack.Add("UpperLegBack", -0.5f);
			playerDownBack.Add("LowerLegBack", 1.4f);
			playerDownBack.Add("FootBack", -0.4f);
			playerDownBack.Add("ToesBack", -0.1f);
			run.RigFrames.Add(new RigFrame(playerDownBack, new Vector2(0, 8 / 32f), 0.12));

			Dictionary<string, float> playerContFirstBack = new Dictionary<string, float>();
			playerContFirstBack.Add("Torso", 0.0f);
			playerContFirstBack.Add("Head", 0.0f);
			playerContFirstBack.Add("UpperLegFront", -1.6f);
			playerContFirstBack.Add("LowerLegFront", 1.9f);
			playerContFirstBack.Add("FootFront", 0.5f);
			playerContFirstBack.Add("ToesFront", 0.0f);
			playerContFirstBack.Add("UpperLegBack", 0.4f);
			playerContFirstBack.Add("LowerLegBack", 0.3f);
			playerContFirstBack.Add("FootBack", 0.6f);
			playerContFirstBack.Add("ToesBack", -0.3f);
			run.RigFrames.Add(new RigFrame(playerContFirstBack, new Vector2(0, 11 / 32f), 0.12));

			Dictionary<string, float> playerContSecondBack = new Dictionary<string, float>();
			playerContSecondBack.Add("Torso", 0.0f);
			playerContSecondBack.Add("Head", 0.0f);
			playerContSecondBack.Add("UpperLegFront", -1.8f);
			playerContSecondBack.Add("LowerLegFront", 0.7f);
			playerContSecondBack.Add("FootFront", -0.4f);
			playerContSecondBack.Add("ToesFront", -0.3f);
			playerContSecondBack.Add("UpperLegBack", 0.6f);
			playerContSecondBack.Add("LowerLegBack", 1.6f);
			playerContSecondBack.Add("FootBack", 0.8f);
			playerContSecondBack.Add("ToesBack", 0.2f);
			run.RigFrames.Add(new RigFrame(playerContSecondBack, new Vector2(0, 0.5f), 0.12));

			animations.Add("Running", run);


			RigAnimation jumping = new RigAnimation("Jump", new List<RigFrame>(), false, false);

			Dictionary<string, float> jumpStart = new Dictionary<string, float>();
			jumpStart.Add("Torso", 0.0f);
			jumpStart.Add("Head", 0.0f);
			jumpStart.Add("UpperLegFront", 0.2f);
			jumpStart.Add("LowerLegFront", 0.1f);
			jumpStart.Add("FootFront", 0.4f);
			jumpStart.Add("ToesFront", -0.4f);
			jumpStart.Add("UpperLegBack", -0.3f);
			jumpStart.Add("LowerLegBack", 0.3f);
			jumpStart.Add("FootBack", 0.1f);
			jumpStart.Add("ToesBack", -0.1f);
			jumping.RigFrames.Add(new RigFrame(jumpStart, new Vector2(0, 11 / 32f), 0.05));

			Dictionary<string, float> jumpPrep = new Dictionary<string, float>();
			jumpPrep.Add("Torso", 0.0f);
			jumpPrep.Add("Head", 0.0f);
			jumpPrep.Add("UpperLegFront", -1.8f);
			jumpPrep.Add("LowerLegFront", 2.3f);
			jumpPrep.Add("FootFront", 0.4f);
			jumpPrep.Add("ToesFront", -0.4f);
			jumpPrep.Add("UpperLegBack", -1.2f);
			jumpPrep.Add("LowerLegBack", 2.1f);
			jumpPrep.Add("FootBack", 0.1f);
			jumpPrep.Add("ToesBack", -0.1f);
			jumping.RigFrames.Add(new RigFrame(jumpPrep, new Vector2(0, 4 / 32f), 0.05));

			Dictionary<string, float> legsExtended = new Dictionary<string, float>();
			legsExtended.Add("Torso", 0.0f);
			legsExtended.Add("Head", 0.0f);
			legsExtended.Add("UpperLegFront", 0.3f);
			legsExtended.Add("LowerLegFront", 0.1f);
			legsExtended.Add("FootFront", 0.5f);
			legsExtended.Add("ToesFront", -0.5f);
			legsExtended.Add("UpperLegBack", -1.35f);
			legsExtended.Add("LowerLegBack", 1.4f);
			legsExtended.Add("FootBack", 0.2f);
			legsExtended.Add("ToesBack", -0.2f);
			jumping.RigFrames.Add(new RigFrame(legsExtended, new Vector2(0, 12 / 32f), 0.2));

			animations.Add("Jumping", jumping);

			RigAnimation falling = new RigAnimation("Fall", new List<RigFrame>(), false, false);

			Dictionary<string, float> wobble1 = new Dictionary<string, float>();
			wobble1.Add("Torso", 0.0f);
			wobble1.Add("Head", 0.0f);
			wobble1.Add("UpperLegFront", -0.7f);
			wobble1.Add("LowerLegFront", 0.1f);
			wobble1.Add("FootFront", 0.1f);
			wobble1.Add("ToesFront", -0.1f);
			wobble1.Add("UpperLegBack", 0.2f);
			wobble1.Add("LowerLegBack", 0.9f);
			wobble1.Add("FootBack", 0.4f);
			wobble1.Add("ToesBack", -0.4f);
			falling.RigFrames.Add(new RigFrame(wobble1, new Vector2(0, 11 / 32f), 0.2));

			Dictionary<string, float> wobble2 = new Dictionary<string, float>();
			wobble2.Add("Torso", 0.0f);
			wobble2.Add("Head", 0.0f);
			wobble2.Add("UpperLegFront", -0.8f);
			wobble2.Add("LowerLegFront", 0.15f);
			wobble2.Add("FootFront", 0.2f);
			wobble2.Add("ToesFront", -0.2f);
			wobble2.Add("UpperLegBack", 0.25f);
			wobble2.Add("LowerLegBack", 0.95f);
			wobble2.Add("FootBack", 0.5f);
			wobble2.Add("ToesBack", -0.5f);
			falling.RigFrames.Add(new RigFrame(wobble2, new Vector2(0, 12 / 32f), 0.2));

			animations.Add("Falling", falling);

			CharacterRig characterRig = new CharacterRig(null, "PlayerLegs", torso, animations);

			characterRig.Tags.Add("Legs");
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

			RigAnimation throwing = new RigAnimation("Throwing", new List<RigFrame>(), false, false);

			Dictionary<string, float> playerThrowStart = new Dictionary<string, float>();
			playerThrowStart.Add("Shoulder", -1.5f);
			playerThrowStart.Add("UpperArmFront", 1.8f);
			playerThrowStart.Add("LowerArmFront", -2.2f);
			playerThrowStart.Add("UpperArmBack", 1.6f);
			playerThrowStart.Add("LowerArmBack", -1.6f);
			throwing.RigFrames.Add(new RigFrame(playerThrowStart, new Vector2(0, 12 / 32f), 0.06));

			Dictionary<string, float> playerThrowEnd = new Dictionary<string, float>();
			playerThrowEnd.Add("Shoulder", -1.5f);
			playerThrowEnd.Add("UpperArmFront", 0.2f);
			playerThrowEnd.Add("LowerArmFront", -0.2f);
			playerThrowEnd.Add("UpperArmBack", 2.0f);
			playerThrowEnd.Add("LowerArmBack", -1.5f);
			throwing.RigFrames.Add(new RigFrame(playerThrowEnd, new Vector2(0, 12 / 32f), 0.05));

			Dictionary<string, float> playerThrowRecovery = new Dictionary<string, float>();
			playerThrowRecovery.Add("Shoulder", -1.5f);
			playerThrowRecovery.Add("UpperArmFront", 2.3f);
			playerThrowRecovery.Add("LowerArmFront", -1.7f);
			playerThrowRecovery.Add("UpperArmBack", 2.1f);
			playerThrowRecovery.Add("LowerArmBack", -1.9f);
			throwing.RigFrames.Add(new RigFrame(playerThrowRecovery, new Vector2(0, 12 / 32f), 0.45));

			animations.Add("Throwing", throwing);

			CharacterRig characterRig = new CharacterRig(null, "PlayerArms", shoulder, animations);
			characterRig.Tags.Add("MotionFlip");
			characterRig.Tags.Add("Arms");
			characterRig.MountId = "PlayerLegs";
			return characterRig;
		}
	}
}
