﻿using Fredrick.src.Rigging;
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

			ShaderInfo.Material characterMaterial;
			characterMaterial.Emissive = new Color(0, 0, 0, 255);
			characterMaterial.Diffuse = new Color(100, 150, 100, 255);
			characterMaterial.Specular = new Color(255, 255, 255, 255);
			characterMaterial.Shininess = 0.8f;
			LightingInfo characterLighting = new LightingInfo("PlayerNormal", characterMaterial);

			Bone torso = new Bone("Torso", new Vector2(0), 0, new Vector2(0, 12.5f / 32f), new Vector2(-1.5f / 32f, -12.5f / 32f));
			torso.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 0, 0, 0.15f);
			torso.Drawable.ShaderInfo = characterLighting;

			Bone spine = new Bone("Spine", new Vector2(0), 0, new Vector2(-1.5f / 32f, -11.5f / 32f), new Vector2(0, 16.5f / 32f));
			torso.AddChild(spine);

			Bone Head = new Bone("Head", new Vector2(0), 0, new Vector2(0 / 32f, -5.5f / 32f), new Vector2(0 / 32f, 3.5f / 32f));
			Head.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 32, 0, 0.14f);
			Head.Drawable.ShaderInfo = characterLighting;
			spine.AddChild(Head);


			Bone UpperLegFront = new Bone("UpperLegFront", new Vector2(0), 0, new Vector2(0 / 32f, 6.5f / 32f), new Vector2(0.0f, -7.5f / 32f));
			UpperLegFront.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 64, 32, 0.1f);
			UpperLegFront.Drawable.ShaderInfo = characterLighting;
			torso.AddChild(UpperLegFront);
			Bone LowerLegFront = new Bone("LowerLegFront", new Vector2(0), 0, new Vector2(0.0f, 10.5f / 32f), new Vector2(0.0f, -8.5f / 32f));
			LowerLegFront.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 0, 64, 0.15f);
			LowerLegFront.Drawable.ShaderInfo = characterLighting;
			UpperLegFront.AddChild(LowerLegFront);
			Bone FootFront = new Bone("FootFront", new Vector2(0), 0, new Vector2(-0.5f / 32f, 2.5f / 32f), new Vector2(4.5f / 32f, -0.5f / 32f));
			FootFront.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 32, 64, 0.1f);
			FootFront.Drawable.ShaderInfo = characterLighting;
			LowerLegFront.AddChild(FootFront);
			Bone ToesFront = new Bone("ToesFront", new Vector2(0), 0, new Vector2(-0.5f / 32f, 0.0f), new Vector2(0.5f / 32f, 0.0f));
			ToesFront.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 64, 64, 0.1f);
			ToesFront.Drawable.ShaderInfo = characterLighting;
			FootFront.AddChild(ToesFront);

			Bone UpperLegBack = new Bone("UpperLegBack", new Vector2(0), 0, new Vector2(-0 / 32f, 6.5f / 32f), new Vector2(0.0f, -7.5f / 32f));
			UpperLegBack.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 64, 32, 0.1f);
			UpperLegBack.Drawable.ShaderInfo = characterLighting;
			torso.AddChild(UpperLegBack);
			Bone LowerLegBack = new Bone("LowerLegBack", new Vector2(0), 0, new Vector2(0.0f, 10.5f / 32f), new Vector2(0.0f, -8.5f / 32f));
			LowerLegBack.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 0, 64, 0.15f);
			LowerLegBack.Drawable.ShaderInfo = characterLighting;
			UpperLegBack.AddChild(LowerLegBack);
			Bone FootBack = new Bone("FootBack", new Vector2(0), 0, new Vector2(-0.5f / 32f, 2.5f / 32f), new Vector2(4.5f / 32f, -0.5f / 32f));
			FootBack.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 32, 64, 0.1f);
			FootBack.Drawable.ShaderInfo = characterLighting;
			LowerLegBack.AddChild(FootBack);
			Bone ToesBack = new Bone("ToesBack", new Vector2(0), 0, new Vector2(-0.5f / 32f, 0.0f), new Vector2(0.5f / 32f, 0.0f));
			ToesBack.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 64, 64, 0.1f);
			ToesBack.Drawable.ShaderInfo = characterLighting;
			FootBack.AddChild(ToesBack);

			RigAnimation standing = new RigAnimation("Stand", new List<RigFrame>(), true, false);

			Dictionary<string, float> feetAppart = new Dictionary<string, float>();
			feetAppart.Add("Torso", 0.0f);
			feetAppart.Add("Spine", 0.0f);
			feetAppart.Add("Head", 0.0f);
			feetAppart.Add("UpperLegFront", -0.3f);
			feetAppart.Add("LowerLegFront", -0.1f);
			feetAppart.Add("FootFront", -0.4f);
			feetAppart.Add("ToesFront", 0.8f);
			feetAppart.Add("UpperLegBack", 0.3f);
			feetAppart.Add("LowerLegBack", -0.3f);
			feetAppart.Add("FootBack", -0.1f);
			feetAppart.Add("ToesBack", 0.1f);
			standing.RigFrames.Add(new RigFrame(feetAppart, new Vector2(0, 17 / 32f), 0.5));

			Dictionary<string, float> feetAppart2 = new Dictionary<string, float>();
			feetAppart2.Add("Torso", 0.0f);
			feetAppart2.Add("Spine", 0.0f);
			feetAppart2.Add("Head", 0.0f);
			feetAppart2.Add("UpperLegFront", -0.25f);
			feetAppart2.Add("LowerLegFront", -0.15f);
			feetAppart2.Add("FootFront", -0.5f);
			feetAppart2.Add("ToesFront", 0.2f);
			feetAppart2.Add("UpperLegBack", 0.35f);
			feetAppart2.Add("LowerLegBack", -0.35f);
			feetAppart2.Add("FootBack", -0.2f);
			feetAppart2.Add("ToesBack", 0.4f);
			standing.RigFrames.Add(new RigFrame(feetAppart2, new Vector2(0, 18 / 32f), 0.3));

			animations.Add("Standing", standing);


			RigAnimation run = new RigAnimation("Running", new List<RigFrame>(), true, false);

			Dictionary<string, float> playerContactFront = new Dictionary<string, float>();
			playerContactFront.Add("Torso", 0.0f);
			playerContactFront.Add("Spine", 0.0f);
			playerContactFront.Add("Head", 0.0f);
			playerContactFront.Add("UpperLegFront", 1.0f);
			playerContactFront.Add("LowerLegFront", 0.2f);
			playerContactFront.Add("FootFront", -0.4f);
			playerContactFront.Add("ToesFront", 0.3f);
			playerContactFront.Add("UpperLegBack", -0.4f);
			playerContactFront.Add("LowerLegBack", -1.6f);
			playerContactFront.Add("FootBack", -0.2f);
			playerContactFront.Add("ToesBack", 0.6f);
			run.RigFrames.Add(new RigFrame(playerContactFront, new Vector2(0, 15 / 32f), 0.12));

			Dictionary<string, float> playerDownFront = new Dictionary<string, float>();
			playerDownFront.Add("Torso", 0.0f);
			playerDownFront.Add("Spine", 0.0f);
			playerDownFront.Add("Head", 0.0f);
			playerDownFront.Add("UpperLegFront", 0.5f);
			playerDownFront.Add("LowerLegFront", -1.4f);
			playerDownFront.Add("FootFront", 0.4f);
			playerDownFront.Add("ToesFront", 0.1f);
			playerDownFront.Add("UpperLegBack", 0.8f);
			playerDownFront.Add("LowerLegBack", -2.2f);
			playerDownFront.Add("FootBack", -0.4f);
			playerDownFront.Add("ToesBack", 0.2f);
			run.RigFrames.Add(new RigFrame(playerDownFront, new Vector2(0, 12 / 32f), 0.12));

			Dictionary<string, float> playerContFirstFront = new Dictionary<string, float>();
			playerContFirstFront.Add("Torso", 0.0f);
			playerContFirstFront.Add("Spine", 0.0f);
			playerContFirstFront.Add("Head", 0.0f);
			playerContFirstFront.Add("UpperLegFront", -0.4f);
			playerContFirstFront.Add("LowerLegFront", -0.3f);
			playerContFirstFront.Add("FootFront", -0.6f);
			playerContFirstFront.Add("ToesFront", 0.3f);
			playerContFirstFront.Add("UpperLegBack", 1.6f);
			playerContFirstFront.Add("LowerLegBack", -1.9f);
			playerContFirstFront.Add("FootBack", -0.5f);
			playerContFirstFront.Add("ToesBack", 0.0f);
			run.RigFrames.Add(new RigFrame(playerContFirstFront, new Vector2(0, 15 / 32f), 0.12));

			Dictionary<string, float> playerContSecondFront = new Dictionary<string, float>();
			playerContSecondFront.Add("Torso", 0.0f);
			playerContSecondFront.Add("Spine", 0.0f);
			playerContSecondFront.Add("Head", 0.0f);
			playerContSecondFront.Add("UpperLegFront", -0.6f);
			playerContSecondFront.Add("LowerLegFront", -1.6f);
			playerContSecondFront.Add("FootFront", -0.8f);
			playerContSecondFront.Add("ToesFront", -0.2f);
			playerContSecondFront.Add("UpperLegBack", 1.8f);
			playerContSecondFront.Add("LowerLegBack", -0.7f);
			playerContSecondFront.Add("FootBack", 0.4f);
			playerContSecondFront.Add("ToesBack", 0.3f);
			run.RigFrames.Add(new RigFrame(playerContSecondFront, new Vector2(0, 18 / 32f), 0.12));

			Dictionary<string, float> playerContactBack = new Dictionary<string, float>();
			playerContactBack.Add("Torso", 0.0f);
			playerContactBack.Add("Spine", 0.0f);
			playerContactBack.Add("Head", 0.0f);
			playerContactBack.Add("UpperLegFront", -0.4f);
			playerContactBack.Add("LowerLegFront", -1.6f);
			playerContactBack.Add("FootFront", -0.2f);
			playerContactBack.Add("ToesFront", 0.6f);
			playerContactBack.Add("UpperLegBack", 1.0f);
			playerContactBack.Add("LowerLegBack", -0.2f);
			playerContactBack.Add("FootBack", -0.4f);
			playerContactBack.Add("ToesBack", 0.3f);
			run.RigFrames.Add(new RigFrame(playerContactBack, new Vector2(0, 15 / 32f), 0.12));

			Dictionary<string, float> playerDownBack = new Dictionary<string, float>();
			playerDownBack.Add("Torso", 0.0f);
			playerDownBack.Add("Spine", 0.0f);
			playerDownBack.Add("Head", 0.0f);
			playerDownBack.Add("UpperLegFront", 0.8f);
			playerDownBack.Add("LowerLegFront", -2.2f);
			playerDownBack.Add("FootFront", -0.4f);
			playerDownBack.Add("ToesFront", 0.2f);
			playerDownBack.Add("UpperLegBack", 0.5f);
			playerDownBack.Add("LowerLegBack", -1.4f);
			playerDownBack.Add("FootBack", 0.4f);
			playerDownBack.Add("ToesBack", 0.1f);
			run.RigFrames.Add(new RigFrame(playerDownBack, new Vector2(0, 12 / 32f), 0.12));

			Dictionary<string, float> playerContFirstBack = new Dictionary<string, float>();
			playerContFirstBack.Add("Torso", 0.0f);
			playerContFirstBack.Add("Spine", 0.0f);
			playerContFirstBack.Add("Head", 0.0f);
			playerContFirstBack.Add("UpperLegFront", 1.6f);
			playerContFirstBack.Add("LowerLegFront", -1.9f);
			playerContFirstBack.Add("FootFront", -0.5f);
			playerContFirstBack.Add("ToesFront", -0.0f);
			playerContFirstBack.Add("UpperLegBack", -0.4f);
			playerContFirstBack.Add("LowerLegBack", -0.3f);
			playerContFirstBack.Add("FootBack", -0.6f);
			playerContFirstBack.Add("ToesBack", 0.3f);
			run.RigFrames.Add(new RigFrame(playerContFirstBack, new Vector2(0, 15 / 32f), 0.12));

			Dictionary<string, float> playerContSecondBack = new Dictionary<string, float>();
			playerContSecondBack.Add("Torso", 0.0f);
			playerContSecondBack.Add("Spine", 0.0f);
			playerContSecondBack.Add("Head", 0.0f);
			playerContSecondBack.Add("UpperLegFront", 1.8f);
			playerContSecondBack.Add("LowerLegFront", -0.7f);
			playerContSecondBack.Add("FootFront", 0.4f);
			playerContSecondBack.Add("ToesFront", 0.3f);
			playerContSecondBack.Add("UpperLegBack", -0.6f);
			playerContSecondBack.Add("LowerLegBack", -1.6f);
			playerContSecondBack.Add("FootBack", -0.8f);
			playerContSecondBack.Add("ToesBack", -0.2f);
			run.RigFrames.Add(new RigFrame(playerContSecondBack, new Vector2(0, 15 / 32f), 0.12));

			animations.Add("Running", run);


			RigAnimation jumping = new RigAnimation("Jump", new List<RigFrame>(), false, false);

			Dictionary<string, float> jumpStart = new Dictionary<string, float>();
			jumpStart.Add("Torso", 0.0f);
			jumpStart.Add("Spine", 0.0f);
			jumpStart.Add("Head", 0.0f);
			jumpStart.Add("UpperLegFront", -0.2f);
			jumpStart.Add("LowerLegFront", -0.1f);
			jumpStart.Add("FootFront", -0.4f);
			jumpStart.Add("ToesFront", 0.4f);
			jumpStart.Add("UpperLegBack", 0.3f);
			jumpStart.Add("LowerLegBack", -0.3f);
			jumpStart.Add("FootBack", -0.1f);
			jumpStart.Add("ToesBack", 0.1f);
			jumping.RigFrames.Add(new RigFrame(jumpStart, new Vector2(0, 17 / 32f), 0.05));

			Dictionary<string, float> jumpPrep = new Dictionary<string, float>();
			jumpPrep.Add("Torso", 0.0f);
			jumpPrep.Add("Spine", 0.0f);
			jumpPrep.Add("Head", 0.0f);
			jumpPrep.Add("UpperLegFront", 1.8f);
			jumpPrep.Add("LowerLegFront", -2.3f);
			jumpPrep.Add("FootFront", -0.4f);
			jumpPrep.Add("ToesFront", 0.4f);
			jumpPrep.Add("UpperLegBack", -1.2f);
			jumpPrep.Add("LowerLegBack", -2.1f);
			jumpPrep.Add("FootBack", -0.1f);
			jumpPrep.Add("ToesBack", 0.1f);
			jumping.RigFrames.Add(new RigFrame(jumpPrep, new Vector2(0, 10 / 32f), 0.05));

			Dictionary<string, float> legsExtended = new Dictionary<string, float>();
			legsExtended.Add("Torso", 0.0f);
			legsExtended.Add("Spine", 0.0f);
			legsExtended.Add("Head", 0.0f);
			legsExtended.Add("UpperLegFront", -0.3f);
			legsExtended.Add("LowerLegFront", -0.1f);
			legsExtended.Add("FootFront", -0.5f);
			legsExtended.Add("ToesFront", 0.5f);
			legsExtended.Add("UpperLegBack", 1.35f);
			legsExtended.Add("LowerLegBack", -1.4f);
			legsExtended.Add("FootBack", -0.2f);
			legsExtended.Add("ToesBack", 0.2f);
			jumping.RigFrames.Add(new RigFrame(legsExtended, new Vector2(0, 18 / 32f), 0.2));

			animations.Add("Jumping", jumping);

			RigAnimation falling = new RigAnimation("Fall", new List<RigFrame>(), false, false);

			Dictionary<string, float> wobble1 = new Dictionary<string, float>();
			wobble1.Add("Torso", 0.0f);
			wobble1.Add("Spine", 0.0f);
			wobble1.Add("Head", 0.0f);
			wobble1.Add("UpperLegFront", 0.7f);
			wobble1.Add("LowerLegFront", -0.1f);
			wobble1.Add("FootFront", -0.1f);
			wobble1.Add("ToesFront", 0.1f);
			wobble1.Add("UpperLegBack", -0.2f);
			wobble1.Add("LowerLegBack", -0.9f);
			wobble1.Add("FootBack", -0.4f);
			wobble1.Add("ToesBack", 0.4f);
			falling.RigFrames.Add(new RigFrame(wobble1, new Vector2(0, 17 / 32f), 0.2));

			Dictionary<string, float> wobble2 = new Dictionary<string, float>();
			wobble2.Add("Torso", 0.0f);
			wobble2.Add("Spine", 0.0f);
			wobble2.Add("Head", 0.0f);
			wobble2.Add("UpperLegFront", 0.8f);
			wobble2.Add("LowerLegFront", -0.15f);
			wobble2.Add("FootFront", -0.2f);
			wobble2.Add("ToesFront", 0.2f);
			wobble2.Add("UpperLegBack", -0.25f);
			wobble2.Add("LowerLegBack", -0.95f);
			wobble2.Add("FootBack", -0.5f);
			wobble2.Add("ToesBack", 0.5f);
			falling.RigFrames.Add(new RigFrame(wobble2, new Vector2(0, 18 / 32f), 0.2));

			animations.Add("Falling", falling);

			//Improve formatting
			CharacterRig characterRig = new CharacterRig(null, "PlayerLegs", new List<string>(), torso, animations);

			characterRig.Tags.Add("Legs");
			characterRig.Tags.Add("MotionFlip");
			characterRig.DrawBatched = false;

			return characterRig;
		}

		public CharacterRig GetPlayerArms()
		{
			Bone shoulder = new Bone("Shoulder", new Vector2(0), 0, new Vector2(0), new Vector2(0));

			Bone UpperArmFront = new Bone("UpperArmFront", new Vector2(0), 0, new Vector2(0 / 32f, 0 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperArmFront.Drawable = new Drawable("UpperArm", new Vector2(16), 32, 32, 0, 0, 0.11f);
			shoulder.AddChild(UpperArmFront);
			Bone LowerArmFront = new Bone("LowerArmFront", new Vector2(0), 0, new Vector2(0.0f, -8 / 32f), new Vector2(0.0f, -6 / 32f));
			LowerArmFront.Drawable = new Drawable("LowerArm", new Vector2(16), 32, 32, 0, 0, 0.12f);
			UpperArmFront.AddChild(LowerArmFront);

			Bone UpperArmBack = new Bone("UpperArmBack", new Vector2(0), 0, new Vector2(-0 / 32f, 0 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperArmBack.Drawable = new Drawable("UpperArm", new Vector2(16), 32, 32, 0, 0, 0.17f);
			shoulder.AddChild(UpperArmBack);
			Bone LowerArmBack = new Bone("LowerArmBack", new Vector2(0), 0, new Vector2(0.0f, -8 / 32f), new Vector2(0.0f, -6 / 32f));
			LowerArmBack.Drawable = new Drawable("LowerArm", new Vector2(16), 32, 32, 0, 0, 0.18f);
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

			CharacterRig characterRig = new CharacterRig(null, "PlayerArms", new List<string>(), shoulder, animations);
			characterRig.Tags.Add("MotionFlip");
			characterRig.Tags.Add("Arms");
			characterRig.MountId = "PlayerLegs";
			return characterRig;
		}

		public IKSolver GetIKArm()
		{

			ShaderInfo.Material characterMaterial;
			characterMaterial.Emissive = new Color(0, 0, 0, 255);
			characterMaterial.Diffuse = new Color(100, 150, 100, 255);
			characterMaterial.Specular = new Color(255, 255, 255, 255);
			characterMaterial.Shininess = 0.8f;
			LightingInfo characterLighting = new LightingInfo("PlayerNormal", characterMaterial);

			Bone shoulder = new Bone("Shoulder", new Vector2(0), 0, new Vector2(0), new Vector2(-0.5f/32f, 10.5f / 32f));//Child connector is placed at location of shoulder

			Bone UpperArmFront = new Bone("UpperArm", new Vector2(0), 0, new Vector2(-0f / 32f, 5.5f / 32f), new Vector2(1.5f / 32f, -7.5f / 32f));
			UpperArmFront.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 64, 0, 0.11f);
			UpperArmFront.Drawable.ShaderInfo = characterLighting;
			shoulder.AddChild(UpperArmFront);
			Bone LowerArm = new Bone("LowerArm", new Vector2(0), 0, new Vector2(-0.5f / 32f, 7.5f / 32f), new Vector2(2.5f / 32f, -9.5f / 32f));
			LowerArm.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 0, 32, 0.12f);
			LowerArm.Drawable.ShaderInfo = characterLighting;
			UpperArmFront.AddChild(LowerArm);
			Bone Hand = new Bone("Hand", new Vector2(0), 0, new Vector2(0.5f / 32f, 2.5f / 32f), new Vector2(0.5f / 32f, -2.5f / 32f));
			Hand.Drawable = new Drawable("Player", new Vector2(16), 32, 32, 32, 32, 0.12f);
			Hand.Drawable.ShaderInfo = characterLighting;
			LowerArm.AddChild(Hand);

			IKSolver armSolver = new IKSolver();
			armSolver.MountId = "PlayerLegs";
			armSolver.Root = shoulder.Copy();

			return armSolver;
		}
	}
}
