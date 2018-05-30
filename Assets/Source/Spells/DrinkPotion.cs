using System;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class DrinkPotion : ISpell
    {
        public DrinkPotion (int id, float basicCastTime, Sprite icon)
        {
            Id = id;
            BasicCastTime = basicCastTime;
            Icon = icon;
        }

        public bool CanCast (Unit.Unit unit, int level = 0, Item item = null)
        {
            if (unit.MovementBlocked)
            {
                return false;
            }

            var itemsTypesContainer = unit.ItemTypesContainerRef;
            if (item != null)
            {
                return itemsTypesContainer.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.Potion) &&
                       item.Charge >= 1.0f;
            }

            foreach (var itemInStorage in unit.MyStorage.Items)
            {
                if (itemsTypesContainer.ItemTypes [itemInStorage.ItemTypeId].Supertypes
                        .Contains (ItemSuperType.Potion) && itemInStorage.Charge >= 1.0f)
                {
                    return true;
                }
            }

            return false;
        }

        public void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            var item = parameter.UsedItem;
            item.Charge -= 1.0f;
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

        public int Id { get; private set; }
        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel => 0.0f;
        public bool MovementRequired => true;
        public Sprite Icon { get; private set; }
        public string Info => "Drink Potion";
    }
}