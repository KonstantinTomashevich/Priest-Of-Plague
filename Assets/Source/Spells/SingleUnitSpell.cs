using System.Linq;
using JetBrains.Annotations;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Assertions.Must;

namespace PriestOfPlague.Source.Spells
{
    public class SingleUnitSpell : AreaSpellBase
    {
        public SingleUnitSpell (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
            Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float requiredBaseMovementPoints,
            float requiredMovementPointsPerLevel, bool affectSelf, float baseAngle, float anglePerLevel,
            float baseDistance, float distancePerLevel, UnitCallbackType unitCallback) : 
            
            base (id, basicCastTime,
            castTimeAdditionPerLevel, movementRequired, icon, info, requiredItemSupertype,
            requiredBaseCharge, requiredChargePerLevel, requiredBaseMovementPoints, requiredMovementPointsPerLevel,
            affectSelf, baseAngle, anglePerLevel, baseDistance, distancePerLevel, unitCallback)
        {
        }

        public override void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            base.Cast (caster, unitsHub, parameter);

            float distance = float.MaxValue;
            Unit.Unit nearestUnit = null;
            
            foreach (var unit in unitsHub.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (caster, unitToCheck,
                    BaseDistance + DistancePerLevel * parameter.Level,
                    BaseAngle + AnglePerLevel * parameter.Level)))
            {
                if (unit != caster && (unit.transform.position - caster.transform.position).magnitude < distance)
                {
                    distance = (unit.transform.position - caster.transform.position).magnitude;
                    nearestUnit = unit;
                }
            }

            if (nearestUnit == null)
            {
                if (AffectSelf)
                {
                    nearestUnit = caster;
                }
                else
                {
                    return;
                }
            }

            UnitCallback (caster, nearestUnit, parameter);
        }
    }
}