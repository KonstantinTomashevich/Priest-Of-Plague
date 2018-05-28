using JetBrains.Annotations;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PriestOfPlague.Source.Spells
{
    // TODO: Unit movement points usage.
    public class MagicDamageWallSpell : ISpell
    {
        public delegate void PerUnitCallbackType (Unit.Unit unit, SpellCastParameter parameter);

        public MagicDamageWallSpell (int id, Sprite icon, string info, ItemSuperType requiredItemSupertype,
            float requiredBaseCharge, float requiredChargePerLevel, float basicCastTime, float castTimeAdditionPerLevel,
            float baseAngle,
            float anglePerLevel, float baseDistance,
            float distancePerLevel, PerUnitCallbackType perUnitCallback)
        {
            Id = id;
            Icon = icon;
            Info = info;
            RequiredItemSupertype = requiredItemSupertype;

            RequiredBaseCharge = requiredBaseCharge;
            RequiredChargePerLevel = requiredChargePerLevel;

            BasicCastTime = basicCastTime;
            CastTimeAdditionPerLevel = castTimeAdditionPerLevel;

            BaseAngle = baseAngle;
            BaseDistance = baseDistance;
            AnglePerLevel = anglePerLevel;
            DistancePerLevel = distancePerLevel;
            PerUnitCallback = perUnitCallback;
        }

        public bool CanCast (Unit.Unit unit, int level = 0, Item item = null)
        {
            ItemTypesContainer itemTypesContainer = unit.ItemTypesContainerRef;

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

        public void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            parameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * parameter.Level);

            foreach (var unit in unitsHub.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (caster, unitToCheck,
                    BaseDistance + DistancePerLevel * parameter.Level,
                    BaseAngle + AnglePerLevel * parameter.Level)))
            {
                if (unit != caster)
                {
                    PerUnitCallback (unit, parameter);
                }
            }
        }

        public int Id { get; private set; }
        public Sprite Icon { get; private set; }
        public string Info { get; private set; }
        public ItemSuperType RequiredItemSupertype { get; private set; }

        public float RequiredBaseCharge { get; private set; }
        public float RequiredChargePerLevel { get; private set; }

        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel { get; private set; }

        public float BaseAngle { get; private set; }
        public float BaseDistance { get; private set; }
        public float AnglePerLevel { get; private set; }
        public float DistancePerLevel { get; private set; }
        public PerUnitCallbackType PerUnitCallback { get; private set; }
    }
}