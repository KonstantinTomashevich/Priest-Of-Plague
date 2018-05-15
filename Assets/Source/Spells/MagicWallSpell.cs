using JetBrains.Annotations;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PriestOfPlague.Source.Spells
{
    public class MagicDamageWallSpell : ISpell
    {
        public struct CastParameter
        {
            public Item UsedItem;
            public int Level;
        }

        public delegate void PerUnitCallbackType (Unit.Unit unit, CastParameter parameter);

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

        public bool CanCast (Unit.Unit unit)
        {
            ItemTypesContainer itemTypesContainer = unit.ItemTypesContainerRef;
            foreach (var item in unit.MyStorage.Items)
            {
                if (itemTypesContainer.ItemTypes [item.ItemTypeId].Supertypes.Contains (RequiredItemSupertype) &&
                    item.Charge >= RequiredBaseCharge + RequiredChargePerLevel)
                {
                    return true;
                }
            }

            return false;
        }


        public void Cast (Unit.Unit caster, UnitsHub unitsHub, object parameter)
        {
            var castParameter = (CastParameter) parameter;
            castParameter.UsedItem.Charge -= (RequiredBaseCharge + RequiredChargePerLevel * castParameter.Level);

            foreach (var unit in unitsHub.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (caster, unitToCheck,
                    BaseDistance + DistancePerLevel * castParameter.Level,
                    BaseAngle + AnglePerLevel * castParameter.Level)))
            {
                PerUnitCallback (unit, castParameter);
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