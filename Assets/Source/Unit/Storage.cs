using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using PriestOfPlague.Source.Core;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public class Storage
    {
        public const string EventItemAdded = "ItemAdded";
        public const string EventItemRemoved = "ItemRemoved";

        public class ItemAddedOrRemovedEventData
        {
            public ItemAddedOrRemovedEventData (Storage eventStorage, int itemId)
            {
                EventStorage = eventStorage;
                ItemId = itemId;
            }

            public Storage EventStorage;
            public int ItemId;
        }

        public Storage (ItemTypesContainer itemTypesContainer)
        {
            ItemTypesContainerRef = itemTypesContainer;
            _maxWeight = 0;
            _currentWeight = 0;
            _items = new Dictionary <int, Item> ();
        }

        public void UpdateItems (float timeStep)
        {
            foreach (var item in Items)
            {
                var itemType = ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
                item.Charge = Math.Min (itemType.MaxCharge + itemType.MaxChargeAdditionPerLevel * item.Level,
                    item.Charge +
                    (itemType.ChargeRegeneration + itemType.ChargeRegenerationAdditionPerLevel * item.Level) *
                    timeStep);
            }
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

            EventsHub.Instance.SendGlobalEvent (EventItemAdded, new ItemAddedOrRemovedEventData (this, item.Id));
            return true;
        }

        public bool RemoveItem (Item item)
        {
            if (_items.Remove (item.Id))
            {
                Debug.Assert (ItemTypesContainerRef.ItemTypes.ContainsKey (item.ItemTypeId));
                ItemType itemType = ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
                _currentWeight -= itemType.Weight;

                Debug.Assert (_currentWeight >= 0.0f);
                EventsHub.Instance.SendGlobalEvent (EventItemRemoved, new ItemAddedOrRemovedEventData (this, item.Id));
                return true;
            }

            return false;
        }

        public void LoadFromXML (ItemsRegistrator itemsRegistrator, XmlNode input)
        {
            _maxWeight = XmlHelper.GetFloatAttribute (input, "MaxWeight");
            _currentWeight = 0.0f;
            _items.Clear ();

            foreach (var itemNode in XmlHelper.IterateChildren (input, "item"))
            {
                var item = Item.LoadFromXML (itemsRegistrator, itemNode);
                AddItem (item);
            }
        }

        public void SaveToXml (XmlElement output)
        {
            output.SetAttribute ("MaxWeight", _maxWeight.ToString (NumberFormatInfo.InvariantInfo));
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
                // TODO: It's a real situation (with ill modifier, for example) what should we do in this case?
                Debug.Assert (value >= _currentWeight);
                _maxWeight = value;
            }
        }

        public float CurrentWeight => _currentWeight;
        public Dictionary <int, Item>.ValueCollection Items => _items.Values;
        public Item this [int id] => _items.ContainsKey (id) ? _items [id] : null;

        private float _maxWeight;
        private float _currentWeight;
        private Dictionary <int, Item> _items;
    }
}