using System.Linq;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public abstract class AreaSpellBase : SpellWithItemBase
    {
        protected AreaSpellBase (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
            Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float requiredBaseMovementPoints,
            float requiredMovementPointsPerLevel, bool affectSelf, float baseAngle, float anglePerLevel,
            float baseDistance, float distancePerLevel, UnitCallbackType unitCallback) : 
            
            base (id, basicCastTime, castTimeAdditionPerLevel,
            movementRequired, false, icon, info, requiredItemSupertype, requiredBaseCharge,
            requiredChargePerLevel, requiredBaseMovementPoints, requiredMovementPointsPerLevel, unitCallback,
            (caster, target) => target == null)
        {
            AffectSelf = affectSelf;
            BaseAngle = baseAngle;
            AnglePerLevel = anglePerLevel;
            BaseDistance = baseDistance;
            DistancePerLevel = distancePerLevel;
        }

        public override void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            parameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * parameter.Level);
            caster.UseMovementPoints (RequiredBaseMovementPoints + RequiredMovementPointsPerLevel * parameter.Level);
        }

        public bool AffectSelf { get; private set; }
        public float BaseAngle { get; private set; }
        public float AnglePerLevel { get; private set; }
        public float BaseDistance { get; private set; }
        public float DistancePerLevel { get; private set; }
    }
}