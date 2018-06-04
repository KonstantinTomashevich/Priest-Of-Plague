using System;
using PriestOfPlague.Source.Spells;
using UnityEngine.AI;

namespace PriestOfPlague.Source.Unit.Ai
{
    public class AiSwordsman : IGameAi
    {
        public float SearchDistance = 20.0f;
        public Unit LastEnemy;

        public void Process (Unit unit)
        {
            GameAiUtils.PickupItemsNear (unit);
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
                var spell = unit.SpellsContainerRef.Spells [SpellsInitializer.HeavySwordAttackSpellId];

                if (unit.CurrentlyCasting != spell)
                {
                    unit.StartCastingSpell (spell);
                }

                var random = new Random ();
                int level = random.Next (1, 10);
                if (unit.CanCast (level))
                {
                    unit.CastSpell (level, GameAiUtils.FindSuitableItemForLevel (unit, level));
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