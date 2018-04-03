using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class ItemType
    {
        public ItemType (int id)
        {
            _id = id;
        }

        public int Id => _id;
        public List <int> OpensSpells
        {
            get { return _opensSpells; }
            set { _opensSpells = value; }
        }

        public float MaxCharge
        {
            get { return _maxCharge; }
            set { _maxCharge = value; }
        }

        public float ChargeRegeneration
        {
            get { return _chargeRegeneration; }
            set { _chargeRegeneration = value; }
        }

        public float BasicForce
        {
            get { return _basicForce; }
            set { _basicForce = value; }
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

        public List <int> Supertypes
        {
            get { return _supertypes; }
            set { _supertypes = value; }
        }

        public float Weight
        {
            get { return _weight; }
            set { _weight = value; }
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