using System.Collections.Generic;
using System.Linq;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Spells;
using UnityEngine;
using UnityEngine.AI;

namespace PriestOfPlague.Source.Unit.Ai
{
    public static class GameAiUtils
    {
        public static bool Near (Unit unit, Unit another, float distance = 2.5f, float maxAngle = 45.0f)
        {
            return UnitsHubCriterias.MaxDistanceAndMaxAngle (unit, another, distance, maxAngle);
        }

        public static bool WillBeAffectedByAreaSpell (Unit caster, Unit another, AreaSpellBase spell, int level = 1)
        {
            return UnitsHubCriterias.MaxDistanceAndMaxAngle (caster, another,
                spell.BaseDistance + spell.DistancePerLevel * level,
                spell.BaseAngle + spell.AnglePerLevel * level);
        }

        public static void GoTo (Unit unit, Vector3 position, float navigationAccuracy = 1.0f)
        {
            if (unit.MovementBlocked)
            {
                return;
            }

            NavMeshHit navMeshHit;
            var navMeshAgent = unit.gameObject.GetComponent <NavMeshAgent> ();

            if (NavMesh.SamplePosition (position, out navMeshHit, navigationAccuracy, navMeshAgent.areaMask))
            {
                navMeshAgent.SetDestination (navMeshHit.position);
                unit.StartCastingSpell (null);
            }
        }

        public static void FollowUnit (Unit follower, Unit another, float navigationAccuracy = 1.0f)
        {
            GoTo (follower, another.transform.position, navigationAccuracy);
        }

        public static Unit NearestEnemy (Unit asker, float maxDistance)
        {
            Unit nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (var unit in asker.UnitsHubRef.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (asker, unitToCheck, maxDistance)))
            {
                if (unit.Alive && unit.Alignment != asker.Alignment)
                {
                    float distance = (unit.transform.position - asker.transform.position).magnitude;
                    if (distance < nearestDistance)
                    {
                        nearestEnemy = unit;
                        nearestDistance = distance;
                    }
                }
            }

            return nearestEnemy;
        }

        public static void PickupItemsNear (Unit unit, List <ItemSuperType> supertypes, float pickupRadius = 1.5f)
        {
            foreach (var itemContainer in GameObject.FindGameObjectsWithTag ("SpawnedItem"))
            {
                var container = itemContainer.GetComponent <SpawnedItemContainer> ();
                if (container.SpawnedItem == null)
                {
                    continue;
                }
                
                if (supertypes.Intersect (unit.ItemTypesContainerRef.ItemTypes [container.SpawnedItem.ItemTypeId]
                        .Supertypes).Any () &&
                    unit.MyStorage.AddItem (container.SpawnedItem))
                {
                    container.SpawnedItem = null;
                    Object.Destroy (itemContainer);
                }
            }
        }

        public static Item FindSuitableItemForLevel (Unit unit, int spellLevel)
        {
            foreach (var item in unit.MyStorage.Items)
            {
                if (unit.CanCast (spellLevel, item))
                {
                    return item;
                }
            }

            return null;
        }
    }
}