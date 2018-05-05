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
        private Dictionary <int, CharacterModifier> _modifiers;
        public Dictionary <int, CharacterModifier> Modifiers => _modifiers;

        public void LoadFromXML (XmlNode input)
        {
            _modifiers.Clear ();
            foreach (var modifierNode in XmlHelper.IterateChildren (input, "modifier"))
            {
                var modifier = CharacterModifier.LoadFromXML (modifierNode);
                _modifiers [modifier.ID] = modifier;
            }
        }
        
        private void Start ()
        {
            _modifiers = new Dictionary <int, CharacterModifier> ();
        }

        /* Move to XML
        public void SetBuffs()
        {
            //Лечение
            CharacterModifier a = new CharacterModifier("Лечение", 5);
            a.SetCharacteristicsChanges(0, 0, 0, 0, 0);
            a.PlusRegen = 3;
            a.BuffsToCancel.Add((int)BuffsAndDebuffsEnum.Weakening);
            a.BuffsToCancel.Add((int)BuffsAndDebuffsEnum.Disease);
            modifiersArray[(int)BuffsAndDebuffsEnum.Healing] = a;

            //Сытость
            a = new CharacterModifier("Сытость", 5);
            a.SetCharacteristicsChanges(1, 0, 1, 1, 0);
            modifiersArray[(int)BuffsAndDebuffsEnum.Satiety] = a;

            //Магическая поддержка
            a = new CharacterModifier("Магическая поддержка", 5);
            a.SetCharacteristicsChanges(1, 1, 1, 1, 1);
            modifiersArray[(int)BuffsAndDebuffsEnum.Magic_support] = a;

            //Ослаблен
            a = new CharacterModifier("Ослабление", 5);
            a.SetCharacteristicsChanges(-1, 0, -1, -1, 0);
            modifiersArray[(int)BuffsAndDebuffsEnum.Weakening] = a;

            //Отравлен
            a = new CharacterModifier("Отравление", 5);
            a.SetCharacteristicsChanges(-1, 0, -1, -1, 0);
            a.PlusRegen = -1;
            modifiersArray[(int)BuffsAndDebuffsEnum.Poisoning] = a;

            //Болен 
            a = new CharacterModifier("Болезнь", 5);
            a.SetCharacteristicsChanges(-1, -1, -1, -1, -1);
            modifiersArray[(int)BuffsAndDebuffsEnum.Disease] = a;

            //Парализован(Опять же не реализовать, так как нужно понимание действий (класс unit))
            a = new CharacterModifier("Паралич", 5);
            a.SetCharacteristicsChanges(0, 0, 0, 0, 0);
            modifiersArray[(int)BuffsAndDebuffsEnum.Paralysis] = a;
        }*/
    }
}