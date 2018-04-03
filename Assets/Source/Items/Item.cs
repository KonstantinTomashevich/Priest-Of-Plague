using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class Item
    {
        public Item (int itemTypeId, float charge = 0, int level = 0)
        {
            _itemTypeId = itemTypeId;
            Charge = charge;
            Level = level;
        }

        public int ItemTypeId => _itemTypeId;
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

        private int _itemTypeId;
        private float _charge;
        private int _level;
    }
}


