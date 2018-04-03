using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class ItemTypesContainer : MonoBehaviour
    {
        public ItemTypesContainer ()
        {
            _itemTypes = new Dictionary <int, ItemType> ();
            _supertypes = new Dictionary <int, string> ();
        }

        public bool AddItemType (ItemType itemType)
        {
            if (ItemTypes.ContainsKey (itemType.Id))
            {
                return false;
            }

            ItemTypes [itemType.Id] = itemType;
            return true;
        }

        public bool RemoveItemType (ItemType itemType)
        {
            return ItemTypes.Remove (itemType.Id);
        }

        public Dictionary <int, ItemType> ItemTypes => _itemTypes;
        public Dictionary <int, string> Supertypes => _supertypes;

        private Dictionary <int, ItemType> _itemTypes;
        private Dictionary <int, string> _supertypes;
    }
}