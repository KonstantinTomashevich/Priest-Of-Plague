using System.Collections.Generic;
using System.Xml;
using PriestOfPlague.Source.Core;
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

        public void LoadFromXML (XmlNode input)
        {
            _itemTypes.Clear ();
            _supertypes.Clear ();
            
            foreach (var supertypeNode in XmlHelper.IterateChildren (input, "supertype"))
            {
                int id = XmlHelper.GetIntAttribute (supertypeNode, "ID");
                string name = supertypeNode.Attributes ["Name"].InnerText;
                _supertypes.Add (id, name);
            }
            
            foreach (var itemTypeNode in XmlHelper.IterateChildren (input, "itemType"))
            {
                var itemType = ItemType.LoadFromXML (itemTypeNode);
                _itemTypes.Add (itemType.Id, itemType);
            }
        }

        public Dictionary <int, ItemType> ItemTypes => _itemTypes;
        public Dictionary <int, string> Supertypes => _supertypes;

        private Dictionary <int, ItemType> _itemTypes;
        private Dictionary <int, string> _supertypes;
    }
}