﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PriestOfPlague.Source.Unit
{
    public enum BuffsAndDebuffsEnum { Healing = 0, Satiety, Magic_support, Weakening, Poisoning, Disease, Paralysis }

    public class CharacterModifiersContainer : MonoBehaviour
    {
        CharacterModifier[] dict = new CharacterModifier[7];
        /// <summary>
        /// Даёт баффы по индексу
        /// </summary>
        /// <param name="IndexIn">Уникальный индекс баффа</param>
        /// <returns>бафф</returns>
        public CharacterModifier GetBuff(int IndexIn) //Меня терзают смутные сомнения о правильности такой реализации...
        {
            if (dict[IndexIn] != null)
                return dict[IndexIn];
            else throw new System.Exception("Неверный индекс!");
        }
        /// <summary>
        /// Устанавливает все баффы
        /// </summary>
        public void SetBuffs()
        {
            //Лечение
            CharacterModifier a = new CharacterModifier("Лечение", 5);
            a.SetArr(0, 0, 0, 0, 0);
            a.PlusRegen = 3;
            a.BuffsForCancel.Add((int)BuffsAndDebuffsEnum.Weakening);
            a.BuffsForCancel.Add((int)BuffsAndDebuffsEnum.Disease);
            dict[(int)BuffsAndDebuffsEnum.Healing] = a;

            //Сытость
            a = new CharacterModifier("Сытость", 5);
            a.SetArr(1, 0, 1, 1, 0);
            dict[(int)BuffsAndDebuffsEnum.Satiety] = a;

            //Магическая поддержка
            a = new CharacterModifier("Магическая поддержка", 5);
            a.SetArr(1, 1, 1, 1, 1);
            dict[(int)BuffsAndDebuffsEnum.Magic_support] = a;

            //Ослаблен
            a = new CharacterModifier("Ослабление", 5);
            a.SetArr(-1, 0, -1, -1, 0);
            dict[(int)BuffsAndDebuffsEnum.Weakening] = a;

            //Отравлен
            a = new CharacterModifier("Отравление", 5);
            a.SetArr(-1, 0, -1, -1, 0);
            a.PlusRegen = -1;
            dict[(int)BuffsAndDebuffsEnum.Poisoning] = a;

            //Болен 
            a = new CharacterModifier("Болезнь", 5);
            a.SetArr(-1, -1, -1, -1, -1);
            dict[(int)BuffsAndDebuffsEnum.Disease] = a;

            //Парализован(Опять же не реализовать, так как нужно понимание действий (класс unit))
            a = new CharacterModifier("Паралич", 5);
            a.SetArr(0, 0, 0, 0, 0);
            dict[(int)BuffsAndDebuffsEnum.Paralysis] = a;
        }

        /// <summary>
        /// Класс содержит информацию о баффах/дебаффах
        /// </summary>

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}