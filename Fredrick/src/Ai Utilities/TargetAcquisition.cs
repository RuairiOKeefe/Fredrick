using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Fredrick.src.Colliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Ai_Utilities
{
	[Serializable]
	public class TargetAcquisition
	{
		private CircleTrigger m_spotTrigger; // Lower radius vision trigger that is used for visibility detection (ie, entities in this trigger are added to spotted)
		private CircleTrigger m_sightTrigger; // Higher radius vision trigger that is used for tracking spotted entities (ie, entities are removed from spotted only when they leave this)

		private float m_hearingRadius; // The radius at which sounds can be heard (Reminder: sounds will have a audible radius as well, and this will just check if the source is within the range of this + audible radius, Ie a gunshot that is audible 5m away with a hearing range of 10m can be heard 15m away)
		private float m_spotRadius; 
		private float m_sightRadius;

		public List<Entity> SpottedEntities = new List<Entity>(); // List of all currently spotted entities, used instead of pure positional info so ai can determine target priorities
		public List<Tuple<Vector2, double>> LostEntities = new List<Tuple<Vector2, double>>(); // List of previously seen or heard enemies, with a timer until they are "forgotten"

		public double DetectionDuration;// may want to stick in ai instead of here, and make ai callback this ect.

		private Entity m_owner;

		public TargetAcquisition()
		{
		}

		public TargetAcquisition(Entity owner, float spotRadius, float sightRadius, float hearingRadius, double detectionDuration)
		{
			m_owner = owner;
			m_spotRadius = spotRadius;
			m_sightRadius = sightRadius;
			m_hearingRadius = hearingRadius;
			DetectionDuration = detectionDuration;
		}

		public TargetAcquisition(Entity owner, TargetAcquisition original)
		{
			m_owner = owner;
			m_spotRadius = original.m_spotRadius;
			m_sightRadius = original.m_sightRadius;
			m_hearingRadius = original.m_hearingRadius;
			DetectionDuration = original.DetectionDuration;
		}

		public bool Spot_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			SpottedEntities.Add(fixtureB.Body.UserData as Entity); //Check this makes sense
			return true;
		}

		public void Sight_OnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			Entity other;
			try
			{
				other = fixtureB.UserData as Entity;
			}
			catch (Exception e)
			{
				throw new Exception("Other fixture is not associated with an Entity", e);
			}
			if (SpottedEntities.Contains(other))
			{
				SpottedEntities.Remove(other);
				//LostEntities.Add(new Tuple<Vector2, double>(other.Position, 10.0)); // Add "attention span" to entities
			}
		}

		public void Load(ContentManager content, Entity owner)
		{
			ColliderCategory colliderCategory = ColliderCategory.Trigger;
			ColliderCategory collidesWith = ColliderCategory.Actor;

			m_spotTrigger = new CircleTrigger(owner, "SpotTrigger", m_spotRadius, colliderCategory, collidesWith);
			m_sightTrigger = new CircleTrigger(owner, "SightTrigger", m_sightRadius, colliderCategory, collidesWith);

			m_spotTrigger.Revive();//need to move to revie function of some kind so triggers are removed when object is out of area? maybe not if pools exist?
			m_sightTrigger.Revive();

			m_spotTrigger.AddCollisionHandler(Spot_OnCollision);
			m_sightTrigger.AddSeparationHandler(Sight_OnSeparation);
		}

		public void Unload()
		{

		}

		public void Update(double deltaTime)
		{
			m_spotTrigger.Update(deltaTime);
			m_sightTrigger.Update(deltaTime);

			for (int i = LostEntities.Count - 1; i == 0; i--)
			{
				// Change tuple type so that time can be deincremented. Maybe make timer type? (You use these constantly my Ru)
				if (LostEntities[i].Item2 <= 0)
					LostEntities.RemoveAt(i);
			}
		}

		public TargetAcquisition Copy(Entity owner)
		{
			return new TargetAcquisition(owner, this);
		}
	}
}
