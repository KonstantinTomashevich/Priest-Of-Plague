using System.Collections.Generic;
using PriestOfPlague.Source.Items;

namespace PriestOfPlague.Source.Unit
{
    public enum EquipmentSlot
    {
        BodyArmor = 0,
        HandsArmor,
        HeadArmor,
        LegsArmor,
        Item1,
        Item2,
        Item3,
        LeftHand,
        RightHand,
        Count,
    }

    public class Equipment
    {
        public Equipment (ItemTypesContainer itemTypesContainer)
        {
            _itemTypesContainer = itemTypesContainer;
            _itemsOnSlots = new List <Item> ((int) EquipmentSlot.Count);
            _checkers = new List <SlotSetAttemptChecker> ((int) EquipmentSlot.Count);
            InitEquipmentSlotsCheckers ();
        }

        public bool CanBeSetted (EquipmentSlot slot, Item item)
        {
            return _checkers [(int) slot] (_itemsOnSlots, item, _itemTypesContainer);
        }

        public Item this[EquipmentSlot slot]
        {
            get { return _itemsOnSlots [(int) slot]; }
            set
            {
                if (CanBeSetted (slot, value))
                {
                    _itemsOnSlots [(int) slot] = value;
                }
            }
        }

        private void InitEquipmentSlotsCheckers ()
        {
            _checkers [(int) EquipmentSlot.BodyArmor] = (slots, item, container) =>
                container.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.BodyArmor);

            _checkers [(int) EquipmentSlot.HandsArmor] = (slots, item, container) =>
                container.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.HandsArmor);

            _checkers [(int) EquipmentSlot.HeadArmor] = (slots, item, container) =>
                container.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.HeadArmor);

            _checkers [(int) EquipmentSlot.LegsArmor] = (slots, item, container) =>
                container.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.LegsArmor);

            SlotSetAttemptChecker itemChecker = (slots, item, container) =>
                container.ItemTypes [item.ItemTypeId].Supertypes.Contains (ItemSuperType.AdditionalItem);

            _checkers [(int) EquipmentSlot.Item1] = itemChecker;
            _checkers [(int) EquipmentSlot.Item2] = itemChecker;
            _checkers [(int) EquipmentSlot.Item3] = itemChecker;

            _checkers [(int) EquipmentSlot.RightHand] = (slots, item, container) =>
            {
                var itemType = container.ItemTypes [item.ItemTypeId];
                return itemType.Supertypes.Contains (ItemSuperType.OneHandedWeapon) ||
                       (itemType.Supertypes.Contains (ItemSuperType.TwoHandedWeapon) &&
                        slots [(int) EquipmentSlot.LeftHand] == null);
            };

            _checkers [(int) EquipmentSlot.LeftHand] = (slots, item, container) =>
            {
                var itemType = container.ItemTypes [item.ItemTypeId];
                var rightHandItem = slots [(int) EquipmentSlot.RightHand];

                return rightHandItem != null &&
                       !container.ItemTypes [rightHandItem.ItemTypeId].Supertypes
                           .Contains (ItemSuperType.TwoHandedWeapon) &&
                       itemType.Supertypes.Contains (ItemSuperType.OneHandedWeapon);
            };
        }

        private ItemTypesContainer _itemTypesContainer;
        private List <Item> _itemsOnSlots;

        private delegate bool SlotSetAttemptChecker (List <Item> itemsOnSlots, Item item,
            ItemTypesContainer itemTypesContainer);

        private List <SlotSetAttemptChecker> _checkers;
    }
}