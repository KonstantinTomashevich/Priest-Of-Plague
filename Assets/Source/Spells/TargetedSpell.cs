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
            float requiredMovementPointsPerLevel, UnitCallbackType unitCallback) : 
            
            base (id, basicCastTime,
            castTimeAdditionPerLevel, movementRequired, true, icon, info, requiredItemSupertype,
            requiredBaseCharge, requiredChargePerLevel, requiredBaseMovementPoints, requiredMovementPointsPerLevel,
            unitCallback)
        {
        }

        public override void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            if (parameter.Target != null)
            {
                UnitCallback (parameter.Target, parameter);
            }
        }
    }
}