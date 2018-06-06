using System;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class DrinkPotion : SpellWithItemBase
    {
        public DrinkPotion (int id, float basicCastTime, Sprite icon) :
            base (id, basicCastTime, 0.0f, true, false, false, icon, "Drink Potion", ItemSuperType.Potion,
                1.0f, 0.0f, 0.0f, 0.0f, null, (caster, target) => target == null)
        {
        }

        public override void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            base.Cast (caster, unitsHub, parameter);
            var item = parameter.UsedItem;
            var itemType = caster.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];

            if (itemType.Supertypes.Contains (ItemSuperType.Meal))
            {
                caster.ApplyModifier (1, item.Level);
            }

            if (itemType.Supertypes.Contains (ItemSuperType.Antidote))
            {
                caster.DecreaseModifiers (4,
                    (int) Math.Round (itemType.BasicForce + item.Level * itemType.ForceAdditionPerLevel));
            }

            if (itemType.Supertypes.Contains (ItemSuperType.HealthPotion))
            {
                caster.Heal (itemType.BasicForce + item.Level * itemType.ForceAdditionPerLevel);
            }

            if (itemType.Supertypes.Contains (ItemSuperType.MpPotion))
            {
                caster.Rest (itemType.BasicForce + item.Level * itemType.ForceAdditionPerLevel);
            }
        }
    }
}