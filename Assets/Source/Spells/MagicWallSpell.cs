using JetBrains.Annotations;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PriestOfPlague.Source.Spells
{
    public class MagicWallSpell : AreaSpellBase
    {
        public MagicWallSpell (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
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
            parameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * parameter.Level);
            caster.UseMovementPoints (RequiredBaseMovementPoints + RequiredMovementPointsPerLevel * parameter.Level);

            foreach (var unit in unitsHub.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (caster, unitToCheck,
                    BaseDistance + DistancePerLevel * parameter.Level,
                    BaseAngle + AnglePerLevel * parameter.Level)))
            {
                if (AffectSelf || unit != caster)
                {
                    UnitCallback (caster, unit, parameter);
                }
            }
        }
    }
}