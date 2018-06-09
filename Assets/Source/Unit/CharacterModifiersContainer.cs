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

        public static int GetIdByInfo (string info)
        {
            switch (info)
            {
                case "Лечение": return 0;
                case "Сытость": return 1;
                case "Поддержка": return 2;
                case "Ослабление": return 3;
                case "Отравление": return 4;
                case "Болезнь": return 5;
                case "Паралич": return 6;
                case "Горение": return 7;
                case "Неуязвимость": return 8;
            }

            return -1;
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