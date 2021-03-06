﻿using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public abstract class SpellWithItemBase : ISpell
    {
        public delegate void UnitCallbackType (Unit.Unit caster, Unit.Unit unit, SpellCastParameter parameter);

        public delegate bool TargetCheckerType (Unit.Unit caster, Unit.Unit target);

        protected SpellWithItemBase (int id, float basicCastTime, float castTimeAdditionPerLevel, bool movementRequired,
            bool speachRequired, bool targetRequired, Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float requiredBaseMovementPoints,
            float requiredMovementPointsPerLevel, UnitCallbackType unitCallback, TargetCheckerType targetChecker)
        {
            Id = id;
            BasicCastTime = basicCastTime;
            CastTimeAdditionPerLevel = castTimeAdditionPerLevel;
            MovementRequired = movementRequired;
            SpeachRequired = speachRequired;
            TargetRequired = targetRequired;
            Icon = icon;
            Info = info;
            RequiredItemSupertype = requiredItemSupertype;
            RequiredBaseCharge = requiredBaseCharge;
            RequiredChargePerLevel = requiredChargePerLevel;
            RequiredBaseMovementPoints = requiredBaseMovementPoints;
            RequiredMovementPointsPerLevel = requiredMovementPointsPerLevel;
            UnitCallback = unitCallback;
            TargetChecker = targetChecker;
        }

        public bool CanCast (Unit.Unit unit, int level = 0, Item item = null, Unit.Unit target = null)
        {
            if ((TargetRequired && target == null) || !TargetChecker (unit, target))
            {
                return false;
            }

            if (MovementRequired && unit.MovementBlocked)
            {
                return false;
            }
            
            if (SpeachRequired && unit.SpeachBlocked)
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

        public virtual void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            parameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * parameter.Level);
            caster.UseMovementPoints (RequiredBaseMovementPoints + RequiredMovementPointsPerLevel * parameter.Level);

            if (parameter.UsedItem.Charge <= 0.00001f &&
                caster.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId].Consumeable)
            {
                caster.MyStorage.RemoveItem (parameter.UsedItem);
            }
        }

        public int Id { get; private set; }
        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel { get; private set; }
        public bool MovementRequired { get; private set; }
        public bool SpeachRequired { get; private set; }
        public bool TargetRequired { get; private set; }
        public Sprite Icon { get; private set; }
        public string Info { get; private set; }

        public ItemSuperType RequiredItemSupertype { get; private set; }
        public float RequiredBaseCharge { get; private set; }
        public float RequiredChargePerLevel { get; private set; }
        public float RequiredBaseMovementPoints { get; private set; }
        public float RequiredMovementPointsPerLevel { get; private set; }
        public UnitCallbackType UnitCallback { get; private set; }
        public TargetCheckerType TargetChecker { get; private set; }
    }
}