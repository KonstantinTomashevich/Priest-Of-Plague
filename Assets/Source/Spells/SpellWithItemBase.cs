using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public abstract class SpellWithItemBase : ISpell
    {
        public delegate void UnitCallbackType (Unit.Unit caster, Unit.Unit unit, SpellCastParameter parameter);

        protected SpellWithItemBase (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
            bool targetRequired, Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float requiredBaseMovementPoints,
            float requiredMovementPointsPerLevel, UnitCallbackType unitCallback)
        {
            Id = id;
            BasicCastTime = basicCastTime;
            CastTimeAdditionPerLevel = castTimeAdditionPerLevel;
            MovementRequired = movementRequired;
            TargetRequired = targetRequired;
            Icon = icon;
            Info = info;
            RequiredItemSupertype = requiredItemSupertype;
            RequiredBaseCharge = requiredBaseCharge;
            RequiredChargePerLevel = requiredChargePerLevel;
            RequiredBaseMovementPoints = requiredBaseMovementPoints;
            RequiredMovementPointsPerLevel = requiredMovementPointsPerLevel;
            UnitCallback = unitCallback;
        }

        public virtual bool CanCast (Unit.Unit unit, int level = 0, Item item = null)
        {
            if (MovementRequired && unit.MovementBlocked)
            {
                return false;
            }

            ItemTypesContainer itemTypesContainer = unit.ItemTypesContainerRef;
            if (unit.CurrentMp < RequiredBaseMovementPoints + RequiredMovementPointsPerLevel * level)
            {
                return false;
            }

            if (item == null)
            {
                foreach (var storageItem in unit.MyStorage.Items)
                {
                    if (itemTypesContainer.ItemTypes [storageItem.ItemTypeId].Supertypes
                            .Contains (RequiredItemSupertype) &&
                        storageItem.Charge >= RequiredBaseCharge + RequiredChargePerLevel * level &&
                        storageItem.Level >= level)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return itemTypesContainer.ItemTypes [item.ItemTypeId].Supertypes.Contains (RequiredItemSupertype) &&
                       item.Charge >= RequiredBaseCharge + RequiredChargePerLevel * level &&
                       item.Level >= level;
            }

            return false;
        }

        public abstract void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter);

        public int Id { get; private set; }
        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel { get; private set; }
        public bool MovementRequired { get; private set; }
        public bool TargetRequired { get; private set; }
        public Sprite Icon { get; private set; }
        public string Info { get; private set; }

        public ItemSuperType RequiredItemSupertype { get; private set; }
        public float RequiredBaseCharge { get; private set; }
        public float RequiredChargePerLevel { get; private set; }
        public float RequiredBaseMovementPoints { get; private set; }
        public float RequiredMovementPointsPerLevel { get; private set; }
        public UnitCallbackType UnitCallback { get; private set; }
    }
}