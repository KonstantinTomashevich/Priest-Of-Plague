using System.Collections.Generic;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class ItemType
    {
        public ItemType (int id)
        {
            _id = id;
            _opensSpells = new List <int> ();
            MaxCharge = 0;
            ChargeRegeneration = 0;
            
            BasicForce = 0;
            ForceAdditionPerLevel = 0;
            MaxChargeAdditionPerLevel = 0;
            ChargeRegenerationAdditionPerLevel = 0;
            
            _supertypes = new List <int> ();
            Weight = 0;
            Icon = null;
        }

        public static ItemType LoadFromXML (XmlNode input)
        {
            var itemType = new ItemType (XmlHelper.GetIntAttribute (input, "ID"));
            foreach (var spellNode in XmlHelper.IterateChildren (input, "opensSpell"))
            {
                itemType._opensSpells.Add (XmlHelper.GetIntAttribute (spellNode, "ID"));
            }

            itemType._maxCharge = XmlHelper.GetFloatAttribute (input, "Max Charge");
            itemType._chargeRegeneration = XmlHelper.GetFloatAttribute (input, "Charge Regeneration");

            itemType._basicForce = XmlHelper.GetFloatAttribute (input, "Basic Force");
            itemType._forceAdditionPerLevel = XmlHelper.GetFloatAttribute (input, "Force Addition Per Level");
            itemType._maxChargeAdditionPerLevel = XmlHelper.GetFloatAttribute (input, "Max Charge Addition Per Level");
            itemType._chargeRegenerationAdditionPerLevel = XmlHelper.GetFloatAttribute (
                input, "Charge Regeneration Addition Per Level");
            
            foreach (var supertypeNode in XmlHelper.IterateChildren (input, "supertype"))
            {
                itemType._opensSpells.Add (XmlHelper.GetIntAttribute (supertypeNode, "ID"));
            }
            
            itemType._weight = XmlHelper.GetFloatAttribute (input, "Weight");
            itemType._icon = Resources.Load <Sprite> (input.Attributes ["Icon"].InnerText);
            return itemType;
        }

        public int Id => _id;
        public List <int> OpensSpells => _opensSpells;

        public float MaxCharge
        {
            get { return _maxCharge; }
            set
            {
                Debug.Assert (value >= 0.0f);
                _maxCharge = value;
            }
        }

        public float ChargeRegeneration
        {
            get { return _chargeRegeneration; }
            set { _chargeRegeneration = value; }
        }

        public float BasicForce
        {
            get { return _basicForce; }
            set
            {
                Debug.Assert (value >= 0.0f);
                _basicForce = value;
            }
        }

        public float ForceAdditionPerLevel
        {
            get { return _forceAdditionPerLevel; }
            set { _forceAdditionPerLevel = value; }
        }

        public float MaxChargeAdditionPerLevel
        {
            get { return _maxChargeAdditionPerLevel; }
            set { _maxChargeAdditionPerLevel = value; }
        }

        public float ChargeRegenerationAdditionPerLevel
        {
            get { return _chargeRegenerationAdditionPerLevel; }
            set { _chargeRegenerationAdditionPerLevel = value; }
        }

        public List <int> Supertypes => _supertypes;
        public float Weight
        {
            get { return _weight; }
            set
            {
                Debug.Assert (value >= 0);
                _weight = value;
            }
        }

        public Sprite Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private int _id;
        private List <int> _opensSpells;
        private float _maxCharge;
        private float _chargeRegeneration;

        private float _basicForce;
        private float _forceAdditionPerLevel;
        private float _maxChargeAdditionPerLevel;
        private float _chargeRegenerationAdditionPerLevel;

        private List <int> _supertypes;
        private float _weight;
        private Sprite _icon;
    }
}