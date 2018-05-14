using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class Item
    {
        public Item (ItemsRegistrator itemsRegistrator, int itemTypeId, float charge = 0, int level = 0) : 
            this (itemsRegistrator, itemsRegistrator.FreeId, itemTypeId, charge, level)
        {
        }

        public Item (ItemsRegistrator itemsRegistrator, int id, int itemTypeId, float charge = 0, int level = 0)
        {
            _id = id;
            itemsRegistrator.Register (this);

            _itemTypeId = itemTypeId;
            Charge = charge;
            Level = level;
        }

        public static Item LoadFromXML (ItemsRegistrator itemsRegistrator, XmlNode input)
        {
            return new Item (itemsRegistrator,
                XmlHelper.GetIntAttribute (input, "Id"),
                XmlHelper.GetIntAttribute (input, "ItemTypeId"),
                XmlHelper.GetFloatAttribute (input, "Charge"),
                XmlHelper.GetIntAttribute (input, "Level"));
        }

        public void SaveToXml (XmlElement output)
        {
            output.SetAttribute ("Id", _id.ToString (NumberFormatInfo.InvariantInfo));
            output.SetAttribute ("ItemTypeID", _itemTypeId.ToString (NumberFormatInfo.InvariantInfo));
            output.SetAttribute ("Charge", _charge.ToString (NumberFormatInfo.InvariantInfo));
            output.SetAttribute ("Level", _level.ToString (NumberFormatInfo.InvariantInfo));
        }

        public int ItemTypeId => _itemTypeId;
        public int Id => _id;

        public float Charge
        {
            get { return _charge; }
            set
            {
                Debug.Assert (value >= 0.0f);
                _charge = value;
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                Debug.Assert (value >= 0);
                _level = value;
            }
        }

        private int _id;
        private int _itemTypeId;
        private float _charge;
        private int _level;
    }
}