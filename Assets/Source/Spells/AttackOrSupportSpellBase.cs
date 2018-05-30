using System.Linq;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public abstract class AttackOrSupportSpellBase : ISpell
    {
        public delegate void UnitCallbackType (Unit.Unit unit, SpellCastParameter parameter);
        
        protected AttackOrSupportSpellBase (int id, Sprite icon, string info, bool movementRequired, bool affectSelf,
            ItemSuperType requiredItemSupertype, float requiredBaseCharge, float requiredChargePerLevel,
            float requiredBaseMovementPoints, float requiredMovementPointsPerLevel, float basicCastTime,
            float castTimeAdditionPerLevel, float baseAngle, float anglePerLevel, float baseDistance,
            float distancePerLevel, UnitCallbackType unitCallback)
        {
            Id = id;
            Icon = icon;
            Info = info;
            MovementRequired = movementRequired;
            AffectSelf = affectSelf;
            RequiredItemSupertype = requiredItemSupertype;
            RequiredBaseCharge = requiredBaseCharge;
            RequiredChargePerLevel = requiredChargePerLevel;
            RequiredBaseMovementPoints = requiredBaseMovementPoints;
            RequiredMovementPointsPerLevel = requiredMovementPointsPerLevel;
            BasicCastTime = basicCastTime;
            CastTimeAdditionPerLevel = castTimeAdditionPerLevel;
            BaseAngle = baseAngle;
            AnglePerLevel = anglePerLevel;
            BaseDistance = baseDistance;
            DistancePerLevel = distancePerLevel;
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

        public virtual void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            parameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * parameter.Level);
            caster.UseMovementPoints (RequiredBaseMovementPoints + RequiredMovementPointsPerLevel * parameter.Level);
        }

        public int Id { get; private set; }
        public Sprite Icon { get; private set; }
        public string Info { get; private set; }
        public bool MovementRequired { get; private set; }
        public bool AffectSelf { get; private set; }
        public ItemSuperType RequiredItemSupertype { get; private set; }

        public float RequiredBaseCharge { get; private set; }
        public float RequiredChargePerLevel { get; private set; }
        public float RequiredBaseMovementPoints { get; private set; }
        public float RequiredMovementPointsPerLevel { get; private set; }

        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel { get; private set; }

        public float BaseAngle { get; private set; }
        public float AnglePerLevel { get; private set; }
        public float BaseDistance { get; private set; }
        public float DistancePerLevel { get; private set; }
        public UnitCallbackType UnitCallback { get; private set; }
    }
}