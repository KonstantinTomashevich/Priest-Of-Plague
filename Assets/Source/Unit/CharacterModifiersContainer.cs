using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PriestOfPlague.Source.Core;
using UnityEngine;


namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifiersContainer : MonoBehaviour
    {
        public TextAsset Xml;
        private Dictionary <int, CharacterModifier> _modifiers;
        public Dictionary <int, CharacterModifier> Modifiers => _modifiers;

        public void LoadFromXML (XmlNode input)
        {
            _modifiers.Clear ();
            foreach (var modifierNode in XmlHelper.IterateChildren (input, "modifier"))
            {
                var modifier = CharacterModifier.LoadFromXML (modifierNode);
                _modifiers [modifier.Id] = modifier;
            }
        }
        
        private void Start ()
        {
            _modifiers = new Dictionary <int, CharacterModifier> ();
            var document = new XmlDocument ();
            
            document.LoadXml (Xml.text);
            LoadFromXML (document.DocumentElement);
        }
    }
}