using System.Collections.Generic;
using System.Collections.ObjectModel;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public class Storage
    {
        public Storage (ItemTypesContainer itemTypesContainer)
        {
            ItemTypesContainerRef = itemTypesContainer;
            _maxWeight = 0;
            _currentWeight = 0;
            _items = new List <Item> ();
        }

        public bool AddItem (Item item)
        {
            if (_items.Contains (item))
            {
                return true;
            }

            ItemType itemType = ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
            Debug.Assert (itemType != null);

            if (_currentWeight + itemType.Weight > _maxWeight)
            {
                return false;
            }

            _currentWeight += itemType.Weight;
            _items.Add (item);
            return true;
        }

        public bool RemoveItem (Item item)
        {
            if (_items.Remove (item))
            {
                ItemType itemType = ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
                Debug.Assert (itemType != null);
                _currentWeight -= itemType.Weight;
                
                Debug.Assert (_currentWeight >= 0.0f);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ItemTypesContainer ItemTypesContainerRef;
        public float MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                Debug.Assert (value >= _currentWeight);
                _maxWeight = value;
            }
        }
        
        public float CurrentWeight => _currentWeight;
        public ReadOnlyCollection <Item> Items => _items.AsReadOnly ();

        private float _maxWeight;
        private float _currentWeight;
        private List <Item> _items;
    }
}