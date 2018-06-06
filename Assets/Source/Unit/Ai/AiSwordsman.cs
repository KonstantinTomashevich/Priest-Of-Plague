using System.Collections.Generic;
using NUnit.Framework;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Spells;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace PriestOfPlague.Source.Unit.Ai
{
    public class AiSwordsman : IGameAi
    {
        public float SearchDistance = 40.0f;
        public Unit LastEnemy;

        public void Process (Unit unit)
        {
            Debug.Assert (unit.Alive);
            
            GameAiUtils.PickupItemsNear (unit, new List <ItemSuperType> { ItemSuperType.OneHandedWeapon });
            if (unit.MovementBlocked)
            {
                unit.gameObject.GetComponent <NavMeshAgent> ().ResetPath ();
            }
            
            Unit nearestEnemy = GameAiUtils.NearestEnemy (unit, SearchDistance);

            if (nearestEnemy != null)
            {
                LastEnemy = nearestEnemy;
            }
            else if (LastEnemy != null && !LastEnemy.Alive)
            {
                LastEnemy = null;
            }

            if (LastEnemy == null)
            {
                unit.GetComponent <NavMeshAgent> ().ResetPath ();
                return;
            }

            if (!GameAiUtils.Near (unit, LastEnemy) ||
                !GameAiUtils.WillBeAffectedByAreaSpell (unit, LastEnemy,
                    unit.SpellsContainerRef.Spells [SpellsInitializer.LightSwordAttackSpellId] as AreaSpellBase))
            {
                GameAiUtils.FollowUnit (unit, LastEnemy);
            }
            else
            {
                unit.GetComponent <NavMeshAgent> ().ResetPath ();
                var spell = unit.SpellsContainerRef.Spells [SpellsInitializer.LightSwordAttackSpellId];

                if (unit.CurrentlyCasting != spell)
                {
                    unit.StartCastingSpell (spell);
                }

                if (unit.CanCast ())
                {
                    unit.CastSpell (1, GameAiUtils.FindSuitableItemForLevel (unit, 1));
                }
            }
        }

        public void OnDamage (Unit unit, Unit damager)
        {
            if (LastEnemy == null)
            {
                LastEnemy = damager;
            }
        }

        public void OnDie (Unit unit)
        {
            LastEnemy = null;
        }
    }
}