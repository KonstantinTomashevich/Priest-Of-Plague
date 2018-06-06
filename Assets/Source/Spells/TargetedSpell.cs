using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class TargetedSpell : SpellWithItemBase
    {
        public TargetedSpell (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
            Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float requiredBaseMovementPoints,
            float requiredMovementPointsPerLevel, UnitCallbackType unitCallback, TargetCheckerType targetChecker) : 
            
            base (id, basicCastTime,
            castTimeAdditionPerLevel, movementRequired, false, true, icon, info, requiredItemSupertype,
            requiredBaseCharge, requiredChargePerLevel, requiredBaseMovementPoints, requiredMovementPointsPerLevel,
            unitCallback, targetChecker)
        {
        }

        public override void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            base.Cast (caster, unitsHub, parameter);
            if (parameter.Target != null)
            {
                UnitCallback (caster, parameter.Target, parameter);
            }
        }
    }
}