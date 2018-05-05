using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifiersContainer : MonoBehaviour
    {
        private List <CharacterModifier> _modifiersArray;

        public CharacterModifier GetBuff (int id)
        {
            if (id >= 0 && id < _modifiersArray.Count)
            {
                return _modifiersArray [id];
            }

            throw new IndexOutOfRangeException ("Unknown modifier id!");
        }

        private void Start ()
        {
            _modifiersArray = new List <CharacterModifier> ();
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
        }**/
    }
}