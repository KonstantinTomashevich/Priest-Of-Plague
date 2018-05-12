using System.Collections.Generic;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public enum ItemSuperType
    {
        FireWand = 0,
    }
    
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

        public void LoadFromXML (XmlNode input)
        {
            _itemTypes.Clear ();
            foreach (var itemTypeNode in XmlHelper.IterateChildren (input, "itemType"))
            {
                var itemType = ItemType.LoadFromXML (itemTypeNode);
                _itemTypes.Add (itemType.Id, itemType);
            }
        }

        public TextAsset Xml;
        public Dictionary <int, ItemType> ItemTypes => _itemTypes;
        public Dictionary <int, string> Supertypes => _supertypes;

        private void Start ()
        {
            var document = new XmlDocument ();
            document.LoadXml (Xml.text);
            LoadFromXML (document.DocumentElement);
        }
        
        private Dictionary <int, ItemType> _itemTypes;
        private Dictionary <int, string> _supertypes;
    }
}