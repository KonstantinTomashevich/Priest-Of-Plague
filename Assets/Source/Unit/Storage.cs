using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using PriestOfPlague.Source.Core;
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
            _items = new Dictionary <int, Item> ();
        }

        public bool AddItem (Item item)
        {
            if (_items.ContainsKey (item.Id))
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
            _items.Add (item.Id, item);
            return true;
        }

        public bool RemoveItem (Item item)
        {
            if (_items.Remove (item.Id))
            {
                ItemType itemType = ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
                Debug.Assert (itemType != null);
                _currentWeight -= itemType.Weight;
                
                Debug.Assert (_currentWeight >= 0.0f);
                return true;
            }

            return false;
        }
        
        public void LoadFromXML (ItemsRegistrator itemsRegistrator, XmlNode input)
        {
            _maxWeight = XmlHelper.GetFloatAttribute (input, "MaxWeight");
            _currentWeight = XmlHelper.GetFloatAttribute (input, "CurrentWeight");
            
            _items.Clear ();
            foreach (var itemNode in XmlHelper.IterateChildren (input, "item"))
            {
                var item = Item.LoadFromXML (itemsRegistrator, itemNode);
                _items.Add (item.Id, item);
            }
        }

        public void SaveToXml (XmlElement output)
        {
            output.SetAttribute ("Max Weight", _maxWeight.ToString (NumberFormatInfo.InvariantInfo));
            output.SetAttribute ("Current Weight", _currentWeight.ToString (NumberFormatInfo.InvariantInfo));

            foreach (var item in Items)
            {
                var itemElement = output.OwnerDocument.CreateElement ("item");
                item.SaveToXml (itemElement);
                output.AppendChild (itemElement);
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
        public Dictionary <int, Item>.ValueCollection Items => _items.Values;
        public Item this [int id] => _items [id];

        private float _maxWeight;
        private float _currentWeight;
        private Dictionary <int, Item> _items;
    }
}